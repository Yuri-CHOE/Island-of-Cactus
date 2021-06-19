using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Setting : MonoBehaviour
{
    public Card_Shuffle card_shuffle;
    public Text cardText;
    public string name, num2;
    char sp;
    int num;

    void Start()
    {
        name = this.gameObject.name;
        sp = '_';
        card_shuffle = GameObject.Find("Game").GetComponent<Card_Shuffle>();
        if (name == "Card")
        {
            num = 0;
        }
        else
        {
            string[] card_num = name.Split(sp);
            num = int.Parse(card_num[1]);
        }
        
        num2 = card_shuffle.num_return(num);

        cardText.GetComponent<Text>().text = num2;

    }
}