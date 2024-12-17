using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using HunkMod.Modules.Weapons;
using UnityEngine;

namespace HaloBrr
{
    /// <summary>
    /// trying to comprehend this class is a health risk, don't do it
    /// </summary>
    public static class WeaponManager
    {
        internal static readonly Dictionary<string, GameObject> modelPrefabsByName = [];
        internal static readonly Dictionary<Type, string> prefabNamesByType = [];

        internal static void Init()
        {
            var bundleFiles = Directory.EnumerateFiles(Path.GetDirectoryName(HaloBrrPlugin.Instance.Info.Location), "*", SearchOption.AllDirectories).Where(p => !Path.HasExtension(p));

            foreach (var bundleName in bundleFiles)
            {
                AssetBundle.LoadFromFileAsync(bundleName).completed += o =>
                {
                    var bundle = (o as AssetBundleCreateRequest).assetBundle;

                    if (bundle != null)
                    {
                        Log.Warning($"Loading assetbundle {bundle.name}...");

                        foreach (var asset in bundle.LoadAllAssets<GameObject>())
                        {
                            Log.Warning($"Found {asset.name}");
                            modelPrefabsByName[asset.name] = asset;
                        }
                    }
                    else
                    {
                        Log.ErrorAssetBundle(bundleName);
                    }
                };
            }
        }

        public static void FinishPatch()
        {
            if (HunkMod.MainPlugin.MODVERSION == "2.4.4")
                HaloBrrPlugin.Harm.CreateClassProcessor(typeof(Fix)).Patch();

            if (prefabNamesByType.Any())
            {
                HaloBrrPlugin.Harm.CreateClassProcessor(typeof(ReplacePrefab)).Patch();
                HaloBrrPlugin.Harm.CreateClassProcessor(typeof(ReplacePickup)).Patch();
            }
            else
                Log.Error("No replacements found, did you forget to add it?");
        }

        public static void AddWeapon<T>(string prefabReplacementName) where T : BaseWeapon
        {
            prefabNamesByType[typeof(T)] = prefabReplacementName;

            Log.Warning($"Replacing {typeof(T).Name}.{nameof(BaseWeapon.modelPrefab)} with {prefabReplacementName}");
        }

        internal static GameObject LoadModelPrefab(MethodBase propertyInfo, bool isPickup = false)
        {
            var modelName = prefabNamesByType[propertyInfo.DeclaringType];
            if (isPickup)
                modelName += "Pickup";

            if (modelPrefabsByName.TryGetValue(modelName, out var modelPrefab) && modelPrefab != null)
            {
                Log.Warning($"Successfully replaced {propertyInfo.DeclaringType.Name} with new model {modelPrefab.name}");
                return modelPrefab;
            }

            Log.ErrorAsset(modelName);
            return null;
        }

    }

    [HarmonyPatch]
    public class ReplacePrefab
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods() => WeaponManager.prefabNamesByType.Keys.Select(t => AccessTools.PropertyGetter(t, nameof(BaseWeapon.modelPrefab)));

        [HarmonyPrefix]
        public static bool Prefix(MethodBase __originalMethod, ref GameObject __result) => (__result = WeaponManager.LoadModelPrefab(__originalMethod)) == null;
    }

    [HarmonyPatch]
    public class ReplacePickup
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods() => WeaponManager.prefabNamesByType.Keys.Select(t => AccessTools.Method(t, nameof(BaseWeapon.Init)));

        [HarmonyPostfix]
        public static void Postfix(MethodBase __originalMethod, BaseWeapon __instance) => __instance.itemDef.pickupModelPrefab = WeaponManager.LoadModelPrefab(__originalMethod, true);
    }
}
