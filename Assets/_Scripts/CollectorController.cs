using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorController : MonoBehaviour
{
    public static CollectorController instance;
    public List<ItemControl> boxes;
    public float boxesLimit;
    private Collector collector;
    private void Awake()
    {
        if (instance == null) instance = this;

        collector = FindObjectOfType<Collector>();
    }

    private void Start()
    {
        boxesLimit = PlayerPrefs.GetFloat("Box",3);
    }


    public bool CheckBoxLimit()
    {
        return boxes.Count < boxesLimit;
    }


    public void DropBoxes()
    {
        boxes.Reverse();

      StartCoroutine(DropLastBox());
 
    }

    private IEnumerator DropLastBox()
    {
        if(boxes.Count>0 )
        {
            collector.DropItem(boxes[0].transform);
            boxes.Remove(boxes[0]);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(DropLastBox());
        }
     
    }

    public void UpgradeBox(float decrease)
    {
        boxesLimit += decrease;
        PlayerPrefs.SetFloat("Box", boxesLimit);
        PlayerPrefs.Save();
    
    }
}
