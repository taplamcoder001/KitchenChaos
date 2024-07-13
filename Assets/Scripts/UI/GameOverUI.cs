using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeDeliverytext;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    private void Start() {
        mainMenuButton.onClick.AddListener(() =>
            Loader.Load(Loader.Scene.MainMenuScene)
        );
        restartButton.onClick.AddListener(() =>
            Loader.Load(Loader.Scene.GameScene)
        );
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            Time.timeScale = 0f;
            recipeDeliverytext.text = DeliveryManager.Instance.GetSuccessFullRecipeAmount().ToString();
        }
        else{
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
