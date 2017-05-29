namespace Project_Team.KappaEvade.SpellDetector.Events
{
    using DetectedData;
    using Detectors;

    public class OnSkillShotDetected
    {
        public delegate void SkillShotDetected(DetectedSkillshotData args);
        public static event SkillShotDetected OnDetect;
        internal static void Invoke(DetectedSkillshotData args)
        {
            var invocationList = OnDetect?.GetInvocationList();
            if (invocationList != null)
                foreach (var m in invocationList)
                    m?.DynamicInvoke(args);
        }

        static OnSkillShotDetected()
        {
            var skillshotDetector = new SkillshotDetector();
        }
    }

    public class OnSkillShotDelete
    {
        public delegate void SkillShotDelete(DetectedSkillshotData args);
        public static event SkillShotDelete OnDelete;
        internal static bool Invoke(DetectedSkillshotData args)
        {
            var invocationList = OnDelete?.GetInvocationList();
            if (invocationList != null)
                foreach (var m in invocationList)
                    m?.DynamicInvoke(args);

            return true;
        }
    }
}
