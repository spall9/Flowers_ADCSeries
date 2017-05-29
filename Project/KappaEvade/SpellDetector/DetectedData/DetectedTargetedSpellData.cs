namespace Project_Team.KappaEvade.SpellDetector.DetectedData
{
    using Databases.SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    public class DetectedTargetedSpellData
    {
        public AIHeroClient Caster;
        public Obj_AI_Base Target;
        public MissileClient Missile;
        public Vector3 Start;
        public TargetedSpellData Data;
        private float? _speed;
        public float Speed
        {
            get { return _speed ?? Data.Speed; }
            set { _speed = value; }
        }
        public float CastDelay => Missile != null && Data.Speed > 0 && Speed < int.MaxValue ? 0 : Data.CastDelay;
        public float MaxTravelTime => Start.Distance(Target.ServerPosition) / Speed * 1000 + CastDelay;
        public float StartTick = Core.GameTickCount;
        public float EndTick => StartTick + MaxTravelTime;
        public float TicksLeft => EndTick - Core.GameTickCount;
        public float TicksPassed => Core.GameTickCount - StartTick;
        public bool Ended => TicksLeft <= 0;

        public float GetSpellDamage(Obj_AI_Base target)
        {
            return !target.IsValidTarget() ? 0 : Caster.GetSpellDamage(target, Data.slot);
        }

        public bool WillHit(Obj_AI_Base target)
        {
            if (target == null)
                return false;

            if (Data.WindWall && Prediction.Position.Collision.GetYasuoWallCollision(Missile?.Position ?? Caster.ServerPosition, target.ServerPosition).IsValid())
            {
                return false;
            }

            return Target.IdEquals(target);
        }
    }
}
