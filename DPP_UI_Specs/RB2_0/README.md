# RB2_0 — the as-built spec set (FROZEN)

> **Status: frozen 2026-08-01.** These 19 specs describe **ReBuilt v2.0** exactly as it was
> built, device-tested, and run with participants **P02 (Waldek)** and **P03 (Domenik)**.
> Nothing in this folder changes again. Revisions live in `../RB2_1/`.

---

## Why this folder is frozen

A living spec is right while a version is being built and wrong the moment it has been tested
with participants: the thesis has to be able to say *"this is what P02 and P03 actually saw"*,
and that sentence cannot be true of a file that keeps moving. So RB2_0 stops here.

The freeze is dated **2026-08-01**, after the last RB2.0 change (binary ✗/✓ task status,
spec 04 v3.2) and before Domenik's usability feedback opens RB2.1.

## What is in here

| Spec | Screen |
|---|---|
| `00_design_standards.md` | Shared standards — sizes, colour tokens, type, glyph limits, hover rules |
| `01`–`02` | Main page · Information tab |
| `03`–`08` | Disassembly intro · steps 1–5 |
| `09`–`10` | Completion summary · exploded action zone |
| `11`–`12` | QR entry · Welcome + first-run |
| `13`, `13b`–`13e` | DPP Canva + its four detail pages (Information model, Usage Profile, Compliance & Safety, Service & repair) |
| `14` | Composition & impact (Digital Model Exploration) |

`VERSIONS.md` deliberately does **not** live here — a registry of every prototype version cannot
sit inside one version's folder. It was lifted to `DPP_UI_Specs/VERSIONS.md`.

## Known imperfections, left as they are

- **Relative mock links are stale.** Specs reference `drafts/13_v16_service_1x2.svg`; the mocks
  are at `../drafts/…` from here. Broken since the mid-session folder reorganisation, not since
  the move. Left untouched so the frozen files stay byte-identical to what was tested against.
- Several specs carry `Status: coded, awaiting device check` — accurate as of the freeze.

*Frozen 2026-08-01 · Successor: `../RB2_1/` · Registry: `../VERSIONS.md`*
