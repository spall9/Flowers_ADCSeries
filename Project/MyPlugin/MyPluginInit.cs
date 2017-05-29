namespace Project_Team.MyPlugin
{
    using MyBase;

    using EloBuddy;

    internal class MyPluginInit : MySpellBase
    {
        public MyPluginInit()
        {
            switch (Me.Hero)
            {
                case Champion.Ashe:
                    var ashe = new Ashe.MyLogic();
                    ashe.Init();
                    break;
                case Champion.Ekko:
                    var ekko = new Ekko.MyLogic();
                    ekko.Init();
                    break;
                case Champion.Fiora:
                    var fiora = new Fiora.MyLogic();
                    fiora.Init();
                    break;
                case Champion.Katarina:
                    var katarina = new Katarina.MyLogic();
                    katarina.Init();
                    break;
                case Champion.Leona:
                    var leona = new Leona.MyLogic();
                    leona.Init();
                    break;
                case Champion.Lucian:
                    var lucian = new Lucian.MyLogic();
                    lucian.Init();
                    break;
                case Champion.MasterYi:
                    var masteryi = new MasterYi.MyLogic();
                    masteryi.Init();
                    break;
                case Champion.Yasuo:
                    var yasuo = new Yasuo.MyLogic();
                    yasuo.Init();
                    break;
                case Champion.Zed:
                    var zed = new Zed.MyLogic();
                    zed.Init();
                    break;
            }
        }
    }
}
