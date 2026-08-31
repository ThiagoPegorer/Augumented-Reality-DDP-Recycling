# ReBuilt: an Augmented Reality Digital Product Passport for a Vehicle Control Unit

ReBuilt anchors the Digital Product Passport of a Vehicle Control Unit beside the physical unit on a
head-mounted display, and guides an operator through a five step disassembly on a 3D printed teardown
model. Built for the master's thesis *Digital Product Passport in Augmented Reality for Disassembly
and Recycling Analysis of a Vehicle Control Unit* (Thiago Maitan Pegorer, SRH University, 2026).

The build in this repository is **RBv2.1.1**, the frozen version described in the thesis.

## What you need

| | |
|---|---|
| Unity | **6000.0.73f1** (Unity 6). Other versions will try to upgrade the project |
| Headset | **PICO 4 Ultra**, in color passthrough |
| Backend | Python 3, any recent version, to run the passport server |

The **PICO Unity Integration SDK 3.4.0** is embedded in `Packages/` as a local package. Unity picks
it up automatically. Do not install it again from the Package Manager.

## Project structure

```
.
├── Assets/
│   ├── Scripts/DDP/         45 C# files: the application. Note the folder is DDP, not DPP
│   │   └── UI/              screen and panel controllers
│   ├── Scenes/MainScene.unity   the only scene
│   ├── Plugins/
│   │   ├── Demigiant/DOTween    panel and model animation, vendored, do not reinstall
│   │   └── zxing.unity.dll      QR decoding
│   ├── CAD model/           the teardown model meshes
│   ├── Audio/  Font/  Materials/  Textures/  Resources/  Settings/  XR/  XRI/
│   └── Editor/              editor tooling used during the build
├── Packages/                manifest, lock file, and the embedded PICO SDK
├── ProjectSettings/         XR, player, quality and URP settings
├── backend/                 FastAPI server that serves the passport
│   ├── main.py  models.py  qr_generator.py  export_schema.py
│   └── data/vcu_001.json    the passport record for the modelled unit
├── schema/                  dpp_schema.json, the generated record definition, and a sample
└── docs/                    architecture and setup notes
```

## Open the project

1. Open the folder in Unity **6000.0.73f1**. The first import takes several minutes while `Library/`
   is rebuilt.
2. Open `Assets/Scenes/MainScene.unity`. It is the only scene.
3. `.csproj` files and the solution regenerate themselves. They are not in this copy on purpose.

## Run the backend

```powershell
cd backend
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install -r requirements.txt
uvicorn main:app --reload
```

Check it:

- `http://localhost:8000/docs` interactive OpenAPI documentation
- `http://localhost:8000/dpp/vcu_001` the passport for the modelled unit

| Method | Path | Purpose |
|---|---|---|
| GET | `/` | health check |
| GET | `/dpp` | list known product ids |
| GET | `/dpp/{product_id}` | the passport, validated against the model in `models.py` before it is sent |
| POST | `/dpp/{product_id}/report` | the session summary the headset sends at the end of a run, written to `data/reports/` |

A record that does not match the definition is refused rather than sent incomplete.

## The two settings that change on every machine

**1. The backend address.** `DPPClient.baseUrl` defaults to `http://localhost:8000`, which only works
in the editor. The headset cannot reach your laptop's localhost. Set the field on the `DPPClient`
component in the Inspector to either:

- your laptop's LAN address, with the server started as `uvicorn main:app --host 0.0.0.0`, or
- an ngrok URL, `ngrok http 8000`, if the headset is on a different network.

**2. Android signing.** `ProjectSettings` refers to a keystore named `user.keystore` with the alias
`key1`. **That keystore is deliberately not included.** To build an APK, either point Unity at your
own keystore under Project Settings, Player, Publishing Settings, or create a new one. The project
opens and runs in the editor without it.

## How a session runs

A QR code on the printed unit encodes `dpp:<product_id>`. The scanner ignores any code without that
prefix, so an unrelated code in the room cannot start a session. Once a valid code is read, the
application requests that record from the server and opens the passport: four tabs, Product
specifications, Usage and service, Environmental impact, and Certificates and safety, each of which
must be visited before the guided disassembly unlocks. The disassembly runs five steps against the
3D printed model, and the summary is posted back to the server.

All interaction is hand tracking. Pointing highlights, a pinch selects, a held pinch drags, and two
pinched hands rotate or resize the model depending on the distance between them.

## Scope, so the code is not read for more than it is

- The passport is a **model, not a compliance instrument**. Most values are assumptions or generic
  data, since the author is not the producer of the device.
- One record, one JSON file, no database. Data architecture is out of scope.
- **There is no voice layer and no artificial intelligence assistant.** Guidance is delivered through
  Augmented Reality alone.
- The physical object is a **3D printed teardown model**, larger than the reference unit and without
  its glued joints, conformal coating or fastener behavior. It is not a replica.
- The environmental figures come from a cradle-to-grave life cycle assessment of the reference unit,
  modelled in openLCA. They are not measured from the prototype.

## Reference unit

Bosch Motorsport Vehicle Control Unit MS 50.4, used as the reference product for the bill of
materials, the passport record and the life cycle model.
