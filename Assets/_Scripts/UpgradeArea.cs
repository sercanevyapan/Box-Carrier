using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeArea : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.MenuPanel.gameObject.SetActive(true);
         
       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.MenuPanel.gameObject.SetActive(false);

          
        }
    }
}
