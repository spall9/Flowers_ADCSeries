namespace Flowers_ADCSeries_Loader
{
    using EloBuddy.SDK.Events;

    using System;
    using System.IO;
    using System.Reflection;

    internal class Program
    {
        private static readonly string dllPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EloBuddy\Addons\Libraries\MoonCore.dll";

        private static void Main(string[] eventArgs)
        {
            Loading.OnLoadingComplete += Args =>
            {
                if (File.Exists(dllPath))
                {
                    File.Delete(dllPath);
                }

                //Add the Version check later

                var prdll = Properties.Resources.Flowers__ADC_Series;
                using (var fs = new FileStream(dllPath, FileMode.Create))
                {
                    fs.Write(prdll, 0, prdll.Length);
                }

                //var loadAssembly = Assembly.Load(Properties.Resources.Flowers__ADC_Series);
                //var typeAssembly = ass.GetType("Flowers_ADCSeries.MyLoader");
                //var methonAssembly = type.GetMethod("Init");
                //var objAssembly = ass.CreateInstance("Flowers_ADCSeries.MyLoader.Init");
                //mi.Invoke(objAssembly, null);

                var dllpath = Assembly.LoadFrom(dllPath);
                var main = dllpath.GetType("Flowers_ADCSeries.MyLoader").GetMethod("Init");
                main.Invoke(null, null);
            };
        }
    }
}