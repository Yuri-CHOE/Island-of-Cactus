using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
    public static ShortcutManager script = null;

    [SerializeField]
    CameraManager cm = null;

    // 숏컷 입구 블록
    public int shortCutIn = -1;
    DynamicBlock shortCutInObj = null;

    // 숏컷 출구 블록
    public int shortCutOut = -1;
    DynamicBlock shortCutOutObj = null;

    // 숏컷 안내 텍스트
    public UnityEngine.UI.Text infoText = null;
    string infoT = "지름길을 이용할 수 있습니다";
    string infoF = "비용이 부족하여 이용할 수 없습니다";

    // 숏컷 가격 텍스트
    public UnityEngine.UI.Text shortcutPrice = null;

    // 숏컷 가격
    public int price = 50;

    // 사용자
    public Player customer = null;
    int customerCoin = 0;

    // 구매 가능 여부
    bool _canUse = false;
    public bool canUse { get { return _canUse; } }

    // 비용 색상
    [SerializeField]
    Color colorT = new Color();
    [SerializeField]
    Color colorF = new Color();

    // 종료 대기
    Coroutine endWait = null;


    private void Awake()
    {
        // 퀵 등록
        script = this;
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 인덱스 초도 등록
    public void SetUp()
    {
        // 인 아웃 인덱스 등록
        for (int i = 0; i < BlockManager.script.blockCount; i++)
        {
            DynamicBlock temp = BlockManager.script.GetBlock(i).GetComponent<DynamicBlock>();

            // 인 등록
            if (temp.blockTypeDetail == BlockType.TypeDetail.shortcutIn)
            {
                shortCutIn = i;

                shortCutInObj = temp;

                transform.position = shortCutInObj.transform.position;
            }

            // 아웃 등록
            else if (temp.blockTypeDetail == BlockType.TypeDetail.shortcutOut)
            {
                shortCutOut = i;

                shortCutOutObj = temp;
            }
        }
    }


    public void CallShortcutUI(Player _customer)
    {
        // 플레이어 미입력시 중단
        if (_customer == null)
        {
            Debug.LogError("스크립트 에러 :: _customer 의 값이 null 입니다");
            Debug.Break();
            return;
        }

        // 초기화
        Clear();

        // 사용자 지정
        customer = _customer;

        // 사용자 코인수 확보
        customerCoin = customer.coin.Value;

        // UI 셋팅
        shortcutPrice.text = price.ToString();
        CheckColor();

        // 제어권 지급 - 본인 턴
        GameMaster.script.messageBox.InputControl(Player.me == Turn.now);
        GameMaster.script.messageBox.btnUse.interactable = (customer == Player.me);

        // UI 호출
        GameMaster.script.messageBox.PopUp(MessageBox.Type.ShortCut);

    }


    /// <summary>
    /// 구매 가능 여부 판단 및 색상 변경
    /// </summary>
    void CheckColor()
    {
        // 코인 충분할 경우
        if (customerCoin >= price)
        {
            // 안내 텍스트
            infoText.text = infoT;
            infoText.color = colorT;

            // 가격 색상
            shortcutPrice.color = colorT;

            // 구매 가능 여부
            _canUse = true;
        }
        // 코인 부족할 경우
        else
        {
            // 안내 텍스트
            infoText.text = infoF;
            infoText.color = colorF;

            // 가격 색상
            shortcutPrice.color = colorF;

            // 구매 가능 여부
            _canUse = false;
        }
    }



    public void Use()
    {
        // 구매 불가능시 차단
        if (!_canUse)
            return;

        // 카메라 고정
        cm.CamMoveTo(customer.avatar.transform, CameraManager.CamAngle.Top);

        // 지불 및 이동 요청
        Pay();

        // 메시지 박스 닫기
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);

        // 기존 대기 중단
        if (endWait != null)
            StopCoroutine(endWait);

        // 이동 대기 및 종료 판정
        endWait = StartCoroutine(Waiting());
    }


    /// <summary>
    /// 지름길 이용에 대한 지불 및 이동요청
    /// </summary>
    /// <param name="customer">구매자</param>
    void Pay()
    {
        // 비용 지불
        customer.coin.subtract(price);
        Debug.Log("숏컷 지불 :: " + price);

        // 이동 처리
        customer.movement.MoveSet(shortCutOutObj.transform.position, 5f, true);
    }


    public void Clear()
    {
        // 초기화
        customer = null;
        customerCoin = 0;

        _canUse = false;
    }

    IEnumerator Waiting()
    {
        // 이동 완료 대기
        while (customer.movement.isBusy)
        {
            yield return null;
        }

        // 카메라 해제
        cm.CamFree();

        // 좌표 변경
        customer.movement.location = shortCutOut;

        // 겹침 해소
        customer.movement.AvatarOverFix();

        // 이동 종료 판정
        GameMaster.script.messageBox.Out();
    }

}
