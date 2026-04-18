"""Extract embedded JPEG and PNG images from SWF files."""
import struct
import os
import sys
import zlib

SWF_DIR = r"C:\Users\santi\Downloads\Proyecto Evaluación de la apariencia general"
OUT_DIR = r"d:\SemiApp\docs\images"

os.makedirs(OUT_DIR, exist_ok=True)

def read_swf(path):
    with open(path, "rb") as f:
        sig = f.read(3)
        f.read(1)  # version
        file_len = struct.unpack("<I", f.read(4))[0]
        if sig == b"CWS":
            data = sig + b"\x00" + struct.pack("<I", file_len) + zlib.decompress(f.read())
        elif sig == b"FWS":
            data = sig + b"\x00" + struct.pack("<I", file_len) + f.read()
        else:
            return None
    return data

def parse_rect(data, offset):
    """Skip the RECT structure."""
    nbits = (data[offset] >> 3) & 0x1F
    total_bits = 5 + nbits * 4
    return offset + (total_bits + 7) // 8

def extract_tags(data):
    """Yield (tag_type, tag_data) from SWF data."""
    # Skip header: signature(3) + version(1) + length(4)
    offset = 8
    # Skip RECT
    offset = parse_rect(data, offset)
    # Skip frame rate (2 bytes) and frame count (2 bytes)
    offset += 4

    while offset < len(data):
        if offset + 2 > len(data):
            break
        tag_code_and_length = struct.unpack("<H", data[offset:offset+2])[0]
        offset += 2
        tag_type = tag_code_and_length >> 6
        tag_length = tag_code_and_length & 0x3F
        if tag_length == 0x3F:
            if offset + 4 > len(data):
                break
            tag_length = struct.unpack("<I", data[offset:offset+4])[0]
            offset += 4
        if offset + tag_length > len(data):
            break
        tag_data = data[offset:offset+tag_length]
        offset += tag_length
        yield tag_type, tag_data
        if tag_type == 0:  # End tag
            break

def extract_images(swf_path, prefix):
    data = read_swf(swf_path)
    if data is None:
        print(f"  Skipping {swf_path}: not a valid SWF")
        return 0

    # DefineBits (tag 6) images share the SWF-wide JPEGTables (tag 8) which
    # holds DQT/DHT tables the tag-6 payload lacks. Collect it first.
    jpeg_tables = b""
    for tag_type, tag_data in extract_tags(data):
        if tag_type == 8 and len(tag_data) >= 2:
            jpeg_tables = tag_data
            break

    count = 0
    for tag_type, tag_data in extract_tags(data):
        # Tag types for images:
        # 6 = DefineBits (JPEG, needs JPEGTables merged)
        # 8 = JPEGTables
        # 21 = DefineBitsJPEG2 (self-contained JPEG)
        # 35 = DefineBitsJPEG3 (JPEG with alpha mask)
        # 20 = DefineBitsLossless (PNG-like)
        # 36 = DefineBitsLossless2 (PNG-like with alpha)

        if tag_type in (6, 21, 35):
            if len(tag_data) < 4:
                continue
            char_id = struct.unpack("<H", tag_data[:2])[0]
            if tag_type == 6:
                img_data = tag_data[2:]
            elif tag_type == 21:
                img_data = tag_data[2:]
            elif tag_type == 35:
                alpha_offset = struct.unpack("<I", tag_data[2:6])[0]
                img_data = tag_data[6:6+alpha_offset]

            # SWF JPEG data may have erroneous headers - clean them
            # Remove SWF-specific JPEG markers (FF D9 FF D8)
            img_data = img_data.replace(b"\xff\xd9\xff\xd8", b"")

            # For DefineBits (tag 6), splice JPEGTables into the JPEG stream
            # between APP0 and SOF0. Tag 8 is structured as SOI+tables+EOI;
            # strip the SOI and trailing EOI before splicing.
            if tag_type == 6 and jpeg_tables:
                tables = jpeg_tables
                if tables.startswith(b"\xff\xd8"):
                    tables = tables[2:]
                if tables.endswith(b"\xff\xd9"):
                    tables = tables[:-2]
                # Find insertion point: after APP0 segment (FFE0 + length)
                if img_data[:4] == b"\xff\xd8\xff\xe0" and len(img_data) > 6:
                    app0_len = struct.unpack(">H", img_data[4:6])[0]
                    insert_at = 4 + app0_len
                    img_data = img_data[:insert_at] + tables + img_data[insert_at:]
                else:
                    # No APP0: just prepend tables right after SOI
                    img_data = img_data[:2] + tables + img_data[2:]

            # Check if it's actually a JPEG or PNG
            if img_data[:2] == b"\xff\xd8":
                ext = "jpg"
            elif img_data[:8] == b"\x89PNG\r\n\x1a\n":
                ext = "png"
            elif img_data[:3] == b"GIF":
                ext = "gif"
            else:
                # Try to find JPEG start
                idx = img_data.find(b"\xff\xd8")
                if idx >= 0:
                    img_data = img_data[idx:]
                    ext = "jpg"
                else:
                    continue

            if len(img_data) < 2000:
                continue

            count += 1
            outpath = os.path.join(OUT_DIR, f"{prefix}_{count:02d}.{ext}")
            with open(outpath, "wb") as f:
                f.write(img_data)

        elif tag_type in (20, 36):
            # Lossless bitmap - convert to raw then save as PNG
            if len(tag_data) < 8:
                continue
            char_id = struct.unpack("<H", tag_data[:2])[0]
            fmt = tag_data[2]
            width = struct.unpack("<H", tag_data[3:5])[0]
            height = struct.unpack("<H", tag_data[5:7])[0]

            # Skip small images
            if width < 50 or height < 50:
                continue

            # We'll skip lossless bitmaps for now as they need PIL to convert
            # The JPEG images are the main clinical photos
            pass

    return count

# Process all SWF files
swf_files = sorted([f for f in os.listdir(SWF_DIR) if f.endswith(".swf")])
total = 0
for swf_file in swf_files:
    prefix = os.path.splitext(swf_file)[0]
    path = os.path.join(SWF_DIR, swf_file)
    n = extract_images(path, prefix)
    if n > 0:
        print(f"  {swf_file}: extracted {n} images")
    total += n

print(f"\nTotal: {total} images extracted to {OUT_DIR}")
