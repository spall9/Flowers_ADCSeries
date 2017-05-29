namespace Project_Team
{
    using MyBase;

    using EloBuddy;
    using EloBuddy.SDK.Events;

    internal class MyLoader
    {
        private static void Main(string[] eventArgs)
        {
            Loading.OnLoadingComplete += Args =>
            {
                var myChampions = new MyChampions(Player.Instance.ChampionName);
                myChampions.PrintChat();
            };
        }
    }
}
