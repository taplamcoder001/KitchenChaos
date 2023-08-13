using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
     [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private const string CUT = "Cut";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnCut += OnCut_OnPlayerGrabbedObject;
    }

    private void OnCut_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
