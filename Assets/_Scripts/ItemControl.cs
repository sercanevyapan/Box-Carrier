using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    
    bool isAlreadyCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isAlreadyCollected) return;
        if (other.CompareTag("Player"))
        {
            Collector mPCollect;
            if (other.TryGetComponent(out mPCollect)&& CollectorController.instance.CheckBoxLimit())
            {
              
                mPCollect.AddNewItem(this.transform);
                CollectorController.instance.boxes.Add(this);
                isAlreadyCollected = true;
            }
        }
    }
}
