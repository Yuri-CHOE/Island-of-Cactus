using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public ObjectSwitch turnOobject;

    public Player owner = null;

    public Image face = null;

    public List<GameObject> itemObject;


    [SerializeField]
    Text lifeText;

    [SerializeField]
    Text coinText;

    // 자원 갱신
    Coroutine lifeRefresh = null;
    Coroutine coinRefresh = null;


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

            // 주인이 턴 진행중일 경우
            if (GameData.turn.now == owner)
                turnOobject.setUp(1);
            else
                turnOobject.setUp(0);
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

        // 자원 갱신 등록
    }
}
