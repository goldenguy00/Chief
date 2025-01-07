// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.MUPBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using RoR2;
using UnityEngine;

public class MUPBehavior : WeaponBehavior
{
    public GameObject gunStock;

    public GameObject dotSight;

    public GameObject extendedMag;

    public GameObject muzzleBrake;

    public GameObject laser;

    public LineRenderer laserLine;

    public GameObject pointer;

    public override HunkWeaponDef weaponDef => BaseWeapon<MUP>.instance.weaponDef;

    public void Start()
    {
        this.gunStock = base.childLocator.FindChild("Stock").gameObject;
        this.dotSight = base.childLocator.FindChild("DotSight").gameObject;
        this.extendedMag = base.childLocator.FindChild("ExtendedMag").gameObject;
        this.muzzleBrake = base.childLocator.FindChild("MuzzleBrake").gameObject;
        if (!base.hunk)
        {
            base.GetHunkController();
        }
        this.laser = base.childLocator.FindChild("Laser").gameObject;
        this.laserLine = this.laser.GetComponent<LineRenderer>();
        if ((bool)base.hunk)
        {
            this.pointer = base.hunk.childLocator.FindChild("PointerGreen").gameObject;
            this.pointer.transform.GetChild(0).gameObject.layer = 21;
        }
        this.laser.SetActive(value: false);
        this.gunStock.SetActive(value: false);
        this.dotSight.SetActive(value: false);
        this.extendedMag.SetActive(value: false);
        this.muzzleBrake.SetActive(value: false);
    }

    public void OnDisable()
    {
        if ((bool)this.pointer)
        {
            this.pointer.SetActive(value: false);
        }
    }

    public void LateUpdate()
    {
        if ((bool)base.hunk && base.hunk.characterBody.inventory.GetItemCount(MUP.dotSight) > 0 && base.hunk.weaponDef == BaseWeapon<MUP>.instance.weaponDef && base.hunk.isAiming && (bool)this.pointer && this.laser.activeSelf)
        {
            this.UpdatePointer();
        }
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        base.TryEnableAttachment(MUP.gunStock, this.gunStock);
        base.TryEnableAttachment(MUP.dotSight, this.dotSight);
        base.TryEnableAttachment(MUP.extendedMag, this.extendedMag);
        base.TryEnableAttachment(MUP.muzzleBrake, this.muzzleBrake);
        if (!base.hunk)
        {
            return;
        }
        bool flag = false;
        if (base.hunk.weaponDef == BaseWeapon<MUP>.instance.weaponDef)
        {
            flag = true;
        }
        if (flag && (bool)base.hunk.characterBody && (bool)base.hunk.characterBody.inventory && base.hunk.characterBody.inventory.GetItemCount(MUP.dotSight) > 0)
        {
            this.laser.SetActive(base.hunk.isAiming);
            if (base.hunk.isAiming)
            {
                this.UpdatePointer();
            }
            else
            {
                this.pointer.SetActive(value: false);
            }
        }
    }

    public void UpdatePointer()
    {
        if (!base.hunk)
        {
            return;
        }
        this.laserLine.SetPosition(0, this.laserLine.transform.position);
        Ray aimRay = base.hunk.characterBody.inputBank.GetAimRay();
        if (Physics.Raycast(aimRay.origin, aimRay.direction, out var hitInfo, 5000f, LayerIndex.CommonMasks.bullet))
        {
            if ((bool)this.pointer)
            {
                this.pointer.transform.position = hitInfo.point;
                this.pointer.SetActive(value: true);
            }
            this.laserLine.SetPosition(1, hitInfo.point);
        }
        else
        {
            if ((bool)this.pointer)
            {
                this.pointer.SetActive(value: false);
            }
            this.laserLine.SetPosition(1, aimRay.origin + aimRay.direction * 5000f);
        }
    }
}
