using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniReportSlot : MonoBehaviour
{
    public Player owner = null;

    [SerializeField]
    Text rankText = null;

    [SerializeField]
    Image border = null;


    [SerializeField]
    Image face = null;

    [SerializeField]
    Text score = null;
    
    [SerializeField]
    Text reward = null;

    [SerializeField]
    Text playerName = null;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Player player, Color rankColor, string _rankText)
    {
        Debug.Log("미니게임 :: 정산 - 셋업 요청됨-> " + player.name);

        // 소유자 입력
        owner = player;

        // 등수 표시
        rankText.text = _rankText;

        // 등수 테두리
        border.color = rankColor;

        // 캐릭터 아이콘
        face.sprite = owner.face;

        // 점수
        score.text = owner.miniInfo.score.ToString();

        // 보상
        reward.text = owner.miniInfo.reward.ToString();

        // 플레이어 이름
        playerName.text = owner.name;
    }
}
