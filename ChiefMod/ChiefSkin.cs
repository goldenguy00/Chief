using System;
using System.Linq;
using UnityEngine;
using RoR2;
using HunkMod.Modules.Survivors;

namespace ChiefMod
{
    internal static class ChiefSkin
    {
        internal static string skinName = "Master Chief";
        internal static string skinNameToken = "LONK_SKIN_CHIEF_NAME";

        internal static event Action<SkinDef> OnChiefSkinLoaded;
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
            var skinParams = ScriptableObject.CreateInstance<SkinDefParams>();
            var skin = ScriptableObject.CreateInstance<SkinDef>();
            skin.name = skinNameToken;
            skin.nameToken = skinNameToken;
            skin.skinDefParams = skinParams;
            skin.rootObject = modelObject;
            skin.icon = WeaponManager.LoadAsset<Sprite>("Assets/Chief/Skin/Icons/ChiefIcon.png");
            skin.baseSkins = [];

            skinParams.rendererInfos = HG.ArrayUtils.Clone(characterModel.baseRendererInfos);
            skinParams.rendererInfos[0].defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Bodu.mat");
            skinParams.rendererInfos[1].defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Head.mat");

            skinParams.meshReplacements = renderers.Select(r => new SkinDefParams.MeshReplacement { mesh = null, renderer = r }).ToArray();
            skinParams.meshReplacements[0].mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/body.asset");
            skinParams.meshReplacements[1].mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/head.asset");

            HG.ArrayUtils.ArrayAppend(ref modelSkinController.skins, skin);

            OnChiefSkinLoaded?.Invoke(ChiefPlugin.UseGlobalSkins.Value ? null : skin);
        }
    }
}
