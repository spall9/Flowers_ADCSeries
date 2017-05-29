namespace Project_Team.KappaEvade.SpellDetector.DetectedData
{
    using Databases.SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    public class DetectedSpecialSpellData
    {
        public AIHeroClient Caster;
        public Obj_AI_Base Target;
        public GameObject Object;
        public Vector3 Position;
        public SpecialSpellData Data;
        public float StartTick;
        public float EndTick => StartTick + Data.CastDelay;
        public float TicksLeft => EndTick - Core.GameTickCount;
        public float TicksPassed => StartTick - Core.GameTickCount;
        public bool Ended => Core.GameTickCount - EndTick > 0;
        public bool IsEnemy => Caster != null && Caster.IsEnemy;

        public float GetSpellDamage(Obj_AI_Base target)
        {
            return !target.IsValidTarget() ? 0 : Caster.GetSpellDamage(target, Data.Slot);
        }

        public bool WillHit(Obj_AI_Base target)
        {
            return target.Distance(Position) <= Data.Range || Target != null && Target.IdEquals(target);
        }
    }
}
