namespace Project_Team.KappaEvade.SpellDetector.DetectedData
{
    using Databases.SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    public class DetectedEmpoweredAttackData
    {
        public AIHeroClient Caster;
        public Obj_AI_Base Target;
        public MissileClient Missile;
        public EmpoweredAttackData Data;
        public Vector3 Start;
        public float AttackCastDelay;
        public float Speed;
        public float MaxTravelTime => Start.Distance(Target) / Speed * 1000f + AttackCastDelay;
        public float StartTick = Core.GameTickCount;
        public float EndTick => StartTick + MaxTravelTime;
        public float TicksLeft => EndTick - Core.GameTickCount;
        public float TicksPassed => StartTick - Core.GameTickCount;
        public bool Ended => TicksLeft <= 0 || TicksPassed > AttackCastDelay + Game.Ping && Caster.IsMelee;

        public float GetSpellDamage(Obj_AI_Base target)
        {
            return !target.IsValidTarget() ? 0 : Caster.GetAutoAttackDamage(target, true);
        }

        public bool WillHit(Obj_AI_Base target)
        {
            if (target == null)
                return false;

            if (Caster.IsRanged && Prediction.Position.Collision.GetYasuoWallCollision(Caster.ServerPosition, target.ServerPosition).IsValid())
            {
                return false;
            }

            return Target.IdEquals(target);
        }
    }
}
