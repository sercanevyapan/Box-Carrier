using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyArea : MonoBehaviour
{
    private EnergyBar energyBar;
    private bool istriggered = false;

    private void Awake()
    {
        energyBar = FindObjectOfType<EnergyBar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& istriggered == false)
        {
            energyBar.IncreaseEnergyBar();
            istriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            istriggered = false;
        }
    }
}
