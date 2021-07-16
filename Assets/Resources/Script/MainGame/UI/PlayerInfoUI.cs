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
            owner.life.RefreshOne();
            lifeText.text = owner.life.Value.ToString();

            // 코인 갱신
            owner.coin.RefreshOne();
            coinText.text = owner.coin.Value.ToString();

            // 아이템 갱신
            //itemObject[0].슬롯 = owner.inventory.;      // 미구현==========================

            // 행동력 갱신
            moveText.text = owner.dice.valueTotal.ToString();


            // 주인이 턴 진행중일 경우
            if (GameData.turn.now == owner)
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
    public void SetPlayer(Player player)
    {
        // 플레이어 지정
        owner = player;

        // null 차단
        if (owner == null)
            return;

        // 아이콘 변경
        else
        {
            if (owner.face == null)
            {
                owner.LoadFace();
                Debug.Log("chk");
            }

            Debug.Log(owner.face.name);
            face.sprite = owner.face;
        }

        // 인벤토리 싱크
        if(player.inventory.Count == 0)
            player.inventory = inventory;        
        else
        {
            // 인벤토리 계승
            for(int i = 0; i < inventory.Count; i++)
                inventory[i].CopyByMirror(player.inventory[i]);
        }
    }


    public void Targeting()
    {
        // 선택된 플레이어 타겟팅
        GameData.gameMaster.itemManager.ItemUse(owner);
    }

}
