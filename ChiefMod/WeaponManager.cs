using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChiefMod.Modules;
using HunkMod.Modules.Survivors;
using RoR2;
using UnityEngine;

namespace ChiefMod
{
    /// <summary>
    /// trying to comprehend this class is a health risk, don't do it
    /// </summary>
    internal static class WeaponManager
    {
        internal static readonly Dictionary<string, AssetBundle> loadedBundles = [];
        internal static readonly Dictionary<string, IEnumerable<string>> loadedBundleAssetNames = [];

        internal static event Action OnLoadCompleted;

        internal static void Init()
        {
            var bundleFiles = Directory.EnumerateFiles(System.IO.Path.GetDirectoryName(ChiefPlugin.Instance.Info.Location), "*", SearchOption.AllDirectories).Where(p => !System.IO.Path.HasExtension(p));
            var bundleCount = bundleFiles.Count();

            foreach (var bundleName in bundleFiles)
            {
                AssetBundle.LoadFromFileAsync(bundleName).completed += o =>
                {
                    var bundle = (o as AssetBundleCreateRequest).assetBundle;

                    if (bundle != null)
                    {
                        Log.Debug($"Loading assetbundle {bundle.name}...");

                        loadedBundles[bundle.name] = bundle;
                        loadedBundleAssetNames[bundle.name] = bundle.GetAllAssetNames().AsEnumerable();

                        if (loadedBundles.Count == bundleCount)
                            OnLoadCompleted?.Invoke();
                    }
                    else
                    {
                        Log.ErrorAssetBundle(bundleName);
                    }
                };
            }
        }

        public static void AddWeapon<T>(HunkWeaponDef weaponDef, string prefabReplacementName, SkinDef chiefSkin = null) where T : MonoBehaviour
        {
            if (!prefabReplacementName.StartsWith("Assets/Chief/Weapons/"))
                prefabReplacementName = "Assets/Chief/Weapons/" + prefabReplacementName;

            var weaponPrefab = LoadAsset<GameObject>(prefabReplacementName + ".prefab");
            if (!weaponPrefab)
            {
                Log.ErrorAsset(prefabReplacementName);
                return;
            }
            weaponPrefab.AddComponent<T>();

            Log.Info($"Replacing {weaponDef.name} with {prefabReplacementName}");

            Hunk.AddGunSkin(chiefSkin, weaponDef, weaponPrefab);
            AddPickupPrefab(weaponDef, prefabReplacementName);
        }

        public static void AddWeapon(HunkWeaponDef weaponDef, string prefabReplacementName, SkinDef chiefSkin = null)
        {
            if (!prefabReplacementName.StartsWith("Assets/Chief/Weapons/"))
                prefabReplacementName = "Assets/Chief/Weapons/" + prefabReplacementName;

            var weaponPrefab = LoadAsset<GameObject>(prefabReplacementName + ".prefab");
            if (!weaponPrefab)
            {
                Log.ErrorAsset(prefabReplacementName);
                return;
            }

            Log.Info($"Replacing {weaponDef.name} with {prefabReplacementName}");

            Hunk.AddGunSkin(chiefSkin, weaponDef, weaponPrefab);
            AddPickupPrefab(weaponDef, prefabReplacementName);
        }

        private static void AddPickupPrefab(HunkWeaponDef weaponDef, string prefabReplacementName)
        {
            var weaponPrefab = LoadAsset<GameObject>(prefabReplacementName + "Pickup.prefab");
            if (!weaponPrefab)
            {
                Log.ErrorAsset(prefabReplacementName);
                return;
            }

            PickupPrefabFix.AddPair(weaponDef, weaponPrefab);
        }

        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            foreach (var kvp in loadedBundleAssetNames)
            {
                if (kvp.Value.Contains(path.ToLower()))
                {
                    return loadedBundles[kvp.Key].LoadAsset<T>(path);
                }
            }

            Log.ErrorAsset(path);

            Log.Warning("Current asset paths");
            foreach (var name in loadedBundleAssetNames.SelectMany(kvp => kvp.Value))
                Log.Error(name);

            return null;
        }
    }
}
