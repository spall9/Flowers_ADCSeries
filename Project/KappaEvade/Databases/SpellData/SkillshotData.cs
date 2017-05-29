namespace Project_Team.KappaEvade.Databases.SpellData
{
    using EloBuddy;

    using System;
    using System.Linq;

    public class SkillshotData
    {
        public Type type;
        public Type Explodetype;
        public SpellSlot[] Slots;
        public GameType GameType;
        public string[] CasterNames;
        public string[] SpellNames;
        public string[] MissileNames;
        public string[] ParticleNames;
        public string DisplayName;
        public string DontHaveBuff;
        public string RemoveOnBuffLose;
        public string TargetName;
        public string StartParticalName;
        public string MidParticalName;
        public string EndParticalName;
        public string ParticalObjectName;
        public RequireBuff[] RequireBuffs;
        public int CollideCount = 0;
        public int DangerLevel = 1;
        public float ExplodeWidth = 0;
        public float MoveSpeedScaleMod;
        public float MissileAccel;
        public float MissileMaxSpeed;
        public float MissileMinSpeed;
        public float ExtraDuration = 0;
        public float RingRadius = 0;
        public float Range = 0;
        public float ExtraRange = 0;
        public float Angle = 0;
        public float Width = 0;
        public float Speed = int.MaxValue;
        public float CastDelay = 0;
        public bool IsDangerous;
        public bool IsAutoAttack;
        public bool IsMoving;
        public bool EndIsBuffHolderPosition;
        public bool DecaySpeedWithLessRange;
        public bool EndSticksToTarget;
        public bool DetectAsOne;
        public bool AddEndExplode;
        public bool DontRemoveWithMissile;
        public bool AllowDuplicates;
        public bool HasExplodingEnd;
        public bool CollideExplode;
        public bool StaticStart;
        public bool StaticEnd;
        public bool EndSticksToMissile;
        public bool RangeScaleWithMoveSpeed;
        public bool DontCross;
        public bool DontAddExtraDuration;
        public bool TakeClosestPath;
        public bool EndStickToDirection;
        public bool EndIsCasterDirection;
        public bool IsFixedRange;
        public bool DetectByMissile;
        public bool EndSticksToCaster;
        public bool SticksToCaster;
        public bool SticksToMissile;
        public bool FastEvade;
        public bool StartsFromTarget;
        public Collision[] Collisions;
        public SkillshotData OnDeleteAdd;

        public bool IsCasterName(string name)
        {
            return CasterNames != null && CasterNames.Any(s => s.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsCasterName(Champion name)
        {
            return CasterNames != null && CasterNames.Any(s => s.Equals(name.ToString(), StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsSlot(SpellSlot slot)
        {
            return Slots != null && Slots.Any(s => s.Equals(slot));
        }

        public bool IsSlot(SpellSlot? slot)
        {
            if (slot == null)
                return false;

            return IsSlot((SpellSlot)slot);
        }

        public bool IsDisplayName(string name)
        {
            return !string.IsNullOrEmpty(DisplayName) && DisplayName.Equals(name, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsSpellName(string name)
        {
            return SpellNames != null && SpellNames.Any(s => s.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsMissileName(string name)
        {
            return MissileNames != null && MissileNames.Any(s => s.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsParticleName(string name)
        {
            return ParticleNames != null && ParticleNames.Any(x => name.StartsWith(x, StringComparison.CurrentCultureIgnoreCase)) && name.EndsWith(".troy");
        }

        public bool HasBuff(Obj_AI_Base caster)
        {
            if (caster == null)
                return false;

            if (!string.IsNullOrEmpty(DontHaveBuff))
            {
                if (caster.HasBuff(DontHaveBuff))
                    return false;
            }

            if (RequireBuffs == null)
                return true;

            return RequireBuffs.Any(b => !string.IsNullOrEmpty(b.Name) && caster.GetBuffCount(b.Name) >= b.Count);
        }

        public string MenuItemName => $"{(CasterNames != null ? CasterNames[0] : "")} {(Slots != null ? Slots.All(s => s.Equals(SpellSlot.Unknown)) ? "Special" : Slots[0].ToString() : "")} ({(!string.IsNullOrEmpty(DisplayName) ? DisplayName : SpellNames != null ? SpellNames[0] : MissileNames != null ? MissileNames[0] : ParticleNames != null ? ParticleNames[0] : "")})";
        public bool HasRange => (Range < int.MaxValue && Range < float.MaxValue && Range > 0) && Range != 25000f;
        public bool HasWidth => Width < int.MaxValue && Width < float.MaxValue && Width > 0;
        public bool HasAngle => Angle < int.MaxValue && Angle < float.MaxValue && Angle > 0;
        public bool HasExtraRange => ExtraRange < int.MaxValue && ExtraRange < float.MaxValue && ExtraRange > 0;
        public bool HasRingRadius => RingRadius < int.MaxValue && RingRadius < float.MaxValue && RingRadius > 0;
        public bool CanCollide => CollideCount > 0 && CollideCount < int.MaxValue;
    }

    public class RequireBuff
    {
        public RequireBuff(string name, int count)
        {
            Name = name;
            Count = count;
        }
        public string Name;
        public int Count;
    }

    public enum Type
    {
        LineMissile,
        CircleMissile,
        Cone,
        Arc,
        Ring
    }

    public enum Collision
    {
        YasuoWall,
        Minions,
        Heros,
        Walls,
        Caster
    }
}
