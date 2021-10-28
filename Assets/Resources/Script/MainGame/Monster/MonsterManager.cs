using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //// 퀵등록
    public static MonsterManager script = null;

    // 몬스터 프리팹
    public List<Transform> monsterPrefab = new List<Transform>();


    // 이동 컨트롤러
    public CharacterMover movement = null;

    // 몬스터 애니메이터
    Animator animator = null;


    void Awake()
    {
        script = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 생성
        Create(0);

        // 숨기기
        Hide();

        // 크기 보정
        movement.bodyObject.localScale = new Vector3(5f,5f,-5f);

        // 방향 보정
        movement.bodyObject.Rotate(new Vector3(0f, 180f, 0f));


        // 제어용 가상 플레이어 등록
        movement.owner = Player.system.Monster;
        movement.owner.movement = movement;
        movement.owner.avatarBody = movement.bodyObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Create(int index)
    {
        // 비정상 작동 중단
        if (index > monsterPrefab.Count)
        {
            Debug.LogError("error :: 몬스터 인덱스 초과");
            return;
        }
        else if (monsterPrefab[index] == null)
        {
            Debug.LogError("error :: 해당 몬스터 없음");
            return;
        }

        // 기존 몬스터 파괴
        if (movement.bodyObject != null)
            Remove();

        // 생성
        movement.bodyObject = Transform.Instantiate(
            monsterPrefab[index],
            transform
            );

        // 애니메이터 등록
        animator = movement.bodyObject.GetComponent<Animator>();
    }

    public void Remove()
    {
        // 대상이 없을 경우 중단
        if (movement.bodyObject == null)
            return;

        // 연출 ========미구현
        // StartCoroutine 으로 제거 모션 구동시키면 됨, 중지가 필요 없으므로 별도 보관 불필요

        // 파괴
        Debug.Log("몬스터 :: 제거 요청됨 -> " + movement.bodyObject);
        Transform.Destroy(movement.bodyObject);

        movement.bodyObject = null;
        animator = null;
    }

    /// <summary>
    /// 몬스터 애니메이션 활성화
    /// </summary>
    public void Work()
    {
        Work(true);
    }
    /// <summary>
    /// 몬스터 애니메이션의 활성 여부를 제어
    /// 사전에 몬스터 생성 필요
    /// </summary>
    /// <param name="isActivate">true = 활성, false = 비활성</param>
    public void Work(bool isActivate)
    {
        // 비정상 작동 중단
        if (movement.bodyObject == null)
        {
            Debug.LogError("error :: 몬스터 생성 안됨");
            return;
        }
        else if (animator == null)
        {
            Debug.LogError("error :: Animator 컴포넌트가 존재하지 않음");
            return;
        }

        // 활성 제어
        animator.SetBool("work", isActivate);
    }

    public void Hide()
    {
        // 비활성화
        movement.bodyObject.gameObject.SetActive(false);


        // 위치 지정
        movement.bodyObject.position = new Vector3(0, -10, 0);

        // 애니메이션 off
        Work(false);
    }


    /// <summary>
    /// 특정 위치로 몬스터 호출
    /// </summary>
    /// <param name="position"></param>
    public void Call(int blockIndex)
    {
        // 인덱스 변경
        movement.location = blockIndex;
        movement.owner.location = blockIndex;

        // 이동
        movement.transform.position = BlockManager.script.GetBlock(blockIndex).transform.position;

        // 활성화
        movement.bodyObject.gameObject.SetActive(true);
    }

    public void Focus()
    {
        // 대상 없을 경우 중단
        if (movement == null)
            return;

        // 카메라 포커싱
        GameData.worldManager.cameraManager.CamMoveTo(movement.transform, CameraManager.CamAngle.Top);
    }

    /// <summary>
    /// 돌진 명령
    /// </summary>
    /// <returns></returns>
    public IEnumerator DashOnly(int blockCount)
    {
        // 생성 체크
        if (movement.bodyObject != null)
        {
            // 속도 수정
            movement.moveSpeed = 10f;

            // 계획
            movement.PlanMoveBy(blockCount);
                       
            // 액션 수행 대기
            while (movement.actionsQueue.Count > 0 || movement.actNow.type == Action.ActionType.None)
            {
                yield return null;
            }

        }
        else
        {
            Debug.LogError("error :: 돌진 대상을 생성해야함");
            Debug.Break();
        }
    }

    /// <summary>
    /// 소환 후 돌진 이후 숨김
    /// 소환 -> 애니메이션 -> 포커싱 -> 돌진 -> 숨김 -> 제거
    /// </summary>
    /// <param name="blockIndex">소환 지점</param>
    /// <param name="blockCount">돌진 칸수</param>
    /// <returns></returns>
    public IEnumerator Dash(int blockIndex, int blockCount)
    {
        // 소환
        Call(blockIndex);

        // 애니메이션 작동
        Work();

        // 카메라 포커싱
        script.Focus();

        // 돌진 명령
        yield return DashOnly(blockCount);

        // 몬스터 숨김
        Hide();

        //// 몬스터 제거 - 재활용으로 인한 비활성
        //Remove();

    }


}
