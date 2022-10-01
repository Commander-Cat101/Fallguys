using MelonLoader;
using BTD_Mod_Helper;
using FallGuys;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Enums;
using Assets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper.Api.Display;
using System.Linq;
using Il2CppSystem;
using UnityEngine;

[assembly: MelonInfo(typeof(FallGuys.FallGuys), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FallGuys;

public class FallGuys : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<FallGuys>("FallGuys loaded!");
    }
}
public class FallGuysTower : ModTower
{

    public override string TowerSet => TowerSetType.Magic;
    public override string BaseTower => TowerType.DartMonkey;
    public override int Cost => 500;
    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 0;
    public override int BottomPathUpgrades => 0;
    public override string Portrait => "Cuphead-Icon";

    public override string Icon => "Cuphead-Icon";
    public override bool DontAddToShop => false;
    public override string Description => "Cuphead is here";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        
    }
}
/*public class Display : ModTowerCustomDisplay<FallGuysTower>
{
    public override string AssetBundleName => "Fallguys";
    public override string PrefabName => "Fall_Guys_Trice_skin";
    public override bool UseForTower(int[] tiers)
    {
        return true;
    }
}*/
public class Display : ModTowerCustomDisplay<FallGuysTower>
{
    public override string AssetBundleName => "marine";
    public override string PrefabName => "MarinePrefab";
    public override string MaterialName => "MarineMaterial";
    public override bool UseForTower(int[] tiers)
    {
        return true;
    }
    public override void ModifyDisplayNode(Assets.Scripts.Unity.Display.UnityDisplayNode node)
    {

    }
}