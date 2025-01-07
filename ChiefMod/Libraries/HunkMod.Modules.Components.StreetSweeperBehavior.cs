// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.StreetSweeperBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using UnityEngine;

public class StreetSweeperBehavior : WeaponBehavior
{
    public GameObject magazine;

    public override HunkWeaponDef weaponDef => BaseWeapon<StreetSweeper>.instance.weaponDef;

    public void Start()
    {
        this.magazine = base.childLocator.FindChild("Magazine").gameObject;
    }

    public override void HideMag()
    {
        this.magazine.SetActive(value: false);
    }

    public override void ShowMag()
    {
        this.magazine.SetActive(value: true);
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        if ((bool)base.hunk && base.hunk.weaponDef == BaseWeapon<StreetSweeper>.instance.weaponDef && !base.hunk.isReloading)
        {
            this.ShowMag();
        }
    }
}
