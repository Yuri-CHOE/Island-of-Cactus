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

    // �ڿ� ����
    Coroutine lifeRefresh = null;
    Coroutine coinRefresh = null;


    void Update()
    {
        // �÷��̾� ���� ����
        if (owner != null)
        {
            // ������ ����
            owner.life.RefreshOne();
            lifeText.text = owner.life.Value.ToString();

            // ���� ����
            owner.coin.RefreshOne();
            coinText.text = owner.coin.Value.ToString();

            // ������ ����
            //itemObject[0].���� = owner.inventory.;      // �̱���==========================

            // ������ �� �������� ���
            if (GameData.turn.now == owner)
                turnOobject.setUp(1);
            else
                turnOobject.setUp(0);
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

        // �ڿ� ���� ���
    }
}
