using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public Mouse_Click mouse_Click1;
    public Mouse_Click mouse_Click2;
    public set_active set_Active;
    public string num1, num2, name1, name2;
    public int x, z = 0;

    void Awake()
    {
        num1 = "";
        num2 = "";
        x = 1;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(x == 3)
        {
            mouse_Click1 = GameObject.Find(name1).GetComponent<Mouse_Click>();
            mouse_Click2 = GameObject.Find(name2).GetComponent<Mouse_Click>();
            Debug.Log("»Æ¿Œ" + num1 + " " + num2);
            if(num1 == num2)
            {
                mouse_Click2.i = 4;
                mouse_Click1.i = 4;
                z += 1;
            }
            else
            {
                mouse_Click2.i = 2;
                mouse_Click1.i = 2;
            }
        }
        x = 1;
        if(z == 9)
        {
            set_Active = GameObject.Find("End").GetComponent<set_active>();
            set_Active.active = true;
        }
    }

    public void num1_set(string number, string card_name)
    {
        num1 = number;
        name1 = card_name;
    }

    public void num2_set(string number, string card_name)
    {
        num2 = number;
        name2 = card_name;
    }
}
