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

            var rendererInfos = HG.ArrayUtils.Clone(characterModel.baseRendererInfos);
            rendererInfos[0].defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Bodu.mat");
            rendererInfos[1].defaultMaterial = WeaponManager.LoadAsset<Material>("Assets/Chief/Skin/Materials/Head.mat");

            var meshReplacements = characterModel.baseRendererInfos.Select(info => new SkinDefParams.MeshReplacement { mesh = null, renderer = info.renderer }).ToArray();
            meshReplacements[0].mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/body.asset");
            meshReplacements[1].mesh = WeaponManager.LoadAsset<Mesh>("Assets/Chief/Skin/Meshes/head.asset");

            var skin = R2API.Skins.CreateNewSkinDef(new R2API.SkinDefParamsInfo
            {
                Name = skinNameToken,
                NameToken = skinNameToken,
                SkinDefParams = null,
                RootObject = modelObject,
                Icon = WeaponManager.LoadAsset<Sprite>("Assets/Chief/Skin/Icons/ChiefIcon.png"),
                BaseSkins = [],
                UnlockableDef = null,
                MinionSkinReplacements = [],
                GameObjectActivations = [],
                ProjectileGhostReplacements = [],
                MeshReplacements = meshReplacements,
                RendererInfos = rendererInfos
            });

            HG.ArrayUtils.ArrayAppend(ref modelSkinController.skins, skin);

            OnChiefSkinLoaded?.Invoke(ChiefPlugin.UseGlobalSkins.Value ? null : skin);
        }
    }
}
