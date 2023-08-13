using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticDataManager()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(PlayerController player)
    {
        if(player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
        }
        
    }
}
