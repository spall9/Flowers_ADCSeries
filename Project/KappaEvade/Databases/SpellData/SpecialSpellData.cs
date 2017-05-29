namespace Project_Team.KappaEvade.Databases.SpellData
{
    using EloBuddy;

    public class SpecialSpellData
    {
        public Champion Hero;
        public SpellSlot Slot;
        public string SpellName;
        public string TargetName;
        public string ParticalName;
        public string DisplayName;
        public float Range;
        public int CastDelay;
        public int TargetHealth;
        public int DangerLevel;
        public string MenuItemName
            => $"{Hero} {Slot} ({(!string.IsNullOrEmpty(DisplayName) ? DisplayName : !string.IsNullOrEmpty(SpellName) ? SpellName : !string.IsNullOrEmpty(ParticalName) ? ParticalName : TargetName)}";
    }
}
