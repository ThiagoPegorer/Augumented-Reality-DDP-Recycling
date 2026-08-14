---
name: github-repo-setup
description: "GitHub repo Augumented-Reality-DDP-Recycling. ⚠ Repo root is C:\\Claude\\Projects\\AR_DPP (verified 2026-08-14), NOT XR\\AR_DPP_VCU. Gitignore contents, the never-`git add .` rule, and the .gitattributes TODO."
type: project
---

**Repository:** https://github.com/ThiagoPegorer/Augumented-Reality-DDP-Recycling.git
(note Thiago's spelling: "Augumented" + "DDP", not "Augmented"/"DPP")

## ⚠ REPO ROOT CORRECTED 2026-08-14

**The repo root is `C:\Claude\Projects\AR_DPP`.** Verified directly: `.git/config` there carries the
origin URL above, `.git/HEAD` reads `ref: refs/heads/main`, and **`XR\AR_DPP_VCU` has no `.git` of
its own**. Memory previously said the root was `XR\AR_DPP_VCU`; that was wrong and cost a
wrong-path assumption.

Top-level contents of the repo root: `.gitignore` · `backend/` · `CAD_Specs/` · `Docs/` ·
`DPP_UI_Specs/` · `LCA_Analysis/` · `Memory_Log/` (added 2026-08-14) · `XR/`.

The **Unity project** is `XR\AR_DPP_VCU\` — it holds `Assets/`, `Packages/`, `ProjectSettings/`,
`Library/`, `Logs/`, the generated `.csproj`/`.slnx` files and its own `README.md`.

## Initial commit landed 2026-05-26

2,656 objects, ~99 MiB, branch `main` tracking `origin/main`. Message: "Initial commit: Unity 6 LTS
AR-DPP project + FastAPI backend + DPP schema".

**Excluded by `.gitignore` (verified before first push):** `Library/`, `Temp/`, `Logs/`,
`UserSettings/`, `*_BurstDebugInformation_DoNotShip/`, all `*.csproj`/`*.sln`/`*.slnx`, `test.apk`
(62 MB build artifact), `user.keystore`, `.vscode/`, Python `.venv/` + `__pycache__/`, OS noise.

**Size note:** the ~99 MiB initial pack is heavy mainly due to DOTween + PICO XR DLLs, Input System
samples and TextMesh Pro assets. Well under GitHub's 1 GB recommendation. If the repo grows when CAD
`.fbx`/`.glb` exports land, consider Git LFS for binaries.

## Push workflow — DO NOT `git add .` blindly

```powershell
cd "C:\Claude\Projects\AR_DPP"
git status                                   # check what actually changed
git add <your paths>                         # stage YOUR files by path
git status                                   # confirm the staged list
git commit -m "<message>"
git push
```
No `-u origin main` needed — upstream is set.

**Why path-by-path:** re-opening the Unity project re-touches vendor SDK files (DOTween,
`Packages/PICO Unity Integration SDK-3.4.0-.../`) with CRLF↔LF line-ending churn. The diffs show
every line flipped but the content is identical. Committing that noise bloats history, turns vendor
upgrades into merge hell, and hides the real diff.

⚠ **`git status` times out over the device bridge (>38 s)** and a killed `git status` leaves a stale
`.git/index.lock`. **Run all git in PowerShell.** If the lock reappears: `Get-Process git`, then
delete the lock.

**Cleaner long-term fix (TODO, not urgent):** a `.gitattributes` at the repo root normalising line
endings for the SDK packages, then `git add --renormalize .` once plus a commit.

```
XR/AR_DPP_VCU/Packages/** text=auto eol=lf
XR/AR_DPP_VCU/Assets/Plugins/Demigiant/** text=auto eol=lf
```

**Never in the repo:** Bosch PDFs, participant data, copyrighted literature, CAD originals.

**How to apply:** Treat this URL as the canonical remote. Before recommending a `git pull`, verify
the local branch is still `main` tracking `origin/main`.

Related: [[repo-architecture]], [[git-workflow]], [[thesis-identity]], [[working-agreements]]
