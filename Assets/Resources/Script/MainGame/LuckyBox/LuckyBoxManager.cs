using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBoxManager : MonoBehaviour
{
    public static LuckyBoxManager script = null;


    [SerializeField]
    AudioSource audio = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    UnityEngine.UI.Text nameText = null;
    [SerializeField]
    UnityEngine.UI.Text infoText = null;

    //Player owner = null;

    Vector3 hidingPos = new Vector3();


    public bool isEnd { get { return animator.GetCurrentAnimatorStateInfo(0).IsName("end"); } }
    public bool isReady { get { return animator.GetBool("isReady"); } }

    // 열기 제어
    public Coroutine coroutineOpen = null;


    void Awake()
    {
        // 럭키박스 오브젝트 등록
        script = transform.GetComponent<LuckyBoxManager>();

        // 초기 위치 기록
        hidingPos = transform.position;

        //// 사운드 지정
        //audio.clip = AudioManager.script.osfxLuckybox;
    }

    
    // Update is called once per frame    
    void Update()
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);

        // 초기화 되지 않았으며 현재 진행중 애니메이션이 "idel"일 경우
        if (!animator.GetBool("isReady"))
            if (current.IsName("idel"))
            {
                // 초기화
                Clear();

                // 다음 애니메이션 진행
                animator.SetBool("isReady", true);
            }
    }

    /// <summary>
    /// 대기 장소로 이동
    /// </summary>
    void Hide()
    {
        // 초기 위치로 이동
         transform.position = hidingPos;
    }

    /// <summary>
    /// 초기화 기능
    /// </summary>
    void Clear()
    {
        //owner = null;

        animator.SetBool("doOpen", false);
        animator.SetBool("isOpen", false);
        animator.SetBool("isReady", false);
        animator.SetBool("isCall", false);
    }

    /// <summary>
    /// 강제 초기화
    /// </summary>
    public void ClearForced()
    {
        animator.SetTrigger("Clear");
    }


    public void GetLuckyBox(Player _owner)
    {
        // 초기화 안됬으면 강제 초기화
        if (!isReady)
            ClearForced();

        // 소환 좌표 지정
        Vector3 pos = _owner.avatar.transform.position + Vector3.back * 2f;

        // 높이 지정
        pos.y = 5f;

        // 오브젝트 캐릭터에게 소환
        transform.position = pos;

        // 사운드 출력
        audio.PlayOneShot(AudioManager.script.osfxLuckybox);

        // 다음 애니메이션 진행
        animator.SetBool("isCall", true);
    }


    public void Open()
    {
        // 초기화 안됬으면 중단
        if (!isReady)
            return;

        // 애니메이션 시작
        animator.SetBool("doOpen", true);
    }

    public IEnumerator WaitAndResult(LuckyBox _luckyBox, Player target)
    {

        // 연출 종료 대기
        while (!isEnd)
        {
            //Debug.LogWarning("럭키 박스 :: 연출 종료 대기중");
            yield return null;
        }

        //

        // 결과물 출력
        MessageBox mb = GameData.gameMaster.messageBox;
        //mb.PopUp(mb.pageSwitch.GetObject(3).GetComponent<UnityEngine.UI.MessageBox.MessageBoxRule>(), 3);     // 럭키박스 타입은 현재 3번 ====== 이후 열거형으로 변경할것
        mb.PopUp(MessageBox.Type.LuckyBox);


        // 메시지 박스 확인 대기
        while (mb.gameObject.activeSelf)
        {
            //Debug.LogWarning("럭키 박스 :: 메시지 박스 확인 대기중");
            yield return null;
        }

        // 숨기기
        Hide();

        // 효과 적용
        yield return _luckyBox.Effect(target);
        Debug.Log("Lucky Box :: 효과 종료됨 = " + _luckyBox.name);

        // 이동 종료 대기
        while (target.movement.actNow.type != Action.ActionType.None || target.movement.actionsQueue.Count > 0)
        {
            yield return null;
        }

        // 종료 판정
        BlockWork.isEnd = true;

        // 초기화 진행
        ClearForced();
    }

    /// <summary>
    /// 메시지 박스 내용 설정
    /// </summary>
    /// <param name="__index"></param>
    public void SetTextByIndex(int __index)
    {
        nameText.text = LuckyBox.table[__index].name;
        infoText.text = LuckyBox.table[__index].info;
    }
}
