using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class GroundManager : MonoBehaviour
{
    // ������ ���
    public List<GameObject> groundList = new List<GameObject>();

    // ������ �׶���
    [SerializeField]
    List<GameObject> gol = new List<GameObject>();

    // ������ �θ� ������Ʈ
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
    /// �Էµ� ��Ʈ������ ���� ��ġ
    /// </summary>
    /// <param name="str">�ʵ�=10��, �ʵ屸��=',', ���α���='|', ��Ģ=(int, float, float, float, float, float, float, float, float, float |)</param>
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
        List<int> prefabIndex = new List<int>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> rot = new List<Vector3>();
        List<Vector3> scl = new List<Vector3>();

        int iTemp = 0;
        Vector3 temp = new Vector3();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // �߸��� ������ ���� ����
            if (dataStr[i].Length != 10)
                return;

            // ������Ʈ ID ����ȭ
            if (int.TryParse(dataStr[i][0], out iTemp)) { }
            // ����ȭ �Ϸ�
            prefabIndex.Add(iTemp);
            iTemp = 0;

            // position ����ȭ
            if (float.TryParse(dataStr[i][1], out temp.x)) { }
            if (float.TryParse(dataStr[i][2], out temp.y)) { }
            if (float.TryParse(dataStr[i][3], out temp.z)) { }
            // ����ȭ �Ϸ�
            pos.Add(temp);
            temp = Vector3.zero;

            // rotation ����ȭ;
            if (float.TryParse(dataStr[i][4], out temp.x)) { }
            if (float.TryParse(dataStr[i][5], out temp.y)) { }
            if (float.TryParse(dataStr[i][6], out temp.z)) { }
            // ����ȭ �Ϸ�
            rot.Add(temp);
            temp = Vector3.zero;

            // scale ����ȭ;
            if (float.TryParse(dataStr[i][7], out temp.x)) { }
            if (float.TryParse(dataStr[i][8], out temp.y)) { }
            if (float.TryParse(dataStr[i][9], out temp.z)) { }
            // ����ȭ �Ϸ�
            scl.Add(temp);
            //temp = Vector3.zero;
        }

        // ������Ʈ ����
        BuildUP(prefabIndex.ToArray(), pos.ToArray(), rot.ToArray(), scl.ToArray());
    }


    /// <summary>
    /// ���� �ϰ� ����
    /// </summary>
    /// <param name="prefabIndex">���� ������Ʈ �ε���(golPrefabNum)</param>
    /// <param name="pos">����� ������Ʈ position</param>
    /// <param name="rot">����� ������Ʈ rotation</param>
    /// <param name="scl">����� ������Ʈ scale</param>
    public void BuildUP(int[] prefabIndex, Vector3[] pos, Vector3[] rot, Vector3[] scl)
    {
        Debug.Log("create ground :: batch operation (start)");

        // �� �迭�� ���� ����ġ�� ��������
        if (pos.Length != prefabIndex.Length && pos.Length != rot.Length && pos.Length != scl.Length)
            return;

        // ���� ����Ʈ �� ������Ʈ ����
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
        }
        gol.Clear();

        // �ݺ� ����
        for (int i = 0; i < pos.Length; i++)
            Create(prefabIndex[i], pos[i], rot[i], scl[i]);

        Debug.Log("create ground :: batch operation (finish)");
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="prefabIndex">���� ������Ʈ �ε���(golPrefabNum)</param>
    /// <param name="pos">����� ������Ʈ position</param>
    /// <param name="rot">����� ������Ʈ rotation</param>
    /// <param name="scl">����� ������Ʈ scale</param>
    /// <returns></returns>
    public Transform Create(int prefabIndex, Vector3 pos, Vector3 rot, Vector3 scl)
    {
        // ���� ����� ���� ���
        if (prefabIndex < 0 || prefabIndex >= groundList.Count)
            return null;

        // ����
        Transform copyObject = Instantiate(
            groundList[prefabIndex],            // ������ GameObject
            pos,                                // position
            Quaternion.Euler(rot),              // rotation
            groundMaster                        // �θ� ����
            ).transform;

        // ������ ����
        copyObject.localScale = scl;

        //��� �߰�
        gol.Add(copyObject.gameObject);

        // �θ� ����
        copyObject.SetParent(groundMaster);

        // ������Ʈ �̸� ����
        copyObject.name = string.Format("{0} ({1})", groundList[prefabIndex].name, gol.Count - 1);

        // �α� ���
        Debug.Log(string.Format("create ground :: {0}\n typeIndex={1} position=({2}, {3}, {4}) rotation=({5}, {6}, {7}) scale=({8}, {9}, {10})", copyObject.name, prefabIndex, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, scl.x, scl.y, scl.z));

        return copyObject;
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

