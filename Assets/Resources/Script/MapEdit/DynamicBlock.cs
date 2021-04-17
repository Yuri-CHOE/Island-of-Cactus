using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBlock : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;

    public BlockType.Type blockType;

    public BlockType.TypeDetail blockTypeDetail = BlockType.TypeDetail.plus;

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
            default:
                mat = Resources.Load<Material>("World/Block/BlockNone");
                break;
        }

        meshRenderer.material = mat;
    }

    /// <summary>
    /// blockTypeDetail �����κ��� Material �缳��
    /// </summary>
    public void Refresh()
    {
        SetMaterial(blockTypeDetail);
    }
    

    /// <summary>
    /// blockTypeDetail �� �缳��
    /// </summary>
    public void SetBlock(BlockType.TypeDetail td)
    {
        blockTypeDetail = td;
        Refresh();
    }
}
