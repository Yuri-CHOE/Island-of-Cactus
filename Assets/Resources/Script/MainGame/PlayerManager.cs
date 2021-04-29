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
        // 오류 차단
        if (name.Count == characterIndex.Count && name.Count == isAutoPlayer.Count)
        {
            // 잘못된 자료구조
            Debug.Log("error :: 리스트 길이 다름");
            return;
        }
        //else if (name.Count < minPlayer || name.Count > maxPlayer)    // 게임 메니저에서 min max 가져올것
        //{
        //    // 인원수 오류
        //    Debug.Log("error :: 플레이어 인원수 부적합");
        //    return;
        //}

        // 시스템 플레이어 - 사이클 시작 관리자
        SystemPlayer.cycleStart = new Player("System", 1, true);
        players.Add(SystemPlayer.cycleStart);

        // 플레이어 및 AI 목록 (시스템 제외)
        for (int i = 0; i < name.Count; i++)
        {
            Player temp = new Player(name[i], characterIndex[i], isAutoPlayer[i]);
            players.Add(temp);
            playersNotSystem.Add(temp);
        }

        // 시스템 플레이어 - 미니게임 관리자
        SystemPlayer.miniGame = new Player("System", 2, true);
        players.Add(SystemPlayer.miniGame);

        // 시스템 플레이어 - 사이클 종료 관리자
        SystemPlayer.cycleEnd = new Player("System", 3, true);
        players.Add(SystemPlayer.cycleEnd);
    }
}
