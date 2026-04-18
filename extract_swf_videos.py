"""Extract Sorenson H.263 video streams from SWF files into playable FLV files.

SWFs built by the original Flash project embed video inside DefineSprite tags
(tag 39), as DefineVideoStream (tag 60) + VideoFrame (tag 61) pairs. ffmpeg
cannot read the SWF directly because dimensions are not signalled in the
top-level video stream metadata, so we rewrap each video stream into a real
FLV container with correct dimensions.

Run this to produce FLV files under frames_staging/<swf>/<char_id>.flv.
Then run extract_swf_videos.py frames (or call ffmpeg directly) to pull
representative frames from each FLV.
"""
import os
import struct
import subprocess
import sys
import zlib
from collections import defaultdict

SWF_DIR = r"C:\Users\santi\Downloads\Proyecto Evaluación de la apariencia general"
STAGING = r"d:\SemiApp\frames_staging"
FPS = 12  # SWF frame rate; both disnea and movimientos use 12 fps


def read_swf(path):
    with open(path, "rb") as f:
        sig = f.read(3)
        f.read(1)
        fl = struct.unpack("<I", f.read(4))[0]
        if sig == b"CWS":
            return sig + b"\x00" + struct.pack("<I", fl) + zlib.decompress(f.read())
        if sig == b"FWS":
            return sig + b"\x00" + struct.pack("<I", fl) + f.read()
        return None


def parse_rect(d, o):
    n = (d[o] >> 3) & 0x1F
    return o + (5 + n * 4 + 7) // 8


def iter_tags(data, o):
    while o < len(data):
        if o + 2 > len(data):
            return
        h = struct.unpack("<H", data[o : o + 2])[0]
        o += 2
        tt = h >> 6
        tl = h & 0x3F
        if tl == 0x3F:
            tl = struct.unpack("<I", data[o : o + 4])[0]
            o += 4
        if o + tl > len(data):
            return
        yield tt, data[o : o + tl]
        o += tl
        if tt == 0:
            return


def iter_all_tags(data):
    """Yield (tag_type, tag_data) from top-level AND inside DefineSprite tags."""
    o = 8
    o = parse_rect(data, o)
    o += 4
    for tt, td in iter_tags(data, o):
        if tt == 39 and len(td) >= 4:
            # DefineSprite: UI16 sprite_id + UI16 frame_count + nested tags
            for ntt, ntd in iter_tags(td, 4):
                yield ntt, ntd
        else:
            yield tt, td


def collect_streams(swf_path):
    data = read_swf(swf_path)
    if data is None:
        return {}, {}
    streams = {}
    frames = defaultdict(list)
    for tt, td in iter_all_tags(data):
        if tt == 60 and len(td) >= 10:
            cid, num, w, h = struct.unpack("<HHHH", td[:8])
            codec_id = td[9]
            streams[cid] = {"num_frames": num, "width": w, "height": h, "codec_id": codec_id}
        elif tt == 61 and len(td) >= 4:
            cid, frame_num = struct.unpack("<HH", td[:4])
            frames[cid].append((frame_num, td[4:]))
    for cid in frames:
        frames[cid].sort(key=lambda x: x[0])
    return streams, frames


def build_flv(width, height, codec_id, frames, fps=FPS):
    """Wrap a sequence of raw codec frames into a minimal FLV container.

    FLV tag header: TagType(1)+DataSize(3)+Timestamp(3)+TimestampExtended(1)+StreamID(3)
    Video tag data byte 0: (frame_type<<4) | codec_id
    """
    out = bytearray()
    # FLV header: signature "FLV", version 1, flags=0x01 (video only), header size=9
    out += b"FLV\x01\x01\x00\x00\x00\x09"
    out += b"\x00\x00\x00\x00"  # PreviousTagSize0

    for i, (_, frame_data) in enumerate(frames):
        frame_type = 1 if i == 0 else 2  # first is keyframe
        flag = (frame_type << 4) | codec_id
        tag_data = bytes([flag]) + frame_data
        ts_ms = int(round(i * 1000.0 / fps))

        out += bytes([9])  # video tag
        out += len(tag_data).to_bytes(3, "big")
        out += (ts_ms & 0xFFFFFF).to_bytes(3, "big")
        out += bytes([(ts_ms >> 24) & 0xFF])
        out += b"\x00\x00\x00"  # StreamID
        out += tag_data
        out += (11 + len(tag_data)).to_bytes(4, "big")  # PreviousTagSize
    return bytes(out)


def process_swf(swf_name, scene_threshold=0.3):
    base = os.path.splitext(swf_name)[0]
    swf_path = os.path.join(SWF_DIR, swf_name)
    out_dir = os.path.join(STAGING, base)
    os.makedirs(out_dir, exist_ok=True)

    streams, frames = collect_streams(swf_path)
    print(f"{base}: found {len(streams)} video stream(s)")

    results = []
    for cid, meta in sorted(streams.items()):
        frame_list = frames.get(cid, [])
        if not frame_list:
            print(f"  stream {cid}: no frame data, skipping")
            continue
        flv_bytes = build_flv(meta["width"], meta["height"], meta["codec_id"], frame_list)
        flv_path = os.path.join(out_dir, f"stream_{cid:03d}.flv")
        with open(flv_path, "wb") as f:
            f.write(flv_bytes)
        print(
            f"  stream {cid}: {len(frame_list)} frames {meta['width']}x{meta['height']} "
            f"-> {flv_path} ({len(flv_bytes):,} B)"
        )

        # Extract representative frames: scene changes OR periodic sampling
        # (Sorenson H.263 continuous demos rarely trigger scene detection alone,
        # so we OR it with "every 2 seconds" at 12 fps = every 24th frame.)
        out_pattern = os.path.join(out_dir, f"stream_{cid:03d}_%03d.jpg")
        cmd = [
            "ffmpeg", "-hide_banner", "-loglevel", "error", "-y",
            "-i", flv_path,
            "-vf", f"select='gt(scene,{scene_threshold})+not(mod(n\\,24))'",
            "-fps_mode", "vfr",
            "-pix_fmt", "yuvj420p",
            "-q:v", "3",
            out_pattern,
        ]
        r = subprocess.run(cmd, capture_output=True, text=True)
        if r.returncode != 0:
            print(f"    ffmpeg error: {r.stderr.strip()[:200]}")
            continue
        extracted = sorted(
            f for f in os.listdir(out_dir)
            if f.startswith(f"stream_{cid:03d}_") and f.endswith(".jpg")
        )
        print(f"    extracted {len(extracted)} scene frames")
        results.append((cid, meta, extracted))
    return results


def main():
    os.makedirs(STAGING, exist_ok=True)
    targets = sys.argv[1:] or ["disnea.swf", "movimientos.swf"]
    for swf in targets:
        process_swf(swf)


if __name__ == "__main__":
    main()
