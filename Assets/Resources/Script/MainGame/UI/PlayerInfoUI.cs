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
        // �÷��̾� ���� ����
        if (owner != null)
        {
            lifeText.text = owner.life.Value.ToString();
            coinText.text = owner.coin.Value.ToString();
            //itemObject[0].���� = owner.inventory.;      // �̱���==========================
        }
    }


    // ����
    public void SetPlayer(Player player)
    {
        // �÷��̾� ����
        owner = player;

        // null ����
        if (owner == null)
            return;

        // ������ ����
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
