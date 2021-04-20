using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{

    [SerializeField]
    GameObject blockPrefab;

    [SerializeField]
    List<GameObject> gol;

    [SerializeField]
    Transform blockMaster;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
            blockPrefab,
            pos,
            blockPrefab.transform.rotation
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

        // 부모 지정
        copyObject.SetParent(blockMaster);

        // 오브젝트 이름 변경
        copyObject.name = string.Format("Block ({0})", gol.Count - 1);

        return copyObject;
    }


    /// <summary>
    /// 전체 블록의 각 TypeDetail을 기준으로 Type 및 머테리얼, 이름, 방향 새로고침
    /// </summary>
    public void RefreshAllObject()
    {
        for (int i = 0; i < gol.Count; i++)
        {
            DynamicBlock db = gol[i].GetComponent<DynamicBlock>();

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
}
