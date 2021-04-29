using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopBundle : MonoBehaviour
{
    [SerializeField]
    Toggle toggle;

    [SerializeField]
    Text name;

    [SerializeField]
    Image img;

    [SerializeField]
    Text price;

    public Item item;

    // ���� ���� ����
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
        // �����۸� �ؽ�Ʈ ����
        name.text = item.name;

        // ������ ������ ����
        //�۾���=====================================================
        //img.sprite = new Sprite(������ ������ ����Ʈ ���� ����Ұ�);

        // ������ ���� ����
        price.text = item.cost.ToString();
    }

    public void SetPriceColor(Color color)
    {
        price.color = color;
    }
}
