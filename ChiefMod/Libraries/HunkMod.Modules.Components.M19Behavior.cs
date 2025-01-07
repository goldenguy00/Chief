// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.M19Behavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using RoR2;
using UnityEngine;

public class M19Behavior : WeaponBehavior
{
    public GameObject laser;

    public LineRenderer laserLine;

    public GameObject pointer;

    public GameObject extendedMag;

    public GameObject suppressor;

    public void Start()
    {
        if (!base.hunk)
        {
            base.GetHunkController();
        }
        this.laser = base.childLocator.FindChild("Laser").gameObject;
        this.laserLine = this.laser.GetComponent<LineRenderer>();
        this.pointer = base.hunk.childLocator.FindChild("Pointer").gameObject;
        this.pointer.transform.GetChild(0).gameObject.layer = 21;
        this.laser.SetActive(value: false);
        this.extendedMag = base.childLocator.FindChild("ExtendedMag").gameObject;
        this.suppressor = base.childLocator.FindChild("Suppressor").gameObject;
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
        if (base.hunk.weaponDef == BaseWeapon<M19>.instance.weaponDef && base.hunk.isAiming && (bool)this.pointer && this.laser.activeSelf)
        {
            this.UpdatePointer();
        }
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        base.TryEnableAttachment(M19.extendedMag, this.extendedMag);
        base.TryEnableAttachment(M19.suppressor, this.suppressor);
        if (!base.hunk)
        {
            return;
        }
        bool flag = false;
        if (base.hunk.weaponDef == BaseWeapon<M19>.instance.weaponDef)
        {
            flag = true;
        }
        if (flag)
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
