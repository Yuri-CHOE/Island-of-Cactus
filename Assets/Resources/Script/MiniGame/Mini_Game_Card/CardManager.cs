using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MiniGame;

public class CardManager : MonoBehaviour
{
    // 카드 목록
    public List<PairingCard> deck = new List<PairingCard>();

    // 선택된 카드
    public PairingCard card_1;
    public PairingCard card_2;

    // 정답 처리된 페어 수
    int completePair = 0;

    // AI 채점
    Coroutine AnswerAI = null;


    //// 시작 플래그 미러링
    //bool isStartMirror = false;


    // 엔딩
    //[SerializeField]
    //GameObject ending = null;



    //public Mouse_Click mouse_Click1;
    //public Mouse_Click mouse_Click2;
    //public Scenes_mini num_player;
    //public manage_Player managePlayer;
    //public string num1, num2, name1, name2;
    //public int player, /*z = 0,*/ x, number;

    public void Init()
    {
        // 해당 게임의 점수 배율 설정 - 필수
        MiniGameManager.script.scoreRiseValue = 10;

        // 카드 셋팅
        CardSetUp();

        // 시작 대기 - 자동
        StartCoroutine(DelayedGameStart());
    }

    void Awake()
    {
        //    // 해당 게임의 점수 배율 설정 - 필수
        //    MiniGameManager.script.scoreRiseValue = 10;

        //num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //매인게임에서 미니게임을 플레이할 플레이어 수를 받아옴   
        //managePlayer = GameObject.Find("Game").GetComponent<manage_Player>();

        // AI 셋팅 - 답안 형식
        MiniGameManager.answerType = MiniAI.AnswerType.pair;
        
    }

