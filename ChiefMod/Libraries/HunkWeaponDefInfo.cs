// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkWeaponDefInfo
using System;
using RoR2.Skills;
using UnityEngine;

[Serializable]
public struct HunkWeaponDefInfo
{
    public string nameToken;

    public string descriptionToken;

    public Sprite icon;

    public GameObject crosshairPrefab;

    public int magSize;

    public float magPickupMultiplier;

    public int startingMags;

    public float reloadDuration;

    public string ammoName;

    public SkillDef primarySkillDef;

    public GameObject modelPrefab;

    public HunkWeaponDef.AnimationSet animationSet;

    public bool storedOnBack;

    public bool storedOnHolster;

    public float damageFillValue;

    public float rangefillValue;

    public float fireRateFillValue;

    public float reloadFillValue;

    public float accuracyFillValue;

    public string aimSoundString;

    public string equipSoundString;
}
