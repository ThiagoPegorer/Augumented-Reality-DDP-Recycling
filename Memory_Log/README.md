# Memory Log

Durable, version-controlled snapshots of what earlier Cowork sessions learned, so that a new chat —
or a human reading the repo six months from now — can be brought up to speed without replaying
57 conversations.

## ⚠ Read this before treating anything here as current

**The Claude desktop app's project memory is the source of truth. This folder is a snapshot of it.**

Project memory lives inside the desktop app, not on disk, so it cannot be git-tracked directly. That
means everything here is a hand-made export and **will go stale the moment a session writes a new
memory.** Check the `Last rebuilt:` line at the top of each document. If it predates the work you are
asking about, read project memory instead and then regenerate the file.

Two sources of truth is a hazard, not a feature. This folder exists for three things project memory
cannot do — survive outside the app, be diffed and blamed in git, and be read by a human — and for
nothing else.

## What is here

| File | What it answers |
|---|---|
| `RBv2_1_1_ELEMENT_INVENTORY.md` | *What actually exists in the frozen prototype?* Screens, tabs, model behaviours, gestures, data payload, derived figures, scope boundaries, version lineage |

## How to regenerate

Ask Claude, in a session in this project:

> Rebuild the Memory Log from current project memory. Read every memory file, not just the ones in
> the index, then rewrite `Memory_Log/RBv2_1_1_ELEMENT_INVENTORY.md` and update its `Last rebuilt:`
> date. Trace every claim to a named memory file and mark anything unverified.

## Why this folder was created

On 2026-08-13, a session did not know the prototype had an exploded view — even though a memory file
called `exploded_zone.md` had described it in detail since July.

The cause was not missing memory. It was **an index that listed 5 of 56 files**, so every new session
booted blind to the other 51. `MEMORY.md` was rebuilt on 2026-08-14 to cover all of them.

The lesson generalises: **retrieval fails long before storage does.** If a future session seems not to
know something, check the index before writing the note again.

## Conventions

- Every claim traces to a named memory file, given in a `source:` line under each section.
- Facts that were never verified on device are marked `[unverified]`.
- Numbers that feed the thesis name their own source file, per the zero-trust rule in
  `working_agreements.md`.
- Nothing in this folder is a substitute for `VCU_BOM_v4.xlsx` (materials truth) or
  `LCA_Analysis/Outputs/` (impact truth).
