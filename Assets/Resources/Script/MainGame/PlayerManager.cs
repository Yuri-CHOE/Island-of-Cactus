using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static List<Player> playersNotSystem = new List<Player>();
    public static List<Player> players = new List<Player>();

    public static class SystemPlayer
    {
        public static Player cycleStart;
        public static Player miniGame;
        public static Player cycleEnd;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void SetPlayers(List<string> name, List<int> characterIndex, List<bool> isAutoPlayer)
    {
        // ���� ����
        if (name.Count == characterIndex.Count && name.Count == isAutoPlayer.Count)
        {
            // �߸��� �ڷᱸ��
            Debug.Log("error :: ����Ʈ ���� �ٸ�");
            return;
        }
        //else if (name.Count < minPlayer || name.Count > maxPlayer)    // ���� �޴������� min max �����ð�
        //{
        //    // �ο��� ����
        //    Debug.Log("error :: �÷��̾� �ο��� ������");
        //    return;
        //}

        // �ý��� �÷��̾� - ����Ŭ ���� ������
        SystemPlayer.cycleStart = new Player("System", 1, true);
        players.Add(SystemPlayer.cycleStart);

        // �÷��̾� �� AI ��� (�ý��� ����)
        for (int i = 0; i < name.Count; i++)
        {
            Player temp = new Player(name[i], characterIndex[i], isAutoPlayer[i]);
            players.Add(temp);
            playersNotSystem.Add(temp);
        }

        // �ý��� �÷��̾� - �̴ϰ��� ������
        SystemPlayer.miniGame = new Player("System", 2, true);
        players.Add(SystemPlayer.miniGame);

        // �ý��� �÷��̾� - ����Ŭ ���� ������
        SystemPlayer.cycleEnd = new Player("System", 3, true);
        players.Add(SystemPlayer.cycleEnd);
    }
}
