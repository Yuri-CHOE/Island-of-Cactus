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
    List<int> golTypeNum = new List<int>();               // gol�� �� ������Ʈ�� PrefabList ��ȣ
    List<int> golPrefabNum = new List<int>();             // gol�� �� ������Ʈ�� Prefab �ε�����


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
    /// �Էµ� ��Ʈ������ ���� ��ġ
    /// </summary>
    /// <param name="str">�ʵ�=11��, �ʵ屸��=',', ���α���='|', ��Ģ=(int, int, float, float, float, float, float, float, float, float, float |)</param>
    public void BuildByString(string str)
    {
        // �Է� ������� ����
        if (str == null)
            return;

        // ������ ����ȭ
        List<string[]> dataStr = new List<string[]>();
        string[] line = str.Split('|');

        for (int i = 0; i < line.Length; i++)
            dataStr.Add(line[i].Split(','));

        // ������ ����ȭ
        List<int> typeNum = new List<int>();
        List<int> prefabIndex = new List<int>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> rot = new List<Vector3>();
        List<Vector3> scl = new List<Vector3>();

        int iTemp = 0;
        Vector3 temp = new Vector3();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // �߸��� ������ ���� ����
            if (dataStr[i].Length != 11)
                return;

            // ������Ʈ ID ����ȭ
            if (int.TryParse(dataStr[i][0], out iTemp)) { }
            // ����ȭ �Ϸ�
            typeNum.Add(iTemp);
            iTemp = 0;

            // ������Ʈ ID ����ȭ
            if (int.TryParse(dataStr[i][1], out iTemp)) { }
            // ����ȭ �Ϸ�
            prefabIndex.Add(iTemp);
            iTemp = 0;

            // position ����ȭ
            if (float.TryParse(dataStr[i][2], out temp.x)) { }
            if (float.TryParse(dataStr[i][3], out temp.y)) { }
            if (float.TryParse(dataStr[i][4], out temp.z)) { }
            // ����ȭ �Ϸ�
            pos.Add(temp);
            temp = Vector3.zero;

            // rotation ����ȭ;
            if (float.TryParse(dataStr[i][5], out temp.x)) { }
            if (float.TryParse(dataStr[i][6], out temp.y)) { }
            if (float.TryParse(dataStr[i][7], out temp.z)) { }
            // ����ȭ �Ϸ�
            rot.Add(temp);
            temp = Vector3.zero;

            // scale ����ȭ;
            if (float.TryParse(dataStr[i][8], out temp.x)) { }
            if (float.TryParse(dataStr[i][9], out temp.y)) { }
            if (float.TryParse(dataStr[i][10], out temp.z)) { }
            // ����ȭ �Ϸ�
            scl.Add(temp);
            //temp = Vector3.zero;
        }

        // ������Ʈ ����
        BuildUP(typeNum.ToArray(), prefabIndex.ToArray(), pos.ToArray(), rot.ToArray(), scl.ToArray());
    }


    /// <summary>
    /// ���� �ϰ� ����
    /// </summary>
    /// <param name="decorTypeNum">���� ������Ʈ Ÿ��(golPrefabNum)</param>
    /// <param name="prefabIndex">���� ������Ʈ �ε���(golPrefabNum)</param>
    /// <param name="pos">����� ������Ʈ position</param>
    /// <param name="rot">����� ������Ʈ rotation</param>
    /// <param name="scl">����� ������Ʈ scale</param>
    public void BuildUP(int[] decorTypeNum, int[] prefabIndex, Vector3[] pos, Vector3[] rot, Vector3[] scl)
    {
        Debug.Log("create decor :: batch operation (start)");

        // �� �迭�� ���� ����ġ�� ��������
        if (pos.Length != prefabIndex.Length && pos.Length != rot.Length && pos.Length != scl.Length)
            return;

        // copyTarget ����Ʈ �� ������Ʈ ����
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
        }
        gol.Clear();
        golTypeNum.Clear();
        golPrefabNum.Clear();

        // �ݺ� ����
        for (int i = 0; i < pos.Length; i++)
            Create(decorTypeNum[i], prefabIndex[i], pos[i], rot[i], scl[i]);

        Debug.Log("create decor :: batch operation (finish)");
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="decorTypeNum">���� ������Ʈ Ÿ��(golPrefabNum)</param>
    /// <param name="prefabIndex">���� ������Ʈ �ε���(golPrefabNum)</param>
    /// <param name="pos">����� ������Ʈ position</param>
    /// <param name="rot">����� ������Ʈ rotation</param>
    /// <param name="scl">����� ������Ʈ scale</param>
    /// <returns></returns>
    public Transform Create(int decorTypeNum, int prefabIndex, Vector3 pos, Vector3 rot, Vector3 scl)
    {
        // �߸��� Ÿ�� �ߴ�
        if (decorTypeNum == (int)DecorType.Type.None || decorTypeNum >= DecorType.GetTypeCount())
            return null;

        // ����ȯ
        DecorType.Type dt = (DecorType.Type)decorTypeNum;

        // ���� ����� ���� ���
        if (prefabIndex < 0 || prefabIndex >= GetDecorList(dt).Count)
            return null;

        // ����
        Transform copyObject = Instantiate(
            GetDecorList(dt)[prefabIndex],      // ������ GameObject
            pos,                                // position
            Quaternion.Euler(rot),              // rotation
            decorMaster                         // �θ� ����
            ).transform;

        // ������ ����
        copyObject.localScale = scl;

        //��� �߰�
        gol.Add(copyObject.gameObject);
        golTypeNum.Add(decorTypeNum);
        golPrefabNum.Add(prefabIndex);

        // �θ� ����
        copyObject.SetParent(decorMaster);

        // ������Ʈ �̸� ����
        copyObject.name = string.Format("{0} ({1})", GetDecorList(dt)[prefabIndex].name.Split(' ')[0], gol.Count - 1);

        // �α� ���
        Debug.Log(string.Format("create decor :: {0}\n type={1} typeIndex={2} position=({3}, {4}, {5}) rotation=({6}, {7}, {8}) scale=({9}, {10}, {11})", copyObject.name, decorTypeNum, prefabIndex, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, scl.x, scl.y, scl.z));

        return copyObject;
    }


    /// <summary>
    /// ��ü ������ �̸� ���ΰ�ħ
    /// </summary>
    public void RefreshAllObject()
    {
        for (int i = 0; i < gol.Count; i++)
        {
            // �̸� üũ
            if (gol[i].name != string.Format("{0} ({1})", gol[i].name.Split(' ')[0], gol.Count - 1))
                gol[i].name = string.Format("{0} ({1})", gol[i].name.Split(' ')[0], gol.Count - 1);
        }
    }



    /// <summary>
    /// ��� ������Ʈ ���� ���
    /// </summary>
    public void DeleteObject(GameObject obj)
    {
        // ����� �ƴҰ�� ����
        if (obj == null)
            return;

        // ����� �ƴҰ�� ����
        if (!CheckObjectList(obj))
            return;

        // ����Ʈ ����
        int index = gol.IndexOf(obj);
        golPrefabNum.RemoveAt(index);
        golTypeNum.RemoveAt(index);
        gol.RemoveAt(index);

        // ������Ʈ ����
        Destroy(obj);
    }


    /// <summary>
    /// ������Ʈ ����Ʈ�� ���Ե� ������Ʈ���� Ȯ��
    /// </summary>
    /// <param name="obj">��� ������Ʈ</param>
    public bool CheckObjectList(GameObject obj)
    {
        return gol.Contains(obj);
    }


    /// <summary>
    /// ��ϵ� ������Ʈ�� �ڵ�(string)�� ��ȯ
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
    /// DecorType.Type���� ������ ����Ʈ ��ȯ
    /// </summary>
    /// <param name="dt">��Ĺ� Ÿ��</param>
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
    /// int�� ������ ����Ʈ ��ȯ
    /// </summary>
    /// <param name="dt">��Ĺ� Ÿ��</param>
    /// <returns></returns>
    public List<GameObject> GetDecorList(int num)
    {
        if (num < 0 || num >= DecorType.GetTypeCount())
            return null;

        return GetDecorList((DecorType.Type)num);
    }
}
