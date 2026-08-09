# DPP UI Spec — RB2.1 / 04: DPP page — **v2, the super panel**

> **Living spec.** Supersedes RB2.0 `13` (DPP Canva), `13b`–`13e` and `14` (Composition & impact),
> **and supersedes its own v1** (the 640 × 430 four-tile page built and device-tested 2026-08-04/05).
> Standards: `00_design_standards_rbv2.md` · Prev: `03` · Children: `04a` (use phase), `04b`–`04d`
> Mocks: `../drafts/04b_v2_super_panel.svg` (front), `../drafts/04b_v3_plan_view.svg` (geometry)
> Builder: `RBv2_1/8` · View: `DppPageView.cs`
> **Status: BUILT and device-validated (rounds through 2026-08-08). ⚠ Every §4–§6 mention of
> the Training tab, the certificates badge/entry, or the tab-4 CTA is SUPERSEDED by
> `04e_rail_gate.md` v1.1 — read §0 below first.**

---

## 0. 2026-08-08 delta — 04e rail restructure (BUILT · canonical: `04e_rail_gate.md` v1.1)

- **Training disassembly is GONE** (repetitive with the guided flow that follows).
  `TabCount = 4`: Product specifications · Usage & service · Environmental impact ·
  **Certificates & safety**.
- **Certificates & safety is tab 3** — full 184 × 68 tab, sequential like the others
  (reachable only after Environmental impact), red regulatory identity: opaque fill, bright
  red stroke when reachable, dark red `#5c1622` when dimmed. The fill is never alpha-faded on
  this tab — the red under-rect bleeds through a faded fill (device, 2026-08-08). Its page is
  `tabPages[3]`; the RBv2_1/8 flat-canvas screen of the SAME NAME still exists (retirement
  list) — page lookups must be `data.Find`, never a global find.
- **The teardown route is the rail-bottom CTA** (184 × 40, y 356): recycler reads
  "Continue to disassembly", grey + inert until ALL FOUR tabs are visited, then green →
  `ShowDisassembly`; a locked press swaps the label to "Visit every tab first" for 1.8 s.
  Product user reads "Back" → stakeholder decision. Every page primary reads **"Next"**;
  the certificates page has Back only.
- **Model picks are INERT outside Product specs (Component ID) and Usage (record)** — no
  tab-yanking (`ModelLinkController.HandlePick`; also spec 05 v1.2).
- Rail geometry: tabs from y 34 at 80 pitch (band ends 342); CTA 356–396. The pre-04e
  44-high cert entry above the tabs is retired with `SpCertEntry`/`SpLockButton`.

## 1. Why v1 is being replaced

v1 answered "the model is hidden" by spawning the model first and unfolding the panel from it. The model
was still **beside** the passport, and the panel still owned the attention — which is the complaint P02
and P03 actually made.

v2 answers it structurally: **the model is the middle of the passport.** It sits between the navigation
and the data, permanently on screen, and it re-reads itself every time the user changes tab. There is no
state in which the passport is visible and the model is not.

Second driver: **a recycler should traverse the whole passport before dismantling.** v1 made that
possible; v2 makes it the only path (§6).

**Carried over from v1 unchanged:** the chip standard (`00` §5), the four tab identities, the role logic
(`ScreenRouter.Mode`), the certificates screen, and the two hard-won rules — *no overlay shares a canvas
plane with live controls*, and *every panel screen goes in `ScreenRouter.Show()`'s deactivate pass*.

**Superseded:** the 2 × 2 tile grid, the 24–314 / 326–616 columns, the header layout, the `cx 114 / 422`
button row, and the per-tab `+` child screens.

## 2. Footprint, placement and inclination

**980 × 430**, replacing the single 640 × 430 footprint for this screen. `00` §1's "one footprint,
always" was written when every screen was a flat panel of the same job; it is updated here rather than
excepted. Thiago, 2026-08-06: *"we are not breaking standards, we are doing interactions that substitute
them over time — nothing is inflexible."* **`00` §1 carries two footprints, and this screen is the only
holder of the second.**

