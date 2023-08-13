using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisualPrefabs;
    [SerializeField] private Transform counterTopPoint;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake() {
        plateVisualGameObjectList = new List<GameObject>();    
    }

    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnRemovePlate += PlatesCounter_OnRemovePlate;
    }

    private void PlatesCounter_OnRemovePlate(object sender, System.EventArgs e)
    {
        GameObject platesGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count -1];
        Destroy(platesGameObject);
        plateVisualGameObjectList.Remove(platesGameObject);
    }
    
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTranform = Instantiate(plateVisualPrefabs, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTranform.localPosition = new Vector3(0,plateOffsetY*plateVisualGameObjectList.Count,0);

        plateVisualGameObjectList.Add(plateVisualTranform.gameObject);
    }
}
