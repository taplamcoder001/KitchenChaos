using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Fryed || e.state == StoveCounter.State.Frying;
        if(playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

        SoundManager.Instance.PlayBurnWarningSound(stoveCounter.transform.position);
        
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e){
        float showWarnigBurn = .5f;
        playWarningSound = stoveCounter.GetStateFryed() && e.progressNormalized >= showWarnigBurn;

        if(playWarningSound)
        {
            SoundManager.Instance.PlayBurnWarningSound(stoveCounter.transform.position);
        }
    }

    private void Update() {
        if(playWarningSound){
            warningSoundTimer -= Time.deltaTime;
            if(warningSoundTimer<=0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayBurnWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
