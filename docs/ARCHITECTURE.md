# Project Architecture

## 1. Tech Stack & Platforms
* **Engine & Language:** Godot 4 (.NET / C#)
* **Development Note:** Designed and coded with AI assistance.
* **Target Platforms:** Built **mobile-first** as a standalone app (Desktop `.exe`, Android/iOS), with Godot's built-in **Web (HTML5) export** for browser play.
* **Dependency Policy:** Zero third-party NuGet or asset packages by default. Uses only built-in Godot APIs and standard .NET `System` libraries under the **MIT License**.

## 2. Folder Structure (.NET MVC-Style)
To keep code clean and familiar, the C# codebase follows strict separation of concerns:

* **`Models/`** — Pure C# data classes (POCOs) with zero business logic.
  * `PlayerProfile.cs` *(Persistent save state)*
  * `MashState.cs` *(Active in-stage run state)*
  * `EnemyEntity.cs` *(Enemy and projectile data)*
  * `StageConfig.cs` *(Stage wave definitions)*
* **`Services/`** — Reusable business logic, calculations, and infrastructure plumbing (all suffixed with `Service`).
  * `CombatService.cs` *(Calculates parry windows, guard mitigation, shield bash damage, and NP gain)*
  * `StageService.cs` *(Manages wave timelines and boss encounters)*
  * `ProgressionService.cs` *(Calculates FGO ATK/HP from Level 1–80, handles XP leveling, and unlocks story milestones)*
  * `StorageService.cs` *(Handles JSON serialization, autosaving, and save resets)*
  * `AudioService.cs` *(Manages BGM, SFX, and retro text-blip voice channels)*
* **`Scenes/` & `Scripts/`** — Godot `.tscn` scenes (Views) paired with C# scripts (Controllers) that handle user input, frame ticks, and UI updates.
* **`docs/`** — Design and technical documentation (`ARCHITECTURE.md`, `MODELS.md`, `GAMEPLAY.md`).

## 3. Save & Persistence System
* **Format:** Single JSON file (`save_profile.json`) serialized via `System.Text.Json`.
* **Autosave Triggers:** Automatically saves after completing or failing a stage, upgrading a skill, changing audio settings, or toggling/unlocking a costume. Never writes to disk during active 60 FPS gameplay.
* **Hybrid Save Path (`StorageService.cs`):**
  * **Desktop (`.exe`):** Saves to a local `saves/save_profile.json` folder inside the game's root executable directory (`OS.GetExecutablePath().GetBaseDir()`) for easy access and backup.
  * **Mobile & Web (HTML5):** Automatically falls back to Godot's sandboxed `user://save_profile.json` path (IndexedDB on web, internal app storage on mobile) to comply with OS security rules.
* **New Game / Reset Save:** Overwrites `save_profile.json` with a fresh `new PlayerProfile()` instance while preserving existing audio volume/mute settings.

## 4. Audio & Asset Strategy
* **Copyright Safety:** Uses zero ripped audio or sprites from official Type-Moon / Aniplex releases.
* **Music & SFX:** Original retro chiptune BGM (via BeepBox) and synth sound effects (via jsfxr), or permissively licensed (CC0) assets.
* **Dialogue & NP Callouts:** Classic retro "text-blip" synth chirps with slight pitch variation, routed through the `VoiceVolume` channel.
