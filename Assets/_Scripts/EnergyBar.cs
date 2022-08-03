using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public Image foregroundImage;
    private bool isIncrease;

    private void Start()
    {
        maxEnergy = PlayerPrefs.GetFloat("Energy",100);
    }


    private void Update()
    {
        if (isIncrease)
        {
         
           if (currentEnergy < maxEnergy)
            {
                currentEnergy += 1f;
                foregroundImage.fillAmount = Mathf.MoveTowards(foregroundImage.fillAmount,currentEnergy / maxEnergy, 2f*Time.deltaTime);

                //print(currentEnergy);
                if (currentEnergy >= maxEnergy)
                {
                    isIncrease = false;
                }
            }

            
        }

        CheckEnergyLimit();
    }

    private void CheckEnergyLimit()
    {
        if (currentEnergy>=maxEnergy)
        {
            currentEnergy = maxEnergy;
        }else if (currentEnergy<=0)
        {
            currentEnergy = 0;
        }
    }

    public void DecreaseEnergyBar()
    {
        currentEnergy -= 0.05f;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;

        }

        else if (currentEnergy > 0)
        {

            foregroundImage.fillAmount = Mathf.MoveTowards(foregroundImage.fillAmount, currentEnergy / maxEnergy, Time.deltaTime);


        }

    }

    public void IncreaseEnergyBar()
    {
        isIncrease = true;
    

    }


    public void UpgradeEnergy(float decrease)
    {
        maxEnergy += decrease;
        PlayerPrefs.SetFloat("Energy", maxEnergy);
        PlayerPrefs.Save();
        print(maxEnergy);
    }

}
