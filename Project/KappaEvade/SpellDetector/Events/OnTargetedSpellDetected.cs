namespace Project_Team.KappaEvade.SpellDetector.Events
{
    using DetectedData;
    using Detectors;

    public class OnTargetedSpellDetected
    {
        public delegate void TargetedSpellDetected(DetectedTargetedSpellData args);
        public static event TargetedSpellDetected OnDetect;
        internal static void Invoke(DetectedTargetedSpellData args)
        {
            var invocationList = OnDetect?.GetInvocationList();
            if (invocationList != null)
                foreach (var m in invocationList)
                    m?.DynamicInvoke(args);
        }

        static OnTargetedSpellDetected()
        {
            var targetedSpellDetector = new TargetedSpellDetector();
        }
    }
}
