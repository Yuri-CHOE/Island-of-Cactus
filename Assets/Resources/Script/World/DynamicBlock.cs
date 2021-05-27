using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicBlock : MonoBehaviour
{
    [SerializeField]
    MoveDirection nextDirection = MoveDirection.None;

    [SerializeField]
    MeshRenderer meshRenderer;

    public BlockType.Type blockType;

    public BlockType.TypeDetail blockTypeDetail = BlockType.TypeDetail.plus;


    public List<Player> guest = new List<Player>();

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetMaterial(BlockType.TypeDetail td)
    {
        Material mat;

        switch (blockTypeDetail)
        {
            case BlockType.TypeDetail.plus:
                mat = Resources.Load<Material>("World/Block/Block_Plus");
                break;
            case BlockType.TypeDetail.minus:
                mat = Resources.Load<Material>("World/Block/Block_Minus");
                break;

            case BlockType.TypeDetail.lucky:
                mat = Resources.Load<Material>("World/Block/Block_Lucky");
                break;
            case BlockType.TypeDetail.boss:
            case BlockType.TypeDetail.trap:

                mat = Resources.Load<Material>("World/Block/Block_Event");
                break;
            case BlockType.TypeDetail.shop:
            case BlockType.TypeDetail.unique:
            case BlockType.TypeDetail.shortcutIn:
            case BlockType.TypeDetail.shortcutOut:
                mat = Resources.Load<Material>("World/Block/Block_Special");
                break;

            default:
                mat = Resources.Load<Material>("World/Block/BlockNone");
                break;
        }

        meshRenderer.material = mat;
    }

    /// <summary>
    /// blockTypeDetail 값으로부터 Material 재설정
    /// </summary>
    public void Refresh()
    {
        SetMaterial(blockTypeDetail);
    }
    

    /// <summary>
    /// blockTypeDetail 값 재설정
    /// </summary>
    public void SetBlock(BlockType.TypeDetail td)
    {
        blockTypeDetail = td;
        Refresh();
    }


    /// <summary>
    /// blockType 값 재설정
    /// </summary>
    public void SetType(BlockType.Type t)
    {
        blockType = t;
    }


    /// <summary>
    /// 방향 재설정
    /// </summary>
    /// <param name="yRot"> L=-1, U=0, R=1, D=2,none=3</param>
    public void ReDirection(int yRot)
    {
        // 오류 차단
        yRot %= 4;

        // 방향값 재설정
        nextDirection = (MoveDirection)(yRot + 1);

        transform.rotation = Quaternion.Euler(new Vector3(0f, (float)yRot * 90f, 0f));
        //transform.Rotate(0f, ((float)moveDirection - 1f) * 90f, 0f);

    }
    /// <summary>
    /// 좌표를 향해 방향 재설정
    /// </summary>
    /// <param name="yRot"> L=-1, U=0, R=1, D=2,none=3</param>
    public void ReDirection(Vector3 pos)
    {
        int yRot = 0;

        Vector3 v3 = pos - transform.position;

        // x, z 축 차이 비교 후 큰값 적용
        if (v3 == Vector3.zero)
            return;
        else if (v3.x * v3.x >= v3.z * v3.z)
        {
            if (v3.x < 0)
                yRot = -1;
            else if (v3.x > 0)
                yRot = 1;
        }
        else
        {
            if (v3.z < 0)
                yRot = 2;
            else if (v3.z < 0)
                yRot = 0;
        }

        // 방향값 재설정
        ReDirection(yRot);
    }


    /// <summary>
    /// int형 방향 획득 : L=-1, U=0, R=1, D=2,none=3
    /// </summary>
    public int GetDirection()
    {
        // 오류 차단
        if (nextDirection == MoveDirection.None)
            return 0;

        return (int)nextDirection - 1;
    }

}
