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

            // �ൿ�� ����
            moveText.text = owner.dice.valueTotal.ToString();


            // ������ �� �������� ���
            if (GameData.turn.now == owner)
                turnOobject.setUp(1);
            else
                turnOobject.setUp(0);



            // ���� UI Ȱ�� , ��Ȱ��
            if (owner.isStun || dead.gameObject.activeSelf)
            {
                dead.gameObject.SetActive(true);

                // ������ �ݿ�
                deadCounter.text = owner.stunCount.ToString();
            }
            //else
                //dead.gameObject.SetActive(false);
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

        // �κ��丮 ��ũ
        if(player.inventory.Count == 0)
            player.inventory = inventory;        
        else
        {
            // �κ��丮 ���
            for(int i = 0; i < inventory.Count; i++)
                inventory[i].CopyByMirror(player.inventory[i]);
        }
    }


    public void Targeting()
    {
        // ���õ� �÷��̾� Ÿ����
        GameData.gameMaster.itemManager.ItemUse(owner);
    }

}
