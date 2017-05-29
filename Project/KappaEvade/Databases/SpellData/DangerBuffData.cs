namespace Project_Team.KappaEvade.Databases.SpellData
{
    using EloBuddy;

    public class DangerBuffData
    {
        public Champion Hero;
        public SpellSlot Slot;
        public string BuffName;
        public int Delay;
        public int DangerLevel;
        public int Range = int.MaxValue;
        public int StackCount = 0;
        public int MaxStackCount = 15;
        public delegate int MenuStackCount();
        public MenuStackCount StackCountFromMenu = null;
        public bool IsRanged => Range < int.MaxValue;
        public bool RequireCast;
        public string MenuItemName => $"{Hero} {(Slot == SpellSlot.Unknown ? "Passive" : Slot.ToString())} ({BuffName})";
        public bool HasStackCount => StackCount > 0 && StackCount < int.MaxValue;
    }
}