### 2.1 The panel is not flat — three canvases, toed in

A flat 980-wide panel is 0.98 m across at world scale 0.001, and its outer thirds are read at a slant.
[Geometry, not opinion] at 0.75 m the outer edges sit **±33°** off the forward axis, and a flat surface
there is foreshortened to **cos 33° = 0.84** of its width. Text compresses; legibility drops exactly
where the layout needs it most.

**Each zone is its own world-space canvas, yawed about its vertical axis to face the eye.** The
compression disappears — this is a readability change, not a stylistic one.

| Canvas | Panel-local x | Width | Centre offset | **Yaw** |
|---|---|---|---|---|
| Rail | 0 – 220 | 220 | −0.380 m | **−26.9°** |
| Stage | 220 – 560 | 340 | −0.100 m | **0°** — see §2.4 |
| Data | 560 – 980 | 420 | +0.280 m | **+20.5°** |

All three parent to one rig. **The rig moves as a single object**; the yaw values are local and fixed.

**The stage is deliberately NOT yawed.** Freeing the model from a yawed stage would introduce an
orientation jump at the moment of release. At 0° there is none, and the stage carries a mesh rather than
text, so it gains nothing from facing the eye.

### 2.2 Placement

**Default 0.75 m**, replacing 0.60 m for this screen. Eye height 1.1176 m and yaw-only facing unchanged.

| Distance | Outer edges | Trade |
|---|---|---|
| 0.60 m | ±39° | head turns to reach the rail and the data panel |
| **0.75 m** | **±33°** | **adopted** |
| 0.88 m | ±29° | comfortable, but the text gets small |

The assembly is centred on the user's forward axis, which puts the stage — and therefore the model —
about **8° left of centre**. Imperceptible, and preferable to pushing the wider data panel further out.

⚠ **Device-verify 0.75 m before content is built.** It is the assumption everything else rests on.

### 2.3 One rig, one grab bar

**Locked, the whole assembly is one object.** The three canvases and the model all parent to a single
rig; **one standard grabber bar (200 × 22, `00` §5) sits centred beneath the stage** and drags
everything. Thiago, 2026-08-06: *"when the digital model is LOCKED, all the 3 canvas be just as 1, with
a single drag bar."*

The grab point is under the stage — the user reaches for the object, not for a geometric centre — while
the rig's **pivot** stays at the assembly centre so billboard-on-drag rotates sensibly. The 100 mm
offset between the two is imperceptible mid-drag.

**Unlocked, the model is re-parented out of the stage** into its own root with its own grab bar. The rig
keeps its grabber and keeps behaving identically, now moving two canvases and an empty stage. No special
casing: one child left the hierarchy.

Two grabbers are then live at once. They are spatially separated (one under the freed model's frame, one
under the empty stage) and **the freed model's grabber is tinted `teal/light`** while the rig's stays
neutral, so which-moves-what needs no experiment.

`00` §6 already covers the interaction: gesture 3 is pinch-drag a panel, and the guard *"panel drag beats
model gestures"* is what stops the freed model's grab bar fighting the two-hand rotate that is also live
on it. `PanelGrabHandle` is reused on both bars.

### 2.4 What three canvases buy

Beyond legibility: the stage stops being a transparent hole in a canvas and becomes **empty space between
two canvases**. Nothing can bleed through it, there is no raycast target to suppress, and each surface
gets its own `GraphicRaycaster` — which `00` §4 already requires for independently-oriented UI groups.
**The inclination made the implementation simpler, not harder.**

## 3. Anatomy (panel-local coordinates within each zone)

### 3.1 Rail — navigation only, no headline

