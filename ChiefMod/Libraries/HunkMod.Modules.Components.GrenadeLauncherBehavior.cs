// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.GrenadeLauncherBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using UnityEngine;

public class GrenadeLauncherBehavior : WeaponBehavior
{
    public GameObject baseStock;

    public GameObject bonusStock;

    public Animator animator;

    public override HunkWeaponDef weaponDef => BaseWeapon<GrenadeLauncher>.instance.weaponDef;

    public void Start()
    {
        this.baseStock = base.childLocator.FindChild("Stock").gameObject;
        this.bonusStock = base.childLocator.FindChild("BonusStock").gameObject;
        this.animator = base.GetComponentInChildren<Animator>();
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        if ((bool)base.hunk && (bool)base.hunk.characterBody && (bool)base.hunk.characterBody.inventory)
        {
            if (base.hunk.characterBody.inventory.GetItemCount(GrenadeLauncher.bonusStock) > 0)
            {
                this.baseStock.SetActive(value: false);
                this.bonusStock.SetActive(value: true);
            }
            else
            {
                this.baseStock.SetActive(value: true);
                this.bonusStock.SetActive(value: false);
            }
        }
        else
        {
            this.baseStock.SetActive(value: true);
            this.bonusStock.SetActive(value: false);
        }
        if ((bool)this.animator)
        {
            this.animator.SetBool("isReloading", base.hunk.isReloading);
        }
    }
}
