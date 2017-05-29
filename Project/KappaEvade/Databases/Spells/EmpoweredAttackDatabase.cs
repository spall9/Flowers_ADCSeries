﻿namespace Project_Team.KappaEvade.Databases.Spells
{
    using SpellData;

    using EloBuddy;
    using EloBuddy.SDK;

    using System.Collections.Generic;
    using System.Linq;

    public static class EmpowerdAttackDatabase
    {
        public static List<EmpoweredAttackData> Current;

        static EmpowerdAttackDatabase()
        {
            if (Current != null)
                return;

            Current = List.FindAll(s => s.Hero == Champion.Unknown || EntityManager.Heroes.AllHeroes.Any(h => s.Hero.Equals(h.Hero)));
        }

        private static readonly List<EmpoweredAttackData> List = new List<EmpoweredAttackData>
            {
                new EmpoweredAttackData
                    {
                        Hero = Champion.Unknown,
                        Slot = SpellSlot.Unknown,
                        DisplayName = "Braum Stun",
                        TargetRequiredBuff = "BraumMark",
                        TargetRequiredBuffCount = 3,
                        CrowdControl = true,
                        DangerLevel = 4
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Unknown,
                        Slot = SpellSlot.Unknown,
                        DisplayName = "Nami E",
                        RequireBuff = "NamiE",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Ashe,
                        Slot = SpellSlot.Q,
                        AttackName = "AsheQAttack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Blitzcrank,
                        Slot = SpellSlot.E,
                        AttackName = "PowerFistAttack",
                        CrowdControl = true,
                        DangerLevel = 4
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Caitlyn,
                        Slot = SpellSlot.Unknown,
                        AttackName = "CaitlynHeadshotMissile",
                        DisplayName = "Headshot",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Camille,
                        Slot = SpellSlot.Q,
                        AttackName = "CamilleQAttack",
                        DisplayName = "Normal",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Camille,
                        Slot = SpellSlot.Q,
                        AttackName = "CamilleQAttackEmpowered",
                        DisplayName = "Empowered",
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Darius,
                        Slot = SpellSlot.W,
                        AttackName = "DariusNoxianTacticsONHAttack",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Diana,
                        Slot = SpellSlot.Unknown,
                        AttackName = "DianaBasicAttack3",
                        DisplayName = "Moonsilver Blade",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Draven,
                        Slot = SpellSlot.Q,
                        AttackName = "DravenSpinningAttack",
                        DisplayName = "Axe",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.DrMundo,
                        Slot = SpellSlot.E,
                        AttackName = "MasochismAttack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Ekko,
                        Slot = SpellSlot.Unknown,
                        AttackName = "ekkobasicattackp3",
                        DisplayName = "Passive",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Ekko,
                        Slot = SpellSlot.E,
                        AttackName = "EkkoEAttack",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Fiora,
                        Slot = SpellSlot.E,
                        AttackName = "FioraEAttack",
                        DisplayName = "First E",
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Fizz,
                        Slot = SpellSlot.W,
                        AttackName = "FizzWBasicAttack",
                        DisplayName = "Attack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Garen,
                        Slot = SpellSlot.Q,
                        AttackName = "GarenQAttack",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Gangplank,
                        Slot = SpellSlot.Unknown,
                        RequireBuff = "gangplankpassiveattack",
                        DisplayName = "Passive",
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Galio,
                        Slot = SpellSlot.Unknown,
                        AttackName = "GalioPassiveAttack",
                        DisplayName = "Passive",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Gnar,
                        Slot = SpellSlot.W,
                        TargetRequiredBuff = "gnarwproc",
                        TargetRequiredBuffCount = 2,
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Gragas,
                        Slot = SpellSlot.W,
                        AttackName = "DrunkenRage",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Hecarim,
                        Slot = SpellSlot.E,
                        AttackName = "hecarimrampattack",
                        CrowdControl = true,
                        DangerLevel = 4
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Illaoi,
                        Slot = SpellSlot.W,
                        AttackName = "illaoiwattack",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.JarvanIV,
                        Slot = SpellSlot.Unknown,
                        AttackName = "JarvanIVMartialCadenceAttack",
                        DisplayName = "Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Jayce,
                        Slot = SpellSlot.Unknown,
                        AttackName = "JaycePassiveMeleeAttack",
                        DisplayName = "Melee Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Jayce,
                        Slot = SpellSlot.Unknown,
                        AttackName = "JaycePassiveRangedAttack",
                        DisplayName = "Ranged Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Jayce,
                        Slot = SpellSlot.W,
                        AttackName = "JayceHyperChargeRangedAttack",
                        DisplayName = "Attack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Jax,
                        Slot = SpellSlot.Unknown,
                        RequireBuff = "JaxEmpowerTwo",
                        DisplayName = "Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Jhin,
                        Slot = SpellSlot.Unknown,
                        AttackName = "JhinPassiveAttack",
                        DisplayName = "Passive Crit",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Kassadin,
                        Slot = SpellSlot.W,
                        AttackName = "KassadinBasicAttack3",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Kennen,
                        Slot = SpellSlot.W,
                        AttackName = "KennenMegaProc",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Kindred,
                        Slot = SpellSlot.E,
                        AttackName = "KindredBasicAttackOverrideLightbombFinal",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Leona,
                        Slot = SpellSlot.Q,
                        AttackName = "LeonaShieldOfDaybreakAttack",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Lux,
                        Slot = SpellSlot.Unknown,
                        TargetRequiredBuff = "LuxIlluminatingFraulein",
                        DisplayName = "Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.MasterYi,
                        Slot = SpellSlot.Unknown,
                        AttackName = "MasterYiDoubleStrike",
                        DisplayName = "Passive",
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.MonkeyKing,
                        Slot = SpellSlot.Q,
                        AttackName = "MonkeyKingQAttack",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Mordekaiser,
                        Slot = SpellSlot.Q,
                        AttackName = "mordekaiserqattack2",
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Nasus,
                        Slot = SpellSlot.Q,
                        AttackName = "NasusQAttack",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Nautilus,
                        Slot = SpellSlot.Unknown,
                        AttackName = "NautilusRavageStrikeAttack",
                        DisplayName = "Passive",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Nidalee,
                        Slot = SpellSlot.Q,
                        AttackName = "NidaleeTakedownAttack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Poppy,
                        Slot = SpellSlot.Unknown,
                        AttackName = "PoppyPassiveAttack",
                        RequireBuff = "poppypassivebuff",
                        DisplayName = "Passive",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Quinn,
                        Slot = SpellSlot.W,
                        AttackName = "QuinnWEnhanced",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.RekSai,
                        Slot = SpellSlot.W,
                        RequireBuff = "RekSaiW",
                        DontHaveBuff = "reksaiknockupimmune",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Renekton,
                        Slot = SpellSlot.W,
                        AttackName = "RenektonExecute",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Renekton,
                        Slot = SpellSlot.W,
                        AttackName = "RenektonSuperExecute",
                        DisplayName = "Fury W",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Skarner,
                        Slot = SpellSlot.E,
                        TargetRequiredBuff = "SkarnerPassiveBuff",
                        TargetRequiredBuffCount = 1,
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Sona,
                        Slot = SpellSlot.Q,
                        RequireBuff = "sonaqprocattacker",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Sona,
                        Slot = SpellSlot.Unknown,
                        AttackName = "SonaQAttackUpgrade",
                        DisplayName = "Passive Empowered Q",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Sona,
                        Slot = SpellSlot.Unknown,
                        AttackName = "SonaWAttackUpgrade",
                        DisplayName = "Passive Empowered W",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Sona,
                        Slot = SpellSlot.Unknown,
                        AttackName = "SonaEAttackUpgrade",
                        DisplayName = "Passive Empowered E",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Shen,
                        Slot = SpellSlot.Q,
                        AttackName = "ShenQAttack",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Shyvana,
                        Slot = SpellSlot.Q,
                        AttackName = "ShyvanaDoubleAttackHit",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Sejuani,
                        Slot = SpellSlot.W,
                        AttackName = "SejuaniBasicAttackW",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Tristana,
                        Slot = SpellSlot.E,
                        DisplayName = "Explode",
                        TargetRequiredBuff = "tristanaecharge",
                        TargetRequiredBuffCount = 3,
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Trundle,
                        Slot = SpellSlot.Q,
                        AttackName = "TrundleQ",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.TwistedFate,
                        Slot = SpellSlot.W,
                        AttackName = "goldcardpreattack",
                        DisplayName = "Gold Card",
                        CrowdControl = true,
                        DangerLevel = 4
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.TwistedFate,
                        Slot = SpellSlot.W,
                        AttackName = "redcardpreattack",
                        DisplayName = "Red Card",
                        CrowdControl = true,
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.TwistedFate,
                        Slot = SpellSlot.W,
                        AttackName = "bluecardpreattack",
                        DisplayName = "Blue Card",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Udyr,
                        Slot = SpellSlot.E,
                        DisplayName = "Stun",
                        RequireBuff = "UdyrBearStance",
                        DontHaveBuff = "UdyrBearStunCheck",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Vayne,
                        Slot = SpellSlot.W,
                        TargetRequiredBuff = "vaynesilvereddebuff",
                        TargetRequiredBuffCount = 2,
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Vi,
                        Slot = SpellSlot.W,
                        TargetRequiredBuff = "viwproc",
                        TargetRequiredBuffCount = 2,
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Vi,
                        Slot = SpellSlot.E,
                        AttackName = "ViEAttack",
                        DangerLevel = 2
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Viktor,
                        Slot = SpellSlot.Q,
                        AttackName = "viktorqbuff",
                        DangerLevel = 1
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Volibear,
                        Slot = SpellSlot.Q,
                        AttackName = "VolibearQAttack",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.XinZhao,
                        Slot = SpellSlot.Q,
                        AttackName = "XenZhaoThrust3",
                        CrowdControl = true,
                        DangerLevel = 3
                    },
                new EmpoweredAttackData
                    {
                        Hero = Champion.Yorick,
                        Slot = SpellSlot.Q,
                        AttackName = "YorickQAttack",
                        DangerLevel = 1
                    },
            };
    }
}
