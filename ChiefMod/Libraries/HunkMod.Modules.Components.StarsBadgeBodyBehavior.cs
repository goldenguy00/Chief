// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// HunkMod, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// HunkMod.Modules.Components.StarsBadgeBodyBehavior
using HunkMod.Modules;
using HunkMod.Modules.Components;
using HunkMod.Modules.Enemies;
using HunkMod.Modules.Survivors;
using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StarsBadgeBodyBehavior : BaseItemBodyBehavior
{
    public GameObject nemesisMaster;

    public GameObject nemesisBody;

    public static float timeBetweenFriendResummons = 30f;

    public static float timeBetweenFriendRetryResummons = 1f;

    public float friendResummonCooldown = 60f;

    public NemesisController nemesis;

    public bool isStageValid
    {
        get
        {
            string text = SceneManager.GetActiveScene().name;
            switch (text)
            {
            default:
                if (!(text == "meridian"))
                {
                    return SceneCatalog.mostRecentSceneDef.sceneType == SceneType.Stage;
                }
                goto case "moon";
            case "moon":
            case "moon2":
            case "bazaar":
            case "arena":
            case "artifactworld":
            case "voidraid":
                return false;
            }
        }
    }

    [ItemDefAssociation(useOnServer = true, useOnClient = false)]
    public static ItemDef GetItemDef()
    {
        return Hunk.starsBadge;
    }

    public void Start()
    {
        this.friendResummonCooldown = Random.Range(30f, 300f);
        if (Config.disableNemesis.Value)
        {
            this.friendResummonCooldown = 0f;
            base.gameObject.AddComponent<YouAreSoFuckingRetarded>();
            base.body = base.GetComponent<CharacterBody>();
            base.body.baseMaxHealth = 1f;
            base.body.baseMoveSpeed = 1f;
            base.body.RecalculateStats();
            if ((bool)base.body.modelLocator)
            {
                base.body.modelLocator.modelTransform.localScale *= 0.25f;
            }
        }
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
        if (!this.nemesisMaster)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            this.friendResummonCooldown -= Time.fixedDeltaTime;
            if (!(this.friendResummonCooldown <= 0f))
            {
                return;
            }
            if (!this.isStageValid)
            {
                this.friendResummonCooldown = 100000f;
                return;
            }
            if (base.body.inventory.GetItemCount(StarsBadgeBodyBehavior.GetItemDef()) <= 0)
            {
                Object.Destroy(this);
                return;
            }
            SpawnCard spawnCard = Nemesis.characterSpawnCard;
            if (Config.disableNemesis.Value)
            {
                spawnCard = Nemesis.fuckYouSpawnCard;
            }
            DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(spawnCard, new DirectorPlacementRule
            {
                placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                minDistance = 15f,
                maxDistance = 30f,
                spawnOnTarget = base.transform
            }, RoR2Application.rng);
            directorSpawnRequest.teamIndexOverride = TeamIndex.Monster;
            directorSpawnRequest.ignoreTeamMemberLimit = true;
            this.nemesisMaster = DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            if (!this.nemesisMaster)
            {
                this.friendResummonCooldown = StarsBadgeBodyBehavior.timeBetweenFriendRetryResummons;
            }
            else
            {
                this.friendResummonCooldown = StarsBadgeBodyBehavior.timeBetweenFriendResummons;
            }
            return;
        }
        if (!this.nemesisBody && (bool)this.nemesisMaster)
        {
            CharacterMaster component = this.nemesisMaster.GetComponent<CharacterMaster>();
            if ((bool)component.bodyInstanceObject)
            {
                this.nemesisBody = component.bodyInstanceObject;
            }
        }
        if ((bool)this.nemesisBody)
        {
            if (!this.nemesis)
            {
                this.nemesis = this.nemesisBody.GetComponent<NemesisController>();
            }
            if ((bool)this.nemesis)
            {
                this.nemesis.target = base.body;
            }
        }
    }

    public void OnDestroy()
    {
        if ((bool)this.nemesisMaster && (bool)this.nemesisBody)
        {
            this.nemesisBody.GetComponent<HealthComponent>().Suicide();
        }
    }
}
