using HarmonyLib;
using HunkMod.Modules.Components;
using RoR2;

namespace ChiefMod
{
    // Class is a hook class
    // This is necessary
    [HarmonyPatch]
    internal class EscapeSequence
    {
        // Apply hook        Class             Method
        [HarmonyPatch(typeof(HunkController), nameof(HunkController.StartBGM))]
        // Run code before original
        [HarmonyPrefix]
        public static bool MusicReplacement(HunkController __instance)
        {
            // Your method now runs before the original HunkController.StartBGM
            //
            //  REQUIRED:
            //      public static bool
            //
            //      HarmonyPatch(typeof(CLASS_NAME), nameof(CLASS_NAME.METHOD_NAME))
            //
            //      return false;
            //
            // OPTIONAL:
            //
            //      HunkController __instance
            //          gives you a reference to the instance that called this method aka the HunkController component
            //          this is attached to the hunk body that will need to hear the sounds

            // Play sound here!
            // You can right click the method name of HunkController.StartBGM and select Peek/Goto Definition to see what the original method does.
            //__instance.bgmPlayID = Util.PlaySound("Halo_Sound_String", __instance.gameObject);

            // true = run original method after
            // false = do not run original method
            return false;
        }//
    }
}
