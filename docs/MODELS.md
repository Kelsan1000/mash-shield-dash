# Data Models (`Models/`)

All models are pure C# data classes (POCOs) with zero business logic or Godot node dependencies.

---

## 1. `PlayerProfile.cs` (Persistent Save State — Finalized)
Serialized to JSON (`save_profile.json`) via `System.Text.Json`. Powers the Title Screen, Stage Select, Status/Upgrade menus, and Settings.

### Progression & Currency
* `int Level` — Starts at `1`, caps at `80` (matches Mash's Part 1 max level). Uses a non-grindy progression curve where clearing stages naturally advances her toward Level 80 without heavy farming.
* `int CurrentExp` — Tracks XP progress toward the next level. Extra XP earned at Level 80 converts into bonus QP.
* `long Qp` — Currency earned from stage runs, spent on skill upgrades and stretch-goal costumes.
* *(Note: `Atk` and `Hp` are **not** stored in the save file. They are calculated dynamically from `Level` using Mash's FGO stat scaling—from ~1,261 ATK / 1,854 HP at Lv. 1 up to ~8,730 ATK / 12,877 HP at Lv. 80—so save files never desync from balance tweaks.)*

### Combat Kit
* `int Skill1Level` — *Honorable Wall of Snowflakes* (Improves Guard damage mitigation).
* `int Skill2Level` — *Obscurant Wall of Chalk* (Widens the active Parry timing window).
* `int Skill3Level` — *Shield of Rousing Resolution* (Increases NP gauge gain rate).
* `int NpLevel` — Starts at `1`, caps at `5`. Locked to story progress (automatically upgrades at key Singularities, evolving from *Lord Chaldeas* to *Lord Camelot*).
* *(Note: Craft Essences are intentionally excluded to keep menus and balance clean.)*

### Appearance & Costumes
* `int AscensionStage` — Tracks `0` through `4` as Mash levels up and advances through the story.
* `string EquippedCostume` — Currently active outfit ID (defaults to `"Default"`).
* `List<string> UnlockedCostumes` — Stores unlocked outfits for toggling (supports a QP Costume Shop as a stretch goal).

### Story Progress (Part 1: Observer on Timeless Temple)
* `int HighestUnlockedWorld` — Tracks `1` through `9` (`1` = Singularity F: Fuyuki, `2` = Orleans, ..., `9` = Final Singularity: Solomon).
* `int HighestUnlockedStage` — Tracks the latest unlocked stage within the current World (each Singularity is a World with multiple stages ending in a story-important boss).

### Audio & Text-Blip Settings
* `float BgmVolume` — `0.0f` to `1.0f` (defaults to `0.8f`).
* `float SfxVolume` — `0.0f` to `1.0f` (defaults to `0.8f`).
* `float VoiceVolume` — `0.0f` to `1.0f` (defaults to `0.8f`; controls retro synth "text-blip" speech chirps during dialogue and NP callouts).
* `bool IsMuted` — Master mute toggle (defaults to `false`). Preserved when resetting game progress.

---

## 2. `MashState.cs` (Active Run State — In Progress)
Lives in memory only during an active stage run and resets when the stage ends.

* **Vitals & Gauge:** `int CurrentHp`, `int MaxHp`, `float NpGauge` (`0f` to `100f`).
* **Combat Power:** `int Atk` (calculated from `PlayerProfile.Level` at stage start).
* **Current Stance (`MashStance` enum):** `Running`, `Parrying`, `Guarding`, `Bashing`, `Staggered`, `NoblePhantasm`.
* **Timers:** `float StanceTimer` (active Parry or Shield Bash duration) and `float BashCooldownTimer` (prevents spamming Shield Bash).

---

## 3. Upcoming Models (To Be Defined)
* `EnemyEntity.cs` — Enemy types, HP, damage, stun state, and attack telegraphs.
* `StageConfig.cs` — World/stage metadata, wave spawn timelines, and boss encounters.
