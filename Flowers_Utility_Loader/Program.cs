namespace Flowers_Utility_Loader
{
    using EloBuddy.SDK.Events;

    using System;
    using System.IO;
    using System.Reflection;

    internal class Program
    {
        private static readonly string getAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string dllPath = Path.Combine(getAppDataPath, @"EloBuddy\Addons\Libraries\MoonBrain.dll");
        private static void Main(string[] eventArgs)
        {
            Loading.OnLoadingComplete += Args =>
            {
                if (File.Exists(dllPath))
                {
                    File.Delete(dllPath);
                }

                var prdll = Properties.Resources.Flowers__Utility;
                using (var fs = new FileStream(dllPath, FileMode.Create))
                {
                    fs.Write(prdll, 0, prdll.Length);
                }

                var dllpath = Assembly.LoadFrom(dllPath);
                var main = dllpath.GetType("Flowers_Utility.MyLoader").GetMethod("Init");
                main.Invoke(null, null);
            };
        }
    }
}