| Element | x | y | w | h |
|---|---|---|---|---|
| Tab button ×4 | 18 | 34 · 114 · 194 · 274 | 184 | 68 |
| Compliance badge | 18 | 372 | 184 | 30 |
| Divider | 219 | 18 | 1 | 394 |

Tab button: icon 24 at x 38, two-line label 12.5 at x 72. **Active** = `tab/active-fill` +
`tab/active-stroke` + a 4 × 40 `teal/light` accent bar on the left edge. **Visited** = a 14 px
`teal/accent` tick, top-right. **Not yet reached** = dimmed (§6).

**No title on the rail.** The screen names itself through the active tab and the data panel's header.

**The compliance badge lives here**, not in a header: it is a property of the product, not of a tab, and
the 420-wide data header has no room beside a 19 pt title. Treatment unchanged — `00` §2.1 meaning 4,
red outline and glyph only, never fill.

### 3.2 Stage — the model, and its two states

The model sits at the stage centre. **It re-reads on every tab change** — the lens idea from `04a` §7.2,
applied one level up:

| Active tab | Model shows |
|---|---|
| Product specifications | real-life part colours, materials legible |
| Usage history | end-of-use verdict tint (default) — see `04a` |
| Environmental impact | material tint + recovery-value glow |
| Training disassembly | exploded preview of the five steps |

**LOCKED (default).** Slow yaw, ~12 s loop, no input accepted. A living illustration, which is what stops
it reading as decoration.

**UNLOCKED.** The lock button (40 visual / 52 hit, stage-local cx 170, cy 372) releases the model into a
free **340 × 300** object with its own grab bar and the standard two-hand rotate/zoom (`00` §6). The
stage keeps a **ghost outline** of the model's home plus `return here to re-lock`.

⚠ **The layout does not reflow when the model leaves.** `00` §5: hit targets never move under the user.

⚠ **Re-lock SNAPS the model home; it is not carried back.** Once freed the model no longer follows the
rig, so the user can drift it across the room, or move the panels and leave it behind. The lock button
returns it to the stage with a short DOTween ease from wherever it ended up. Without this, `return here
to re-lock` is a chore and the feature becomes a trap.

⚠ **Containment is mandatory, because code cannot fix the alternative.** `00` §4: *a 3D mesh always wins
the depth test against world-space UI.* A freed model dragged behind or across the rail or data canvas
will occlude it with no z-order remedy. **The freed model is constrained to a volume in front of the
stage**, bounded laterally by the inward faces of the two side canvases and forward toward the user
(see the plan-view mock). It is **not** auto-returned on tab change — the user unlocked it deliberately;
the re-lock button stays visible in the stage.

### 3.3 Data panel

| Element | x | y | w | h |
|---|---|---|---|---|
| Title (active tab name) | 24 | baseline 46 | — | 19 bold |
| Caption, right-aligned | 396 | baseline 44 | — | 10 `text/tip` |
| Rule | 24 | 64 | 372 | 1 |
| Content band | 24 | **76 → 350** | 372 | — |
| Left button | 24 | 362 | **130** | 46 |
| Primary CTA | 166 | 362 | **230** | 46 |

⚠ The standard 180 + 388 button row does not fit a 420 column. **130 + 230, primary still right** —
`00` §5's rule holds; only the widths change, on this screen only.

## 4. The four tabs

Content per tab lives in its own spec: `04a` use phase (written), `04b` training disassembly,
`04c` product specifications, `04d` environmental impact.

**v2 has no `+` and no child screens for the tabs.** Selecting a tab swaps the data panel and re-reads
the model. `04a`–`04d` are therefore **content specs, not screen specs**. The only sibling screen that
remains is **Certificates & safety**, reached from the rail badge.

## 5. Roles

`ScreenRouter.Mode`, set on `03`, read in `OnEnable`.

