// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.ARBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using RoR2;
using UnityEngine;

public class ARBehavior : WeaponBehavior
{
    public GameObject laserSight;

    public GameObject laser;

    public LineRenderer laserLine;

    public GameObject pointer;

    public GameObject magazine;

    public override HunkWeaponDef weaponDef => BaseWeapon<AssaultRifle>.instance.weaponDef;

    public void Start()
    {
        if (!base.hunk)
        {
            base.GetHunkController();
        }
        this.laserSight = base.childLocator.FindChild("LaserSight").gameObject;
        this.laser = base.childLocator.FindChild("Laser").gameObject;
        this.laserLine = this.laser.GetComponent<LineRenderer>();
        this.pointer = base.hunk.childLocator.FindChild("Pointer").gameObject;
        this.pointer.transform.GetChild(0).gameObject.layer = 21;
        this.magazine = base.childLocator.FindChild("Magazine").gameObject;
        this.laserSight.SetActive(value: false);
        this.laser.SetActive(value: false);
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
        if (base.hunk.characterBody.inventory.GetItemCount(SMG.laserSight) > 0 && base.hunk.weaponDef == BaseWeapon<SMG>.instance.weaponDef && base.hunk.isAiming && (bool)this.pointer && this.laser.activeSelf)
        {
            this.UpdatePointer();
        }
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        if ((bool)base.hunk)
        {
            bool flag = false;
            if (base.hunk.weaponDef == BaseWeapon<AssaultRifle>.instance.weaponDef)
            {
                flag = true;
                if (!base.hunk.isReloading)
                {
                    this.ShowMag();
                }
            }
            if ((bool)base.hunk.characterBody)
            {
                if ((bool)base.hunk.characterBody.inventory)
                {
                    if (base.hunk.characterBody.inventory.GetItemCount(SMG.laserSight) > 0)
                    {
                        this.laserSight.SetActive(value: true);
                        if (flag)
                        {
                            this.laser.SetActive(base.hunk.isAiming);
                        }
                        if (base.hunk.isAiming && flag)
                        {
                            this.UpdatePointer();
                        }
                        else
                        {
                            this.pointer.SetActive(value: false);
                        }
                    }
                    else
                    {
                        this.laserSight.SetActive(value: false);
                    }
                }
                else
                {
                    this.laserSight.SetActive(value: false);
                }
            }
            else
            {
                this.laserSight.SetActive(value: false);
            }
        }
        else
        {
            this.laserSight.SetActive(value: false);
        }
    }

    public void UpdatePointer()
    {
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

    public override void HideMag()
    {
        this.magazine.SetActive(value: false);
    }

    public override void ShowMag()
    {
        this.magazine.SetActive(value: true);
    }
}
