using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DecorManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> decorBone = new List<GameObject>();
    public List<GameObject> decorCactus = new List<GameObject>();
    public List<GameObject> decorOasis = new List<GameObject>();
    public List<GameObject> decorObelisk = new List<GameObject>();
    public List<GameObject> decorPlant = new List<GameObject>();
    public List<GameObject> decorPyramid = new List<GameObject>();
    public List<GameObject> decorRock = new List<GameObject>();
    public List<GameObject> decorSand = new List<GameObject>();
    public List<GameObject> decorTemple = new List<GameObject>();
    public List<GameObject> decorTree = new List<GameObject>();

    [SerializeField]
    List<GameObject> gol = new List<GameObject>();
    List<int> golTypeNum = new List<int>();               // gol의 각 오브젝트별 PrefabList 번호
    List<int> golPrefabNum = new List<int>();             // gol의 각 오브젝트별 Prefab 인덱스값


    [SerializeField]
    Transform decorMaster;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 입력된 스트링으로 데코 배치
    /// </summary>
    /// <param name="str">필드=11개, 필드구분=',', 라인구분='|', 규칙=(int, int, float, float, float, float, float, float, float, float, float |)</param>
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
        List<int> typeNum = new List<int>();
        List<int> prefabIndex = new List<int>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> rot = new List<Vector3>();
        List<Vector3> scl = new List<Vector3>();

        int iTemp = 0;
        Vector3 temp = new Vector3();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // 잘못된 데이터 구조 차단
            if (dataStr[i].Length != 11)
                return;

            // 오브젝트 ID 구조화
            if (int.TryParse(dataStr[i][0], out iTemp)) { }
            // 구조화 완료
            typeNum.Add(iTemp);
            iTemp = 0;

            // 오브젝트 ID 구조화
            if (int.TryParse(dataStr[i][1], out iTemp)) { }
            // 구조화 완료
            prefabIndex.Add(iTemp);
            iTemp = 0;

            // position 구조화
            if (float.TryParse(dataStr[i][2], out temp.x)) { }
            if (float.TryParse(dataStr[i][3], out temp.y)) { }
            if (float.TryParse(dataStr[i][4], out temp.z)) { }
            // 구조화 완료
            pos.Add(temp);
            temp = Vector3.zero;

            // rotation 구조화;
            if (float.TryParse(dataStr[i][5], out temp.x)) { }
            if (float.TryParse(dataStr[i][6], out temp.y)) { }
            if (float.TryParse(dataStr[i][7], out temp.z)) { }
            // 구조화 완료
            rot.Add(temp);
            temp = Vector3.zero;

            // scale 구조화;
            if (float.TryParse(dataStr[i][8], out temp.x)) { }
            if (float.TryParse(dataStr[i][9], out temp.y)) { }
            if (float.TryParse(dataStr[i][10], out temp.z)) { }
            // 구조화 완료
            scl.Add(temp);
            //temp = Vector3.zero;
        }

        // 오브젝트 생성
        BuildUP(typeNum.ToArray(), prefabIndex.ToArray(), pos.ToArray(), rot.ToArray(), scl.ToArray());
    }


    /// <summary>
    /// 데코 일괄 생성
    /// </summary>
    /// <param name="decorTypeNum">원본 오브젝트 타입(golPrefabNum)</param>
    /// <param name="prefabIndex">원본 오브젝트 인덱스(golPrefabNum)</param>
    /// <param name="pos">결과물 오브젝트 position</param>
    /// <param name="rot">결과물 오브젝트 rotation</param>
    /// <param name="scl">결과물 오브젝트 scale</param>
    public void BuildUP(int[] decorTypeNum, int[] prefabIndex, Vector3[] pos, Vector3[] rot, Vector3[] scl)
    {
        Debug.Log("create decor :: batch operation (start)");

        // 각 배열의 길이 불일치시 강제종료
        if (pos.Length != prefabIndex.Length && pos.Length != rot.Length && pos.Length != scl.Length)
            return;

        // copyTarget 리스트 및 오브젝트 제거
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
        }
        gol.Clear();
        golTypeNum.Clear();
        golPrefabNum.Clear();

        // 반복 생성
        for (int i = 0; i < pos.Length; i++)
            Create(decorTypeNum[i], prefabIndex[i], pos[i], rot[i], scl[i]);

        Debug.Log("create decor :: batch operation (finish)");
    }


    /// <summary>
    /// 데코 생성
    /// </summary>
    /// <param name="decorTypeNum">원본 오브젝트 타입(golPrefabNum)</param>
    /// <param name="prefabIndex">원본 오브젝트 인덱스(golPrefabNum)</param>
    /// <param name="pos">결과물 오브젝트 position</param>
    /// <param name="rot">결과물 오브젝트 rotation</param>
    /// <param name="scl">결과물 오브젝트 scale</param>
    /// <returns></returns>
    public Transform Create(int decorTypeNum, int prefabIndex, Vector3 pos, Vector3 rot, Vector3 scl)
    {
        // 잘못된 타입 중단
        if (decorTypeNum == (int)DecorType.Type.None || decorTypeNum >= DecorType.GetTypeCount())
            return null;

        // 형변환
        DecorType.Type dt = (DecorType.Type)decorTypeNum;

        // 복사 대상이 없을 경우
        if (prefabIndex < 0 || prefabIndex >= GetDecorList(dt).Count)
            return null;

        // 생성
        Transform copyObject = Instantiate(
            GetDecorList(dt)[prefabIndex],      // 복사할 GameObject
            pos,                                // position
            Quaternion.Euler(rot),              // rotation
            decorMaster                         // 부모 지정
            ).transform;

        // 스케일 지정
        copyObject.localScale = scl;

        //목록 추가
        gol.Add(copyObject.gameObject);
        golTypeNum.Add(decorTypeNum);
        golPrefabNum.Add(prefabIndex);

        // 부모 지정
        copyObject.SetParent(decorMaster);

        // 오브젝트 이름 변경
        copyObject.name = string.Format("{0} ({1})", GetDecorList(dt)[prefabIndex].name.Split(' ')[0], gol.Count - 1);

        // 로그 출력
        Debug.Log(string.Format("create decor :: {0}\n type={1} typeIndex={2} position=({3}, {4}, {5}) rotation=({6}, {7}, {8}) scale=({9}, {10}, {11})", copyObject.name, decorTypeNum, prefabIndex, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, scl.x, scl.y, scl.z));

        return copyObject;
    }


    /// <summary>
    /// 전체 데코의 이름 새로고침
    /// </summary>
    public void RefreshAllObject()
    {
        for (int i = 0; i < gol.Count; i++)
        {
            // 이름 체크
            if (gol[i].name != string.Format("{0} ({1})", gol[i].name.Split(' ')[0], gol.Count - 1))
                gol[i].name = string.Format("{0} ({1})", gol[i].name.Split(' ')[0], gol.Count - 1);
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
        int index = gol.IndexOf(obj);
        golPrefabNum.RemoveAt(index);
        golTypeNum.RemoveAt(index);
        gol.RemoveAt(index);

        // 오브젝트 제거
        Destroy(obj);
    }


    /// <summary>
    /// 오브젝트 리스트에 포함된 오브젝트인지 확인
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
                .Append(golTypeNum[i])
                .Append(',')
                .Append(golPrefabNum[i])
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


    public int GetDecorCount()
    {
        return
            decorBone.Count +
            decorCactus.Count +
            decorObelisk.Count +
            decorPlant.Count +
            decorPyramid.Count +
            decorRock.Count +
            decorSand.Count +
            decorTemple.Count +
            decorTree.Count +
            decorOasis.Count;
    }


    /// <summary>
    /// DecorType.Type으로 적합한 리스트 반환
    /// </summary>
    /// <param name="dt">장식물 타입</param>
    /// <returns></returns>
    public List<GameObject> GetDecorList(DecorType.Type dt)
    {
        switch (dt)
        {
            case DecorType.Type.Bone:
                return decorBone;

            case DecorType.Type.Cactus:
                return decorCactus;

            case DecorType.Type.Obelisk:
                return decorObelisk;

            case DecorType.Type.Plant:
                return decorPlant;

            case DecorType.Type.Pyramid:
                return decorPyramid;

            case DecorType.Type.Rock:
                return decorRock;

            case DecorType.Type.Sand:
                return decorSand;

            case DecorType.Type.Temple:
                return decorTemple;

            case DecorType.Type.Tree:
                return decorTree;

            case DecorType.Type.Oasis:
                return decorOasis;

            default:
                Debug.Log("null");
                return null;
        }
    }
    /// <summary>
    /// int로 적합한 리스트 반환
    /// </summary>
    /// <param name="dt">장식물 타입</param>
    /// <returns></returns>
    public List<GameObject> GetDecorList(int num)
    {
        if (num < 0 || num >= DecorType.GetTypeCount())
            return null;

        return GetDecorList((DecorType.Type)num);
    }
}
