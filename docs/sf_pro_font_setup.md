# SF Pro font setup (TextMeshPro)

The DPP UI spec (`DPP_UI_Specs/00 §3`) calls for **SF Pro** — `SF Pro Display` (≥18 px) and `SF Pro Text` (<18 px), weights 400 + 700. The UI builder (`DPP → Build Phase 1 — Main Page`) auto-detects TMP font assets in `Assets/Fonts/` and falls back to LiberationSans if none exist, so you can do this before or after building.

## 1. Get the font files (Windows)

1. Download **SF Pro** from Apple: https://developer.apple.com/fonts/ → "SF Pro" (`SF-Pro.dmg`).
2. A `.dmg` doesn't open natively on Windows — extract it with **7-Zip**: right-click `SF-Pro.dmg` → 7-Zip → Open archive, then open the inner `SFProFonts...pkg` → `Payload~` until you see the `.otf` files.
3. You need at least these two:
   - `SF-Pro-Display-Regular.otf`
   - `SF-Pro-Display-Bold.otf`
   (Optionally also `SF-Pro-Text-Regular.otf` / `SF-Pro-Text-Bold.otf` for small sizes.)

> License note: Apple's license permits using SF Pro in software UI for Apple-platform-adjacent design work; for a public GitHub repo, **don't commit the .otf files** — add `Assets/Fonts/*.otf` to `.gitignore` and commit only the generated TMP `SDF.asset` files, or keep fonts local.

## 2. Import into Unity

1. Create the folder `Assets/Fonts` and drag the `.otf` files in.
2. `Window → TextMeshPro → Font Asset Creator`:
   - **Source Font File:** `SF-Pro-Display-Regular`
   - **Sampling Point Size:** Auto
   - **Atlas Resolution:** 1024 × 1024
   - **Character Set:** Extended ASCII
   - **Render Mode:** SDFAA
   - Click **Generate Font Atlas** → **Save** as `Assets/Fonts/SF Pro Display SDF.asset`
3. Repeat for the Bold file → save as `Assets/Fonts/SF Pro Display Bold SDF.asset`.

Naming matters only in one way: the builder treats any TMP font asset in `Assets/Fonts/` whose filename contains **"Bold"** as the bold face, and the first one without it as the regular face.

## 3. Apply

- If you import fonts **before** building: nothing else to do — the builder picks them up.
- If you already built the UI with LiberationSans: just run `DPP → Build Phase 1 — Main Page` again (it rebuilds the canvas cleanly with the new fonts).

## 4. Special characters

The middle dot `·` in "Guided recycling · 5 steps" is part of Extended ASCII, so it is covered. The `›` chevron on the Disassembly card is drawn with vector bars (not a glyph), so it works with any font.
