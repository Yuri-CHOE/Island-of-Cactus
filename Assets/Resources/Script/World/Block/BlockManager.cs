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
    /// �Էµ� ��Ʈ������ ��� ��ġ
    /// </summary>
    /// <param name="str">�ʵ�=4��, �ʵ屸��=',', ���α���='|', ��Ģ=(float,float,float,int|float,float,float,int)</param>
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
        List<Vector3> v3 = new List<Vector3>();
        List<int> yRot = new List<int>();
        List<BlockType.TypeDetail> td = new List<BlockType.TypeDetail>();

        for (int i = 0; i < dataStr.Count; i++)
        {

            // �߸��� ������ ���� ����
            if (dataStr[i].Length != 5)
                return;

            // position ����ȭ
            Vector3 temp = new Vector3();
            if (float.TryParse(dataStr[i][0], out temp.x)) { }
            if (float.TryParse(dataStr[i][1], out temp.y)) { }
            if (float.TryParse(dataStr[i][2], out temp.z)) { }

            // ���� ����ȭ
            int temp2 = 0;
            if (int.TryParse(dataStr[i][3], out temp2)) { }

            // ��� Ÿ�� ����ȭ
            int temp3 = 0;
            if (int.TryParse(dataStr[i][4], out temp3)) { }

            // ����ȭ �Ϸ�
            v3.Add(temp);
            yRot.Add(temp2);
            td.Add((BlockType.TypeDetail)temp3);
        }
                
        // ������Ʈ ����
        BuildUP(v3.ToArray(), yRot.ToArray(), td.ToArray());
    }


    /// <summary>
    /// ��� �ϰ� ����
    /// </summary>
    /// <param name="pos">����� ������Ʈ position ���</param>
    /// <param name="direction"> L=-1, U=0, R=1, D=2,none=3</param>
    /// <param name="td">����� ������Ʈ ��� Ÿ�� ���</param>
    public void BuildUP(Vector3[] pos, int[] direction, BlockType.TypeDetail[] td)
    {
        // position �迭�� BlockType.TypeDetail �迭�� ���� ����ġ�� ��������
        if (pos.Length != td.Length && pos.Length != direction.Length)
            return;

        // copyTarget ����Ʈ �� ������Ʈ ����
        for (int i = 0; i < gol.Count; i++)
        {
            Destroy(gol[i]);
            //DeleteObject(gol[i]);
        }
        gol.Clear();

        // �ݺ� ����
        for (int i = 0; i < pos.Length; i++)
            Create(pos[i], direction[i], td[i]);

        // ��� ����Ʈ ���ΰ�ħ
        RefreshAllObject();
    }


    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="copyTarget">���� ������Ʈ</param>
    /// <param name="pos">����� ������Ʈ position</param>
    /// <param name="direction"> L=-1, U=0, R=1, D=2,none=3</param>
    /// <param name="td">����� ������Ʈ ��� Ÿ��</param>
    public Transform Create(Vector3 pos, int direction, BlockType.TypeDetail td)
    {

        // ���� ����� ���� ���
        if (blockPrefab == null)
            return null;

        // ����
        Transform copyObject = Instantiate(
            blockPrefab,
            pos,
            blockPrefab.transform.rotation
            ).transform;

        // ��� �Ӽ� ���� �� ����
        DynamicBlock db = copyObject.GetComponent<DynamicBlock>();
        db.blockType = BlockType.GetTypeByDetail(td);
        db.blockTypeDetail = td;
        db.Refresh();

        // ��� ���� ����
        db.ReDirection(direction);

        //��� �߰�
        gol.Add(copyObject.gameObject);

        // �θ� ����
        copyObject.SetParent(blockMaster);

        // ������Ʈ �̸� ����
        copyObject.name = string.Format("Block ({0})", gol.Count - 1);

        return copyObject;
    }


    /// <summary>
    /// ��ü ����� �� TypeDetail�� �������� Type �� ���׸���, �̸�, ���� ���ΰ�ħ
    /// </summary>
    public void RefreshAllObject()
    {
        for (int i = 0; i < gol.Count; i++)
        {
            DynamicBlock db = gol[i].GetComponent<DynamicBlock>();

            // ��� Ÿ�� ���ΰ�ħ
            db.SetType(BlockType.GetTypeByDetail(db.blockTypeDetail));

            // ��� ���׸��� ���ΰ�ħ
            db.Refresh();

            // �̸� üũ
            if (gol[i].name != string.Format("Block ({0})", i))
                gol[i].name = string.Format("Block ({0})", i);

            // ��� ���� �缳��
            if (i != gol.Count - 1)     // ������ ����Ʈ�� �ƴ��� üũ
                db.ReDirection(gol[i + 1].transform.position);
            else
                db.ReDirection(gol[0].transform.position);
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
        gol.Remove(obj);
        RefreshAllObject();

        // ������Ʈ ����
        Destroy(obj);
    }


    /// <summary>
    /// ��� ����Ʈ�� ���Ե� ������Ʈ���� Ȯ��
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
