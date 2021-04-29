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
            int index = Random.Range(0, max);// 이부분 max를 테이블읽어서 아이템 최대 수량으로 교체
            bundle[i].item = new Item(index);        // new 로 새로 만들지 말고 아이템 테이블에서 인덱스로 참조 시킬것

            // 가격 텍스트 설정
            bundle[i].Refresh();
        }

        // 가격 텍스트 색상 초기화
        CheckMoney();

        // 아이템 공개
        itemGrop.SetActive(true);
    }

    void CheckMoney()
    {
        for (int i = 0; i < bundle.Count; i++)
        {
            if (bundle[i].item.cost > playerMoney)
            {
                // 예산 초과
                bundle[i].SetPriceColor(redPrice);
                bundle[i].canBuy = false;
            }
            else
            {
                // 구매 가능
                bundle[i].SetPriceColor(normalPrice);
                bundle[i].canBuy = true;
            }
        }
    }
}
