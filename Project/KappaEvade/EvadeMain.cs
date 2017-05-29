namespace Project_Team.KappaEvade
{
    using Databases.Spells;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using SharpDX;

    using System.Linq;

    internal static class EvadeMain
    {
        internal static Menu mainMenu;

        internal static void Init(Menu ParentMenu)
        {
            mainMenu = ParentMenu.AddSubMenu("Evade Settings", "Project_Team.KappaEvade");

            mainMenu.Add("enable", new CheckBox("Enable mainMenu"));
            mainMenu.Add("executeBlock", new CheckBox("Block Any Spell if it will Kill Player"));

            var validAttacks = EmpowerdAttackDatabase.Current.FindAll(x => EntityManager.Heroes.Enemies.Any(h => h.Hero.Equals(x.Hero)));
            if (validAttacks.Any())
            {
                mainMenu.AddGroupLabel("Empowered Attacks");
                mainMenu.Add("AABlock", new CheckBox("Block Empowered Attacks"));
                foreach (var s in validAttacks.OrderBy(s => s.Hero))
                {
                    var spellname = s.MenuItemName;
                    if (!SpellBlocker.EnabledSpells.Any(x => x.SpellName.Equals(spellname)))
                    {
                        mainMenu.AddLabel(spellname);
                        mainMenu.Add("enable" + spellname, new CheckBox("Enable", s.DangerLevel > 1 || s.CrowdControl));
                        mainMenu.Add("danger" + spellname, new Slider("Danger Level", s.DangerLevel, 1, 5));
                        SpellBlocker.EnabledSpells.Add(new SpellBlocker.EnabledSpell(spellname));
                        mainMenu.AddSeparator(0);
                    }
                }
            }

            var validBuffs = DangerBuffDataDatabase.Current.FindAll(x => EntityManager.Heroes.Enemies.Any(h => h.Hero.Equals(x.Hero)));
            if (validBuffs.Any())
            {
                mainMenu.AddSeparator(5);
                mainMenu.AddGroupLabel("Danger Buffs");
                mainMenu.Add("buffBlock", new CheckBox("Block Danger Buffs"));

                foreach (var s in validBuffs.OrderBy(s => s.Hero))
                {
                    var spellname = s.MenuItemName;
                    if (!SpellBlocker.EnabledSpells.Any(x => x.SpellName.Equals(spellname)))
                    {
                        mainMenu.AddLabel(spellname);
                        mainMenu.Add("enable" + spellname, new CheckBox("Enable", s.DangerLevel > 1));
                        if (s.HasStackCount)
                        {
                            var stackCount = mainMenu.Add("stackCount", new Slider("Block at Stack Count", s.StackCount, 1, s.MaxStackCount));
                            s.StackCountFromMenu = () => stackCount.CurrentValue;
                        }
                        mainMenu.Add("danger" + spellname, new Slider("Danger Level", s.DangerLevel, 1, 5));
                        SpellBlocker.EnabledSpells.Add(new SpellBlocker.EnabledSpell(spellname));
                        mainMenu.AddSeparator(0);
                    }
                }
            }


            var validTargeted = TargetedSpellDatabase.Current.FindAll(x => EntityManager.Heroes.Enemies.Any(h => h.Hero.Equals(x.hero)));
            if (validTargeted.Any())
            {
                mainMenu.AddSeparator(5);
                mainMenu.AddGroupLabel("Targeted Spells");
                mainMenu.Add("targetedBlock", new CheckBox("Block Targeted Spells"));
                foreach (var s in validTargeted.OrderBy(s => s.hero))
                {
                    var spellname = s.MenuItemName;
                    if (!SpellBlocker.EnabledSpells.Any(x => x.SpellName.Equals(spellname)))
                    {
                        mainMenu.AddLabel(spellname);
                        mainMenu.Add("enable" + spellname, new CheckBox("Enable", s.DangerLevel > 1));
                        mainMenu.Add("fast" + spellname, new CheckBox("Fast Block (Instant)", s.FastEvade));
                        mainMenu.Add("danger" + spellname, new Slider("Danger Level", s.DangerLevel, 1, 5));
                        SpellBlocker.EnabledSpells.Add(new SpellBlocker.EnabledSpell(spellname));
                        mainMenu.AddSeparator(0);
                    }
                }
            }

            var specialSpells = SpecialSpellsDatabase.Current.FindAll(s => EntityManager.Heroes.Enemies.Any(h => s.Hero.Equals(h.Hero)));
            if (specialSpells.Any())
            {
                mainMenu.AddSeparator(5);
                mainMenu.AddGroupLabel("Special Spells");
                mainMenu.Add("specialBlock", new CheckBox("Block Special Spells"));
                foreach (var s in specialSpells)
                {
                    var display = s.MenuItemName;
                    if (!SpellBlocker.EnabledSpells.Any(x => x.SpellName.Equals(display)))
                    {
                        mainMenu.AddLabel(display);
                        mainMenu.Add($"enable{display}", new CheckBox("Enable", s.DangerLevel > 1));
                        mainMenu.Add($"fast{display}", new CheckBox("Fast Block (Instant)", s.DangerLevel > 2));
                        mainMenu.Add($"danger{display}", new Slider("Danger Level", s.DangerLevel, 1, 5));
                        SpellBlocker.EnabledSpells.Add(new SpellBlocker.EnabledSpell(display));
                    }
                }
            }

            var validskillshots =
                SkillshotDatabase.Current.Where(s => (s.GameType.Equals(GameType.Normal) || s.GameType.Equals(Game.Type))
                && EntityManager.Heroes.Enemies.Any(h => s.IsCasterName(Champion.Unknown) || s.IsCasterName(h.Hero))).OrderBy(s => s.CasterNames[0]);
            if (validskillshots.Any())
            {
                mainMenu.AddSeparator(5);
                mainMenu.AddGroupLabel("SkillShots");
                mainMenu.Add("skillshotBlock", new CheckBox("Block SkillShots"));

                foreach (var s in validskillshots)
                {
                    var display = s.MenuItemName;
                    if (!SpellBlocker.EnabledSpells.Any(x => x.SpellName.Equals(display)))
                    {
                        mainMenu.AddLabel(display);
                        mainMenu.Add($"enable{display}", new CheckBox("Enable", s.DangerLevel > 1));
                        mainMenu.Add($"fast{display}", new CheckBox("Fast Block (Instant)", s.FastEvade));
                        mainMenu.Add($"danger{display}", new Slider("Danger Level", s.DangerLevel, 1, 5));
                        SpellBlocker.EnabledSpells.Add(new SpellBlocker.EnabledSpell(display));
                    }
                }
            }

            SpellBlocker.Init();
        }

        public static float PredictHealth(this Obj_AI_Base target, float Time = 250f)
        {
            if (Time == 250f)
                Time += Game.Ping;
            return Prediction.Health.GetPrediction(target, (int)Time);
        }

        public static Vector3 PrediectPosition(this Obj_AI_Base target, float Time = 250)
        {
            if (Time == 250)
                Time += Game.Ping;
            return Prediction.Position.PredictUnitPosition(target, (int)Time).To3D();
        }
    }
}
