using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterTank : MonoBehaviour
{
    public Image WaterBar;
    public Text CurrentWater;
    public Text Indication;
    private bool fill;

    public GameObject WaterTraces;
    public Image Toggle;
    public AudioSource StartB;
    public AudioSource Water;
    public AudioSource Cancel;

    public float percent; 
    public ArduinoManager AM;

    void Update()
    {
        // leer los centimetros del arduino y convertirlos a un porcentaje 
        WaterBar.fillAmount = percent;
        CurrentWater.text = percent.ToString("0%");

        if (fill && CurrentWater.text == "100%")
        {
            fill = false;
            Toggle.color = new Color(0f, 0f, 0f, 0f);
            Cancel.Play();
            Water.enabled = false;
            Indication.text = "LLENAR";
            WaterTraces.SetActive(false);

            // escribirle al arduino que desactive la bomba del tanque que NO medimos 
            AM.DbNm();                                    
        }
    }

    public void ToggleFill()
    {
        if (WaterBar.fillAmount == 1)
            Cancel.Play();
        else if (!fill && WaterBar.fillAmount < 1)
        {
            fill = true;
            StartB.Play();
            Water.enabled = true;
            Toggle.color = new Color(1f, 1f, 1f, 0.5f);
            Indication.text = "DETENER";
            WaterTraces.SetActive(true);

            // escribirle al arduino que active la bomba del tanque que NO medimos  
            AM.AbNm();             
        }
        else if (fill && WaterBar.fillAmount < 1)
        {
            fill = false;
            Cancel.Play();
            Water.enabled = false;
            Toggle.color = new Color(0f, 0f, 0f, 0f);
            Indication.text = "LLENAR";
            WaterTraces.SetActive(false);

            // escribirle al arduino que desactive la bomba del tanque que NO medimos 
            AM.DbNm(); 
        }
    }
}
