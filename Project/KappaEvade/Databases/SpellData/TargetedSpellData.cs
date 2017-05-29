namespace Project_Team.KappaEvade.Databases.SpellData
{
    using EloBuddy;

    public class TargetedSpellData
    {
        public Champion hero;
        public SpellSlot slot;
        public string[] MissileNames;
        public int DangerLevel;
        public float CastDelay = 0;
        public float Speed = float.MaxValue;
        public bool FastEvade;
        public bool WindWall;
        public string MenuItemName => $"{hero} {slot}";
    }
}
