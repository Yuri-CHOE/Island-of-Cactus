using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public static ItemShop script = null;

    [SerializeField]
    CanvasGroup canvasGroup = null;

    [SerializeField]
    GameObject msgBox = null;
    public GameObject itemGrop = null;

    [SerializeField]
    GameObject npc = null;
    [SerializeField]
    Image npcImg = null;
    [SerializeField]
    Text npcText = null;

    int playerMoney { get { return Turn.now.coin.Value; } }
    int totalSelectMoney { get { int temp = 0; for (int i = 0; i < bundle.Count; i++) { if (bundle[i].toggle.isOn) temp += bundle[i].priceValue; } return temp; } }

    public List<ItemShopBundle> bundle = new List<ItemShopBundle>();

    public Player customer = null;

    [SerializeField]
    Color normalPrice = new Color();
    [SerializeField]
    Color redPrice = new Color();

    private void Awake()
    {
        // �� ���
        script = this;

        // ���� �¾�
        for (int i = 0; i < bundle.Count; i++)
        {
            bundle[i].slot.SetUp(null, new ItemUnit());
        }
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
        // ���� ���� üũ �� ����
        if (customer != Turn.now)
            return;

            /*
             ��ü ���� ��ȸ�ؼ� �ݾ� ¡�� �� ���õ� ������ �κ��� �߰�
             ��ư���� ������
             */


            int buyCount = 0;

        // ���õ� ��� ���� ����ó��
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
            {
                Debug.Log("������ ���� :: ����" + i);
                Pay(customer, bundle[i]);
                buyCount++;
            }

        // ������ ����
        customer = null;

        // ���� ����
        if (buyCount > 0)
            GameMaster.script.messageBox.Out();
    }

    /// <summary>
    /// ������ ��ǰ�� ���� ���� �� ����
    /// </summary>
    /// <param name="customer">������</param>
    /// <param name="ib">��ǰ</param>
    void Pay(Player _customer, ItemShopBundle ib)
    {
        // ��� ����
        customer.coin.subtract(ib.priceValue);

        // ������ ����
        customer.AddItem(ib.slot, 1);
    }

    public void OpenShop()
    {
        // ������ ����
        customer = Turn.now;

        // ���� �ؽ�Ʈ ���� �ʱ�ȭ
        CheckMoney();

        // ���� ����
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Itemshop);

        // ����� ���� - ���� ��
        GameMaster.script.messageBox.InputControl(Player.me == Turn.now);
        //canvasGroup.blocksRaycasts = (Player.me == Turn.now);
    }

    void CheckMoney()
    {
        // ���� �ݾ�
        int spareMoney = playerMoney - totalSelectMoney;

        // �ܿ� ���� ���� ���� �ľ�
        int spareSlotCount = Player.inventoryMax - Turn.now.inventoryCount;

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
        int spareSlotCount = Player.inventoryMax - Turn.now.inventory.Count;

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

        // ���� ����
        target.slot.count = 1;

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
