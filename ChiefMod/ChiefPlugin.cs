using BepInEx;
using HarmonyLib;
using HunkMod.Modules.Weapons;

namespace ChiefMod
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ChiefPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = "_" + PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LONK";
        public const string PluginName = "ChiefMod";
        public const string PluginVersion = "1.0.4";

        public static ChiefPlugin Instance { get; private set; }
        internal static Harmony Harm;

        public void Awake()
        {
            Instance = this;
            Harm = new Harmony(PluginGUID);

            Log.Init(Logger);

            //ApplySoundReplacements();
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
            WeaponManager.AddWeapon<AssaultRifle>("mdlAssaultRifleUNSC");
            WeaponManager.AddWeapon<Shotgun>("mdlShotgunHalo");

            WeaponManager.FinishPatch();
        }
    }
}
