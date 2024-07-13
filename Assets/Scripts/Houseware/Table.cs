using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private GameObject containFood;
    private bool hasDiners;
    public bool HasDisners => hasDiners;
    private Transform transformTable;
    public Transform TransformTable => transformTable;

    [SerializeField] private Transform containChair;
    private List<Chair> listChair;
    public List<Chair> ListChair => listChair;
    private int slotNum; // Show slot estimate dinner
    public int SlotNum => slotNum;
    private bool hasFood = false;
    public bool HasFood => hasFood;

    private void Awake()
    {
        listChair = new List<Chair>();
        AddChairInList();
    }

    private void AddChairInList()
    {
        foreach (Transform chairTranform in containChair)
        {
            Chair chair = chairTranform.GetComponent<Chair>();
            listChair.Add(chair);
        }
        slotNum = listChair.Count;
    }

    public void OnHasDinner()
    {
        hasDiners = true;
    }

    public void OnHasFood()
    {
        hasFood = true;
        containFood.SetActive(hasFood);
    }

    public void ResetTable()
    {
        hasDiners = false;
        hasFood = false;
        containFood.SetActive(hasFood);
    }
}
