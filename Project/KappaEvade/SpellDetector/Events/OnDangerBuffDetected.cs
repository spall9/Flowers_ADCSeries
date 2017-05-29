namespace Project_Team.KappaEvade.SpellDetector.Events
{
    using DetectedData;
    using Detectors;

    public class OnDangerBuffDetected
    {
        public delegate void DetectedDangerBuff(DetectedDangerBuffData args);
        public static event DetectedDangerBuff OnDetect;
        internal static void Invoke(DetectedDangerBuffData args)
        {
            var invocationList = OnDetect?.GetInvocationList();
            if (invocationList != null)
                foreach (var m in invocationList)
                    m?.DynamicInvoke(args);
        }

        static OnDangerBuffDetected()
        {
            new DangerBuffDetector();
        }
    }
}
