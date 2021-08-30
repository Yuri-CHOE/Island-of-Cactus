using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveViewerPlayer : MonoBehaviour
{
    public int playerNum = 0;

    public Image face = null;

    public Text playerName = null;

    public int lifeValue = 0;
    public Text life = null;

    public int coinValue = 0;
    public Text coin = null;

    public List<ItemSlot> slot = new List<ItemSlot>();



    public void SetUp()
    {
        // 플레이어 존재하지 않을 경우
        if (GameSaver.scPlayers.Count < playerNum)
        {
            gameObject.SetActive(false);
            return;
        }

        string[] temp = GameSaver.scPlayers[playerNum - 1];

        int characterIndex = int.Parse(temp[2]);
        string playerName = temp[4];
        int _life = int.Parse(temp[7]);
        int _coin = int.Parse(temp[8]);
        List<int> itemIndex = new List<int>();
        List<int> itemCount = new List<int>();
        for (int i = 0; i < Player.inventoryMax; i++)
        {
            int _index = int.Parse(temp[12 + i * 2]);
            int _count = int.Parse(temp[13 + i * 2]); 

            itemIndex.Add(_index);
            itemCount.Add(_count);
        }

        SetUp(characterIndex, playerName, _life, _coin, itemIndex, itemCount);
    }
    public void SetUp(int characterIndex, string _playerName, int _life, int _coin, List<int> itemIndex, List<int> itemCount)
    {
        face.sprite = Character.table[characterIndex].GetIcon();

        playerName.text = _playerName;

        lifeValue = _life;
        life.text = lifeValue.ToString();

        coinValue = _coin;
        coin.text = coinValue.ToString();

        for (int i = 0; i < slot.Count; i++)
        {
            if (itemIndex.Count < i)
                break;
            if (itemCount.Count < i)
                break;

            if (itemIndex[i] > 0)
            {
                slot[i].item = Item.table[itemIndex[i]];
                slot[i].count = itemCount[i];
                slot[i].Refresh();
            }
        }
    }
}
