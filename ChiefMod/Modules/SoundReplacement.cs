using HarmonyLib;
using HunkMod.Modules.Components;

namespace ChiefMod.Modules
{
    [HarmonyPatch(typeof(HunkDialogue))]
    internal class Sound_replacement
    {
        [HarmonyPatch(nameof(HunkDialogue.Step1))]
        [HarmonyPrefix]

        public static bool Replacestep1()
        {
            return false;
        }
    }
}
