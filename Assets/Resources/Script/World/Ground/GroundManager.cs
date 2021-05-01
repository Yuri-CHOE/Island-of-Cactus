using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GroundManager : MonoBehaviour
{
    // 프리팹 목록
    public List<GameObject> groundList = new List<GameObject>();

    // 생성될 그라운드
    [SerializeField]
    List<GameObject> gol = new List<GameObject>();

    // 생성시 부모 오브젝트
    [SerializeField]
    Transform groundMaster;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    /// <summary>
    /// 입력된 스트링으로 지형 배치
    /// </summary>
    /// <param name="str">필드=10개, 필드구분=',', 라인구분='|', 규칙=(int, float, float, float, float, float, float, float, float, float |)</param>
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
        List<int> prefabIndex = new List<int>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> rot = new List<Vector3>();
        List<Vector3> scl = new List<Vector3>();

        int iTemp = 0;
        Vector3 temp = new Vector3();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // 잘못된 데이터 구조 차단
            if (dataStr[i].Length != 10)
                return;

            // 오브젝트 ID 구조화
            if (int.TryParse(dataStr[i][0], out iTemp)) { }
            // 구조화 완료
            prefabIndex.Add(iTemp);
            iTemp = 0;

            // position 구조화
            if (float.TryParse(dataStr[i][1], out temp.x)) { }
            if (float.TryParse(dataStr[i][2], out temp.y)) { }
            if (float.TryParse(dataStr[i][3], out temp.z)) { }
            // 구조화 완료
            pos.Add(temp);
            temp = Vector3.zero;

            // rotation 구조화;
            if (float.TryParse(dataStr[i][4], out temp.x)) { }
            if (float.TryParse(dataStr[i][5], out temp.y)) { }
            if (float.TryParse(dataStr[i][6], out temp.z)) { }
            // 구조화 완료
            rot.Add(temp);
            temp = Vector3.zero;

            // scale 구조화;
            if (float.TryParse(dataStr[i][7], out temp.x)) { }
            if (float.TryParse(dataStr[i][8], out temp.y)) { }
            if (float.TryParse(dataStr[i][9], out temp.z)) { }
            // 구조화 완료
            scl.Add(temp);
            //temp = Vector3.zero;
        }

        // 오브젝트 생성
        BuildUP(prefabIndex.ToArray(), pos.ToArray(), rot.ToArray(), scl.ToArray());
    }


    /// <summary>
    /// 지형 일괄 생성
    /// </summary>
    /// <param name="prefabIndex">원본 오브젝트 인덱스(golPrefabNum)</param>
    /// <param name="pos">결과물 오브젝트 position</param>
    /// <param name="rot">결과물 오브젝트 rotation</param>
    /// <param name="scl">결과물 오브젝트 scale</param>
    public void BuildUP(int[] prefabIndex, Vector3[] pos, Vector3[] rot, Vector3[] scl)
    {
        Debug.Log("create ground :: batch operation (start)");

        // 각 배열의 길이 불일치시 강제종료
        if (pos.Length != prefabIndex.Length && pos.Length != rot.Length && pos.Length != scl.Length)
            return;

        // 기존 리스트 및 오브젝트 제거
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
        }
        gol.Clear();

        // 반복 생성
        for (int i = 0; i < pos.Length; i++)
            Create(prefabIndex[i], pos[i], rot[i], scl[i]);

        Debug.Log("create ground :: batch operation (finish)");
    }


    /// <summary>
    /// 지형 생성
    /// </summary>
    /// <param name="prefabIndex">원본 오브젝트 인덱스(golPrefabNum)</param>
    /// <param name="pos">결과물 오브젝트 position</param>
    /// <param name="rot">결과물 오브젝트 rotation</param>
    /// <param name="scl">결과물 오브젝트 scale</param>
    /// <returns></returns>
    public Transform Create(int prefabIndex, Vector3 pos, Vector3 rot, Vector3 scl)
    {
        // 복사 대상이 없을 경우
        if (prefabIndex < 0 || prefabIndex >= groundList.Count)
            return null;

        // 생성
        Transform copyObject = Instantiate(
            groundList[prefabIndex],            // 복사할 GameObject
            pos,                                // position
            Quaternion.Euler(rot),              // rotation
            groundMaster                        // 부모 지정
            ).transform;

        // 스케일 지정
        copyObject.localScale = scl;

        //목록 추가
        gol.Add(copyObject.gameObject);

        // 부모 지정
        copyObject.SetParent(groundMaster);

        // 오브젝트 이름 변경
        copyObject.name = string.Format("{0} ({1})", groundList[prefabIndex].name, gol.Count - 1);

        // 로그 출력
        Debug.Log(string.Format("create ground :: {0}\n typeIndex={1} position=({2}, {3}, {4}) rotation=({5}, {6}, {7}) scale=({8}, {9}, {10})", copyObject.name, prefabIndex, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, scl.x, scl.y, scl.z));

        return copyObject;
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
                .Append(i)
                .Append(',')

                .Append(gol[i].transform.position.x)
                .Append(',')
                .Append(gol[i].transform.position.y)
                .Append(',')
                .Append(gol[i].transform.position.z)
                .Append(',')

                .Append(gol[i].transform.rotation.x)
                .Append(',')
                .Append(gol[i].transform.rotation.y)
                .Append(',')
                .Append(gol[i].transform.rotation.z)
                .Append(',')

                .Append(gol[i].transform.localScale.x)
                .Append(',')
                .Append(gol[i].transform.localScale.y)
                .Append(',')
                .Append(gol[i].transform.localScale.z)

                ;

            if (i != gol.Count - 1)
                sb.Append('|');
        }

        return sb.ToString();
    }
}

