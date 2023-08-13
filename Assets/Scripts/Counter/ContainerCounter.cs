using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenPrefabs;

    public override void Interact(PlayerController player)
    {
        if(!player.HasKitchenObject())
        {
            // Player is not carring anything
            KitchenObject.SpawnKitchenObject(kitchenPrefabs,player);
            OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
        }
        
    }
}
