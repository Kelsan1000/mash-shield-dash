## 3. EnemyEntity.cs (Active Enemy & Boss State — Finalized)

Lives in memory for each active enemy during a stage run. Supports single enemies and multi-boss encounters (`n` bosses) with independent states and gauges.

### Identity & Archetypes
- `string Id` & `string Name` — Unique enemy identifier and display name.
- `EnemyArchetype Archetype` — Enum:
  - `Standard` — Basic melee fodder; defeated with a Shield Bash or countered with Parry/Guard.
  - `Heavy` — Armored/charging unit; blockable with Guard (with elevated `GuardChipMultiplier`), resists frontal Bashes until staggered by a Parry.
  - `Ranged` — Fires a projectile on approach (requiring Guard or Parry), then behaves identically to `Standard` at melee range.
  - `Boss` — Moves alongside Mash with multi-hit combos, telegraphed Command Card patterns, RNG skills, and a continuous NP gauge.
- `int FormationSlot` — Lane/slot index (`0`, `1`, `2`, ...) for positioning multiple simultaneous bosses and stacking UI bars.

### Vitals, Damage & Loot
- `int CurrentHp` & `int MaxHp` — Active and maximum hit points.
- `int Atk` — Base damage dealt on hit.
- `float GuardChipMultiplier` — Multiplier applied when Mash blocks (`1.0f` default; elevated for `Heavy` units).
- `int ExpReward` & `int QpReward` — XP and QP added to `MashState` upon defeat.

### Boss Noble Phantasm
- `float NpGauge` & `float MaxNpGauge` — Continuous boss NP meter (`0f` to `100f`; `0f` for non-bosses so UI hides NP bars on regular enemies). Immediately triggers NP at `100f` when not staggered; fill rate slows down while `Staggered`.
- `string NpAnimationId` — Cut-in/animation key triggered when `NpGauge` reaches `100f`.

### Runtime State, Stagger & Pattern Recognition
- `EnemyState CurrentState` — Enum: `Approaching`, `Waiting`, `Telegraphing`, `Attacking`, `CastingSkill`, `Recovering`, `Staggered`, `Defeated`.
- `float StateTimer` — Tracks remaining duration in the current state.
- `bool HasFiredProjectile` — Tracks whether a `Ranged` unit has loosed its approach projectile.
- `string CurrentAttackPatternId` — Active attack pattern key driving wind-up animation and parry rhythm.
- `CommandCardType CurrentAttackCard` — Enum: `None`, `Buster`, `Arts`, `Quick` (visual telegraph cue; `Arts` boosts boss NP gain, `Buster` boosts damage, `Quick` executes multi-hit strings).
- `float CurrentAttackNpGain` — Base NP gauge gain for the current move.
- `int StaggerHitsRequired` & `int CurrentStaggerHits` — Parries required to break posture on the current pattern (`0` for `Standard`/`Ranged`, `1` for `Heavy`, dynamic per pattern for `Boss`). Resets to `0` after `Staggered`, a `Recovering` counter-hit, or when Mash takes an unblocked hit.

### Boss RNG Skills & Multi-Boss Pacing
- `List<string> AvailableSkillIds` & `string ActiveSkillId` — Pool of signature boss skills and the currently telegraphed/active skill.
- `float AtkBuffMultiplier` & `float NpGainMultiplier` — Temporary self-buff multipliers granted by skills.
- `float SkillBuffTimer` & `float SkillCooldownTimer` — Timers controlling active buff duration and minimum time between skill rolls.
- `int ConsecutiveWaits` & `int MaxConsecutiveWaits` — Tracks consecutive `Waiting` rolls so bosses can pass turns to teammates without causing dead air when only one boss remains.
