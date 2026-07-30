# openLCA → EF impact factors — cheat-sheet

**Purpose:** pull an Environmental Footprint (EF) impact profile for each BOM material from ecoinvent, then paste the per-kg values into the Excel `EF_Library` (one column per impact category instead of only CO₂).
**Made:** 2026-07-01, from a live run on `market for aluminium, primary, ingot`.

---

## Before you start — two things to fix

1. **EF version.** The database `ecoinvent_38_apos_3011_with_methods` only has **EF 3.0**, not 3.1. For the final thesis, import a newer method pack first (you have `openLCA LCIA Methods 2.5.0` / `2.6.0` zips in your Ecoinvent package, folder `OpenLCA LCIA methods/`). Menu: **File ▸ Import ▸ … ▸ zip**, pick the pack, wait, then EF 3.1 appears in the method dropdown. EF 3.0 is fine for practising now.
2. **Single-source rule.** Every material factor — CO₂ included — must come from the *same* database + method. Do **not** mix ecoinvent EF numbers with the old IDEMAT numbers. (Example: primary aluminium is 9.84 kg CO₂-eq/kg in ecoinvent EF vs 11.75 in IDEMAT — same order, different method. Pick one.)

---

## The workflow (repeat per material)

1. **Open the database** — double-click `ecoinvent_38_apos_3011_with_methods` in the left Navigation panel. Wait until its name goes **bold** and the tree expands (Processes, Flows, …). First open takes 10–30 s.
2. **Search the material** — click the search box (top-right), type the material name (e.g. `aluminium, primary, ingot`), press **Enter**.
3. **Narrow the results** — in the `Filter` box type `market for` to get the clean market process (or leave it to pick a specific production process). Choose the region that fits an EU-built VCU (e.g. `IAI Area, EU27 & EFTA`).
4. **Open the process** — click the result (it opens the process editor).
5. **Direct calculation** — click the **Direct calculation** button → **Run calculation**. (This auto-builds the supply chain; no manual product-system needed for a simple factor.)
6. **Calculation properties** — set:
   - Allocation method → **As defined in processes**
   - Impact assessment method → **ei - EF v3.0** (or EF 3.1 once imported)
   - Calculation type → **Lazy / On-demand**
   - click **Finish**.
7. **Read the profile** — in the Result, open the **Impact analysis** tab. Target amount defaults to **1 kg**, so every row is a per-kg factor. Use **Export to Excel** to dump all categories at once.
8. **Paste** the vector into a new row of the extended `EF_Library` in `VCU_LCA_Model_v3.xlsx`.

---

## What the result looks like (1 kg primary aluminium, EF v3.0)

| EF category | Value | Unit |
|---|---|---|
| Climate change – GWP100 | 9.84 | kg CO₂-eq |
| **Resource use, minerals & metals (ADP)** | 1.91×10⁻⁵ | kg Sb-eq |
| Energy resources, non-renewable | 121.6 | MJ |
| Acidification | 0.0713 | mol H⁺-eq |
| Ecotoxicity, freshwater | 223.4 | CTUe |
| Eutrophication, freshwater | 0.0043 | kg PO₄-eq |
| Ozone depletion | 7.3×10⁻⁷ | kg CFC-11-eq |
| Photochemical ozone formation | 0.0318 | kg NMVOC-eq |

EF 3.0 splits climate change and toxicity into sub-indicators, so the table has ~25 rows. Your **headline** row for the thesis is *resource use, minerals & metals* — the metals story that justifies AR-guided disassembly.

---

## Material → ecoinvent search terms (starter — verify each in openLCA)

Search these, then pick the market / EU-region variant. Names are guides, not exact dataset IDs — confirm in the search results.

| BOM material | Search term in openLCA |
|---|---|
| Aluminium housing | `aluminium, primary, ingot` (+ casting: `aluminium, cast alloy`) |
| PCB substrate (FR-4) | `printed wiring board` / `glass fibre reinforced plastic, epoxy` |
| Copper (traces) | `copper, cathode` / `market for copper` |
| Solder (SnAgCu) | `tin` + `silver` (or a solder paste dataset if present) |
| Brass connectors | `brass` |
| Passives (ceramic) | `ceramic tile` / `barium sulfate`-type proxies — verify |
| Silicon / ICs | `integrated circuit` / `silicon, electronics grade` |
| Steel fasteners | `steel, chromium steel 18/8` / `market for steel` |
| Gold | `gold` |
| Silver | `silver` |
| Palladium | `palladium` |
| Nickel | `nickel, class 1` |

---

## For the recycling credit (avoided burden)

The **same primary-production factors double as your recycling credits.** Recovered metal displaces virgin production, so:

`EoL credit (per material) = − recovered_mass × recovery_rate × primary_production_factor`

So one openLCA lookup per material feeds **both** the S1 raw-material burden **and** the S5 avoided-burden credit. Keep the sign convention explicit in the sheet: S5 processing energy is **positive**, the material credit is **negative**, and the net is what you report.

---

## System-model note (confirm with supervisor)

This is the APOS database. Your Excel declares "Substitution (avoided burden)". If you want to keep controlling the credit manually (transparent for non-programmer supervisors), the **cutoff** database + manual avoided-burden is the cleanest match. Whichever you pick, use it for *all* stages.
