"""
Regenerate schema/dpp_schema.json from the Pydantic models.

WHY THIS EXISTS: until v0.6 the JSON Schema was hand-maintained alongside
models.py and DPPModels.cs — three copies of the same shape, kept in sync by
memory. Two of them have to stay hand-written (Python and C#), but the JSON
Schema does not: Pydantic can emit it.

Run from the backend/ folder, with the venv active:

    python export_schema.py

Prints what it wrote. Commit the regenerated file with the model change.
"""
import json
import pathlib

from models import DPP

OUT = pathlib.Path(__file__).resolve().parent.parent / "schema" / "dpp_schema.json"


def main() -> None:
    schema = DPP.model_json_schema()
    schema["$schema"] = "https://json-schema.org/draft/2020-12/schema"
    schema["title"] = "Digital Product Passport (VCU)"
    schema["description"] = (
        "Generated from backend/models.py — DO NOT EDIT BY HAND. "
        "Run `python export_schema.py` after changing the models. "
        "Attribute coverage against CIRPASS D2.2 Table 6 (pp.41-42) is "
        "documented in DPP_UI_Specs/13b_information_model.md."
    )

    OUT.parent.mkdir(parents=True, exist_ok=True)
    OUT.write_text(json.dumps(schema, indent=2, ensure_ascii=False) + "\n", encoding="utf-8")

    defs = schema.get("$defs", {})
    print(f"wrote {OUT}")
    print(f"  {len(defs)} definitions, {len(schema.get('properties', {}))} top-level properties")


if __name__ == "__main__":
    main()
