using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HunkMod.Modules.Components;
using HunkMod.Modules.Survivors;
using RoR2;
using UnityEngine;

namespace ChiefMod.Modules
{
    [HarmonyPatch]
    internal class PickupPrefabFix
    {
        public static readonly Dictionary<ushort, (GameObject oldPrefab, GameObject newPrefab)> prefabsByWeaponIndex = [];
         
        public static void AddPair(HunkWeaponDef weaponDef, GameObject newPrefab)
        {
            prefabsByWeaponIndex[weaponDef.index] = (weaponDef.itemDef.pickupModelPrefab, newPrefab);
        }

        [HarmonyPatch(typeof(HunkController), nameof(HunkController.Start))]
        [HarmonyPostfix]
        public static void ReplacePickupModel(HunkController __instance)
        {
            if (Hunk.gunSkins.Count == 0 || !__instance.modelSkinController || !__instance.characterBody)
                return;

            if (!__instance.characterBody.isPlayerControlled || !__instance.characterBody.hasEffectiveAuthority)
                return;

            var currentSkinDef = __instance.modelSkinController.skins[__instance.characterBody.skinIndex];
            foreach (Hunk.HunkGunSkin gunSkin in Hunk.gunSkins)
            {
                if (prefabsByWeaponIndex.TryGetValue(gunSkin.weaponDef.index, out var weaponPair))
                {
                    var prefab = (!gunSkin.skinDef || gunSkin.skinDef == currentSkinDef) ? weaponPair.newPrefab : weaponPair.oldPrefab;
                    gunSkin.weaponDef.itemDef.pickupModelPrefab = prefab;

                    var pickupDef = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(gunSkin.weaponDef.itemDef.itemIndex));
                    pickupDef.displayPrefab = prefab;
                }
            }
        }
    }
}
