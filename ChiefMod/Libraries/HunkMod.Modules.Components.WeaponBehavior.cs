// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.WeaponBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using RoR2;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public HunkController hunk;

    public ChildLocator childLocator;

    public virtual HunkWeaponDef weaponDef => BaseWeapon<MUP>.instance.weaponDef;

    public bool isEquipped { get; set; }

    public void Awake()
    {
        this.childLocator = base.GetComponent<ChildLocator>();
        this.GetHunkController();
    }

    public void GetHunkController()
    {
        if ((bool)this.hunk)
        {
            return;
        }
        CharacterModel componentInParent = base.GetComponentInParent<CharacterModel>();
        if ((bool)componentInParent)
        {
            CharacterBody body = componentInParent.body;
            if ((bool)body)
            {
                this.hunk = body.GetComponent<HunkController>();
            }
        }
    }

    public void TryEnableAttachment(ItemDef itemDef, GameObject target)
    {
        if ((bool)this.hunk)
        {
            if ((bool)this.hunk.characterBody)
            {
                if ((bool)this.hunk.characterBody.inventory)
                {
                    if (this.hunk.characterBody.inventory.GetItemCount(itemDef) > 0)
                    {
                        target.SetActive(value: true);
                    }
                    else
                    {
                        target.SetActive(value: false);
                    }
                }
                else
                {
                    target.SetActive(value: false);
                }
            }
            else
            {
                target.SetActive(value: false);
            }
        }
        else
        {
            target.SetActive(value: false);
        }
    }

    public void FixedUpdate()
    {
        if (!this.hunk)
        {
            this.GetHunkController();
        }
        else
        {
            this.RunFixedUpdate();
        }
    }

    public virtual void RunFixedUpdate()
    {
        if ((bool)this.hunk && (bool)this.weaponDef)
        {
            this.isEquipped = this.hunk.weaponDef == this.weaponDef;
        }
        else
        {
            this.isEquipped = false;
        }
    }

    public virtual void HideMag()
    {
    }

    public virtual void ShowMag()
    {
    }
}
