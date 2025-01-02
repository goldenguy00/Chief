using System.Security.Permissions;
using System.Security;
using BepInEx;
using HarmonyLib;
using System.Runtime.CompilerServices;
using UnityEngine.AddressableAssets;
using RoR2;
using System;
using BepInEx.Configuration;
using HunkMod;
using UnityEngine;
using HunkMod.Modules.Weapons;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using UnityEngine.UIElements;
using System.Reflection;
using HunkMod.Modules.Survivors;
using HunkMod.Modules;
using HunkMod.Modules.Components;
using System.Collections.Generic;
using Chief;
using System.Linq;

namespace HunkHalo
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class HunkHaloPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "_" + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "score";
        public const string PluginName = "HunkHalo";
        public const string PluginVersion = "0.0.1";

        public static HunkHaloPlugin Instance { get; private set; }
        internal static Harmony Harm;

        public void Awake()
        {
            Instance = this;
            Harm = new Harmony(PluginGUID);

            Log.Init(Logger);

            ApplySoundReplacements();
            AddWeaponReskins();
        }

        private void ApplySoundReplacements()
        {
            Harm.CreateClassProcessor(typeof(EscapeSequence)).Patch();
        }

        private void AddWeaponReskins()
        {
            WeaponManager.Init();

            WeaponManager.AddWeapon<SMG>("mdlSMGHalo");
            WeaponManager.AddWeapon<MUP>("mdlMUPHalo");

            WeaponManager.FinishPatch();
        }
    }
}
