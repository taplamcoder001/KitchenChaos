using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image clockUI;

    private void Update() {
        clockUI.fillAmount = KitchenGameManager.Instance.GetTimer();
    }
}
