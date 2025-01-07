using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using HunkMod.Modules;
using HunkMod.Modules.Survivors;
using HunkMod.Modules.Weapons;
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
                        Log.Warning($"Loading assetbundle {bundle.name}...");

                        loadedBundles[bundle.name] = bundle;
                        loadedBundleAssetNames[bundle.name] = bundle.GetAllAssetNames().AsEnumerable();

                        bundle.LoadAllAssetsAsync().completed += o =>
                        {
                            foreach (var asset in (o as AssetBundleRequest).allAssets)
                            {
                                if (asset is GameObject go)
                                    ChiefSkin.ConvertAllRenderersToHopooShader(go);
                                else if (asset is Material mat)
                                    HunkAssets.ConvertMaterial(mat);
                            }

                            if (loadedBundles.Count == bundleCount)
                                OnLoadCompleted?.Invoke();

                        };
                    }
                    else
                    {
                        Log.ErrorAssetBundle(bundleName);
                    }
                };
            }
        }

        public static void AddWeapon(HunkWeaponDef weaponDef, string prefabReplacementName)
        {
            if (!prefabReplacementName.StartsWith("Assets/Chief/Weapons/"))
                prefabReplacementName = "Assets/Chief/Weapons/" + prefabReplacementName;

            if (!prefabReplacementName.EndsWith(".prefab"))
                prefabReplacementName += ".prefab";

            var weaponPrefab = LoadAsset<GameObject>(prefabReplacementName);
            if (!weaponPrefab)
            {
                Log.ErrorAsset(prefabReplacementName);
                return;
            }

            Log.Warning($"Replacing {weaponDef.name} with {prefabReplacementName}");

            Hunk.AddGunSkin(null, weaponDef, weaponPrefab);
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
