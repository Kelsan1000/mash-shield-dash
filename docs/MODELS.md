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
* `int Skill1Level` — *Honorable Wall of Snowflakes* (Passive: Improves Guard damage mitigation).
* `int Skill2Level` — *Obscurant Wall of Chalk* (Passive: Widens the active Parry timing window).
* `int Skill3Level` — *Shield of Rousing Resolution* (Passive: Increases NP gauge gain rate).
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

## 2. `MashState.cs` (Active Run State — Finalized)
Lives in memory only during an active stage run and resets when the stage ends.

### Vitals & Power
* `int CurrentHp` — Active hit points during the current run.
* `int MaxHp` — Calculated from `PlayerProfile.Level` at stage start.
* `int Atk` — Calculated from `PlayerProfile.Level` at stage start.

### Noble Phantasm
* `float NpGauge` — Starts at `0f` by default (unless overridden by a special stage gimmick). Activating NP subtracts `100f`, allowing back-to-back uses if overcharged.
* `float MaxNpGauge` — Scales with `PlayerProfile.NpLevel` (`100f` at NP1, `200f` at NP2–4, `300f` at NP5).

### Run Loot & Rhythm Flow
* `int EarnedExp` — XP accumulated during the current stage (awarded in full even on defeat).
* `int EarnedQp` — QP accumulated during the current stage (awarded in full even on defeat).
* `int ComboStreak` — Tracks consecutive successful Parries, Shield Bashes, and Guard blocks without taking damage; resets to `0` on HP damage.

### Stance & Timers
* `MashStance CurrentStance` — Enum: `Running`, `Parrying`, `Guarding`, `Bashing`, `Staggered`, `NoblePhantasm`. *(No attack cooldowns—players can chain inputs freely for rhythmic combat.)*
* `float StanceTimer` — Counts down active duration for temporary stances (`Parrying`, `Bashing`, `Staggered`, or the brief `NoblePhantasm` cut-in pause) before returning to `Running`.
* `float InvulnerableTimer` — Mercy i-frames after taking a hit, plus brief invulnerability granted when triggering Noble Phantasm.
* `float NpBuffTimer` — Counts down the active ATK Up and DEF Up buff duration granted after unleashing Noble Phantasm.

---

## 3. Upcoming Models (To Be Defined)
* `EnemyEntity.cs` — Enemy types, HP, damage, stun state, and attack telegraphs.
* `StageConfig.cs` — World/stage metadata, wave spawn timelines, and boss encounters.
