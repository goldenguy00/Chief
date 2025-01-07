using HunkMod.Modules.Weapons;
using RoR2.Skills;
using UnityEngine;

namespace ChiefMod.Modules
{
    internal class PlasmaRifle : BaseWeapon<PlasmaRifle>
    {
        public override string weaponNameToken => "";
        public override string weaponNameTokenFull => base.weaponNameTokenFull;
        public override string weaponDescToken => base.weaponDescToken;
        public override string weaponName => "";
        public override string weaponDesc => "";
        public override string iconName => "";
        public override GameObject crosshairPrefab => _crosshairPrefab;
        public override int magSize => 0;
        public override float magPickupMultiplier => 0;
        public override int startingMags => 0;
        public override float reloadDuration => 0;
        public override string ammoName => "";
        public override SkillDef primarySkillDef => _primarySkillDef;
        public override GameObject modelPrefab => WeaponManager.LoadAsset<GameObject>("mdlPlasmaRifle");
        public override HunkWeaponDef.AnimationSet animationSet => HunkWeaponDef.AnimationSet.SMG;
        public override bool storedOnBack => false;
        public override bool storedOnHolster => true;
        public override float damageFillValue => 0;
        public override float rangefillValue => 0;
        public override float fireRateFillValue => 0;
        public override float reloadFillValue => 0;
        public override float accuracyFillValue => 0;
        public override string aimSoundString => "";
        public override string equipSoundString => "";

        private SkillDef _primarySkillDef;
        private GameObject _crosshairPrefab;

        public PlasmaRifle() : base()
        {

        }

        public override void Init()
        {
            base.Init();
        }
    }
}
