using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnSoundSuccess;
    public event EventHandler OnSoundFail;
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnReciprTimer;
    private float spawnReciprTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successFullRecipeAmount;
    

    private void Awake() {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update() {
        spawnReciprTimer -= Time.deltaTime;
        if(spawnReciprTimer<=0f)
        {
            spawnReciprTimer = spawnReciprTimerMax;
            if(waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0,recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject){
        for(int i =0; i<waitingRecipeSOList.Count;i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has same number ingredient
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // Cycling through all ingredient in the Recipe
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // Cycling through all ingredient in the Plate
                        if(recipeKitchenObjectSO == plateKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if(plateContentsMatchesRecipe)
                {
                    // Player delivery the correct recipe!
                    waitingRecipeSOList.RemoveAt(i);
                    successFullRecipeAmount++;
                    OnSoundSuccess?.Invoke(this,EventArgs.Empty);
                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    return;
                }
            }
        }
        // No mathes found!
        // Player did not delivery a correct Recipe
        OnSoundFail?.Invoke(this,EventArgs.Empty);
    }

    public List<RecipeSO> GetRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessFullRecipeAmount()
    {
        return successFullRecipeAmount;
    }
}
