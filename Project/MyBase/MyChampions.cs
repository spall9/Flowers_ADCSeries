namespace Project_Team.MyBase
{
    using MyPlugin;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;

    using System;
    using System.Linq;

    internal class MyChampions : MySpellBase
    {
        internal static Menu mainMenu;

        private static readonly string[] charName = {"Ashe", "Ekko", "Fiora", "Katarina", "Leona", "Lucian", "MasterYi", "Yasuo", "Zed"};
        private static readonly string[] notReallyDone = { "Ashe", "Ekko", "Fiora", "Katarina", "Leona", "Lucian", "MasterYi", "Yasuo", "Zed" };

        public void PrintChat()
        {
            if (notReallyDone.Contains(Player.Instance.ChampionName))
            {
                Chat.Print("Project:: " + Player.Instance.ChampionName + " -> Will Support!");
            }
            else if (!charName.Contains(Player.Instance.ChampionName))
            {
                Chat.Print("Project:: " + Player.Instance.ChampionName + " -> Not Support!");
            }
            else
                Chat.Print("Project:: " + Player.Instance.ChampionName + " -> Init Successful!");
        }

        public MyChampions(string name)
        {
            if (!charName.Contains(name))
            {
                return;
            }

            Flash = Me.GetSpellSlotFromName("SummonerFlash");
            Ignite = Me.GetSpellSlotFromName("SummonerDot");

            mainMenu = MainMenu.AddMenu("Project:: " + Player.Instance.ChampionName, "Project:: " + Player.Instance.ChampionName);

            var myChampionsMessage = new MyPluginInit();
        }

        protected MyChampions()
        {
            Game.OnUpdate += OnUpdateEvent;

            AttackableUnit.OnDamage += OnDamageEvent;

            Player.OnIssueOrder += OnIssueOrderEvent;
            Player.OnPostIssueOrder += OnPostIssueOrderEvent;

            GameObject.OnCreate += OnCreateEvent;
            GameObject.OnDelete += OnDeleteEvent;

            Obj_AI_Base.OnBuffGain += OnBuffGainEvent;
            Obj_AI_Base.OnBuffLose += OnBuffLoseEvent;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCastEvent;
            Obj_AI_Base.OnSpellCast += OnSpellCastEvent;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimationEvent;

            Orbwalker.OnPreAttack += OnPreAttackEvent;
            Orbwalker.OnPostAttack += OnPostAttackEvent;
            Orbwalker.OnUnkillableMinion += OnUnkillableMinionEvent;

            Spellbook.OnCastSpell += OnCastSpellEvent;
            Spellbook.OnStopCast += OnStopCastEvent;

            Dash.OnDash += OnDashEvent;
            Interrupter.OnInterruptableSpell += OnInterruptableSpellEvent;
            Gapcloser.OnGapcloser += OnGapcloserEvent;

            Drawing.OnDraw += OnDrawEvent;
        }

        private void OnUpdateEvent(EventArgs Args)
        {
            if (Me.IsDead || Me.IsRecalling())
                return;

            OnUpdate();
        }

        protected virtual void OnUpdate()
        { }

        private void OnDamageEvent(AttackableUnit sender, AttackableUnitDamageEventArgs Args)
        {
            if (Me.IsDead)
                return;

            OnDamage(sender, Args);
        }

        protected virtual void OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        { }

        private void OnIssueOrderEvent(Obj_AI_Base sender, PlayerIssueOrderEventArgs Args)
        {
            if (Me.IsDead || Me.IsRecalling() || !sender.IsMe)
                return;

            OnIssueOrder(Args);
        }

        protected virtual void OnIssueOrder(PlayerIssueOrderEventArgs args)
        { }

        private void OnPostIssueOrderEvent(Obj_AI_Base sender, PlayerIssueOrderEventArgs Args)
        {
            if (Me.IsDead || Me.IsRecalling() || !sender.IsMe)
                return;

            OnPostIssueOrder(Args);
        }

        protected virtual void OnPostIssueOrder(PlayerIssueOrderEventArgs args)
        { }

        private void OnCreateEvent(GameObject sender, EventArgs Args)
        {
            if (Me.IsDead)
                return;

            OnCreate(sender);
        }

        protected virtual void OnCreate(GameObject sender)
        { }

        private void OnDeleteEvent(GameObject sender, EventArgs Args)
        {
            if (Me.IsDead)
                return;

            OnDelete(sender);
        }

        protected virtual void OnDelete(GameObject sender)
        { }

        private void OnBuffGainEvent(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs Args)
        {
            if (Me.IsDead || !sender.IsMe)
                return;

            OnBuffGain(Args);
        }

        protected virtual void OnBuffGain(Obj_AI_BaseBuffGainEventArgs Args)
        { }

        private void OnBuffLoseEvent(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs Args)
        {
            if (Me.IsDead || !sender.IsMe)
                return;

            OnBuffLose(Args);
        }

        protected virtual void OnBuffLose(Obj_AI_BaseBuffLoseEventArgs args)
        { }

        private void OnProcessSpellCastEvent(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs Args)
        {
            if (Me.IsDead)
                return;

            OnProcessSpellCast(sender, Args);
        }

        protected virtual void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs Args)
        { }

        private void OnSpellCastEvent(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs Args)
        {
            if (Me.IsDead || !sender.IsMe)
                return;

            OnSpellCast(Args);
        }

        protected virtual void OnSpellCast(GameObjectProcessSpellCastEventArgs Args)
        { }

        private void OnPlayAnimationEvent(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs Args)
        {
            if (Me.IsDead || !sender.IsMe)
                return;

            OnPlayAnimation(Args);
        }

        protected virtual void OnPlayAnimation(GameObjectPlayAnimationEventArgs Args)
        { }

        private void OnPreAttackEvent(AttackableUnit target, Orbwalker.PreAttackArgs Args)
        {
            if (Me.IsDead || target.IsDead || !target.IsEnemy)
                return;

            OnBeforeAttack(target, Args);

            if (target.Type == GameObjectType.AIHeroClient)
                OnBeforeAttackHero(target as AIHeroClient, Args);

            if (target.Type == GameObjectType.obj_AI_Turret ||
                target.Type == GameObjectType.obj_BarracksDampener ||
                target.Type == GameObjectType.obj_HQ ||
                target.Type == GameObjectType.obj_Turret ||
                target.Type == GameObjectType.obj_Barracks)
                OnBeforeAttackTurret(target, Args);

            if (target.Type == GameObjectType.obj_AI_Minion)
                if (target.Team == GameObjectTeam.Neutral)
                    OnBeforeAttackMob(target as Obj_AI_Minion, Args);
                else
                    OnBeforeAttackMinion(target as Obj_AI_Minion, Args);
        }

        protected virtual void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs Args)
        { }

        protected virtual void OnBeforeAttackHero(AIHeroClient target, Orbwalker.PreAttackArgs Args)
        { }

        protected virtual void OnBeforeAttackTurret(AttackableUnit target, Orbwalker.PreAttackArgs Args)
        { }

        protected virtual void OnBeforeAttackMob(Obj_AI_Minion target, Orbwalker.PreAttackArgs Args)
        { }

        protected virtual void OnBeforeAttackMinion(Obj_AI_Minion target, Orbwalker.PreAttackArgs Args)
        { }

        private void OnPostAttackEvent(AttackableUnit target, EventArgs Args)
        {
            if (Me.IsDead || target.IsDead || !target.IsEnemy)
                return;

            OnAfterAttack(target);

            if (target.Type == GameObjectType.AIHeroClient)
                OnAfterAttackHero(target as AIHeroClient);

            if (target.Type == GameObjectType.obj_AI_Turret ||
                target.Type == GameObjectType.obj_BarracksDampener ||
                target.Type == GameObjectType.obj_HQ ||
                target.Type == GameObjectType.obj_Turret ||
                target.Type == GameObjectType.obj_Barracks)
                OnAfterAttackTurret(target);

            if (target.Type == GameObjectType.obj_AI_Minion)
                if (target.Team == GameObjectTeam.Neutral)
                    OnAfterAttackMob(target as Obj_AI_Minion);
                else
                    OnAfterAttackMinion(target as Obj_AI_Minion);
        }

        protected virtual void OnAfterAttack(AttackableUnit target)
        { }

        protected virtual void OnAfterAttackHero(AIHeroClient target)
        { }

        protected virtual void OnAfterAttackTurret(AttackableUnit target)
        { }

        protected virtual void OnAfterAttackMob(Obj_AI_Minion target)
        { }

        protected virtual void OnAfterAttackMinion(Obj_AI_Minion target)
        { }

        private void OnUnkillableMinionEvent(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs Args)
        {
            if (Me.IsDead || target.IsDead || !target.IsEnemy)
                return;

            OnUnkillableMinion(target, Args.RemainingHealth);
        }

        protected virtual void OnUnkillableMinion(Obj_AI_Base target, float remainingHealth)
        { }

        private void OnCastSpellEvent(Spellbook sender, SpellbookCastSpellEventArgs Args)
        {
            if (Me.IsDead || !sender.Owner.IsMe)
                return;

            OnCastSpell(Args);
        }

        protected virtual void OnCastSpell(SpellbookCastSpellEventArgs Args)
        { }

        private void OnStopCastEvent(Obj_AI_Base sender, SpellbookStopCastEventArgs Args)
        {
            if (Me.IsDead || !sender.IsMe)
                return;

            OnStopCast(Args);
        }

        protected virtual void OnStopCast(SpellbookStopCastEventArgs Args)
        { }

        private void OnDashEvent(Obj_AI_Base sender, Dash.DashEventArgs Args)
        {
            if (sender.Type != GameObjectType.AIHeroClient)
                return;

            OnDash(sender as AIHeroClient, Args);
        }

        protected virtual void OnDash(AIHeroClient sender, Dash.DashEventArgs Args)
        { }

        private void OnInterruptableSpellEvent(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs Args)
        {
            if (Me.IsDead || !sender.IsEnemy || sender.IsDead || sender.Type != GameObjectType.AIHeroClient)
                return;

            OnInterruptableSpell(sender as AIHeroClient, Args);
        }

        protected virtual void OnInterruptableSpell(AIHeroClient sender, Interrupter.InterruptableSpellEventArgs Args)
        { }

        private void OnGapcloserEvent(AIHeroClient sender, Gapcloser.GapcloserEventArgs Args)
        {
            if (Me.IsDead || !sender.IsEnemy || sender.IsDead)
                return;

            OnGapcloser(sender, Args);
        }

        protected virtual void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs Args)
        { }

        private void OnDrawEvent(EventArgs Args)
        {
            if (Me.IsDead)
                return;

            OnDraw();
        }

        protected virtual void OnDraw()
        { }
    }
}
