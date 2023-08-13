using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance{get; private set;}

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State{
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;

    private float waittingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    [SerializeField] private float gamePlayingTimerMax = 10f;
    private bool isGamePaused = false;


    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
        gamePlayingTimer = gamePlayingTimerMax;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    public void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        TogglePausedGame();
    }

    private void Update() {
        switch (state){
            case State.WaitingToStart:
                waittingToStartTimer -= Time.deltaTime;
                if(waittingToStartTimer<0){
                    state = State.CountdownToStart;

                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -=Time.deltaTime;
                if(countdownToStartTimer<0){
                    state = State.GamePlaying;

                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if(gamePlayingTimer<0){
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetTimer()
    {
        return 1 - (gamePlayingTimer/gamePlayingTimerMax);
    }

    public void TogglePausedGame()
    {
        isGamePaused = !isGamePaused;
        if(isGamePaused){
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this,EventArgs.Empty);
        }
        else{
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this,EventArgs.Empty);
        }
    }
}