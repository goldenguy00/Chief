// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkWeaponDef
using HunkMod;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;

[CreateAssetMenu(fileName = "wpn", menuName = "ScriptableObjects/WeaponDef", order = 1)]
public class HunkWeaponDef : ScriptableObject
{
    public enum AnimationSet
    {
        Pistol,
        SMG,
        Rocket,
        Unarmed,
        Throwable,
        PistolAlt,
        Railgun,
        Jacket
    }

    [Header("General")]
    public string nameToken = "";

    public string descriptionToken = "";

    public Sprite icon = null;

    public GameObject crosshairPrefab = null;

    public int magSize = 8;

    public float magPickupMultiplier = 1f;

    public int startingMags = 1;

    public float reloadDuration = 2.4f;

    public string ammoName = "";

    public bool allowAutoReload = true;

    public bool exposeWeakPoints = true;

    public bool roundReload = false;

    public bool canPickUpAmmo = true;

    public bool isInfinite = false;

    public bool hasCrosshair = true;

    [Header("Skills")]
    public SkillDef primarySkillDef;

    [Header("Visuals")]
    public GameObject modelPrefab;

    public AnimationSet animationSet = AnimationSet.SMG;

    public bool storedOnBack = true;

    public bool storedOnHolster = false;

    public string aimSoundString = "sfx_hunk_smg_aim";

    public string equipSoundString = "sfx_hunk_equip_smg";

    [Header("UI")]
    public float damageFillValue;

    public float rangefillValue;

    public float fireRateFillValue;

    public float reloadFillValue;

    public float accuracyFillValue;

    [HideInInspector]
    public ushort index;

    [HideInInspector]
    public ItemDef itemDef;

    public static HunkWeaponDef CreateWeaponDefFromInfo(HunkWeaponDefInfo weaponDefInfo)
    {
        HunkWeaponDef hunkWeaponDef = (HunkWeaponDef)ScriptableObject.CreateInstance(typeof(HunkWeaponDef));
        hunkWeaponDef.name = weaponDefInfo.nameToken;
        hunkWeaponDef.nameToken = weaponDefInfo.nameToken;
        hunkWeaponDef.descriptionToken = weaponDefInfo.descriptionToken;
        hunkWeaponDef.icon = weaponDefInfo.icon;
        hunkWeaponDef.crosshairPrefab = weaponDefInfo.crosshairPrefab;
        hunkWeaponDef.magSize = weaponDefInfo.magSize;
        hunkWeaponDef.magPickupMultiplier = weaponDefInfo.magPickupMultiplier;
        hunkWeaponDef.startingMags = weaponDefInfo.startingMags;
        hunkWeaponDef.reloadDuration = weaponDefInfo.reloadDuration;
        hunkWeaponDef.ammoName = weaponDefInfo.ammoName;
        hunkWeaponDef.primarySkillDef = weaponDefInfo.primarySkillDef;
        hunkWeaponDef.modelPrefab = weaponDefInfo.modelPrefab;
        hunkWeaponDef.animationSet = weaponDefInfo.animationSet;
        hunkWeaponDef.storedOnBack = weaponDefInfo.storedOnBack;
        hunkWeaponDef.storedOnHolster = weaponDefInfo.storedOnHolster;
        hunkWeaponDef.damageFillValue = weaponDefInfo.damageFillValue;
        hunkWeaponDef.rangefillValue = weaponDefInfo.rangefillValue;
        hunkWeaponDef.fireRateFillValue = weaponDefInfo.fireRateFillValue;
        hunkWeaponDef.reloadFillValue = weaponDefInfo.reloadFillValue;
        hunkWeaponDef.accuracyFillValue = weaponDefInfo.accuracyFillValue;
        hunkWeaponDef.aimSoundString = weaponDefInfo.aimSoundString;
        hunkWeaponDef.equipSoundString = weaponDefInfo.equipSoundString;
        return hunkWeaponDef;
    }

    public HunkWeaponDef CloneWeapon(bool addToCatalog = true)
    {
        HunkWeaponDef hunkWeaponDef = Object.Instantiate(this);
        HunkWeaponCatalog.AddWeapon(hunkWeaponDef, addItem: false);
        ContentAddition.AddItemDef(hunkWeaponDef.itemDef);
        return hunkWeaponDef;
    }
}
