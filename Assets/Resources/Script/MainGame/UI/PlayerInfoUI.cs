using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public ObjectSwitch turnOobject;

    public Image face;

    public List<GameObject> item;

    [SerializeField]
    int Life = 10;
    public Text lifeText;
    public int life
    {
        get { return Life; }
        set
        {
            lifeText.text = Life.ToString();
        }
    }

    [SerializeField]
    int Coin = 0;
    public Text coinText;
    public int coin
    {
        get { return Coin; }
        set
        {
            coinText.text = Coin.ToString();
        }
    }

    void a() { int i = life; life = 1; }
}