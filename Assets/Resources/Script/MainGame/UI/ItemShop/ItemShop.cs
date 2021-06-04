using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public static ItemShop script = null;

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

    int playerMoney { get { return GameData.turn.now.coin.Value; } }
    int totalSelectMoney { get { int temp = 0; for (int i = 0; i < bundle.Count; i++) { if (bundle[i].toggle.isOn) temp += bundle[i].priceValue; } return temp; } }

    public List<ItemShopBundle> bundle;

    [SerializeField]
    Color normalPrice;
    [SerializeField]
    Color redPrice;

    private void Awake()
    {
        // �� ���
        script = this;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckMoney();
    }

    public void Buy()
    {
        /*
         ��ü ���� ��ȸ�ؼ� �ݾ� ¡�� �� ���õ� ������ �κ��� �߰�
         */

        // ������ ����
        Player customer = GameData.turn.now;

        int buyCount = 0;

        // ���õ� ��� ���� ����ó��
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
            {
                Debug.LogWarning("������ ���� :: ����" + i);
                Pay(customer, bundle[i]);
                buyCount++;
            }
        

        // ���� ����
        if (buyCount > 0)
            Out();
    }

    /// <summary>
    /// ������ ��ǰ�� ���� ���� �� ����
    /// </summary>
    /// <param name="customer">������</param>
    /// <param name="ib">��ǰ</param>
    void Pay(Player customer, ItemShopBundle ib)
    {
        // ��� ����
        customer.coin.subtract(ib.priceValue);

        // ������ ����
        customer.AddItem(ib.slot, 1);
    }

    public void Out()
    {
        // �޽��� �ڽ� �ʱ�ȭ �� �ݱ�
        GameMaster.script.messageBox.PopUp(-1);

        // ���� ����
        BlockWork.isEnd = true;
    }

    public void OpenShop()
    {
        for (int i = 0; i < bundle.Count; i++)
        {
            // ������ ���̺��� �����ϰ� ����
            bundle[i].item = Item.table[Random.Range(0, Item.table.Count)];

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
        //// ���� �ݾ�
        //int spareMoney = playerMoney - totalSelectMoney;

        //for (int i = 0; i < bundle.Count; i++)
        //{
        //    Debug.LogError("���� üũ"+ spareMoney +" <--> "+ bundle[i].item.cost);
        //    // ���õ��� ���� ���鿡 ���� ���� üũ
        //    if (!bundle[i].toggle.isOn)
        //    {
        //        if (bundle[i].item.cost > spareMoney)
        //        {
        //            // ���� �ʰ�
        //            bundle[i].SetPriceColor(redPrice);
        //            bundle[i].canBuy = false;
        //            bundle[i].toggle.enabled = false;
        //        }
        //        else
        //        {
        //            // ���� ����
        //            bundle[i].SetPriceColor(normalPrice);
        //            bundle[i].canBuy = true;
        //            bundle[i].toggle.enabled = true;
        //        }
        //    }
        //} 

        // ���� �ݾ�
        int spareMoney = playerMoney - totalSelectMoney;

        // �ܿ� ���� ���� ���� �ľ�
        int spareSlotCount = Player.inventoryMax - GameData.turn.now.inventoryCount;

        // ���� ���� �ľ�
        int selectCount = 0;
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
                selectCount++;

        for (int i = 0; i < bundle.Count; i++)
        {
            //Debug.LogError("���� üũ" + spareMoney + " <--> " + bundle[i].item.cost);

            // ���õ��� ���� ���鿡 ���� ���� üũ
            if (!bundle[i].toggle.isOn)
            {
                if (bundle[i].item.cost > spareMoney || (spareSlotCount - selectCount <= 0))
                {
                    // ���� �ʰ�
                    bundle[i].SetPriceColor(redPrice);
                    bundle[i].canBuy = false;
                    bundle[i].toggle.enabled = false;
                }
                else
                {
                    // ���� ����
                    bundle[i].SetPriceColor(normalPrice);
                    bundle[i].canBuy = true;
                    bundle[i].toggle.enabled = true;
                }
            }
        }
    }

    void CheckInventoryMax()
    {
        // �ܿ� ���� ���� ���� �ľ�
        int spareSlotCount = Player.inventoryMax - GameData.turn.now.inventory.Count;

        // ���� ���� �ľ�
        int selectCount = 0;
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
                selectCount++;

        for (int i = 0; i < bundle.Count; i++)
        {
            // �ܿ� ���� ���� ��� ���� ����
            if (spareSlotCount - selectCount <= 0)
            {
                // ���� �ʰ�
                bundle[i].SetPriceColor(redPrice);
                bundle[i].canBuy = false;
                bundle[i].toggle.enabled = false;
            }
            else
            {
                // ���� ����
                bundle[i].SetPriceColor(normalPrice);
                bundle[i].canBuy = true;
                bundle[i].toggle.enabled = true;
            }
        }
    }

    public void SetItemBundle(int bundleIndex, Item item)
    {
        // �߸��� ���� �Է½� �ߴ�
        if (bundleIndex < 0 || bundleIndex >= bundle.Count)
        {
            Debug.LogError("���� :: �߸��� �޼ҵ� ��� -> Out of range");
            Debug.Break();
            return;
        }

        // ��� ����
        ItemShopBundle target = bundle[bundleIndex];

        // ���� �ʱ�ȭ
        target.toggle.enabled = true;
        target.toggle.isOn = false;

        // ������ ����
        target.item = item;

        // ���ΰ�ħ
        target.Refresh();
    }

    public void SetItemBundle(Item item0, Item item1, Item item2, Item item3)
    {
        SetItemBundle(0, item0);
        SetItemBundle(1, item1);
        SetItemBundle(2, item2);
        SetItemBundle(3, item3);
    }

}
