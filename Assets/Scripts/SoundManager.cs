using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_FREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance{get; private set;}


    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;
    private void Awake() {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_FREFS_SOUND_EFFECTS_VOLUME,1f);
    }

    private void Start() {
        DeliveryManager.Instance.OnSoundSuccess += DeliveryManager_OnSoundSuccess;
        DeliveryManager.Instance.OnSoundFail += DeliveryManager_OnSoundFail;
        PlayerController.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash,trashCounter.transform.position);   
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop,cuttingCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop,baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender,System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup,PlayerController.Instance.transform.position);
    }

    private void DeliveryManager_OnSoundSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess,DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnSoundFail(object sender,System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail,DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volumeMutiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volumeMutiplier * volume);
    }

    private void PlaySound(AudioClip audioClip,Vector3 position,float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volume);
    }

    public void PlayfootstepSound(Vector3 position,float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep,position,volume);
    }

    public void ChangeVolume(){
        volume += .1f;
        if(volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_FREFS_SOUND_EFFECTS_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
