namespace Project_Team.KappaEvade
{
    using SpellDetector.DetectedData;
    using SpellDetector.Detectors;
    using SpellDetector.Events;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Utils;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SpellBlocker
    {
        internal static Spell.Skillshot EvadeSpell;
        internal static Spell.Skillshot EvadeSpell2;

        public static void Init()
        {
            //EvadeSpell = ?
            //EvadeSpell2 = ?
            Game.OnTick += Game_OnTick;
            OnEmpoweredAttackDetected.OnDetect += OnEmpoweredAttackDetected_OnDetect;
            OnDangerBuffDetected.OnDetect += OnDangerBuffDetected_OnDetect;
            OnTargetedSpellDetected.OnDetect += OnTargetedSpellDetected_OnDetect;
            OnSkillShotDetected.OnDetect += OnSkillShotDetected_OnDetect;
            OnSpecialSpellDetected.OnDetect += OnSpecialSpellDetected_OnDetect;
        }

        private static float _lastBlock;
        private static void Game_OnTick(EventArgs args)
        {
            if (delay(EvadeSpell) > Core.GameTickCount - _lastBlock || !EvadeSpell.IsReady())
                return;

            var BlockSpells = DetectedSpells.OrderByDescending(s => s.DangerLevel);
            if (BlockSpells.Any())
            {
                foreach (var BlockSpell in BlockSpells)
                {
                    #region skillshot
                    var skillshot = BlockSpell.DetectedData as DetectedSkillshotData;
                    if (skillshot != null && EvadeMain.mainMenu["skillshotBlock"].Cast<CheckBox>().CurrentValue)
                    {
                        var dashInfo = Player.Instance.GetDashInfo();
                        if (dashInfo != null)
                        {
                            var travelTime = (Player.Instance.ServerPosition.Distance(dashInfo.EndPos) / EvadeSpell.Speed) * 1000f;
                            var predPos = Player.Instance.PrediectPosition(travelTime);
                            var skillshotpred = Player.Instance.PrediectPosition(skillshot.TravelTime(predPos.To2D()));
                            if (predPos.Distance(skillshotpred) > Player.Instance.BoundingRadius)
                                continue; // skip if dashing outside of skillshot
                        }
                        if (skillshot.Ended || !SkillshotDetector.SkillshotsDetected.Contains(skillshot))
                        {
                            DetectedSpells.Remove(BlockSpell);
                            continue;
                        }

                        if (skillshot.WillHit(Player.Instance))
                        {
                            if (skillshot.TravelTime(Player.Instance) <= delay(EvadeSpell) || BlockSpell.FastEvade)
                                CastW(skillshot.Caster, BlockSpell.SpellName);
                        }

                        break;
                    }
                    #endregion

                    #region buff
                    var buff = BlockSpell.DetectedData as DetectedDangerBuffData;
                    if (buff != null && EvadeMain.mainMenu["buffBlock"].Cast<CheckBox>().CurrentValue)
                    {
                        if (buff.Ended || !DangerBuffDetector.DangerBuffsDetected.Contains(buff))
                        {
                            DetectedSpells.Remove(BlockSpell);
                            continue;
                        }

                        if (buff.WillHit(Player.Instance))
                        {
                            if (buff.TicksLeft <= delay(EvadeSpell))
                                CastW(buff.Caster, BlockSpell.SpellName);
                        }

                        break;
                    }
                    #endregion

                    #region targted
                    var targted = BlockSpell.DetectedData as DetectedTargetedSpellData;
                    if (targted != null && EvadeMain.mainMenu["targetedBlock"].Cast<CheckBox>().CurrentValue)
                    {
                        if (targted.Ended || !TargetedSpellDetector.DetectedTargetedSpells.Contains(targted))
                        {
                            DetectedSpells.Remove(BlockSpell);
                            continue;
                        }

                        if (targted.WillHit(Player.Instance))
                        {
                            if (targted.TicksLeft <= delay(EvadeSpell) || BlockSpell.FastEvade)
                                CastW(targted.Caster, BlockSpell.SpellName);
                        }

                        break;
                    }
                    #endregion

                    #region autoattack
                    var autoattack = BlockSpell.DetectedData as DetectedEmpoweredAttackData;
                    if (autoattack != null && EvadeMain.mainMenu["AABlock"].Cast<CheckBox>().CurrentValue)
                    {
                        if (autoattack.Ended || !EmpoweredAttackDetector.DetectedEmpoweredAttacks.Contains(autoattack))
                        {
                            DetectedSpells.Remove(BlockSpell);
                            continue;
                        }

                        if (autoattack.WillHit(Player.Instance))
                        {
                            if (autoattack.TicksLeft <= delay(EvadeSpell))
                                CastW(autoattack.Caster, BlockSpell.SpellName);
                        }

                        break;
                    }
                    #endregion

                    #region special
                    var special = BlockSpell.DetectedData as DetectedSpecialSpellData;
                    if (special != null && EvadeMain.mainMenu["specialBlock"].Cast<CheckBox>().CurrentValue)
                    {
                        if (special.Ended || !SpecialSpellDetector.DetectedSpecialSpells.Contains(special))
                        {
                            DetectedSpells.Remove(BlockSpell);
                            continue;
                        }

                        if (special.WillHit(Player.Instance))
                        {
                            if (special.TicksLeft <= delay(EvadeSpell) || BlockSpell.FastEvade)
                                CastW(special.Caster, BlockSpell.SpellName);
                        }

                        break;
                    }
                    #endregion
                }
            }
        }

        private static float delay(Spell.Skillshot spell)
        {
            return spell.CastDelay + (Game.Ping / 2f);
        }

        private static void OnSpecialSpellDetected_OnDetect(DetectedSpecialSpellData args)
        {
            if (!args.IsEnemy || !args.WillHit(Player.Instance))
                return;

            var spellname = args.Data.MenuItemName;
            var spell = EnabledSpells.FirstOrDefault(s => s.SpellName.Equals(spellname));
            if (spell == null)
            {
                Logger.Warn($"{spellname} Not valid Spell");
                return;
            }

            if (!spell.Enabled && (!EvadeMain.mainMenu["executeBlock"].Cast<CheckBox>().CurrentValue || Player.Instance.PredictHealth(args.TicksLeft) > args.GetSpellDamage(Player.Instance)))
            {
                Logger.Warn($"{spellname} Not Enabled from Menu");
                return;
            }

            var newSpell = new DetectedSpell
            {
                Caster = args.Caster,
                DangerLevel = spell.DangerLevel,
                FastEvade = spell.FastEvade,
                SpellName = spellname,
                DetectedData = args
            };

            if (!DetectedSpells.Contains(newSpell))
                DetectedSpells.Add(newSpell);
        }

        private static void OnEmpoweredAttackDetected_OnDetect(DetectedEmpoweredAttackData args)
        {
            if (args.Caster == null || !args.Caster.IsEnemy || !args.WillHit(Player.Instance))
                return;

            var spellname = args.Data.MenuItemName;
            var spell = EnabledSpells.FirstOrDefault(s => s.SpellName.Equals(spellname));

            if (spell == null)
            {
                Logger.Warn($"{spellname} Not valid Spell");
                return;
            }

            if (!spell.Enabled && (!EvadeMain.mainMenu["executeBlock"].Cast<CheckBox>().CurrentValue || Player.Instance.PredictHealth(args.TicksLeft) > args.GetSpellDamage(Player.Instance)))
            {
                Logger.Warn($"{spellname} Not Enabled from Menu");
                return;
            }

            var newSpell = new DetectedSpell
            {
                Caster = args.Caster,
                DangerLevel = spell.DangerLevel,
                SpellName = spellname,
                DetectedData = args
            };

            if (!DetectedSpells.Contains(newSpell))
                DetectedSpells.Add(newSpell);
        }

        private static void OnDangerBuffDetected_OnDetect(DetectedDangerBuffData args)
        {
            if (args.Caster == null || !args.Caster.IsEnemy || !args.WillHit(Player.Instance))
                return;

            var spellname = args.Data.MenuItemName;
            var spell = EnabledSpells.FirstOrDefault(s => s.SpellName.Equals(spellname));

            if (spell == null)
            {
                Logger.Warn($"{spellname} Not valid Spell");
                return;
            }

            if (!spell.Enabled && (!EvadeMain.mainMenu["executeBlock"].Cast<CheckBox>().CurrentValue || Player.Instance.PredictHealth(args.TicksLeft) > args.GetSpellDamage(Player.Instance)))
            {
                Logger.Warn($"{spellname} Not Enabled from Menu");
                return;
            }

            var newSpell = new DetectedSpell
            {
                Caster = args.Caster,
                DangerLevel = spell.DangerLevel,
                SpellName = spellname,
                DetectedData = args
            };

            if (!DetectedSpells.Contains(newSpell))
                DetectedSpells.Add(newSpell);
        }

        private static void OnTargetedSpellDetected_OnDetect(DetectedTargetedSpellData args)
        {
            if (args.Caster == null || !args.Caster.IsEnemy || !args.Target.IsMe || !args.WillHit(Player.Instance))
                return;

            var spellname = args.Data.MenuItemName;
            var spell = EnabledSpells.FirstOrDefault(s => s.SpellName.Equals(spellname));

            if (spell == null)
            {
                Logger.Warn($"{spellname} Not valid Spell");
                return;
            }

            if (!spell.Enabled && (!EvadeMain.mainMenu["executeBlock"].Cast<CheckBox>().CurrentValue || Player.Instance.PredictHealth(args.TicksLeft) > args.GetSpellDamage(Player.Instance)))
            {
                Logger.Warn($"{spellname} Not Enabled from Menu");
                return;
            }

            var newSpell = new DetectedSpell
            {
                Caster = args.Caster,
                DangerLevel = spell.DangerLevel,
                FastEvade = spell.FastEvade,
                SpellName = spellname,
                DetectedData = args
            };

            if (!DetectedSpells.Contains(newSpell))
                DetectedSpells.Add(newSpell);

            /*
            if (spell != null && spell.FastEvade)
                CastW(args.Caster, spellname);
            else
            {
                if (args.TicksLeft <= delay)
                    CastW(args.Caster, spellname);
            }*/
        }

        private static void OnSkillShotDetected_OnDetect(DetectedSkillshotData args)
        {
            if (args.Caster == null || !args.Caster.IsEnemy || !args.WillHit(Player.Instance))
                return;

            var spellname = args.Data.MenuItemName;
            var spell = EnabledSpells.FirstOrDefault(s => s.SpellName.Equals(spellname));

            if (spell == null)
            {
                Logger.Warn($"{spellname} Not valid Spell");
                return;
            }

            if (!spell.Enabled && (!EvadeMain.mainMenu["executeBlock"].Cast<CheckBox>().CurrentValue ||
                Player.Instance.PredictHealth(args.TravelTime(Player.Instance)) > args.GetSpellDamage(Player.Instance)))
            {
                Logger.Warn($"{spellname} Not Enabled from Menu");
                return;
            }

            var newSpell = new DetectedSpell
            {
                Caster = args.Caster,
                DangerLevel = spell.DangerLevel,
                FastEvade = spell.FastEvade,
                SpellName = spellname,
                DetectedData = args
            };

            if (!DetectedSpells.Contains(newSpell))
                DetectedSpells.Add(newSpell);
        }

        private static void CastW(Obj_AI_Base caster, string spellname = "")
        {
            if (!EvadeMain.mainMenu["enable"].Cast<CheckBox>().CurrentValue)
                return;

            if (!EvadeSpell.IsReady())
                return;

            var wtarget =
                TargetSelector.SelectedTarget != null && EvadeSpell.IsInRange(EvadeSpell.GetPrediction(TargetSelector.SelectedTarget).CastPosition)
                ? TargetSelector.SelectedTarget
                : EvadeSpell.IsInRange(EvadeSpell.GetPrediction(EvadeSpell.GetTarget()).CastPosition)
                ? EvadeSpell.GetTarget()
                : caster;

            var castpos = EvadeSpell.IsInRange(EvadeSpell.GetPrediction(wtarget).CastPosition) ? (EvadeSpell.GetPrediction(wtarget).CastPosition + wtarget.ServerPosition) / 2 : Game.CursorPos;

            EvadeSpell.Cast(castpos);
            _lastBlock = Core.GameTickCount;
            Logger.Info($"BLOCK {spellname}");
        }

        public static List<EnabledSpell> EnabledSpells = new List<EnabledSpell>();

        public class EnabledSpell
        {
            public EnabledSpell(string spellname)
            {
                SpellName = spellname;
            }

            public string SpellName;
            public bool Enabled => EvadeMain.mainMenu[$"enable{SpellName}"].Cast<CheckBox>().CurrentValue;
            public bool FastEvade => EvadeMain.mainMenu[$"fast{SpellName}"].Cast<CheckBox>().CurrentValue;
            public int DangerLevel => EvadeMain.mainMenu[$"danger{SpellName}"].Cast<Slider>().CurrentValue;
        }

        public static List<DetectedSpell> DetectedSpells = new List<DetectedSpell>();

        public class DetectedSpell
        {
            public Obj_AI_Base Caster;
            public object DetectedData;
            public string SpellName;
            public int DangerLevel;
            public bool FastEvade;
        }
    }
}
