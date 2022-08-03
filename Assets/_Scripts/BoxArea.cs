using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectorController.instance.DropBoxes();
        }
    }
}
