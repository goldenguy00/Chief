// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.RevolverBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using UnityEngine;

public class RevolverBehavior : WeaponBehavior
{
    public GameObject speedloader;

    public Transform barrel;

    public override HunkWeaponDef weaponDef => BaseWeapon<Revolver>.instance.weaponDef;

    public void Start()
    {
        this.speedloader = base.childLocator.FindChild("Speedloader").gameObject;
        this.barrel = base.childLocator.FindChild("Barrel");
        this.speedloader.SetActive(value: false);
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        if ((bool)base.hunk)
        {
            if ((bool)base.hunk.characterBody)
            {
                if ((bool)base.hunk.characterBody.inventory)
                {
                    if (base.hunk.characterBody.inventory.GetItemCount(Revolver.speedloader) > 0)
                    {
                        this.speedloader.SetActive(value: true);
                    }
                    else
                    {
                        this.speedloader.SetActive(value: false);
                    }
                }
                else
                {
                    this.speedloader.SetActive(value: false);
                }
            }
            else
            {
                this.speedloader.SetActive(value: false);
            }
        }
        else
        {
            this.speedloader.SetActive(value: false);
        }
    }

    public void LateUpdate()
    {
        if ((bool)base.hunk)
        {
            if ((bool)base.hunk.characterBody)
            {
                if ((bool)base.hunk.characterBody.inventory)
                {
                    if (base.hunk.characterBody.inventory.GetItemCount(Revolver.speedloader) > 0)
                    {
                        this.barrel.transform.localScale = Vector3.zero;
                    }
                    else
                    {
                        this.barrel.transform.localScale = Vector3.one;
                    }
                }
                else
                {
                    this.barrel.transform.localScale = Vector3.one;
                }
            }
            else
            {
                this.barrel.transform.localScale = Vector3.one;
            }
        }
        else
        {
            this.barrel.transform.localScale = Vector3.one;
        }
    }
}
