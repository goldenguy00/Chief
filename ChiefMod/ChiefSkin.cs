using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx.Logging;
using MonoMod.RuntimeDetour.HookGen;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine;
using RoR2;
using HunkMod.Modules;
using HunkMod.Modules.Survivors;

namespace ChiefMod
{
    internal static class ChiefSkin
    {
        internal static string skinName = "Master Chief";
        internal static string skinNameToken = "LONK_SKIN_CHIEF_NAME";

        public static void Init()
        {
            BodyCatalog.availability.CallWhenAvailable(AddRobHunkBodyChiefSkin);
            On.RoR2.Language.LoadStrings += Language_LoadStrings;
        }

        private static void Language_LoadStrings(On.RoR2.Language.orig_LoadStrings orig, Language self)
        {
            orig(self);
            self.SetStringByToken(skinNameToken, skinName);
        }

        public static void AddRobHunkBodyChiefSkin()
        {
            GameObject bodyObject = BodyCatalog.FindBodyPrefab(Hunk.bodyName);
            if (!bodyObject)
            {
                Log.Warning("Failed to add \"" + skinName + "\" skin because \"" + Hunk.bodyName + "\" doesn't exist");
                return;
            }

            ModelLocator modelLoc = bodyObject.GetComponent<ModelLocator>();
            if (!modelLoc)
            {
                Log.Warning("Failed to add \"" + skinName + "\" skin to \"" + Hunk.bodyName + "\" because it doesn't have \"ModelLocator\" component");
                return;
            }

            GameObject modelObject = modelLoc.modelTransform.gameObject;
            ModelSkinController modelSkinController = (modelObject ? modelObject.GetComponent<ModelSkinController>() : null);
            if (!modelSkinController)
            {
                Log.Warning("Failed to add \"" + skinName + "\" skin to \"" + Hunk.bodyName + "\" because it doesn't have \"ModelSkinController\" component");
                return;
            }

            CharacterModel characterModel = modelObject.GetComponent<CharacterModel>();
            if (!characterModel)
            {
                Log.Warning("Failed to add \"" + skinName + "\" skin to \"" + Hunk.bodyName + "\" because it doesn't have \"CharacterModel\" component");
                return;
            }

            Renderer[] renderers = characterModel.baseRendererInfos.Select(info => info.renderer).ToArray();

            var skin = Skins.CreateSkinDef(skinNameToken, WeaponManager.LoadAsset<Sprite>("Assets/Chief/Skin/Icons/ChiefIcon.png"),
            [
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Bodu.mat"),
                    defaultShadowCastingMode = ShadowCastingMode.On,
                    ignoreOverlays = false,
                    renderer = renderers[0]
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Head.mat"),
                    defaultShadowCastingMode = ShadowCastingMode.On,
                    ignoreOverlays = false,
                    renderer = renderers[1]
                }
            ], characterModel.mainSkinnedMeshRenderer, modelObject);

            skin.meshReplacements =
            [
                new SkinDef.MeshReplacement
                    {
                        mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/body.asset"),
                        renderer = renderers[0]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/head.asset"),
                        renderer = renderers[1]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = null,
                        renderer = renderers[2]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = null,
                        renderer = renderers[3]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = null,
                        renderer = renderers[4]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = null,
                        renderer = renderers[5]
                    },
                    new SkinDef.MeshReplacement
                    {
                        mesh = null,
                        renderer = renderers[6]
                    }
            ];

            Array.Resize(ref modelSkinController.skins, modelSkinController.skins.Length + 1);
            modelSkinController.skins[^1] = skin;
            BodyCatalog.skins[(int)BodyCatalog.FindBodyIndex(bodyObject)] = modelSkinController.skins;

            WeaponManager.AddWeaponSkins(skin);
        }

        public static void ConvertAllRenderersToHopooShader(GameObject objectToConvert, bool onlyMeshes = true)
        {
            foreach (Renderer renderer in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                if (!renderer || !renderer.material)
                {
                    continue;
                }

                if (onlyMeshes)
                {
                    if (!renderer.GetComponent<LineRenderer>() && !renderer.GetComponent<TrailRenderer>() && !renderer.GetComponent<ParticleSystemRenderer>())
                    {
                        ConvertAllRenderersToHopooShader(renderer);
                    }
                }
                else
                {
                    ConvertAllRenderersToHopooShader(renderer);
                }
            }
        }

        public static void ConvertAllRenderersToHopooShader(Renderer renderer)
        {
            Material[] materials = renderer.materials;
            foreach (var material in materials)
            {
                if (material)
                {
                    Log.Warning("Converting material " + material.name);
                    HunkAssets.ConvertMaterial(material);
                }
            }
        }
    }
}
