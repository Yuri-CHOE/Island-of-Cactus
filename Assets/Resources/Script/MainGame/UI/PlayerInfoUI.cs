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
            lifeText.text = owner.life.RefreshOne().ToString();

            // ���� ����
            coinText.text = owner.coin.RefreshOne().ToString();

            // ������ ����
            //itemObject[0].���� = owner.inventory.;      // �̱���==========================

            // �ൿ�� ����
            moveText.text = owner.dice.valueTotal.ToString();


            // ������ �� �������� ���
            if (Turn.now == owner)
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
    public void SetPlayer(Player _owner)
    {
        // �÷��̾� ����
        owner = _owner;

        // null ����
        if (owner == null)
            return;

        // ������ ����
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

        // �κ��丮 ���� ����
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].owner = owner;

            // �κ��丮 ��ũ
            inventory[i].SetUp(owner, owner.inventory[i]);
        }
    }


    public void Targeting()
    {
        // ���õ� �÷��̾� Ÿ����
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
        // �������� ������ ��� ���� ��ȸ
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            // �� ���� �˻�
            if (inventory[i].isEmpty)
            {
                // ��ܿ��� ����
                for (int j = i + 1; j < inventory.Count; j++)
                {
                    // ��ܿ� ���Կ� ������ ���� ���
                    if (!inventory[j].isEmpty)
                    {
                        // ��ܿ���
                        inventory[i].Take(inventory[j]);

                        // �ߴ�
                        break;
                    }
                }
            }
        }
    }
}
