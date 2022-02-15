using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{

    private bool isTriggered = false;
    private GameObject plate;
    public GameObject plateAnimationParent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Plate"))
        {
            isTriggered = true;
            plate = other.gameObject;
            plate.transform.SetParent(plateAnimationParent.transform);
        }
        
    }

    public bool GetTriggered()
    {
        return isTriggered;
    }

    public void SetTrigger(bool triggerState)
    {
        isTriggered = triggerState;
    }

    public GameObject GetPlate()
    {
        return plate;
    }
}
