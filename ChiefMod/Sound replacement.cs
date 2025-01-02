using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using HunkMod.Modules.Components;

namespace ChiefMod
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
