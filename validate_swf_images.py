"""Validate that every image embedded in every SWF is present in docs/images/.

Run this after extract_swf_images.py to confirm coverage. Exits non-zero if any
image-bearing SWF has a count mismatch between embedded images and extracted files.
"""
import os
import struct
import sys
import zlib
from collections import Counter

SWF_DIR = r"C:\Users\santi\Downloads\Proyecto Evaluación de la apariencia general"
OUT_DIR = r"d:\SemiApp\docs\images"

IMAGE_TAGS = {6, 21, 35}
LOSSLESS_TAGS = {20, 36}
VIDEO_TAG = 60
# Must match extract_swf_images.py: bitmaps below this are GUI icons, not clinical photos.
MIN_IMAGE_BYTES = 2000


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


def iter_tags(data):
    o = 8
    o = parse_rect(data, o)
    o += 4
    while o < len(data):
        if o + 2 > len(data):
            break
        h = struct.unpack("<H", data[o : o + 2])[0]
        o += 2
        tt = h >> 6
        tl = h & 0x3F
        if tl == 0x3F:
            tl = struct.unpack("<I", data[o : o + 4])[0]
            o += 4
        if o + tl > len(data):
            break
        yield tt, data[o : o + tl]
        o += tl
        if tt == 0:
            break


def count_by_prefix(folder):
    counts = Counter()
    for f in os.listdir(folder):
        pfx, _, rest = f.rpartition("_")
        if pfx and rest.split(".")[0].isdigit():
            counts[pfx] += 1
    return counts


def main():
    present = count_by_prefix(OUT_DIR)
    rows = []
    failures = 0
    for name in sorted(os.listdir(SWF_DIR)):
        if not name.endswith(".swf"):
            continue
        pfx = name[:-4]
        data = read_swf(os.path.join(SWF_DIR, name))
        if data is None:
            continue
        c = Counter()
        expected = 0
        lossless = 0
        for tt, td in iter_tags(data):
            c[tt] += 1
            if tt in IMAGE_TAGS:
                body = td[6:] if tt == 35 else td[2:]
                body = body.replace(b"\xff\xd9\xff\xd8", b"")
                if len(body) >= MIN_IMAGE_BYTES:
                    expected += 1
            elif tt in LOSSLESS_TAGS:
                lossless += 1
        has_video = c.get(VIDEO_TAG, 0) > 0
        got = present.get(pfx, 0)
        if expected == 0 and lossless == 0 and not has_video:
            status = "no-images"
        elif has_video and expected == 0:
            status = "video-only"
        elif expected == got:
            status = "ok"
        else:
            status = f"MISSING {expected - got}"
            failures += 1
        rows.append((pfx, expected, lossless, got, status))

    print(f"{'SWF':20s} {'embedded':>8s} {'lossless':>8s} {'present':>7s}  status")
    print("-" * 60)
    for pfx, exp, lossless, got, status in rows:
        print(f"{pfx:20s} {exp:>8d} {lossless:>8d} {got:>7d}  {status}")

    print()
    if failures:
        print(f"FAIL: {failures} SWF(s) have fewer extracted images than embedded.")
        sys.exit(1)
    print("OK: every image-bearing SWF matches its extraction count.")


if __name__ == "__main__":
    main()
