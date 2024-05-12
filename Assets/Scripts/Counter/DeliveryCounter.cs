using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance{get; private set;}

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void Interact(PlayerController player)
    {
        if(player.HasKitchenObject()){
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // Only accepts Plate

                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
                // plateKitchenObject.DestroySelf();
            }
        }
    }
}
