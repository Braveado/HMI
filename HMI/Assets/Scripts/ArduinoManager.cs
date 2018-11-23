using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoManager : MonoBehaviour 
{
    SerialPort puerto;
    public WaterTank tanque;
    public FireContainer fuego;
    string str;
    string[] arr;
    int dg;
    float cm;

	// abrir puertos
	void Start () 
    {
        puerto = new SerialPort("COM5", 9600);
        puerto.Open();
        puerto.ReadTimeout = 25;
    }
	
	void Update () 
    {
        str = "";
        try
        {
            str = puerto.ReadLine();
            if(str != "")
            {
                arr = str.Split(' ');
                dg = int.Parse(arr[0]);
                cm = float.Parse(arr[1]);
            }
        }
        catch (System.Exception)
        {
        }

        // leer los grados
        if (dg > 0)
        {
            fuego.degrees = dg;            
        }
        else
        {
            fuego.degrees = 0;
        }

        // leer los centimetros
        if (cm > 0.0f)
        {
            tanque.percent = (cm - 17) / (6 - 17);
            tanque.percent = Mathf.Clamp(tanque.percent, 0.0f, 1.0f);                        
        }
        else
        {
            tanque.percent = 0.0f;
        }
	}

    public void AbNm()
    {
        puerto.Write("1");
        print("ACTIVAR la bomba que NO medimos");
    }

    public void DbNm()
    {
        puerto.Write("2");
        print("DESACTIVAR la bomba que NO medimos");
    }

    public void AbSm()
    {
        puerto.Write("3");
        print("ACTIVAR la bomba que SI medimos");
    }

    public void DbSm()
    {
        puerto.Write("4");
        print("DESACTIVAR la bomba que SI medimos");
    }
    
}
