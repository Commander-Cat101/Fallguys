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
using uObject = UnityEngine.Object;


[assembly: MelonInfo(typeof(FallGuys.FallGuys), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace FallGuys;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Unity.Display;
using HarmonyLib;

public class FallGuys : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        
        foreach (var asset in MelonAssembly.Assembly.GetManifestResourceNames())
            MelonLogger.Msg(asset);
        //previous two lines are for debugging/finding names of assets
        
        assetBundle = AssetBundle.LoadFromMemory(ExtractResource("fallguys.bundle"));// if using unityexplorer, there is an error, but everything still works
        ModHelper.Msg<FallGuys>("FallGuys loaded!");
    }
    
    public static AssetBundle assetBundle;
    
    private byte[] ExtractResource(String filename)
    {
        Assembly a = MelonAssembly.Assembly; // get the assembly
        return a.GetEmbeddedResource(filename).GetByteArray(); // get the embedded bundle as a raw file that unity can read
    }
}

public class FallGuysTower : ModTower
{
    public override string TowerSet => TowerSetType.Magic;
    public override string BaseTower => TowerType.DartMonkey;
    public override string DisplayName => "FallGuysTower";
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
        towerModel.display = new() {guidRef = "FallGuysTower-Prefab"}; //required for custom displays to be recognized
        towerModel.GetBehavior<DisplayModel>().display = new() {guidRef = "FallGuysTower-Prefab"}; //required for custom displays to be recognized
    }
}

[HarmonyPatch(typeof(Factory.__c__DisplayClass21_0), nameof(Factory.__c__DisplayClass21_0._CreateAsync_b__0))]
static class FactoryCreateAsyncPatch
{
    [HarmonyPrefix]
    public static bool Prefix(ref Factory.__c__DisplayClass21_0 __instance, ref UnityDisplayNode prototype)
    {
        GameObject gObj;
        
        switch (__instance.objectId.guidRef) // makes sure to support loading more than one custom display
        {
            case "FallGuysTower-Prefab":
                gObj = UnityEngine.Object.Instantiate(FallGuys.assetBundle.LoadAsset("Fall_Guys_Trice_skin").Cast<GameObject>(), __instance.__4__this.DisplayRoot); //load the asset from the asset bundle and instantiates/creates it
                break;
            default:
                return true; //if the display is not custom, let the game create the base display
        }
        
        gObj.name = __instance.objectId.guidRef; //should be optional in theory, but i left it because its good for debugging/organization
        gObj.transform.position = new Vector3(Factory.kOffscreenPosition.x, 0, 0); //move the object offscreen so the game doesn't try to render it when its not needed 
        
        gObj.AddComponent<UnityDisplayNode>(); //adds a UnityDisplayNode component to the object, this is needed for the game to recognize it as a display
        prototype = gObj.GetComponent<UnityDisplayNode>(); //gets the UnityDisplayNode component from the object
        __instance.__4__this.active.Add(prototype); //adds the object to the active list, this is needed for the game to show the display
        __instance.onComplete.Invoke(prototype); //calls the onComplete delegate thats automatically created by the game, this is needed for the game to use it as a display
        
        return false; //prevents the game from creating the base display once a custom display is created
    }
}
