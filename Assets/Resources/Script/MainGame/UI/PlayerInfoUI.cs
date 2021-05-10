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


    //[SerializeField]
    //int Life = 10;
    //public Text lifeText;
    //public int life
    //{
    //    get { return Life; }
    //    set
    //    {
    //        lifeText.text = Life.ToString();
    //        Life = value;
    //    }
    //}

    //[SerializeField]
    //int Coin = 0;
    //public Text coinText;
    //public int coin
    //{
    //    get { return Coin; }
    //    set
    //    {
    //        coinText.text = Coin.ToString();
    //        Coin = value;
    //    }
    //}

    //void a() { int i = life; life = 1; }


    void Update()
    {
        // 플레이어 지정 이후
        if (owner != null)
        {
            lifeText.text = owner.life.Value.ToString();
            coinText.text = owner.coin.Value.ToString();
            //itemObject[0].슬롯 = owner.inventory.;      // 미구현==========================
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
    }
}
