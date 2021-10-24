using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// 신형
public class MiniGamePlayer : MonoBehaviour
{
    // 플레이어
    Player _owner = null;
    public Player owner { get { return _owner; } set { SetOwner(value); } }

    // 아이콘 UI
    [SerializeField]
    Image icon = null;

    // 미니게임 성과
    ref MiniScore info { get { return ref owner.miniInfo; } }

    // 미니게임 점수 추가갑
    public int scorePlus = 0;

    // 한번에 추가될 점수
    int scoreDot = 0;


    // 점수 박스
    public Image scoreBox = null;

    // 색상
    [SerializeField] Color deadColor = new Color();

    // 점수 텍스트
    public Text txtScore = null;

    // 턴 표시 라인
    public CanvasGroup turnImg = null;

    // 턴 블링크
    float blinkValue = 0f;
    float blinkSpeed = 2f;


    // 턴 확인
    public bool isMyTurn { get { return owner == MiniGameManager.script.mpm.turnNow; } }


    void Start()
    {

    }

    void Update()
    {
        // 시작 전 차단
        //if (MiniGameManager.progress != ActionProgress.Working)
        if (!MiniGameManager.script.isGameStart)
            return;

        // 점수 처리
        if (scorePlus != 0)
        {
            // 보정
            if (scorePlus < -10)
                scoreDot = -10;
            else if (scorePlus > 10)
                scoreDot = 10;
            else
                scoreDot = scorePlus;

            // 변동량 만큼 제외
            scorePlus -= scoreDot;

            // 변동량 * 점수 상승폭 만큼 점수 반영
            //info.score += scoreDot;
            info.ScorePlus(scoreDot);

            // 점수 업데이트
            ScoreTextSync(info.score);
        }

        //// 사망 처리
        //if (owner.miniInfo.isDead)
        //{
        //    // 색상 변경
        //    scoreBox.color = deadColor;

        //    // 스크립트 비활성
        //    enabled = false;
        //}

        // 턴 표시
        if (isMyTurn)
        {
            // 값 증가
            blinkValue += Time.deltaTime * blinkSpeed;

            // 값 보정
            blinkValue %= 2f;

            // 투명도 반영
            // 1 - blinkValue : ( -1 -> 0 -> 1 -> -1 ) 값 순환
            // 절대값 처리 Mathf.Abs() : ( 1 -> 0 -> 1 -> 1 ) 값 순환
            turnImg.alpha = Mathf.Abs(1f - blinkValue);
        }
    }

    public void SetOwner(Player __owner)
    {
        // 주인 등록
        _owner = __owner;

        // 주인의 미니게임 정보 싱크
        info = _owner.miniInfo;

        // 주인의 미니게임 UI 등록
        _owner.miniPlayerUI = this;

        // 아이콘 사용
        icon.sprite = owner.character.GetIcon();

        // 참여 여부 결정
        if (!info.join)
        {
            // 자체 활성화 혹은 비활성화
            gameObject.SetActive(false);
        }
        else
        {
            // 점수 싱크
            ScoreTextSync(info.score);

            // AI 플레이어 자동 준비
            if (_owner.type == Player.Type.AI)
                _owner.miniInfo.isReady = true;
        }
    }

    void ScoreTextSync(int value)
    {
        //txtScore.text = info.score.ToString();
        txtScore.text = value.ToString();
    }

    /// <summary>
    /// 미니게임 플레이 권한 박탈 (사망)
    /// </summary>
    public void Death()
    {
        // 사망 처리
        info.isDead = true;

        // UI 반영
        scoreBox.color = deadColor;

        // 스크립트 비활성
        enabled = false;
    }

    public void BlinkOff()
    {
        blinkValue = 1f;
        turnImg.alpha = 0f;
    }

}

/// <summary>
/// 구형
/// </summary>
//public class MiniGamePlayer : MonoBehaviour
//{
//    // 플레이어
//    public Player owner = null;

//    // 플레이어 아이콘
//    public Sprite face = null;

//    //미니게임 점수
//    public int score = 0;
//    public bool plusScore = false;
//    public bool minusScore = false;
//    public bool scoreSetCheck = false;

//    //미니게임 등수
//    public int rank = 0;

//    //미니게임 참가 여부
//    public bool join = false;

//    //현재 자신의 차례인지 확인
//    public bool myTurn = false;

//    //플레이어 death 오브젝트
//    public GameObject deathUi;

//    //플레이어 스코어 텍스트 오브젝트
//    public Text txtScore;


//    void Start()
//    {
//        score = 0;
//        txtScore.text = score.ToString();
//    }

//    void Update()
//    {
//        if (join)
//        {
//            // 자체 활성화 혹은 비활성화
//            gameObject.SetActive(false);
//        }

//        turnCehck();
//        scoreCheck();
//    }

//    public void LoadFace()
//    {
//        // 아이콘 로드
//        Debug.Log(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));
//        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));

//        // 이미지 유효 검사
//        if (temp == null)
//        {
//            // 기본 아이콘 대체 처리
//            Debug.Log(@"UI/playerInfo/player");
//            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
//        }

//        // 최종 실패 처리
//        if (temp == null)
//            Debug.Log("로드 실패 :: UI/playerInfo/player");
//        // 아이콘 변경
//        else
//            face = temp;
//    }

//    void turnCehck()
//    {
//        if (myTurn)
//        {
//            deathUi.gameObject.SetActive(false);
//        }
//        else
//        {
//            deathUi.gameObject.SetActive(true);
//        }
//    }

//    void scoreCheck()
//    {
//        scoreSetCheck = false;

//        if (plusScore)
//        {
//            score += 100;
//            txtScore.text = score.ToString();
//            scoreSetCheck = true;
//            plusScore = false;
//        }
//    }
//}