using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    [SerializeField]
    GameObject msgBox;

    [SerializeField]
    GameObject npc;
    [SerializeField]
    Image npcImg;
    [SerializeField]
    Text npcText;

    [SerializeField]
    int playerMoney = 999;

    [SerializeField]
    List<ItemShopBundle> bundle;

    [SerializeField]
    Color normalPrice;
    [SerializeField]
    Color redPrice;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
