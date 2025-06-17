using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var directoryName = System.IO.Path.GetDirectoryName(ChiefPlugin.Instance.Info.Location);
            var bundleFiles = Directory.EnumerateFiles(directoryName, "*", SearchOption.AllDirectories).Where(p => !System.IO.Path.HasExtension(p));

            var bundleCount = 0;
            foreach (var bundleName in bundleFiles)
            {
                bundleCount++;
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
            
            if (!weaponPrefab.TryGetComponent<ModelPanelParameters>(out var mdlParams))
                mdlParams = weaponPrefab.AddComponent<ModelPanelParameters>();

            var parent = weaponPrefab.transform;
            if (!mdlParams.focusPointTransform)
            {
                mdlParams.focusPointTransform = parent.Find("FocusPoint") ?? new GameObject("FocusPoint").transform;
                mdlParams.focusPointTransform.SetParent(parent);
            }

            if (!mdlParams.cameraPositionTransform)
            {
                mdlParams.cameraPositionTransform = parent.Find("CameraPosition") ?? new GameObject("CameraPosition").transform;
                mdlParams.cameraPositionTransform.SetParent(weaponPrefab.transform);
            }

            PickupPrefabFix.AddPair(weaponDef, weaponPrefab);
        }
    }
}
