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

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace HaloBrr
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class HaloBrrPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com." + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "score";
        public const string PluginName = "HaloBrrr";
        public const string PluginVersion = "0.0.1";

        public void Awake()
        {
            // dont care
            // also didnt ask
            var assetBundle = new AssetBundle();

            SMG.instance.weaponDef.modelPrefab = assetBundle.LoadAsset<GameObject>("HaloSmg");
        }
    }
}
