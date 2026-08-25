# 4.2 AR DPP Prototype (RBv2.1.1) — figure set and draft

**This output is PROSE.** Phrasing retained in the document is at risk of needing to be denoted
under the affidavit.

Every statement below was read off the frames themselves, at full resolution where a number is
quoted. Nothing is taken from memory or from the specification files.

---

## PART 1 — the figure set

You have **44 frames**, not 17. The section cannot carry 44. Eight in the body, four to an appendix.

### Body, eight figures

| # | file (timestamp) | what it shows | calls |
|---|---|---|---|
| 1 | `14.15.44` | welcome screen, "Welcome to ReBuilt", printed unit on the desk | Figure 14 |
| 2 | `14.17.07` | role selection, Product user and Recycler, hand selecting | Figure 14 |
| 3 | `14.18.12` | the three-canvas rig in the room, Product ID open | Figure 17 |
| 4 | `14.18.57` | Component ID, the eight passport parts listed | Figure 15 |
| 5 | `14.19.27` | Environmental impact, the EF 3.1 screening shares | Figure 15 |
| 6 | `14.23.55` | Certificates and safety, with the rail control green | Figure 15 |
| 7 | `14.26.11` | guided step 1, the printed unit being opened with the hex key | Figure 16 |
| 8 | `14.33.33` | summary, 7 min 30 s, report stored | Figure 16 |

### Appendix, four figures

`14.19.10` Usage and service, state of health · `14.22.27` component detail with drawing and material
masses · `14.20.00` model unlocked beside the printed unit with the gesture readout · `14.31.39`
step 3, the board lifted out with the printed components visible.

**Frame 5 is the one that earns the section its place.** It shows the application displaying
72.5 percent, 6.7 percent and 6.6 percent, the same screening shares reported earlier in this
chapter. That is the passport carrying the LCA, visible in a photograph.

---

## PART 2 — the draft

### 4.2 AR DPP Prototype (RBv2.1.1)

The implementation described in the previous chapter produced a running application, and this section
reports it as delivered in build RBv2.1.1. Every image below is a screen capture taken on the headset
during a demonstration run performed by the author on 25 August 2026. Passthrough is active in all of
them, so the printed unit and the virtual model appear in the same frame.

`[FIGURE: Welcome screen and the printed unit on the workbench | source: own elaboration]`

The application opens on a welcome screen headed Welcome to ReBuilt, with the subtitle Digital
Product Passport for guided dismantling. The routine drawn in Figure 14 continues with a role
selection screen offering Product user and Recycler, each carrying a one-line statement of what it
grants. The recycler role was selected for this run.

`[FIGURE: Role selection | source: own elaboration]`

`[FIGURE: The three world-space canvases as they appear in the room | source: own elaboration]`

The layout drawn in Figure 17 appears in the room as three panels. The navigation rail sits on the
left, the virtual model in the centre, and the record content on the right. The rail lists four
entries: Product specifications, Usage and service, Environmental impact, and Certificates and
safety.

Product specifications opens on Product ID. It displays the manufacturer Bosch Motorsport, the name
Vehicle Control Unit MS 50.4, the type F02U.V02.965-02, the serial VCU0001, the production date
2026-03-27, the origin DE, and the category EEE electronic control unit, WEEE category 5, small
equipment. The Component ID view lists the eight passport parts, from the two housing shells to the
four integrated circuit groups.

`[FIGURE: Component ID, the eight passport parts | source: own elaboration]`

`[FIGURE: Environmental impact, share of the weighted footprint | source: own elaboration]`

The Environmental impact view is headed Share of the weighted footprint, EF 3.1 screening, Sc1. It
displays minerals and metals at 72.5 percent, climate change at 6.7 percent, freshwater
eutrophication at 6.6 percent, and the remaining thirteen categories together at 14.3 percent. Those
are the screening shares reported earlier in this chapter, carried into the record the application
serves.

Certificates and safety lists a CE conformity marking declared under 2014/30/EU, two substances of
very high concern declared under REACH, the waste electricals category, and the IP67 ingress rating.
In the same frame the control at the foot of the rail reads Continue to disassembly and is green.

`[FIGURE: Certificates and safety, with the disassembly control open | source: own elaboration]`

`[FIGURE: Guided disassembly, step 1 of 5 | source: own elaboration]`

The guided sequence drawn in Figure 16 replaces the four record entries with a briefing, five steps
and a summary. The first step is headed Open the housing, is marked step 1 of 5, and names an Allen
key, hex 2.5 mm, as the tool. In the frame the printed unit is being opened with that key while the
step list stays on screen.

The run closes on a summary headed Nice work, unit fully dismantled. It reports an elapsed time of
7 min 30 s and a per-step table of times and recovered masses, and it carries the note that the
material splits are assumed and are to be validated in openLCA. A dialogue reports that the session
is stored. The elapsed time shown belongs to the demonstration run and is not a study result.

`[FIGURE: Summary of the guided run | source: own elaboration]`

Two features of the delivered build came from the first block of participant sessions, and both are
dated in the source code. P02 and P03 reported not perceiving the three-dimensional model, and the
panel was rebuilt so that the model sits between the navigation and the record content. P02 reported
missing sound when clicking and interacting on 2026-07-31, and the click, grab and drag sounds were
added on 2026-08-01.

A recorded run of the application is available at `[FILL: Google Drive URL]`.
`[DECIDE: one sentence saying what the recording shows.]`

Every session report the application writes carries a field named co2_avoided_kg. The value in that
field is the modelled avoided impact of 15.4315 kg CO2 eq held in the passport record, echoed back by
the application. No real unit was dismantled in any session, so that field is not reported as a
result anywhere in this chapter.

---

```
=== Check before using ===

Needs a number or fact from you:
  - The Google Drive URL, and one sentence on what the recording shows.

Verified this session, first-hand:
  - All quoted screen values read at full resolution from the frames: 72.5 / 6.7 / 6.6 / 14.3
    percent, 7 min 30 s, the Product ID fields, the tool string, the eight Component ID rows.
  - `data/reports/` now holds P01 to P09. **P06 to P09 are on disk** (24 Aug 20:19; 25 Aug 11:59,
    12:33, 12:46) with elapsed times 187, 235, 450 and 201 s. Block 2 is n = 4, not 5.
  - The five untagged 2026-08-09 verification runs have been removed from the folder.
  - The colours match. The rendered model and the printed unit both show a brown housing with green
    connectors, and the printed components are yellow, blue, red and brown. My earlier flag was
    wrong and has been corrected in memory.

⚠ Check before publishing the summary figure:
  - The frame carries the line "Report was successfully sent", and **no report file was written
    after 12:46 today**. Nothing in the backend data tree changed after 13:00. Either the
    demonstration run posted somewhere else, or that message is not backed by a stored file.
    [Guessing] I cannot tell which from here. If the message is not backed by a file, publishing
    the frame puts an unverified success claim in the thesis.

⚠ Two smaller things in the frames:
  - The summary headline uses an em dash. If you transcribe it, replace it.
  - The Certificates view names two substances by CAS number. That is model data and is fine to
    publish, but check it is what you intend to state about the reference product.

Possible contradiction:
  - None found between the frames and Chapter 3.
```