    void Start()
    {
        //player = num_player.member_num;
        //num1 = "";
        //num2 = "";
        //x = 1;
        //number = 1;

        //// 카드 셋팅
        //CardSetUp();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (x == 3)
    //    {
    //        mouse_Click1 = GameObject.Find(name1).GetComponent<Mouse_Click>();
    //        mouse_Click2 = GameObject.Find(name2).GetComponent<Mouse_Click>();
    //        Debug.Log("확인" + num1 + " " + num2);
    //        if (num1 == num2)
    //        {
    //            mouse_Click2.i = 4;
    //            mouse_Click1.i = 4;
    //            z += 1;

    //            if (z == 9)
    //            {
    //                GameObject.Find("Canvas").transform.Find("Ending").gameObject.SetActive(true);
    //            }
    //            managePlayer.plusScore = true;
    //        }
    //        else
    //        {
    //            mouse_Click2.i = 2;
    //            mouse_Click1.i = 2;
    //        }
    //        managePlayer.turn = true;
    //        x = 1;
    //    }

    //    if (z == 9)         //카드짝이 다 맞추어짐
    //    {
    //        managePlayer.scoreSetChecking();
    //        managePlayer.ranking = true;
    //        z = 0;
    //    }
    //}

    //public void num1_set(string number, string card_name)
    //{
    //    //첫번째 카드 숫자와 카드 번호
    //    num1 = number;
    //    name1 = card_name;
    //}

    //public void num2_set(string number, string card_name)
    //{
    //    //두번째 카드 숫자와 카드 번호
    //    num2 = number;
    //    name2 = card_name;
    //}


    //------------------------------------------------------------------------------------

    void Update()
    {
        // 두번째 카드 선택 시
        if (card_2 != null)
        {
            //// 선택 직후
            //if (card_2.animator.GetCurrentAnimatorStateInfo(0).IsName(""))
            //    // 비교
            //    PairCheck();

            // 뒤집기 완료시
            if (card_2.animator.GetCurrentAnimatorStateInfo(0).IsName("aniWait"))
            {
                // 체크 완료 처리
                card_1.SetAniStateCheckFinish();
                card_2.SetAniStateCheckFinish();

                // 초기화
                PairClear();

                // 모든 카드 짝 맞춰진 경우
                if (completePair == 9)
                    Ending();
                // 아직 아닐경우
                else
                    MiniGameManager.script.mpm.NextTurn();
            }
        }

        // AI 처리
        if(MiniGameManager.script.mpm.turnNow.type == Player.Type.AI)
            // 채점 하지 않는중이면
            if (AnswerAI == null)
                if(MiniGameManager.isAnswerSubmit)
                {
                    Debug.Log("미니 AI :: 채점 시작 -> " + MiniGameManager.script.mpm.turnNow.name);
                    AnswerAI = StartCoroutine(AnswerCheckAI());
                }
    }

    IEnumerator AnswerCheckAI()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(MiniGameManager.script.mpm.turnNow.miniAi.brain.latency.value);

        // 시작 딜레이
        yield return wait;
        wait.Reset();

        PairingCard temp = null;

        // 첫번째 카드 선택
        try
        {
            temp = MiniGameManager.answer.pieces[0].obj.GetComponent<PairingCard>();
            PairSelect(temp);
        }
        catch
        {
            Debug.LogError("error :: AI가 선택한 오브젝트의 스크립트 -> " + temp);
            Debug.LogError("error :: AI가 선택한 오브젝트 -> " + MiniGameManager.answer.pieces[0].obj.name);
            Debug.LogError("error :: AI가 선택한 오브젝트의 값 -> " + temp.cardNum);
        }

        // 딜레이
        wait.waitTime = MiniGameManager.script.mpm.turnNow.miniAi.brain.latency.value / 2;
        yield return wait;

        // 두번째 카드 선택
        try
        {
            temp = MiniGameManager.answer.pieces[1].obj.GetComponent<PairingCard>();
            PairSelect(temp);
        }
        catch
        {
            Debug.LogError("error :: AI가 선택한 오브젝트의 스크립트 -> " + temp);
            Debug.LogError("error :: AI가 선택한 오브젝트 -> " + MiniGameManager.answer.pieces[1].obj.name);
            Debug.LogError("error :: AI가 선택한 오브젝트의 값 -> " + temp.cardNum);
        }

        // AI 답안 리셋
        MiniGameManager.answer = CustomAI.MiniGame.MiniAI.Answer.none;
        MiniGameManager.isAnswerSubmit = false;

        // 종료 처리
        AnswerAI = null;
    }


    public void CardSetUp()
    {
        // 덱 홀수 차단
        if (deck.Count % 2 != 0)
        {
            Debug.LogError("error :: 홀수 덱 -> " + deck.Count);
            Debug.Break();
            return;
        }


        // 값 설정 ----------------------------

        // 페어 수
        int count = deck.Count / 2;
        Debug.Log("덱 설정 :: 덱 수량 -> " + deck.Count);
        Debug.Log("덱 설정 :: 페어 수량 -> " + count);

        // 카드 숫자 목록 설정
        List<int> cardNum = new List<int>();
        for (int i = 0; i < count; i++)
        {
            // 페어이므로 2번 등록
            cardNum.Add(i + 1);
            cardNum.Add(i + 1);
        }


        // 값 등록 ----------------------------

        // 등록
        int shuffledIndex = -1;
        for (int i = 0; i < deck.Count; i++)
        {
            // 인덱스 등록
            deck[i].Index = i;

            // 랜덤 카드 숫자
            shuffledIndex = Random.Range(0, cardNum.Count);

            // 카드 숫자 등록
            deck[i].cardNum = cardNum[shuffledIndex];

            // 카드 숫자 표기
            deck[i].numText.text = deck[i].cardNum.ToString();
            Debug.Log("카드 :: 숫자 배정됨 -> " + deck[i].numText.text);

            // 선택된 숫자 제외
            cardNum.RemoveAt(shuffledIndex);
        }

        // AI 셋팅 -  선택지
        for (int i = 0; i < deck.Count; i++)
        {
            MiniAI.Piece tempPiece = new MiniAI.Piece(deck[i].transform, deck[i].cardNum);
            MiniAI.restPiece.Add(tempPiece);
        }
    }


