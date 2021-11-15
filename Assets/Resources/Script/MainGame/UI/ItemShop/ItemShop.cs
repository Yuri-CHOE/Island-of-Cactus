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
        // 퀵 등록
        script = this;

        // 번들 셋업
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
        // 구매 권한 체크 및 차단
        if (customer != Turn.now)
            return;

            /*
             전체 번들 순회해서 금액 징수 후 선택된 아이템 인벤에 추가
             버튼으로 구동됨
             */


            int buyCount = 0;

        // 선택된 모든 번들 구매처리
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
            {
                Debug.Log("아이템 구매 :: 슬롯" + i);
                Pay(customer, bundle[i]);
                buyCount++;
            }

        // 구매자 제거
        customer = null;

        // 종료 판정
        if (buyCount > 0)
            GameMaster.script.messageBox.Out();
    }

    /// <summary>
    /// 아이템 상품에 대한 지불 및 구매
    /// </summary>
    /// <param name="customer">구매자</param>
    /// <param name="ib">상품</param>
    void Pay(Player _customer, ItemShopBundle ib)
    {
        // 비용 지불
        customer.coin.subtract(ib.priceValue);

        // 아이템 지급
        customer.AddItem(ib.slot, 1);
    }

    public void OpenShop()
    {
        // 구매자 지정
        customer = Turn.now;

        // 가격 텍스트 색상 초기화
        CheckMoney();

        // 상점 오픈
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Itemshop);

        // 제어권 지급 - 본인 턴
        GameMaster.script.messageBox.InputControl(Player.me == Turn.now);
        //canvasGroup.blocksRaycasts = (Player.me == Turn.now);
    }

    void CheckMoney()
    {
        // 가용 금액
        int spareMoney = playerMoney - totalSelectMoney;

        // 잔여 습득 가능 수량 파악
        int spareSlotCount = Player.inventoryMax - Turn.now.inventoryCount;

        // 번들 수량 파악
        int selectCount = 0;
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
                selectCount++;

        for (int i = 0; i < bundle.Count; i++)
        {
            //Debug.LogError("가격 체크" + spareMoney + " <--> " + bundle[i].item.cost);

            // 선택되지 않은 번들에 대한 가격 체크
            if (!bundle[i].toggle.isOn)
            {
                if (bundle[i].item.cost > spareMoney || (spareSlotCount - selectCount <= 0))
                {
                    // 예산 초과
                    bundle[i].SetPriceColor(redPrice);
                    bundle[i].canBuy = false;
                    bundle[i].toggle.enabled = false;
                }
                else
                {
                    // 구매 가능
                    bundle[i].SetPriceColor(normalPrice);
                    bundle[i].canBuy = true;
                    bundle[i].toggle.enabled = true;
                }
            }
        }
    }

    void CheckInventoryMax()
    {
        // 잔여 습득 가능 수량 파악
        int spareSlotCount = Player.inventoryMax - Turn.now.inventory.Count;

        // 번들 수량 파악
        int selectCount = 0;
        for (int i = 0; i < bundle.Count; i++)
            if (bundle[i].toggle.isOn)
                selectCount++;

        for (int i = 0; i < bundle.Count; i++)
        {
            // 잔여 슬롯 없을 경우 선택 차단
            if (spareSlotCount - selectCount <= 0)
            {
                // 예산 초과
                bundle[i].SetPriceColor(redPrice);
                bundle[i].canBuy = false;
                bundle[i].toggle.enabled = false;
            }
            else
            {
                // 구매 가능
                bundle[i].SetPriceColor(normalPrice);
                bundle[i].canBuy = true;
                bundle[i].toggle.enabled = true;
            }
        }
    }

    public void SetItemBundle(int bundleIndex, Item item)
    {
        // 잘못된 범위 입력시 중단
        if (bundleIndex < 0 || bundleIndex >= bundle.Count)
        {
            Debug.LogError("오류 :: 잘못된 메소드 사용 -> Out of range");
            Debug.Break();
            return;
        }

        // 대상 지정
        ItemShopBundle target = bundle[bundleIndex];

        // 선택 초기화
        target.toggle.enabled = true;
        target.toggle.isOn = false;

        // 아이템 설정
        target.item = item;

        // 수량 설정
        target.slot.count = 1;

        // 새로고침
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
