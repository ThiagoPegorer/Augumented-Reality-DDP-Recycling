# Memory Log

Durable, version-controlled snapshots of what earlier Cowork sessions learned, so that a new chat —
or a human reading this repo six months from now — can be brought up to speed without replaying
sixty conversations.

## ⚠ Read this before treating anything here as current

**The Claude desktop app's project memory is the source of truth. This folder is a snapshot of it.**

Project memory lives inside the desktop app, not on disk, so it cannot be git-tracked directly.
Everything here is a hand-made export and **goes stale the moment a session writes a new memory.**
Check the `Last rebuilt:` line at the top of each document. If it predates the work you are asking
about, read project memory instead, then regenerate.

Two sources of truth is a hazard, not a feature. This folder exists for three things project memory
cannot do — survive outside the app, be diffed and blamed in git, and be read by a human — and for
nothing else.

## Contents

| Path | What it holds |
|---|---|
| `RBv2_1_1_ELEMENT_INVENTORY.md` | *What actually exists in the frozen prototype?* Screens, tabs, model behaviours, gestures, data payload, derived figures, scope boundaries, version lineage |
| `memory/` | Mirror of the project-memory knowledge files, grown one session at a time |
| `memory/MEMORY.md` | The index. **Read this first** — it lists every memory file that exists |

## 🔁 Maintenance rule — this folder rots if the rule is skipped

**Nothing syncs this folder automatically.** At every session end, after project memory is written
and before the Notion session row is called done:

1. Export the current `MEMORY.md` to `memory/MEMORY.md`.
2. Export **every knowledge memory file created or modified that session** to `memory/<name>.md`.
   Usually one to three files, so the mirror converges toward completeness over time.
   *Knowledge* files carry facts, decisions and findings. **Skip operational files** — the session
   logging routine, the Notion workspace map, the git workflow — they are meaningless outside the
   app and churn every session.
3. If the **prototype** changed (it should not under the RBv2.1.1 freeze), refresh
   `RBv2_1_1_ELEMENT_INVENTORY.md` and bump its `Last rebuilt:` date.
4. Hand Thiago the git command. **Never run git** — that is his, always.

⚠ **This rule also lives in the `session_logging_routine.md` memory file, but a memory file is read
at a session's discretion.** The only place a rule is injected into *every* session unconditionally
is the Cowork **project instructions**. Until this line is added there, the rule is likely, not
guaranteed:

> At session end, after writing project memory, also export `MEMORY.md` plus every knowledge memory
> file changed that session into `Memory_Log/memory/` in the repo, and give Thiago the git command.

## Regenerating the inventory from scratch

Ask, in a session in this project:

> Rebuild the Memory Log from current project memory. Read every memory file, not just the ones in
> the index, then rewrite `Memory_Log/RBv2_1_1_ELEMENT_INVENTORY.md` and update its `Last rebuilt:`
> date. Trace every claim to a named memory file and mark anything unverified.

## Why this folder exists

On 2026-08-13 a session did not know the prototype had an exploded view — even though a memory file
called `exploded_zone.md` had described it in detail since July.

The cause was not missing memory. It was **an index listing 5 of 56 files**, so every new session
booted blind to the other 51. `MEMORY.md` was rebuilt on 2026-08-14 to cover all of them.

The lesson generalises: **retrieval fails long before storage does.** If a future session seems not
to know something, check the index before writing the note again.

## Conventions

- Every claim traces to a named memory file, given in a `source:` line under each section.
- Facts never verified on device are marked `[unverified]`.
- Numbers that feed the thesis name their own source file, per the zero-trust rule in
  `working_agreements.md`.
- Nothing here substitutes for `VCU_BOM_v4.xlsx` (materials truth) or `LCA_Analysis/Outputs/`
  (impact truth).
