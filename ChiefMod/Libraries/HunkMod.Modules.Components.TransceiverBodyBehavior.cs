// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.TransceiverBodyBehavior
using HunkMod.Modules.Components;
using HunkMod.Modules.Survivors;
using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Networking;

public class TransceiverBodyBehavior : BaseItemBodyBehavior
{
    public GameObject hunkMaster;

    public GameObject hunkBody;

    public static float timeBetweenFriendResummons = 30f;

    public static float timeBetweenFriendRetryResummons = 1f;

    public float friendResummonCooldown = 0f;

    public HunkController hunk;

    public bool setName = false;

    [ItemDefAssociation(useOnServer = true, useOnClient = false)]
    public static ItemDef GetItemDef()
    {
        return Hunk.transceiver;
    }

    public void FixedUpdate()
    {
        if (!base.body)
        {
            base.body = base.GetComponent<CharacterBody>();
        }
        if (!base.body || !base.body.master || !base.body.inventory || !base.body.isPlayerControlled)
        {
            return;
        }
        if (!this.hunkMaster)
        {
            this.setName = false;
            if (!NetworkServer.active)
            {
                return;
            }
            this.friendResummonCooldown -= Time.fixedDeltaTime;
            if (!(this.friendResummonCooldown <= 0f))
            {
                return;
            }
            if (base.body.inventory.GetItemCount(TransceiverBodyBehavior.GetItemDef()) <= 0)
            {
                Object.Destroy(this);
                return;
            }
            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(Hunk.characterSpawnCard, new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                minDistance = 15f,
                maxDistance = 30f,
                spawnOnTarget = base.transform
            }, RoR2Application.rng);
            directorSpawnRequest.teamIndexOverride = base.body.teamComponent.teamIndex;
            directorSpawnRequest.ignoreTeamMemberLimit = true;
            this.hunkMaster = DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            if (!this.hunkMaster)
            {
                this.friendResummonCooldown = TransceiverBodyBehavior.timeBetweenFriendRetryResummons;
            }
            else
            {
                this.friendResummonCooldown = TransceiverBodyBehavior.timeBetweenFriendResummons;
            }
            return;
        }
        if (!this.hunkBody)
        {
            CharacterMaster component = this.hunkMaster.GetComponent<CharacterMaster>();
            if ((bool)component.bodyInstanceObject)
            {
                this.hunkBody = component.bodyInstanceObject;
            }
        }
        if ((bool)this.hunkBody)
        {
            if (!this.hunk)
            {
                this.hunk = this.hunkBody.GetComponent<HunkController>();
            }
            if ((bool)this.hunk && !this.setName)
            {
                this.setName = true;
                this.hunk.characterBody.baseMaxHealth *= 3f;
                this.hunk.characterBody.baseDamage *= 0.33f;
                this.hunk.characterBody.baseNameToken = "GHOST";
            }
        }
    }

    public void OnDestroy()
    {
        if ((bool)this.hunkMaster && (bool)this.hunkBody)
        {
            this.hunkBody.GetComponent<HealthComponent>().Suicide();
        }
    }
}
