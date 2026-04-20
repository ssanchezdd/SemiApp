"""Rebuild all marcha APNGs from the staged FLV streams with denser sampling.

Target: at least 2x the current frame count per APNG. Sampling strategy:
- For streams with >120 total frames: every 5th frame.
- For shorter streams: every 2nd frame.
- Floor: at least 50 frames per APNG (use every frame if needed).

Also emits a small middle-frame JPG thumbnail per stream so the caller can
visually verify which stream maps to which pathology.

Uses transpose=1 (90 deg CW) to match the orientation already established
for the previous extraction pass.
"""
import os
import subprocess
import sys
from pathlib import Path

STAGING = Path(r"d:\SemiApp\frames_staging\marcha")
OUT = Path(r"d:\SemiApp\docs\images")
THUMBS = Path(r"d:\SemiApp\docs\images\marcha_thumbs")

# Map cid (stream char id) -> marcha index. Follows the previously established
# order: stream 003 = marcha_01, 061 = marcha_02, 193 = marcha_03, ...
CID_ORDER = [3, 61, 193, 200, 207, 214, 221, 233, 240]


def total_frames(flv: Path) -> int:
    r = subprocess.run(
        ["ffprobe", "-v", "0", "-select_streams", "v:0", "-count_packets",
         "-show_entries", "stream=nb_read_packets", "-of", "csv=p=0", str(flv)],
        capture_output=True, text=True,
    )
    return int(r.stdout.strip() or 0)


def pick_mod(total: int) -> int:
    """Choose a 'every Nth frame' modulus. Aim for maximum frame density
    (use every frame where possible)."""
    if total <= 200:
        return 1  # use every frame
    return 2      # for stream_003 (411) -> 205 frames


def build_apng(flv: Path, out_png: Path, mod: int, fps: int = 8):
    select = f"select='not(mod(n,{mod}))'"
    vf = f"{select},transpose=1"
    cmd = [
        "ffmpeg", "-hide_banner", "-loglevel", "error", "-y",
        "-i", str(flv),
        "-vf", vf,
        "-plays", "0",
        "-r", str(fps),
        "-f", "apng",
        str(out_png),
    ]
    r = subprocess.run(cmd, capture_output=True, text=True)
    if r.returncode != 0:
        print(f"  ERROR building {out_png.name}: {r.stderr.strip()[:200]}")
        return False
    return True


def build_thumb(flv: Path, out_jpg: Path):
    """Grab the middle frame (no rotation) as a preview thumbnail."""
    total = total_frames(flv)
    mid = max(1, total // 2)
    cmd = [
        "ffmpeg", "-hide_banner", "-loglevel", "error", "-y",
        "-i", str(flv),
        "-vf", f"select=eq(n\\,{mid}),transpose=1",
        "-frames:v", "1",
        "-q:v", "3",
        str(out_jpg),
    ]
    subprocess.run(cmd, capture_output=True)


def main():
    THUMBS.mkdir(parents=True, exist_ok=True)
    print(f"{'marcha':>10s}  {'cid':>4s}  {'total':>6s}  {'mod':>4s}  {'kept':>5s}  out")
    for i, cid in enumerate(CID_ORDER, start=1):
        flv = STAGING / f"stream_{cid:03d}.flv"
        if not flv.exists():
            print(f"  skip: {flv} missing")
            continue
        total = total_frames(flv)
        mod = pick_mod(total)
        kept = (total + mod - 1) // mod
        out = OUT / f"marcha_{i:02d}.png"
        thumb = THUMBS / f"marcha_{i:02d}.jpg"
        ok = build_apng(flv, out, mod)
        build_thumb(flv, thumb)
        sz = out.stat().st_size / 1024 / 1024 if ok else 0
        print(f"  marcha_{i:02d}  {cid:>4d}  {total:>6d}  {mod:>4d}  {kept:>5d}  {sz:5.2f} MB")


if __name__ == "__main__":
    main()