| | **Product user** | **Recycler** |
|---|---|---|
| Entry tab | any — all four available immediately | **Product specifications**, forced (§6) |
| Tab access | all lit, free navigation | linear walkthrough (§6) |
| Left button | **`Quit`, red** → `WelcomeController.ShowWelcome()` | `Back` (§6) |
| Primary CTA | `Scan next product` → `QRScanController.BeginNewScan()` | `Next`, then `Continue to disassembly` on tab 4 |

The v1 header back arrow is gone — there is no header. The Product user's one-step-back edge is `Quit`.

## 6. The Recycler walkthrough — **Recycler only**

Replaces v1's locked-CTA idea. Thiago, 2026-08-06: a lock refuses; a path leads.

1. Selecting **Recycler** on `03` opens the DPP page on **Product specifications**.
2. Tabs not yet reached render **dimmed** and are not selectable.
3. The primary CTA reads **`Next`** and advances to the following tab.
4. On reaching **Training disassembly** (tab 4), the CTA becomes **`Continue to disassembly`**.
5. **Visited tabs stay lit and selectable**, so the recycler can go back and re-read at any point.
6. `Back` steps to the previous tab. On tab 1 it returns to the stakeholder screen (`03`).

`Back` / `Next` therefore form a pair that walks the passport, and the disassembly route is reachable
only from the tab that explains it.

⚠ **This is not the gate deleted on 2026-08-01.** That was a `CONTINUE TO DISASSEMBLY?` interstitial
asking *"are you sure?"* — a confirmation, and one more panel to read. This is a route through content
that already exists, adding no panel and refusing nothing.

**The Product user is not walked.** They have no disassembly to reach, so all four tabs are lit from the
start and the CTA is always `Scan next product`.

## 7. Open items

1. **Device-verify 0.75 m and the three yaw angles** before content is built.
2. `00` edits: second footprint in §1, placement + inclination in §1, the 130 + 230 button row in §5.
3. Rail surface `#081733` — add a `navy/rail` token to `00` §2, or drop it and separate the zones with
   the divider alone.
4. ~~Stage yaw~~ — **settled 2026-08-06: 0°** (§2.1), so releasing the model causes no orientation jump.
5. ~~Grabber bar arrangement~~ — **settled 2026-08-06: one rig bar beneath the stage** (§2.3).
6. What the model shows for **Product specifications** and **Environmental impact** is named here but
   specified nowhere yet (`04c`, `04d`).
7. Snap-home tween duration and easing for re-lock and for the automatic re-lock on tab change (§3.2).

## 8. Iteration log

- **2026-08-04** — v1 designed across mocks `04_v1` → `04_v11`: 2 × 2 tile grid, chip standard,
  compliance badge promoted to a button with its own screen.
- **2026-08-05** — v1 built and device-tested. Three defects fixed (TMP chip collapse, missing icons,
  modal surface); certificates promoted from modal to screen after it fired the CTA underneath it;
  Training disassembly restored for both roles; `Quit` red.
- **2026-08-06 (a)** — **v2**: three-zone super panel, driven by the persistent complaint that the model
  was hidden and the panel owned the attention. Mock `04b_v2`.
- **2026-08-06 (b)** — inclination adopted (§2.1) after the flat 980 panel was shown to lose 16 % of its
  width to foreshortening at the edges; placement 0.60 → 0.75 m; one-canvas-with-a-hole replaced by
  three toed-in canvases; the locked CTA replaced by the Recycler walkthrough (§6); freed-model
  containment made mandatory (§3.2). Plan-view mock `04b_v3`.
- **2026-08-06 (c)** — one rig, one grab bar (§2.3): locked, the three canvases and the model are a
  single draggable object; unlocked, the model re-parents out with its own bar while the rig keeps
  moving the rest. Stage yaw fixed at 0° and re-lock defined as a snap home, both consequences of that.
  Changing tab re-locks the model automatically, so a freed model can never show a stale tint.

*Created 2026-08-04 · v2 2026-08-06 · Status: specified, not built · Children: `04a`–`04d`*
