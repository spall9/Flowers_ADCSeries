namespace Project_Team.MyBase
{
    using EloBuddy;
    using EloBuddy.SDK;

    internal abstract class MySpellBase
    {
        protected static AIHeroClient Me { get; } = Player.Instance;

        protected static SpellSlot Flash { get; set; } = SpellSlot.Unknown;

        protected static SpellSlot Ignite { get; set; } = SpellSlot.Unknown;

        protected static Spell.Active AsheQ { get; set; }
        protected static Spell.Skillshot AsheW { get; set; }
        protected static Spell.Skillshot AsheE { get; set; }
        protected static Spell.Skillshot AsheR { get; set; }

        protected static Spell.Skillshot EkkoQ { get; set; }
        protected static Spell.Skillshot EkkoW { get; set; }
        protected static Spell.Skillshot EkkoE { get; set; }
        protected static Spell.Active EkkoR { get; set; }

        protected static Spell.Skillshot FioraQ { get; set; }
        protected static Spell.Skillshot FioraW { get; set; }
        protected static Spell.Active FioraE { get; set; }
        protected static Spell.Targeted FioraR { get; set; }

        protected static Spell.Skillshot KatarinaQ { get; set; }
        protected static Spell.Active KatarinaW { get; set; }
        protected static Spell.Targeted KatarinaE { get; set; }
        protected static Spell.Active KatarinaR { get; set; }

        protected static Spell.Active LeonaQ { get; set; }
        protected static Spell.Active LeonaW { get; set; }
        protected static Spell.Skillshot LeonaE { get; set; }
        protected static Spell.Skillshot LeonaR { get; set; }

        protected static Spell.Skillshot LucianQ { get; set; }
        protected static Spell.Skillshot LucianQ1 { get; set; }
        protected static Spell.Skillshot LucianW { get; set; }
        protected static Spell.Skillshot LucianE { get; set; }
        protected static Spell.Skillshot LucianR { get; set; }

        protected static Spell.Skillshot MasterYiQ { get; set; }
        protected static Spell.Active MasterYiW { get; set; }
        protected static Spell.Active MasterYiE { get; set; }
        protected static Spell.Active MasterYiR { get; set; }

        protected static Spell.Skillshot YasuoQ { get; set; }
        protected static Spell.Skillshot YasuoQ3 { get; set; }
        protected static Spell.Skillshot YasuoW { get; set; }
        protected static Spell.Targeted YasuoE { get; set; }
        protected static Spell.Active YasuoR { get; set; }

        protected static Spell.Skillshot ZedQ { get; set; }
        protected static Spell.Skillshot ZedW { get; set; }
        protected static Spell.Active ZedE { get; set; }
        protected static Spell.Targeted ZedR { get; set; }
    }
}
