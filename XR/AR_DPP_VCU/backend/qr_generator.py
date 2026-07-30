"""
QR code generator for DPP product IDs.

Generates a printable QR code that the Unity AR app on PICO 4 will scan.
The QR encodes a structured payload `dpp:<product_id>` so the Unity side
can recognize it as a DPP code (vs. random text) and extract the product_id.

Usage
-----
    python qr_generator.py vcu_001
    python qr_generator.py vcu_001 --size 12 --out qr/

The script writes a PNG to `qr/<product_id>.png` (relative to this file).

Why `dpp:<product_id>` and not the full URL?
--------------------------------------------
Encoding the bare product_id (with a `dpp:` scheme prefix) keeps the QR
backend-agnostic: Unity has the backend base URL configured at runtime
(localhost / LAN IP / ngrok). If the backend host changes, the QR code
on the physical VCU stays valid.
"""
from __future__ import annotations
import argparse
import sys
from pathlib import Path

import qrcode
from qrcode.constants import ERROR_CORRECT_M

QR_SCHEME = "dpp"  # custom URI scheme so Unity can validate "this is a DPP QR"


def build_qr_payload(product_id: str) -> str:
    """Return the string that gets encoded in the QR code."""
    return f"{QR_SCHEME}:{product_id}"


def generate_qr(
    product_id: str,
    out_dir: Path,
    box_size: int = 10,
    border: int = 4,
) -> Path:
    """
    Generate a QR code PNG for `product_id` in `out_dir`.

    Returns the path to the written PNG.
    """
    out_dir.mkdir(parents=True, exist_ok=True)
    payload = build_qr_payload(product_id)

    qr = qrcode.QRCode(
        version=None,                 # auto-fit
        error_correction=ERROR_CORRECT_M,  # ~15% recovery — robust for AR scanning
        box_size=box_size,
        border=border,
    )
    qr.add_data(payload)
    qr.make(fit=True)

    img = qr.make_image(fill_color="black", back_color="white")
    out_path = out_dir / f"{product_id}.png"
    img.save(out_path)
    return out_path


def main() -> int:
    parser = argparse.ArgumentParser(
        description="Generate a printable QR code for a DPP product_id."
    )
    parser.add_argument("product_id", help="e.g. vcu_001")
    parser.add_argument(
        "--out",
        default=str(Path(__file__).parent / "qr"),
        help="Output directory (default: ./qr/)",
    )
    parser.add_argument(
        "--size",
        type=int,
        default=10,
        help="Box size in pixels per QR module (default: 10)",
    )
    parser.add_argument(
        "--border",
        type=int,
        default=4,
        help="Quiet zone in modules (default: 4, the QR spec minimum)",
    )
    args = parser.parse_args()

    out_dir = Path(args.out)
    payload = build_qr_payload(args.product_id)
    out_path = generate_qr(args.product_id, out_dir, args.size, args.border)

    print(f"QR generated:")
    print(f"  product_id: {args.product_id}")
    print(f"  payload:    {payload}")
    print(f"  file:       {out_path}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
