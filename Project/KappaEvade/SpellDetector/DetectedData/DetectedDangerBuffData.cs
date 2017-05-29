namespace Project_Team.KappaEvade.SpellDetector.DetectedData
{
    using Databases.SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    public class DetectedDangerBuffData
    {
        public AIHeroClient Caster;
        public Obj_AI_Base Target;
        public DangerBuffData Data;
        public BuffInstance Buff;

        public int StackCount => Data.StackCountFromMenu?.Invoke() ?? Data.StackCount;
        public float StartTick = Core.GameTickCount;

        public float EndTick
        {
            get
            {
                if (Data.Delay > 0 && Data.Delay < int.MaxValue)
                {
                    return StartTick + Data.Delay;
                }
                if (Buff != null)
                {
                    return Buff.EndTime * 1000f;
                }
                return StartTick + 2000f;
            }
        }
        public float TicksLeft => EndTick - Core.GameTickCount;
        public bool Ended => TicksLeft <= 0 || (Caster != null && Caster.IsDead) || (Target != null && Target.IsDead);

        public float GetSpellDamage(Obj_AI_Base target)
        {
            return !target.IsValidTarget() ? 0 : Caster.GetSpellDamage(target, Data.Slot);
        }

        public bool WillHit(Obj_AI_Base target)
        {
            if (target == null)
                return false;

            if (Data.HasStackCount && StackCount > Buff.Count)
                return false;

            if (Data.IsRanged && !Data.RequireCast)
            {
                return Caster != null && target.IsInRange(Prediction.Position.PredictUnitPosition(Caster, (int)TicksLeft), Data.Range);
            }

            return Target != null && Target.IdEquals(target);
        }
    }
}