    public void PairSelect(PairingCard clickedCard)
    {
        // 천번째 선택
        if (card_1 == null)
            card_1 = clickedCard;

        // 두번째 선택
        else if (card_2 == null)
        {
            // 첫번째 선택된 카드가 아닐 경우 선택 처리
            if(clickedCard != card_1)
                card_2 = clickedCard;
        }

        // 세번째 선택 (너무 빠른 선택)
        else
        {
            Debug.LogWarning("warning :: 이전 처리 대기중");
            return;
        }

        // 카드 임시 공개
        Debug.Log("카드 선택됨 :: 카드 숫자 -> " + clickedCard.cardNum);
        clickedCard.SetAniStateOpen();

        //// AI 학습처리
        //MiniAI.Piece obj = new MiniAI.Piece(clickedCard.transform, clickedCard.cardNum);
        //MiniAI.LearnEveryAI(obj);

        // 페어 체크
        if (card_2 != null)
            PairCheck();
    }

    public void PairClear()
    {
        card_1 = null;
        card_2 = null;
    }

    bool PairCheck()
    {
        //// 차단 및 알림
        //if (card_1 == null)
        //{
        //    Debug.LogError("error :: 카드 비교 오류 -> card_1 is null");
        //    Debug.Break();
        //    return;
        //}
        //if (card_2 == null)
        //{
        //    Debug.LogError("error :: 카드 비교 오류 -> card_2 is null");
        //    Debug.Break();
        //    return;
        //}

        // 값 일치 여부
        bool result = card_1.cardNum == card_2.cardNum;

        // 일치
        if (result)
        {
            // 정답 처리
            card_1.SetAniStateRelease();
            card_2.SetAniStateRelease();

            completePair++;

            // 점수 추가
            MiniGameManager.script.ScoreAdd(1);

            // AI 반영
            List<MiniAI.Piece> pl = new List<MiniAI.Piece>();
            pl.Add(new MiniAI.Piece(card_1.transform, card_1.cardNum));
            pl.Add(new MiniAI.Piece(card_2.transform, card_2.cardNum));
            MiniAI.OpenAnswer(new MiniAI.Answer(pl));
        }
        // 불일치
        else
        {
            // AI 학습처리
            MiniAI.LearnEveryAI(new MiniAI.Piece(card_1.transform, card_1.cardNum));
            MiniAI.LearnEveryAI(new MiniAI.Piece(card_2.transform, card_2.cardNum));
        }

        Debug.Log(
            string.Format(
                "카드 비교 :: {0} -> {1} == {2}",
                result.ToString(),
                card_1.numText.text,
                card_2.numText.text
                )
            );

        return result;
    }

    void Starting()
    {
        // 초기화
        completePair = 0;

        // 공통 내용 실행
        MiniGameManager.script.Starting();
    }

    /// <summary>
    /// 코루틴으로 호출시 게임 시작 조건 충족까지 대기 후 실행됨
    /// 조건 : MiniGameManager.script.isGameStart
    /// 실행 내용 : 게임 초기 셋팅
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayedGameStart()
    {
        // 게임 시작 대기
        //while (MiniGameManager.progress < ActionProgress.Working)
        while (! MiniGameManager.script.isGameStart)
            yield return null;


        // 게임 시작
        Debug.LogWarning("미니게임 :: 게임 시작됨");

        // 모든 카드 첫 오픈
        for (int i = 0; i < deck.Count; i++)
            deck[i].animator.Play("aniTouch");

        // 첫프레임 활성화
        MiniGameManager.script.mpm.isFirstFrame = true;
    }

    void Ending()
    {
        // 게임 시작
        Debug.LogWarning("미니게임 :: 게임 종료됨");

        // 초기화
        completePair = 0;

        // 공통 내용 실행
        MiniGameManager.script.Ending();
    }
}
