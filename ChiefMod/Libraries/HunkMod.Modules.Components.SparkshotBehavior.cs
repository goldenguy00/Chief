// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.SparkshotBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Weapons;
using UnityEngine;

public class SparkshotBehavior : WeaponBehavior
{
    public GameObject condenser;

    public override HunkWeaponDef weaponDef => BaseWeapon<Sparkshot>.instance.weaponDef;

    public void Start()
    {
        this.condenser = base.childLocator.FindChild("Condenser").gameObject;
    }

    public override void RunFixedUpdate()
    {
        base.RunFixedUpdate();
        base.TryEnableAttachment(Sparkshot.condenser, this.condenser);
    }
}
