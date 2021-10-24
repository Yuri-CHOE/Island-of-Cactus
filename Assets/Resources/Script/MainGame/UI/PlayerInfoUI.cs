using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public ObjectSwitch turnOobject;

    public Player owner = null;

    public Image face = null;

    public Image dead = null;
    public Text deadCounter = null;

    public List<ItemSlot> inventory;


    [SerializeField]
    Text lifeText;

    [SerializeField]
    Text coinText;

    [SerializeField]
    Text moveText;


    void Update()
    {
        // 플레이어 지정 이후
        if (owner != null)
        {
            // 라이프 갱신
            lifeText.text = owner.life.RefreshOne().ToString();

            // 코인 갱신
            coinText.text = owner.coin.RefreshOne().ToString();

            // 아이템 갱신
            //itemObject[0].슬롯 = owner.inventory.;      // 미구현==========================

            // 행동력 갱신
            moveText.text = owner.dice.valueTotal.ToString();


            // 주인이 턴 진행중일 경우
            if (Turn.now == owner)
                turnOobject.setUp(1);
            else
                turnOobject.setUp(0);



            // 감옥 UI 활성 , 비활성
            if (owner.isStun || dead.gameObject.activeSelf)
            {
                dead.gameObject.SetActive(true);

                // 라이프 반영
                deadCounter.text = owner.stunCount.ToString();
            }
            //else
                //dead.gameObject.SetActive(false);
        }
    }


    // 셋팅
    public void SetPlayer(Player _owner)
    {
        // 플레이어 지정
        owner = _owner;

        // null 차단
        if (owner == null)
            return;

        // 아이콘 변경
        else
        {
            if (owner.face == null)
            {
                owner.LoadFace();
                Debug.Log("warning :: owner is null");
            }

            Debug.Log(owner.face.name);
            face.sprite = owner.face;
        }

        // 인벤토리 주인 지정
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].owner = owner;

            // 인벤토리 싱크
            inventory[i].SetUp(owner, owner.inventory[i]);
        }
    }


    public void Targeting()
    {
        // 선택된 플레이어 타겟팅
        GameData.gameMaster.itemManager.target = owner;
        //GameData.gameMaster.itemManager.ItemUseByUI(owner);
        GameData.gameMaster.itemManager.ItemUseByUI();
    }

    public ItemSlot FindSlot(ItemUnit itemUnit)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].CheckUnit(itemUnit))
                return inventory[i];
        }

        return null;
    }


    public void SortInventory()
    {
        // 마지막을 제외한 모든 슬롯 순회
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            // 빈 슬롯 검색
            if (inventory[i].isEmpty)
            {
                // 당겨오기 수행
                for (int j = i + 1; j < inventory.Count; j++)
                {
                    // 당겨올 슬롯에 아이템 있을 경우
                    if (!inventory[j].isEmpty)
                    {
                        // 당겨오기
                        inventory[i].Take(inventory[j]);

                        // 중단
                        break;
                    }
                }
            }
        }
    }
}
