# 04e — Rail restructure: Training removed · Certificates as TAB 3 · gated teardown CTA

**Status:** v1.1 · 2026-08-08 · mock `drafts/04e_certificates_tab_v2.svg` approved + implemented;
**round 2 same day (device feedback):** Certificates & safety became a FULL FOURTH TAB — same
184×68 size as the others, sequential (reachable only after Environmental impact), red identity
painted by the view, its page in `tabPages[3]`. The 44-high special entry (§1 below, struck) and
the separate `_certOpen/_certVisited` machinery are gone; the gate is simply "all four tabs
visited". Round-2 changes bundled here: ① model picks are INERT outside Product specs (Component
ID) and Usage (usage record) — no more tab-yanking; ② Usage part record lost its duplicated Back
pill (the page's bottom Back already closes the record first); ③ Environmental's primary reads
"Next" again (it is mid-chain now — `PrimaryLabel` is "Next" on every page; the certificates
page has Back only). `ShowCertificates`/`CloseCertificates` survive as wrappers for stale
serialized events. The CertificatesPage CONTENT stays exactly as built.

## 1. What changed and why

Training disassembly is REMOVED from the rail and from `tabPages` ("to be honest, it will be
repetitive with the next steps" — Thiago). Its interim page (the "Guided teardown" stub that
carried the Continue button since 2026-08-07) is gone with it. The teardown's single route in
is now a **gated CTA at the rail bottom**.

Rail order (top → bottom, `SpTabTop 34`, tabs 68 h at 80 pitch):

| y | Entry | Style |
|---|---|---|
| 34 / 114 / 194 | Product specifications · Usage & service · Environmental impact | standard tabs, visited ticks (recycler) |
| 276 | **Certificates & safety** | unchanged red regulatory entry (00 §2.1 meaning 4), moved from above the tabs into the old Training slot; opens the existing CertificatesPage |
| 356 | **RailCta** (184 × 40 pill) | role-dependent, painted by the view |

`SuperPanelView.TabCount` 4 → **3**.

## 2. The gate (recycler)

`GateOpen` = all three data tabs visited **and** the certificates page opened at least once.
Opening counts; nothing stricter — scroll depth or dwell time is unverifiable and punishes
participants. Certificates & safety is therefore de-facto mandatory without joining the
Back/Next walkthrough chain (it stays a reference page, reachable any time).

CTA states: grey `#2b3a52` + dim label while locked; green `#27C46C` + dark bold label when
open → `router.ShowDisassembly()`. A locked press is never silent: the pill's label swaps to
the missing item ("Read Certificates & safety first" / "Visit every tab first") for 1.8 s,
then repaints. Trap 1 respected: every state colour goes through
`HoverHighlight.SetRestFillColor`.

## 3. Product user

Same slot, same pill, different contract: label "Back", navy fill, always enabled →
`router.ShowStakeholder()`. No gate — a product user never enters the teardown, and their
tabs were never sequential to begin with.

## 4. Page-grammar consequence

The Environmental page is now the LAST data tab, so its primary button no longer reads
"Continue to disassembly": `PrimaryLabel` there is **"Certificates & safety"** and
`NextTab()` on the last tab calls `ShowCertificates()` — the chain Specs → Usage → Env →
Certificates stays unbroken, and the teardown is reachable ONLY through the rail gate.

## 5. Files touched (2026-08-08)

- `Assets/Scripts/DDP/UI/SuperPanelView.cs` — TabCount 3; `_certVisited`; `GateOpen`;
  `OnRailCta` + `CtaHint` + `PaintRailCta`; NextTab/PrimaryLabel retarget; SelectTab3 gone.
- `Assets/Editor/DPPUIBuilder.SuperPanel.cs` — 3-entry tab arrays; `SpTabTop 34` /
  `SpCertY 276` / `SpCtaY 356`; TrainingPage build + goBtn wiring removed; RailCta built +
  wired (`OnRailCta`, refs `railCtaButton/Fill/Label`).
- `Assets/Editor/DPPUIBuilder.UsePhase.cs` / `DPPUIBuilder.EnvImpact.cs` — tabPages merges
  drop slot 3; /3 now also re-finds EnvironmentalPage into slot 2.
- `Assets/Editor/DPPUIBuilder.Verify.cs` — SuperPanelView rows for the three railCta refs.

## 6. Editor chain

`RBv2_1_1/1 → RBv2_1/9 → RBv2_1_1/2 → RBv2_1_1/3 → RBv2_1_1/4 → Verify wiring → SAVE`.
/1 must re-run for the new rail; its full rig rebuild also guarantees no orphaned
TrainingPage survives (trap 4). Never re-run `RBv2_0/4` or `/5` while the stage clone exists.

## 7. Retirement notes

- `ic_training` icon and `ic_lca_arrow.png` are now unused assets — delete in the next
  cleanup pass (never via a `_to_delete/` inside `Assets/`).
- RBv2_0 teardown screens (disassemblyIntro / stepFlow / summary) are UNCHANGED and still
  the CTA's destination via `ScreenRouter.ShowDisassembly`.
