using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.Utility;
using System.Collections;
using UnityEngine;
using static CraftData;
using static HandReticle;
using static Nautilus.Assets.PrefabTemplates.FabricatorTemplate;

namespace ExteriorACU.BaseRooms;

public static class ExteriorWaterParkPrefab
{
    private static readonly string classId = "ExteriorWaterPark";
    private static readonly string name = "Exterior Alien Containment";

    public static PrefabInfo Info { get; private set; } = PrefabInfo
        .WithTechType(classId, name, "Exterior containment and breeding unit for leviathan size creatures.")
        .WithIcon(SpriteManager.Get(TechType.BaseWaterPark));

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);

        // Other configuration here

        prefab.SetGameObject(GetPrefabAsync);

        prefab.SetRecipe(RecipeExtensions.ConvertToRecipeData(CraftData.Get(TechType.BaseWaterPark)));
        
        prefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);

        prefab.Register();
    }

    private static IEnumerator GetPrefabAsync(IOut<GameObject> exteriorWaterParkPrefab)
    {
        // Create a game object as the main object, and another for the model
        var obj = new GameObject(classId+"Main");
        var model = new GameObject(classId+"Model");

        // Makes the model a child of the main object
        model.transform.SetParent(obj.transform);

        // Here we define where our object is allowed to be placed. Nautilus uses a bitmask enum to do this
        var constructableFlags = ConstructableFlags.Inside | ConstructableFlags.Outside | ConstructableFlags.Rotatable | ConstructableFlags.Ground;

        // Constructable setup
        Constructable constructable = PrefabUtils.AddConstructable(obj, Info.TechType, constructableFlags, model);
        // Needed at all?
        //constructable.ghostMaterial = MaterialUtils.GhostMaterial;

        // Wait for the base pieces to initialize first in case they haven't already
        yield return new WaitUntil(() => Base.initialized);

        // Base piece setup
        Base.PieceDef waterParkTop = GetPiece(Base.Piece.LargeRoomWaterParkCeilingTop);
        GameObject waterParkTopCopy = GameObject.Instantiate(waterParkTop.prefab.gameObject);
        waterParkTopCopy.transform.SetParent(model.transform);
        // you will likely have to move each piece up or down, this is where you do it.

        Base.PieceDef waterParkSide = GetPiece(Base.Piece.LargeRoomWaterParkSide);
        GameObject waterParkSideCopy = GameObject.Instantiate(waterParkSide.prefab.gameObject);
        waterParkSideCopy.transform.SetParent(model.transform);

        Base.PieceDef waterParkBottom = GetPiece(Base.Piece.LargeRoomWaterParkFloorBottom);
        GameObject waterParkBottomCopy = GameObject.Instantiate(waterParkBottom.prefab.gameObject);
        waterParkBottomCopy.transform.SetParent(model.transform);

        // Adds the TechTag, Prefab Identifier, and LargeWorldEntity components to make the item saveable
        PrefabUtils.AddBasicComponents(obj, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);

        // Make Nautilus recognize our main game object as the prefab
        exteriorWaterParkPrefab.Set(obj);
    }

    private static Base.PieceDef GetPiece(Base.Piece basePiece)
    {
        var index = (int)basePiece;
        return Base.pieces[index];
    }
}
