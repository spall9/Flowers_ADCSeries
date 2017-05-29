namespace Project_Team.KappaEvade.SpellDetector.DetectedData
{
    using ClipperLib;

    using Databases.SpellData;
    using Detectors;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    using SharpDX;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Type = Databases.SpellData.Type;

    public class DetectedSkillshotData
    {
        public void Update()
        {
            CheckCollision();

            var exPolygon = generatePolygon(Player.Instance.BoundingRadius + 35);

            EvadePolygon = exPolygon;
            OriginalPolygon = generatePolygon2();
            DrawingPolygon = generatePolygon();
        }

        private void CheckCollision()
        {
            if (Data.Collisions == null)
                return;

            CollidePoint = null;
            CorrectCollidePoint = null;

            var check = new CollisionResult(this);
            CorrectCollidePoint = check.CorrectCollidePoint;
            CollidePoint = check.CollidePoint;
        }

        public Obj_AI_Base Caster = null, Target = null;
        public Obj_AI_Base BuffHolder
        {
            get
            {
                if (Data.RequireBuffs == null)
                    return null;

                return EntityManager.Heroes.AllHeroes.OrderBy(h => h.Distance(End)).FirstOrDefault(Data.HasBuff);
            }
        }
        public MissileClient Missile = null;
        public Obj_GeneralParticleEmitter Particle = null;
        public SkillshotData Data;

        public Vector2 Center => (CurrentPosition + CollideEndPosition) / 2;
        public Vector2 Start, End;
        public Vector2 Direction => (End - Start).Normalized();
        public Vector2 Direction2 => (EndPosition - Start).Normalized();
        public Vector2? CorrectCollidePoint;
        public Vector2? CollidePoint;

        public Vector2 CurrentPosition
        {
            get
            {
                if (Data.StaticStart)
                    return Start;

                if (Data.SticksToCaster && Caster != null)
                {
                    return Caster.ServerPosition.To2D();
                }
                if (Data.SticksToMissile && Missile != null)
                {
                    return Missile.Position.Extend(Start, -(Width / 2f));
                }
                if (Data.StartsFromTarget && Target != null)
                {
                    return Target.ServerPosition.Extend(Start, -Target.BoundingRadius);
                }

                return CalculatedPosition();
            }
        }

        public Vector2 EndPosition
        {
            get
            {
                if (Data.StaticEnd)
                    return End;

                if (IsGlobal && Data.type == Type.LineMissile)
                {
                    return CurrentPosition.Extend(CurrentPosition + Direction, Range);
                }

                if (Data.EndIsBuffHolderPosition && BuffHolder != null)
                {
                    var dashInfo = BuffHolder.GetDashInfo();
                    return (dashInfo?.EndPos ?? BuffHolder.ServerPosition).To2D();
                }

                if (Data.EndSticksToTarget && Target != null)
                {
                    return Target.ServerPosition.To2D();
                }

                var end = Vector2.Zero;

                if (Caster != null)
                {
                    if (Data.EndSticksToCaster)
                    {
                        return Caster.ServerPosition.To2D();
                    }
                    if (Data.SticksToCaster && Data.IsMoving)
                    {
                        return Caster.ServerPosition.To2D() + Direction * Range;
                    }
                    var lastPath = Caster.Path.LastOrDefault().To2D();
                    var direction = (Caster.Direction().Distance(Caster) < 50 ? Caster.Direction() : lastPath);
                    if (Data.EndIsCasterDirection)
                    {
                        end = Data.IsCasterName("Riven") ? lastPath : direction;
                    }
                    if (Data.IsCasterName("Sion") && Data.IsSlot(SpellSlot.R))
                    {
                        Speed = Caster.MoveSpeed;
                        end = Caster.ServerPosition.Extend(Start, -Range);
                    }
                    if (Data.EndStickToDirection)
                    {
                        return CurrentPosition.Extend(Direction, Range);
                    }
                }

                if (Missile != null && Data.EndSticksToMissile)
                {
                    return Missile.Position.To2D();
                }

                if (end.IsZero)
                    end = End;

                var result = Data.IsFixedRange ? Start.Extend(end, Range) : end.Extend(Start, -ImpactRange);

                if (Start.Distance(result) > Range)
                    result = Start.Extend(result, Range);

                return result;
            }
        }

        public Vector2 CollideEndPosition => CollidePoint ?? EndPosition;

        public Vector2 CalculatedPosition(float aftertime = 0)
        {
            var time = Math.Max(0, TicksPassed - CastDelay);
            int x;

            if (Data.MissileAccel.Equals(0f))
                x = (int)(time * Speed / 1000f);
            else
            {
                var time1 = (Data.MissileAccel > 0f ? Data.MissileMaxSpeed : Data.MissileMinSpeed - Speed) * 1000f / Data.MissileAccel;

                if (time <= time1)
                {
                    x = (int)(time * Speed / 1000f + 0.5f * Data.MissileAccel * Math.Pow(time / 1000f, 2f));
                }
                else
                {
                    x = (int)(time1 * Speed / 1000f + 0.5f * Data.MissileAccel * Math.Pow(time1 / 1000f, 2f)
                         + (time - time1) / 1000f * (Data.MissileAccel < 0f ? Data.MissileMaxSpeed : Data.MissileMinSpeed));
                }
            }

            var end = CollidePoint ?? EndPosition;

            time = (int)Math.Max(0, Math.Min(end.Distance(Start), x));
            return (Start + Direction * time);
        }

        public bool DetectedByMissile, FromFOW;
        public bool IsOnScreen => DrawingPolygon != null && DrawingPolygon.Points.Any(p => p.To3DWorld().IsOnScreen()) || Center.To3DWorld().IsOnScreen();
        public bool CollideExplode => CorrectCollidePoint.HasValue && Data.CollideExplode;
        public bool ExplodeEnd => CollidePoint.HasValue && Data.HasExplodingEnd;
        public bool IsGlobal => Data.Range >= 4000 && Data.Range < 15000;
        public bool WillHit(Vector2 target) => EvadePolygon != null && EvadePolygon.IsInside(target);
        public bool WillHit(Obj_AI_Base target) => target.IsValidTarget() && EvadePolygon != null && EvadePolygon.IsInside(target);
        public bool IsInside(Vector2 target) => target.IsValid() && OriginalPolygon.IsInside(target);
        public bool IsInside(Obj_AI_Base target) => target.IsValidTarget() && generatePolygon2(target.BoundingRadius).IsInside(target);
        public bool Ended
        {
            get
            {
                if ((Data.SticksToCaster || Data.EndSticksToCaster) && Caster.IsDead)
                {
                    return true;
                }

                if ((IsGlobal || (Data.SticksToMissile || Data.EndSticksToMissile)) && Missile != null && Missile.IsDead)
                {
                    return true;
                }

                if (Target != null && Target.IsDead)
                {
                    return true;
                }

                return Core.GameTickCount - EndTick > 0 || TicksPassed > 30000;
            }
        }
        public bool AboutToHit(Obj_AI_Base target, int delay)
        {
            if (!target.IsValidTarget())
                return false;

            return AboutToHit(target.ServerPosition.To2D(), delay);
        }
        public bool AboutToHit(Vector2 target, int delay = 100)
        {
            var travelTime = TravelTime(target);

            return WillHit(target) && travelTime < delay;
        }
        public bool IsAboutToHit(int time, Obj_AI_Base unit)
        {
            if (Target == null)
                return false;

            time += Game.Ping / 2;
            return WillHit(Target.ServerPosition.To2D()) && time >= TravelTime(unit);
        }

        public int DangerLevel;

        public float GetSpellDamage(Obj_AI_Base target)
        {
            if (target == null || Caster == null || Data.Slots == null || !Data.Slots.Any())
                return 0;

            var hero = Caster as AIHeroClient;
            if (hero == null)
                return 0;

            return hero.GetSpellDamage(target, Data.Slots[0]);
        }

        public float StartTick = Core.GameTickCount;
        public float EndTick => StartTick + MaxTravelTime(CollideEndPosition);
        public float TicksLeft => EndTick - Core.GameTickCount;
        public float TicksPassed => Core.GameTickCount - StartTick;
        public float extraDelay = 0;
        public float CastDelay
        {
            get
            {
                var result = 0f;
                if (!DetectedByMissile || Data.type != Type.LineMissile || (Speed <= 0 || Speed >= int.MaxValue))
                {
                    result += Data.CastDelay;
                }

                if (!Data.DontAddExtraDuration)
                {
                    result += Data.ExtraDuration;
                }

                return result + extraDelay;
            }
        }
        private float? _speed;
        public float Speed
        {
            get
            {
                return _speed ?? Data.Speed;
            }
            set
            {
                _speed = value;
            }
        }
        private float extraValue => 0;
        private float? _range;
        private float? _width;
        public float Range
        {
            get
            {
                try
                {
                    return (_range ?? Data.Range) + extraValue + ImpactRange;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong Range Skillshot ID: {Data.MenuItemName} {(Particle != null ? "Particle: " + Particle.Name : "")}");
                    return Data.Range;
                }
            }
            set
            {
                _range = value;
            }
        }
        public float ImpactRange
        {
            get
            {
                try
                {
                    return Data.ExtraRange + (extraValue / 2f);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong ExtraRange Skillshot ID: {Data.MenuItemName}");
                    return Data.ExtraRange;
                }
            }
        }
        public float Width
        {
            get
            {
                try
                {
                    return _width ?? Data.Width + extraValue;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong Width Skillshot ID: {Data.MenuItemName}");
                    return Data.Width;
                }
            }
            set
            {
                _width = value;
            }
        }
        public float RingRadius
        {
            get
            {
                try
                {
                    return Data.RingRadius;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong RingRadius Skillshot ID: {Data.MenuItemName}");
                    return Data.RingRadius;
                }
            }
        }
        public float Angle
        {
            get
            {
                try
                {
                    return Data.Angle;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong Angle Skillshot ID: {Data.MenuItemName}");
                    return Data.Angle;
                }
            }
        }
        public float ExplodeWidth
        {
            get
            {
                try
                {
                    return Data.ExplodeWidth;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Wrong ExplodeWidth Skillshot ID: {Data.MenuItemName}");
                    return Data.ExplodeWidth;
                }
            }
        }
        public float MaxTravelTime(Vector2 target)
        {
            return (Start.Distance(target) / Speed * 1000f) + Data.CastDelay + Data.ExtraDuration;
        }
        public float MaxTravelTime(Obj_AI_Base target)
        {
            return MaxTravelTime(target.ServerPosition.To2D());
        }
        public float TravelTime(Vector2 target)
        {
            var correct = ((Start.Distance(target) / Speed) * 1000f) + CastDelay;
            if (Data.type == Type.CircleMissile)
                correct = (CollideEndPosition.Distance(target) / Speed) * 1000f + CastDelay;

            return correct - TicksPassed;
        }
        public float TravelTime(Obj_AI_Base target)
        {
            return TravelTime(target.ServerPosition.To2D());
        }
        public float CurrentTravelTime(Vector2 pos)
        {
            return CurrentPosition.Distance(pos) / Speed * 1000f + CastDelay;
        }

        public Geometry.Polygon EvadePolygon, ExplodePolygon, OriginalPolygon, DrawingPolygon, PathPolygon;
        private Geometry.Polygon generatePolygon(float extraWidth = 0)
        {
            var extraAngle = Math.Max(1, Math.Max(1, extraWidth) / 5);
            extraWidth += Width;

            Geometry.Polygon polygon = null;
            switch (Data.type)
            {
                case Type.LineMissile:
                    polygon = new Geometry.Polygon.Rectangle(CurrentPosition, CollidePoint == null ? CollideEndPosition.Extend(CurrentPosition, -(extraWidth * 0.75f)) : CollideEndPosition, extraWidth);
                    break;
                case Type.CircleMissile:
                    polygon = new Geometry.Polygon.Circle(Data.IsMoving ? CurrentPosition : CollideEndPosition, extraWidth);
                    break;
                case Type.Cone:
                    polygon = new Geometry.Polygon.Sector(CurrentPosition, CollideEndPosition, (float)((Angle + extraAngle) * Math.PI / 180), Range);
                    break;
                case Type.Arc:
                    polygon = new CustomGeometry.Arc(Start, CollideEndPosition, (int)extraWidth).ToSDKPolygon();
                    break;
                case Type.Ring:
                    polygon = new CustomGeometry.Ring(CollideEndPosition, extraWidth, RingRadius).ToSDKPolygon();
                    break;
            }

            if (polygon != null && (ExplodeEnd || CollideExplode))
            {
                var newpolygon = polygon;
                var pos = CurrentPosition;
                var collidepoint = CollideExplode ? CorrectCollidePoint.GetValueOrDefault() : CollideEndPosition;
                switch (Data.Explodetype)
                {
                    case Type.CircleMissile:
                        ExplodePolygon = new Geometry.Polygon.Circle(collidepoint, ExplodeWidth);
                        break;
                    case Type.LineMissile:
                        var st = collidepoint - (collidepoint - pos).Normalized().Perpendicular() * (ExplodeWidth);
                        var en = collidepoint + (collidepoint - pos).Normalized().Perpendicular() * (ExplodeWidth);
                        ExplodePolygon = new Geometry.Polygon.Rectangle(st, en, ExplodeWidth / 2);
                        break;
                    case Type.Cone:
                        var st2 = collidepoint - Direction * (ExplodeWidth * 0.25f);
                        var en2 = collidepoint + Direction * (ExplodeWidth * 3);
                        ExplodePolygon = new Geometry.Polygon.Sector(st2, en2, (float)(Angle * Math.PI / 180), ExplodeWidth);
                        break;
                }

                var poly = Geometry.ClipPolygons(new[] { newpolygon, ExplodePolygon });
                var vectors = new List<IntPoint>();
                foreach (var p in poly)
                    vectors.AddRange(p.ToPolygon().ToClipperPath());

                polygon = vectors.ToPolygon();
            }

            return polygon;
        }
        public Geometry.Polygon generatePolygon2(float extraWidth = 0) // no collision end
        {
            extraWidth += Width;
            switch (Data.type)
            {
                case Type.LineMissile:
                    return new Geometry.Polygon.Rectangle(CurrentPosition, EndPosition, extraWidth);
                case Type.CircleMissile:
                    return new Geometry.Polygon.Circle(Data.IsMoving ? CurrentPosition : EndPosition, extraWidth);
                case Type.Cone:
                    return new Geometry.Polygon.Sector(CurrentPosition, EndPosition, (float)(Angle * Math.PI / 180), Range);
                case Type.Arc:
                    return new CustomGeometry.Arc(Start, CollideEndPosition, (int)extraWidth).ToSDKPolygon();
                case Type.Ring:
                    return new CustomGeometry.Ring(CollideEndPosition, extraWidth, RingRadius).ToSDKPolygon();
            }

            return null;
        }
        public Geometry.Polygon CollisionPolygon => new Geometry.Polygon.Rectangle(CurrentPosition, EndPosition, Math.Max(Data.Width, Data.Angle));

        public Geometry.Polygon PredictedPolygon(float afterTime, float extraWidth = 0)
        {
            var extraAngle = Math.Max(1, Math.Max(1, extraWidth) / 4);
            extraWidth += Width;

            Geometry.Polygon polygon = null;
            switch (Data.type)
            {
                case Type.LineMissile:
                    polygon = new Geometry.Polygon.Rectangle(CalculatedPosition(afterTime), CollideEndPosition, extraWidth);
                    break;
                case Type.CircleMissile:
                    polygon = new Geometry.Polygon.Circle(Data.IsMoving ? CalculatedPosition(afterTime) : CollideEndPosition, extraWidth);
                    break;
                case Type.Cone:
                    polygon = new Geometry.Polygon.Sector(CurrentPosition, CollideEndPosition, (float)((Angle + extraAngle) * Math.PI / 180), Range);
                    break;
                case Type.Arc:
                    polygon = new CustomGeometry.Arc(Start, CollideEndPosition, (int)extraWidth).ToSDKPolygon();
                    break;
                case Type.Ring:
                    polygon = new CustomGeometry.Ring(CollideEndPosition, extraWidth, RingRadius).ToSDKPolygon();
                    break;
            }

            if (polygon != null && (ExplodeEnd || CollideExplode))
            {
                var newpolygon = polygon;
                var pos = CurrentPosition;
                var collidepoint = CollideExplode ? CorrectCollidePoint.GetValueOrDefault() : CollideEndPosition;
                switch (Data.Explodetype)
                {
                    case Type.CircleMissile:
                        ExplodePolygon = new Geometry.Polygon.Circle(collidepoint, ExplodeWidth);
                        break;
                    case Type.LineMissile:
                        var st = collidepoint - (collidepoint - pos).Normalized().Perpendicular() * (ExplodeWidth);
                        var en = collidepoint + (collidepoint - pos).Normalized().Perpendicular() * (ExplodeWidth);
                        ExplodePolygon = new Geometry.Polygon.Rectangle(st, en, ExplodeWidth / 2);
                        break;
                    case Type.Cone:
                        var st2 = collidepoint - Direction * (ExplodeWidth * 0.25f);
                        var en2 = collidepoint + Direction * (ExplodeWidth * 3);
                        ExplodePolygon = new Geometry.Polygon.Sector(st2, en2, (float)(Angle * Math.PI / 180), ExplodeWidth);
                        break;
                }

                var poly = Geometry.ClipPolygons(new[] { newpolygon, ExplodePolygon });
                var vectors = new List<IntPoint>();
                foreach (var p in poly)
                    vectors.AddRange(p.ToPolygon().ToClipperPath());

                polygon = vectors.ToPolygon();
            }

            return polygon;
        }
    }
}
