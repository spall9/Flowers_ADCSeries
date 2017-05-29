namespace Project_Team.KappaEvade.Databases.Spells
{
    using SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    using System.Collections.Generic;
    using System.Linq;

    public static class SpecialSpellsDatabase
    {
        public static List<SpecialSpellData> Current;

        static SpecialSpellsDatabase()
        {
            if (Current != null)
                return;

            Current = List.FindAll(s => s.Hero == Champion.Unknown || EntityManager.Heroes.AllHeroes.Any(h => s.Hero.Equals(h.Hero)));
        }

        private static readonly List<SpecialSpellData> List = new List<SpecialSpellData>
            {
                new SpecialSpellData
                    {
                        Hero = Champion.LeeSin,
                        Slot = SpellSlot.Q,
                        SpellName = "BlindMonkQTwo",
                        DisplayName = "2nd Cast",
                        CastDelay = 100,
                        DangerLevel = 2
                    },
                new SpecialSpellData
                    {
                        Hero = Champion.Tryndamere,
                        Slot = SpellSlot.W,
                        SpellName = "TryndamereW",
                        DisplayName = "Slow / Facing",
                        Range = 850,
                        CastDelay = 250,
                        DangerLevel = 3
                    },
            };
    }
}
