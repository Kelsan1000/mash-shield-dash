using System.Collections.Generic;

namespace MashShieldDash.Models
{
    public enum EnemyArchetype
    {
        Standard,
        Heavy,
        Ranged,
        Boss
    }

    public enum EnemyState
    {
        Approaching,
        Waiting,
        Telegraphing,
        Attacking,
        CastingSkill,
        Recovering,
        Staggered,
        Defeated
    }

    public enum CommandCardType
    {
        None,
        Buster,
        Arts,
        Quick
    }

    public class EnemyEntity
    {
        // Identity & Archetype
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public EnemyArchetype Archetype { get; set; } = EnemyArchetype.Standard;
        public int FormationSlot { get; set; } = 0;

        // Vitals, Damage & Loot
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int Atk { get; set; }
        public float GuardChipMultiplier { get; set; } = 1.0f;
        public int ExpReward { get; set; }
        public int QpReward { get; set; }

        // Boss Noble Phantasm (UI hidden when MaxNpGauge <= 0f)
        public float NpGauge { get; set; } = 0f;
        public float MaxNpGauge { get; set; } = 0f;
        public string NpAnimationId { get; set; } = string.Empty;

        // Runtime State & Timers
        public EnemyState CurrentState { get; set; } = EnemyState.Approaching;
        public float StateTimer { get; set; } = 0f;
        public bool HasFiredProjectile { get; set; } = false;

        // Stagger & Pattern Recognition
        public string CurrentAttackPatternId { get; set; } = string.Empty;
        public CommandCardType CurrentAttackCard { get; set; } = CommandCardType.None;
        public float CurrentAttackNpGain { get; set; } = 0f;
        public int StaggerHitsRequired { get; set; } = 0;
        public int CurrentStaggerHits { get; set; } = 0;

        // Boss RNG Skills & Buffs
        public List<string> AvailableSkillIds { get; set; } = new();
        public string ActiveSkillId { get; set; } = string.Empty;
        public float AtkBuffMultiplier { get; set; } = 1.0f;
        public float NpGainMultiplier { get; set; } = 1.0f;
        public float SkillBuffTimer { get; set; } = 0f;
        public float SkillCooldownTimer { get; set; } = 0f;

        // Multi-Boss Turn Pacing
        public int ConsecutiveWaits { get; set; } = 0;
        public int MaxConsecutiveWaits { get; set; } = 2;
    }
}
