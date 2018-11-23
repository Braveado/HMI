using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireContainer : MonoBehaviour
{
    public Image FireOverlay;
    private bool alarmAnimation;
    private float alarmAlpha;

    public Text CurrentDegrees;
    public Text Indication;
    private bool fire;
    private float fireAmount;

    public GameObject WaterTraces;
    public GameObject SmokeFx;
    public GameObject FireFx;
    public GameObject WaterFx;
    public Image Toggle;
    public AudioSource Help;
    public AudioSource Fire;
    public AudioSource Thanks;
    public AudioSource Cancel;

    public WaterTank Tank;

    public int degrees;
    public ArduinoManager AM;

    void Update()
    {
        // leer la temperatura del arduino                
        CurrentDegrees.text = degrees.ToString();        

        if (!fire)
        {
            if (int.Parse(CurrentDegrees.text) > 40)
            {
                fire = true;
                Toggle.color = new Color(0f, 0f, 0f, 0f);

                // escribirle al arduino que active la bomba del tanque que SI medimos
                AM.AbSm();
            }
        }

        if (fire && Indication.text == "")
        {
            Indication.text = "SILENCIAR";            
            CurrentDegrees.color = new Color(1f, 0f, 0f, 1f);
            Help.enabled = true;
            Fire.enabled = true;
            FireOverlay.enabled = true;           
            FireFx.SetActive(true);
        }        

        if(FireOverlay.enabled)
        {
            if(!alarmAnimation)
            {
                alarmAlpha += 1f * Time.deltaTime;
                FireOverlay.color = new Color(1f, 0f, 0f, alarmAlpha);
            }
            else if (alarmAnimation)
            {
                alarmAlpha -= 1f * Time.deltaTime;
                FireOverlay.color = new Color(1f, 0f, 0f, alarmAlpha);
            }

            if (alarmAlpha >= 1f)
                alarmAnimation = true;
            else if (alarmAlpha <= 0f)
                alarmAnimation = false;
        }

        if(fire)
        {
            if (int.Parse(CurrentDegrees.text) <= 40)
            {
                fire = false;
                Fire.enabled = false;                
                CurrentDegrees.color = new Color(1f, 1f, 1f, 1f);
                Toggle.color = new Color(1f, 1f, 1f, 0.5f);
                FireOverlay.enabled = false;
                WaterTraces.SetActive(false);
                FireFx.SetActive(false);
                WaterFx.SetActive(false);
                SmokeFx.SetActive(false);

                // escribirle al arduino que desactive la bomba del tanque que SI medimos   
                AM.DbSm();
            }
            else if (Tank.WaterBar.fillAmount > 0)
            {                                
                WaterTraces.SetActive(true);
                WaterFx.SetActive(true);
                SmokeFx.SetActive(true);                
            }
            else if (Tank.WaterBar.fillAmount <= 0)
            {                
                WaterTraces.SetActive(false);
                WaterFx.SetActive(false);
                SmokeFx.SetActive(false);
            }            
        }
    }

    public void ToggleExtinguish()
    {
        if (fire)
            Cancel.Play();
        if (!fire && Toggle.color.a > 0)
        {
            Help.enabled = false;
            Thanks.Play();
            Indication.text = "";
            Toggle.color = new Color(0f, 0f, 0f, 0f);
        }
    }
}
