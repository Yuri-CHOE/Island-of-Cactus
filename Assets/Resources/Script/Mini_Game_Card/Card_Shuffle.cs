using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Shuffle : MonoBehaviour
{
    public string[] Card_num;
    string temp;
    int random1, random2;

    // Start is called before the first frame update
    void Awake()
    {
        Shuffle();
    }

    void Shuffle()
    {
        Card_num = new string[18] { "1", "1", "2", "2", "3", "3", "4", "4", "5", "5", "6", "6", "7", "7", "8", "8", "9", "9"};
        temp = "";

        for (int i = 0; i < 9; i++)
        {
            random1 = Random.Range(0, 18);
            random2 = Random.Range(0, 18);

            if (random1 == random2)
            {
                i -= 1;
            }
            else
            {
                temp = Card_num[random1];
                Card_num[random1] = Card_num[random2];
                Card_num[random2] = temp;
            }
        }

    }

    public string num_return(int i)
    {
        return Card_num[i];
    }
}
