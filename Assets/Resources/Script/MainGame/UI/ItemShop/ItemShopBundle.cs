using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopBundle : MonoBehaviour
{
    [SerializeField]
    Toggle toggle;

    [SerializeField]
    Text itenName;

    [SerializeField]
    Image img;

    [SerializeField]
    Text price;

    public Item item;

    // 구매 가능 여부
    public bool canBuy = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        // 초기화 안됬을 경우 중단
        if (item == null)
            return;

        // 아이템명 텍스트 변경
        itenName.text = item.name;

        // 아이템 아이콘 변경
        //작업중=====================================================
        //img.sprite = new Sprite(아이템 아이콘 리스트 만들어서 사용할것);

        // 아이템 가격 변경
        price.text = item.cost.ToString();
    }

    public void SetPriceColor(Color color)
    {
        price.color = color;
    }
}
