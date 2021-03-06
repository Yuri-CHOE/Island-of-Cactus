using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public static BlockManager script = null;

    public static List<DynamicBlock> dynamicBlockList = new List<DynamicBlock>();

    [SerializeField]
    GameObject blockPrefab;

    [SerializeField]
    List<GameObject> gol;
    public int blockCount { get { return gol.Count; } }


    [SerializeField]
    Transform blockMaster;

    // 스타트 블록
    public Transform startBlock = null;    
    // 스타트 블록 하위 스타트 포인트 => 첫번째 블록 좌표     
    public Transform startPoint = null;


    private void Awake()
    {
        // 퀵등록
        script = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 최초 블록 좌표 초기화
        if (gol.Count > 0) gol[0].transform.position = new Vector3(startPoint.position.x, startPoint.position.y, startPoint.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 입력된 스트링으로 스타트 블록 설정
    /// </summary>
    /// <param name="str">스타트 블록 코드</param>
    public void SetStartBlock(string str)
    {
        // 입력 없을경우 차단
        if (str == null)
            return;

        // 데이터 구조화
        List<string> dataStr = new List<string>();
        dataStr.AddRange(str.Split(','));

        // 잘못된 데이터 구조 차단
        if (dataStr.Count != 9)
            return;

        // 데이터 적용 - 포지션
        Vector3 pos = new Vector3();
        if (float.TryParse(dataStr[0], out pos.x)) { }
        if (float.TryParse(dataStr[1], out pos.y)) { }
        if (float.TryParse(dataStr[2], out pos.z)) { }
        startBlock.position = pos;

        // 데이터 적용 - 로테이션
        Vector3 rot = new Vector3();
        if (float.TryParse(dataStr[3], out rot.x)) { }
        if (float.TryParse(dataStr[4], out rot.y)) { }
        if (float.TryParse(dataStr[5], out rot.z)) { }
        startBlock.rotation = Quaternion.Euler(rot);

        // 데이터 적용 - 스케일
        Vector3 scl = new Vector3();
        if (float.TryParse(dataStr[6], out scl.x)) { }
        if (float.TryParse(dataStr[7], out scl.y)) { }
        if (float.TryParse(dataStr[8], out scl.z)) { }
        startBlock.localScale = scl;
    }


    /// <summary>
    /// 입력된 스트링으로 블록 배치
    /// </summary>
    /// <param name="str">필드=4개, 필드구분=',', 라인구분='|', 규칙=(float,float,float,int|float,float,float,int)</param>
    public void BuildByString(string str)
    {
        // 입력 없을경우 차단
        if (str == null)
            return;

        // 데이터 구조화
        List<string[]> dataStr = new List<string[]>();
        string[] line = str.Split('|');

        for (int i = 0; i < line.Length; i++)
            dataStr.Add(line[i].Split(','));

        // 데이터 구조화
        List<Vector3> v3 = new List<Vector3>();
        List<int> yRot = new List<int>();
        List<BlockType.TypeDetail> td = new List<BlockType.TypeDetail>();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // 잘못된 데이터 구조 차단
            if (dataStr[i].Length != 5)
                return;

            // position 구조화
            Vector3 temp = new Vector3();
            if (float.TryParse(dataStr[i][0], out temp.x)) { }
            if (float.TryParse(dataStr[i][1], out temp.y)) { }
            if (float.TryParse(dataStr[i][2], out temp.z)) { }

            // 방향 구조화
            int temp2 = 0;
            if (int.TryParse(dataStr[i][3], out temp2)) { }

            // 블록 타입 구조화
            int temp3 = 0;
            if (int.TryParse(dataStr[i][4], out temp3)) { }

            // 구조화 완료
            v3.Add(temp);
            yRot.Add(temp2);
            td.Add((BlockType.TypeDetail)temp3);
        }
                
        // 오브젝트 생성
        BuildUP(v3.ToArray(), yRot.ToArray(), td.ToArray());
    }


    /// <summary>
    /// 블록 일괄 생성
    /// </summary>
    /// <param name="pos">결과물 오브젝트 position 목록</param>
    /// <param name="direction"> L=-1, U=0, R=1, D=2,none=3</param>
    /// <param name="td">결과물 오브젝트 블록 타입 목록</param>
    public void BuildUP(Vector3[] pos, int[] direction, BlockType.TypeDetail[] td)
    {
        Debug.Log("create block :: batch operation (start)");

        // position 배열과 BlockType.TypeDetail 배열의 길이 불일치시 강제종료
        if (pos.Length != td.Length && pos.Length != direction.Length)
            return;

        // copyTarget 리스트 및 오브젝트 제거
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
            //DeleteObject(gol[i]);
        }
        gol.Clear();

        // 반복 생성
        for (int i = 0; i < pos.Length; i++)
            Create(pos[i], direction[i], td[i]);

        // 모든 리스트 새로고침
        RefreshAllObject();

        Debug.Log("create block :: batch operation (finish)");
    }


    /// <summary>
    /// 블록 생성
    /// </summary>
    /// <param name="copyTarget">원본 오브젝트</param>
    /// <param name="pos">결과물 오브젝트 position</param>
    /// <param name="direction"> L=-1, U=0, R=1, D=2,none=3</param>
    /// <param name="td">결과물 오브젝트 블록 타입</param>
    public Transform Create(Vector3 pos, int direction, BlockType.TypeDetail td)
    {

        // 복사 대상이 없을 경우
        if (blockPrefab == null)
            return null;

        // 생성
        Transform copyObject = Instantiate(
            blockPrefab,                            // 복사할 Ga
            pos,                                    // position
            Quaternion.Euler(Vector3.zero),         // rotation
            blockMaster                             // 부모 지정
            ).transform;

        // 블록 속성 지정 및 적용
        DynamicBlock db = copyObject.GetComponent<DynamicBlock>();
        db.blockType = BlockType.GetTypeByDetail(td);
        db.blockTypeDetail = td;
        db.Refresh();

        // 블록 방향 설정
        db.ReDirection(direction);

        //목록 추가
        gol.Add(copyObject.gameObject);

        // 오브젝트 이름 변경
        copyObject.name = string.Format("Block ({0})", gol.Count - 1);

        // 로그 출력
        Debug.Log(string.Format("create block :: {0}\n position=({1}, {2}, {3}) direction={4} blockType={5} blockTypeDetail{6}", copyObject.name, pos.x, pos.y, pos.z, direction*90, db.blockType, db.blockTypeDetail));

        return copyObject;
    }


    /// <summary>
    /// 전체 블록의 각 TypeDetail을 기준으로 Type 및 머테리얼, 이름, 방향 새로고침
    /// </summary>
    public void RefreshAllObject()
    {
        DynamicBlock db;

        for (int i = 0; i < gol.Count; i++)
        {
            db = gol[i].GetComponent<DynamicBlock>();

            // 블록 타입 새로고침
            db.SetType(BlockType.GetTypeByDetail(db.blockTypeDetail));

            // 블록 머테리얼 새로고침
            db.Refresh();

            // 이름 체크
            if (gol[i].name != string.Format("Block ({0})", i))
                gol[i].name = string.Format("Block ({0})", i);

            // 블록 방향 재설정
            if (i != gol.Count - 1)     // 마지막 리스트가 아닌지 체크
                db.ReDirection(gol[i + 1].transform.position);
            else
                db.ReDirection(gol[0].transform.position);
        }
    }


    /// <summary>
    /// 대상 오브젝트 삭제 기능
    /// </summary>
    public void DeleteObject(GameObject obj)
    {
        // 블록이 아닐경우 차단
        if (obj == null)
            return;

        // 블록이 아닐경우 차단
        if (!CheckObjectList(obj))
            return;

        // 리스트 제거
        gol.Remove(obj);
        RefreshAllObject();

        // 오브젝트 제거
        Destroy(obj);
    }


    /// <summary>
    /// 블록 리스트에 포함된 오브젝트인지 확인
    /// </summary>
    /// <param name="obj">대상 오브젝트</param>
    public bool CheckObjectList(GameObject obj)
    {
        return gol.Contains(obj);
    }


    /// <summary>
    /// 등록된 오브젝트를 코드(string)로 반환
    /// </summary>
    public string GetBuildCode()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < gol.Count; i++)
        {
            sb
                .Append(gol[i].transform.position.x)
                .Append(',')
                .Append(gol[i].transform.position.y)
                .Append(',')
                .Append(gol[i].transform.position.z)
                .Append(',')
                .Append(gol[i].GetComponent<DynamicBlock>().GetDirection())
                .Append(',')
                .Append((int)gol[i].GetComponent<DynamicBlock>().blockTypeDetail)
                ;

            if (i != gol.Count - 1)
                sb.Append('|');
        }

        return sb.ToString();
    }


    /// <summary>
    /// 출발지에서 일정량 떨어진 블록의 인덱스값 계산
    /// </summary>
    /// <param name="index">출발지 인덱스</param>
    /// <param name="movement"> 이동 할 거리</param>
    /// <returns></returns>
    public int indexLoop(int index, int movement)
    {
        int result = index + movement;

        // 값 최소화
        result = result % gol.Count;

        // 음수 처리
        if (result < 0)
            result += gol.Count;

        // 초과 처리
        if(result >= gol.Count)
            result -= gol.Count;
        
        return result;
    }

    /// <summary>
    /// 인덱스 값으로 블록 오브젝트 반환
    /// </summary>
    /// <param name="number">인덱스 값</param>
    /// <returns></returns>
    public GameObject GetBlock(int number)
    {
        return gol[indexLoop(0, number)];
    }

    /// <summary>
    /// 장애물 생성
    /// </summary>
    //public void CreateBarricade(int blockIndex, DynamicObject dynamicObject)
    //{
    //    // 유효범위 필터링
    //    if (blockIndex > 0 || blockIndex >= gol.Count)
    //        return;

    //    DynamicObject.objectList[blockIndex].Add(dynamicObject);
    //}


    public void SetCorner()
    {
        // 블록 1개 이하일 경우 중단
        if (gol.Count <= 1)
            return;

        DynamicBlock dbNow;
        DynamicBlock dbPre;

        for (int i = 0; i < gol.Count; i++)
        {
            // 현재칸 설정
            dbNow = gol[i].GetComponent<DynamicBlock>();

            // 다음칸 설정
            dbPre = gol[indexLoop(i, -1)].GetComponent<DynamicBlock>();

            // 값 설정
            dbNow.isCorner = dbNow.GetDirection() != dbPre.GetDirection();
        }

    }

    public void SetDynamicBlockList()
    {
        dynamicBlockList.Clear();

        for (int i = 0; i < blockCount; i++)
            dynamicBlockList.Add(
                gol[i].GetComponent<DynamicBlock>()
                );
    }
}
