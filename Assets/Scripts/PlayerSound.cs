using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerController player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Awake() {
        player = GetComponent<PlayerController>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer<=footstepTimerMax)
        {
            footstepTimer= footstepTimerMax;
            
            if(player.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayfootstepSound(player.transform.position,volume);
            }
        }
    }
}
