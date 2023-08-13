using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs{
        public State state;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public enum State{
        Idle,
        Frying,
        Fryed,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurnedRecipeSO[] burnedRecipeSOArray;

    private FryingRecipeSO fryingRecipeSO;
    private BurnedRecipeSO burnedRecipeSO;

    private float fryingTimer;
    private float burnedTimer;
    private State state;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {

        switch (state){
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax   
                });

                if(HasKitchenObject())
                {
                    if(fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output,this);

                        state = State.Fryed;
                        burnedTimer = 0f;
                        burnedRecipeSO = GetBurnedRecipeWithInput(GetKitchenObject().GetKitchenObjectSO());
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }
                    
                }
                break;
            case State.Fryed:
                burnedTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = burnedTimer/burnedRecipeSO.burnedTimerMax   
                });

                if(HasKitchenObject())
                {
                    if(burnedTimer > burnedRecipeSO.burnedTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burnedRecipeSO.output,this);

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }
                }
                break;
            case State.Burned:
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
                break;
        }
    }


    public override void Interact(PlayerController player)
    {
        if(!HasKitchenObject())
        {
            // There is no KitchenObject here
            if(player.HasKitchenObject())
            {
                // Player is carring anything
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){
                    // Player carring something that can be Fryed 
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                        state = state
                    });

                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer/fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player is no carring anything
            }
        }
        else
        {
            // There is a KitchenObject here
            if(player.HasKitchenObject())
            {
                // Player is carring anything
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });

                        OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is no carring anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                    state = state
                });

                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }
        }
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetInputForOutput(KitchenObjectSO inputkitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputkitchenObjectSO);
        if(fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputkitchenObjectSO)
    {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(inputkitchenObjectSO == fryingRecipeSO.input)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurnedRecipeSO GetBurnedRecipeWithInput(KitchenObjectSO inputkitchenObjectSO)
    {
        foreach(BurnedRecipeSO burnedRecipeSO in burnedRecipeSOArray)
        {
            if(inputkitchenObjectSO == burnedRecipeSO.input)
            {
                return burnedRecipeSO;
            }
        }
        return null;
    }

}
