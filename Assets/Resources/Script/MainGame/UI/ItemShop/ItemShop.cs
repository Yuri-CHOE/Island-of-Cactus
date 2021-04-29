using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    [SerializeField]
    GameObject msgBox;
    [SerializeField]
    GameObject itemGrop;

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
        CheckMoney();
    }

    public void OpenShop()
    {
        for (int i = 0; i < bundle.Count; i++)
        {
            int max = 1;    
            int index = Random.Range(0, max);// �̺κ� max�� ���̺��о ������ �ִ� �������� ��ü
            bundle[i].item = new Item(index);        // new �� ���� ������ ���� ������ ���̺��� �ε����� ���� ��ų��

            // ���� �ؽ�Ʈ ����
            bundle[i].Refresh();
        }

        // ���� �ؽ�Ʈ ���� �ʱ�ȭ
        CheckMoney();

        // ������ ����
        itemGrop.SetActive(true);
    }

    void CheckMoney()
    {
        for (int i = 0; i < bundle.Count; i++)
        {
            if (bundle[i].item.cost > playerMoney)
            {
                // ���� �ʰ�
                bundle[i].SetPriceColor(redPrice);
                bundle[i].canBuy = false;
            }
            else
            {
                // ���� ����
                bundle[i].SetPriceColor(normalPrice);
                bundle[i].canBuy = true;
            }
        }
    }
}
