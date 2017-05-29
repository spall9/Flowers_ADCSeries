namespace Project_Team.KappaEvade.Databases.Items
{
    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    using System.Linq;

    public enum ItemCastType
    {
        Active, Targeted, Position
    }

    public enum TargetingType
    {
        All, AllAllies, AllyHeros, AllyMinions, AllEnemies, EnemyHeros, EnemyMinions, MyHero
    }

    public enum CastTime
    {
        AoE, MyHealth, EnemyHealth, AllyHealth, PostAttack, PreAttck, OnTick, Cleanse
    }

    public class ItemData
    {
        public ItemData(ItemId id, ItemCastType type, float range, float speed, float castDelay, params TargetingType[] targets)
        {
            item = new Item(id, range);
            CastDelay = castDelay;
            Speed = speed;
            CastType = type;
            TargetType = targets;
        }
        public ItemData(int id, ItemCastType type, float range, float speed, float castDelay, params TargetingType[] targets)
        {
            item = new Item(id, range);
            CastDelay = castDelay;
            Speed = speed;
            CastType = type;
            TargetType = targets;
        }

        public Item item;
        public string Name => item.ItemInfo.Name;
        public float Range => item.Range;
        public float Width = 0;
        public float Radius => Width / 2f;
        public float CastDelay;
        public float Speed;
        public int MyHP, AllyHP, EnemyHP, AoeHit;
        public ItemCastType CastType;
        public TargetingType[] TargetType;
        public CastTime[] CastTimes;
        public Orbwalker.ActiveModes[] OrbModes;
        public bool Ready => item.IsReady() && item.IsOwned(Player.Instance);

        public void Cast()
        {
            if (Ready && CastType.Equals(ItemCastType.Active))
            {
                item.Cast();
            }
        }

        public void Cast(Obj_AI_Base target)
        {
            if(!Ready || !target.IsValidTarget(Range + Radius))
                return;

            switch (CastType)
            {
                case ItemCastType.Active:
                    {
                        item.Cast();
                    }
                    break;
                case ItemCastType.Targeted:
                    {
                        item.Cast(target);
                    }
                    break;
                case ItemCastType.Position:
                    {
                        var travelTime = (int)((Player.Instance.Distance(target) / Speed * 1000f) + CastDelay);
                        var castpos = Prediction.Position.PredictUnitPosition(target, travelTime);

                        if(castpos.Distance(Player.Instance) <= Range)
                            Cast(castpos.To3D());
                        else if(castpos.Distance(Player.Instance) <= Range + Radius)
                            Cast(Player.Instance.ServerPosition.Extend(castpos, Range + Radius).To3DWorld());
                    }
                    break;
            }
        }

        public void Cast(Vector3 target)
        {
            if (Ready && IsInRange(target) && target.IsValid())
            {
                item.Cast(target);
            }
        }

        public Vector3 GetPrediction(Obj_AI_Base target)
        {
            var data = new Prediction.Position.PredictionData(
                Prediction.Position.PredictionData.PredictionType.Circular,
                (int)Range,
                (int)Radius,
                0,
                (int)CastDelay,
                (int)Speed,
                int.MaxValue);
            return Prediction.Position.GetPrediction(target, data).CastPosition;
        }

        public bool IsInRange(Obj_AI_Base target)
        {
            return target != null && IsInRange(target.Position, target.BoundingRadius/2);
        }

        public bool IsInRange(AttackableUnit target)
        {
            return target != null && IsInRange(target.Position, target.BoundingRadius / 2);
        }

        public bool IsInRange(Vector3 target, float hitbox = 0)
        {
            return Player.Instance.IsInRange(target, Range + Radius + hitbox);
        }

        public bool ShouldCast(AttackableUnit target/*, params CastTime[] timeCasted*/)
        {
            if (!Ready || !target.IsValidTarget() || !IsInRange(target))
                return false;

            return TargetTypeMatch(target);// && this.matchCastType(timeCasted);
        }

        public bool TargetTypeMatch(AttackableUnit target)
        {
            var targettype =
                target is AIHeroClient ? target.IsEnemy ? TargetingType.EnemyHeros : TargetingType.AllyHeros
                : target is Obj_AI_Minion ? target.IsEnemy ? TargetingType.EnemyMinions : TargetingType.AllyMinions
                : TargetingType.All;

            var allCheck = target.IsEnemy ? TargetingType.AllEnemies : TargetingType.AllAllies;
            return MatchType(TargetingType.All, allCheck, targettype);
        }

        private bool MatchType(params TargetingType[] types)
        {
            return TargetType == null || types.Any(TargetType.Contains);
        }

        public bool MatchCastType(params CastTime[] timeCasted)
        {
            return CastTimes == null || timeCasted.Any(CastTimes.Contains);
        }

        public bool ModeIsActive => OrbModes == null || Orbwalker.ModeIsActive(OrbModes);
    }
}