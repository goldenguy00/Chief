using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using HunkMod.Modules.Components;
using HunkMod.Modules.Survivors;
using HunkMod.Modules.Weapons;
using HunkMod.Modules;
using HunkMod;
using UnityEngine;
using System.Linq;

namespace HaloBrr
{
    [HarmonyPatch]
    public class Fix
    {
        [HarmonyPatch(typeof(HunkMod.MainPlugin), nameof(HunkMod.MainPlugin.LateSetup))]
        [HarmonyPrefix]
        public static bool Patch2() => false;

        [HarmonyPatch(typeof(Hunk), nameof(Hunk.Hook))]
        [HarmonyPrefix]
        public static bool Patch()
        {
            On.RoR2.CameraModes.CameraModeBase.CollectLookInput += Hunk.CameraModeBase_CollectLookInput;
            RoR2.GlobalEventManager.onCharacterDeathGlobal += Hunk.GlobalEventManager_onCharacterDeathGlobal;
            On.RoR2.CharacterBody.Start += Hunk.AddBadgeStart;
            RoR2.Inventory.onInventoryChangedGlobal += Hunk.ValidateBadge;
            RoR2.UI.HUD.onHudTargetChangedGlobal += Hunk.HUDSetup;
            On.RoR2.GlobalEventManager.OnTeamLevelUp += Hunk.GlobalEventManager_OnTeamLevelUp;
            On.RoR2.GlobalEventManager.OnInteractionBegin += Hunk.GlobalEventManager_OnInteractionBegin;
            On.RoR2.DeathRewards.OnKilledServer += Hunk.DeathRewards_OnKilledServer;
            On.RoR2.UI.NotificationUIController.ShowCurrentNotification += Hunk.NotificationUIController_ShowCurrentNotification;
            On.RoR2.ChestBehavior.Open += Hunk.ChestBehavior_Open;
            On.RoR2.ChestBehavior.ItemDrop += Hunk.ChestBehavior_ItemDrop;
            On.RoR2.ChestBehavior.Roll += Hunk.ChestBehavior_Roll;
            On.RoR2.BarrelInteraction.CoinDrop += Hunk.BarrelInteraction_CoinDrop;
            On.RoR2.ShopTerminalBehavior.DropPickup += Hunk.ShopTerminalBehavior_DropPickup;
            On.RoR2.RouletteChestController.EjectPickupServer += Hunk.RouletteChestController_EjectPickupServer;
            On.RoR2.PurchaseInteraction.OnInteractionBegin += Hunk.PurchaseInteraction_OnInteractionBegin;
            On.RoR2.PickupDropletController.OnCollisionEnter += Hunk.PickupDropletController_OnCollisionEnter;
            On.RoR2.SkillLocator.ApplyAmmoPack += Hunk.SkillLocator_ApplyAmmoPack;
            if (Config.fancyShieldGlobal.Value)
            {
                On.RoR2.CharacterModel.UpdateOverlays += Hunk.CharacterModel_UpdateOverlays2;
            }
            else if (Config.fancyShield.Value)
            {
                On.RoR2.CharacterModel.UpdateOverlays += Hunk.CharacterModel_UpdateOverlays;
            }

            On.RoR2.HealthComponent.TakeDamage += Hunk.HealthComponent_TakeDamage;
            On.RoR2.Inventory.ShrineRestackInventory += Hunk.Inventory_ShrineRestackInventory;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddHeartCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddSpadeCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddDiamondCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddClubCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddStarCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddWristbandCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddStarsBadgeCostType;
            RoR2.CostTypeCatalog.modHelper.getAdditionalEntries += Hunk.AddSampleCostType;
            On.RoR2.CharacterBody.OnDeathStart += Hunk.CharacterBody_OnDeathStart;
            On.RoR2.CharacterMaster.OnBodyDeath += Hunk.CharacterMaster_OnBodyDeath;
            On.RoR2.HealthComponent.UpdateLastHitTime += HealthComponent_UpdateLastHitTime;
            On.RoR2.HealthComponent.TakeDamage += Hunk.TVirusDeathOn;
            RoR2.GlobalEventManager.onCharacterDeathGlobal += Hunk.TVirusDeathDefied;
            On.RoR2.SceneDirector.Start += Hunk.SceneDirector_Start;
            On.RoR2.Run.Start += Hunk.Run_Start;
            On.RoR2.UI.ObjectivePanelController.GetObjectiveSources += Hunk.ObjectivePanelController_GetObjectiveSources;
            On.EntityStates.Missions.BrotherEncounter.Phase4.OnEnter += Hunk.Phase4_OnEnter;
            On.RoR2.GenericPickupController.AttemptGrant += Hunk.GenericPickupController_AttemptGrant;
            On.RoR2.UI.HealthBar.Update += Hunk.HealthBar_Update;
            On.RoR2.Util.GetBestBodyName += Hunk.MakeInfectedName;
            On.RoR2.UI.LoadoutPanelController.Rebuild += Hunk.LoadoutPanelController_Rebuild;
            On.EntityStates.Bison.Charge.FixedUpdate += Hunk.Charge_FixedUpdate;
            On.EntityStates.ClayBruiser.Weapon.MinigunFire.FixedUpdate += Hunk.MinigunFire_FixedUpdate;
            On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate += Hunk.WeaponSlam_FixedUpdate;
            On.EntityStates.ParentMonster.GroundSlam.FixedUpdate += Hunk.GroundSlam_FixedUpdate;
            On.EntityStates.BeetleGuardMonster.GroundSlam.FixedUpdate += Hunk.GroundSlam_FixedUpdate1;
            On.EntityStates.JellyfishMonster.JellyNova.FixedUpdate += Hunk.JellyNova_FixedUpdate;
            On.EntityStates.AcidLarva.LarvaLeap.FixedUpdate += Hunk.LarvaLeap_FixedUpdate;
            On.EntityStates.GolemMonster.ChargeLaser.FixedUpdate += Hunk.ChargeLaser_FixedUpdate;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.FixedUpdate += Hunk.Flamebreath_FixedUpdate;
            On.EntityStates.LemurianBruiserMonster.ChargeMegaFireball.FixedUpdate += Hunk.ChargeMegaFireball_FixedUpdate;
            On.EntityStates.ClayGrenadier.FaceSlam.FixedUpdate += Hunk.FaceSlam_FixedUpdate;
            On.EntityStates.MiniMushroom.SporeGrenade.FixedUpdate += Hunk.SporeGrenade_FixedUpdate;
            if (MainPlugin.infernoInstalled)
            {
                On.EntityStates.BeetleMonster.HeadbuttState.FixedUpdate += Hunk.HeadbuttState_FixedUpdate;
            }

            On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter += Hunk.FireMegaFireball_OnEnter;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.FireFlame += Hunk.Flamebreath_FireFlame;
            On.RoR2.EscapeSequenceController.BeginEscapeSequence += Hunk.EscapeSequenceController_BeginEscapeSequence;
            On.RoR2.CharacterBody.OnSkillActivated += Hunk.CharacterBody_OnSkillActivated;
            if (Config.reduceScreenShake.Value)
            {
                On.RoR2.ShakeEmitter.Start += Hunk.ShakeEmitter_Start;
            }

            On.RoR2.InfiniteTowerBossWaveController.OnAllEnemiesDefeatedServer += Hunk.InfiniteTowerBossWaveController_OnAllEnemiesDefeatedServer;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.Awake += Hunk.BaseMainMenuScreen_Awake;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.Update += Hunk.BaseMainMenuScreen_Update;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += Hunk.PlayVisionsAnimation;
            On.EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary.PlayChargeAnimation += Hunk.PlayChargeLunarAnimation;
            On.EntityStates.GlobalSkills.LunarNeedle.ThrowLunarSecondary.PlayThrowAnimation += Hunk.PlayThrowLunarAnimation;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += Hunk.PlayRuinAnimation;
            return false;
        }

        public static void HealthComponent_UpdateLastHitTime(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, RoR2.HealthComponent self, float damageValue, Vector3 damagePosition, bool damageIsSilent, GameObject attacker, bool delayedDamage, bool firstHitOfDelayedDamage)
        {
            orig(self, damageValue, damagePosition, damageIsSilent, attacker, delayedDamage, firstHitOfDelayedDamage);
            if ((bool)self && (bool)self.body && self.body.gameObject.name.Contains("RobNemesis") && self.health <= 0f)
            {
                self.health = 1f;
            }

            if (!self || !self.body || (!self.body.HasBuff(Hunk.infectedBuff2) && !self.body.HasBuff(Hunk.infectedBuff3)) || !(self.health <= 0f) || !self.body.master)
            {
                return;
            }

            TVirusMaster component = self.body.master.GetComponent<TVirusMaster>();
            if ((bool)component && component.revivalCount > 0)
            {
                self.health = 1f;
                return;
            }

            CVirusMaster component2 = self.body.master.GetComponent<CVirusMaster>();
            if ((bool)component2 && component2.revivalCount > 0)
            {
                self.health = 1f;
            }
        }
    }
}
