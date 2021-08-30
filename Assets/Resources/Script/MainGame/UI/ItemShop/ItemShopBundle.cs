using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopBundle : MonoBehaviour
{
    public Toggle toggle;

    public ItemSlot slot;

    [SerializeField]
    Text itenName;

    [SerializeField]
    Image img;

    [SerializeField]
    Text price;
    public int priceValue = 0;

    public Item item { get { return slot.item; } set { slot.item = value; } }

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
        // �ʱ�ȭ �ȉ��� ��� �ߴ�
        if (item == null)
            return;

        // �����۸� �ؽ�Ʈ ����
        itenName.text = item.name;

        // ������ ���� ����
        price.text = item.cost.ToString();
        priceValue = item.cost;
    }

    public void Select()
    {
        toggle.isOn = true;
    }

    public void Clear()
    {
        toggle.isOn = false;
        canBuy = true;
    }

    public void SetPriceColor(Color color)
    {
        price.color = color;
    }
}
