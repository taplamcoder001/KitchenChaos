using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    private bool hasDiners;
    public bool HasDisners => hasDiners;
    private Transform transformTable;
    public Transform TransformTable => transformTable;

    [SerializeField] private Transform containChair;
    private List<Chair> listChair;
    public List<Chair> ListChair => listChair;
    private int slotNum; // Show slot estimate dinner
    public int SlotNum => slotNum;
    // hasOrder, pos dish

    private void Awake()
    {
        listChair = new List<Chair>();
        AddChairInList();
    }

    private void AddChairInList()
    {
        // Can use Jobsystem  
        foreach (Transform chairTranform in containChair)
        {
            Chair chair = chairTranform.GetComponent<Chair>();
            listChair.Add(chair);
        }
        slotNum = listChair.Count;
    }

    private void Update()
    {
        if(hasDiners)
        {
            return;
        }
    }

    public void HasDinner()
    {
        hasDiners = true;
    }

    public void ResetTable()
    {
        hasDiners = false;
    }
}
