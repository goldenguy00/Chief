using BepInEx;
using ChiefMod.Modules;
using HarmonyLib;
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using RoR2;

namespace ChiefMod
{
    [BepInDependency("com.rob.Hunk", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ChiefPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "com." + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LONK";
        public const string PluginName = "ChiefMod";
        public const string PluginVersion = "1.0.5";

        public static ChiefPlugin Instance { get; private set; }
        internal static Harmony Harm;

        public void Awake()
        {
            Instance = this;
            Harm = new Harmony(PluginGUID);
            Harm.CreateClassProcessor(typeof(PickupPrefabFix)).Patch();

            Log.Init(Logger);
            ChiefSkin.Init();
            WeaponManager.Init();
            ChiefSkin.OnChiefSkinLoaded += AddWeapons;

            ApplySoundReplacements();
        }
        
        public static void AddWeapons(SkinDef chiefSkin)
        {
            //new PlasmaRifle().Init();

            WeaponManager.AddWeapon<SMGBehavior>(SMG.instance.weaponDef, "mdlSMGHalo", chiefSkin);
            WeaponManager.AddWeapon<MUPBehavior>(MUP.instance.weaponDef, "mdlMUPHalo", chiefSkin);
            WeaponManager.AddWeapon<ARBehavior>(AssaultRifle.instance.weaponDef, "mdlAssaultRifleHalo", chiefSkin);
            WeaponManager.AddWeapon(Shotgun.instance.weaponDef, "mdlShotgunHalo", chiefSkin);
        }

        private void ApplySoundReplacements()
        {
            //Harm.CreateClassProcessor(typeof(EscapeSequence)).Patch();
        }
    }
}
