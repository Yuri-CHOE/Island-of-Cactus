using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockAdder : MonoBehaviour
{
    [SerializeField]
    Transform mainCam;
    [SerializeField]
    GameObject btnMoveL;
    [SerializeField]
    GameObject btnMoveR;
    [SerializeField]
    GameObject btnMoveU;
    [SerializeField]
    GameObject btnMoveD;

    [SerializeField]
    Text targetName;
    [SerializeField]
    ButtonFlag btnRefresh;


    [SerializeField]
    GameObject btnL;
    [SerializeField]
    GameObject btnR;
    [SerializeField]
    GameObject btnU;
    [SerializeField]
    GameObject btnD;

    [SerializeField]
    Vector3 distance;


    [SerializeField]
    List<GameObject> gol;

    //[SerializeField]
    //GameObject parrentObject;

    GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
        BtnCopyOnOFF();
        MoveCamera(GetDirection(false));
        Copy(GetDirection(true), BlockType.TypeDetail.plus);
        CheckRefreshBtn();
    }



    void Refresh()
    {
        // Ŭ�� üũ
        if (!Input.GetMouseButtonUp(0))
            return;

        // UI Ŭ�� ����ó��
        if (EventSystem.current.currentSelectedGameObject != null)
            return;


        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject clickObj = null;

        if ( Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            clickObj = hit.transform.gameObject;
            Debug.Log(clickObj.name);
        }

        // ������Ʈ Ŭ�� ������ ���
        if (clickObj == null)
            return;

        // Ŭ���� ����� ����� �ƴҰ��
        if (gol.Contains(clickObj))
            target = clickObj;
        else
            target = null;



        if (target == null)
        {
            targetName.text = "is not block";
            return;
        }
        else
            targetName.text = target.name;

    }


    void Copy(MoveDirection direction, BlockType.TypeDetail td)
    {
        if (direction == MoveDirection.None)
            return;

        Refresh();

        // ���� ����� ���� ���
        if (target == null)
            return;
        
        // ����
        Transform copyObject = Instantiate(target).transform;

        // ��ġ ����
        if (direction == MoveDirection.Left)
        {
            copyObject.position = new Vector3(
                copyObject.position.x - distance.x,
                copyObject.position.y,
                copyObject.position.z
                );
        }
        else if (direction == MoveDirection.Right)
        {
            copyObject.position = new Vector3(
                copyObject.position.x + distance.x,
                copyObject.position.y,
                copyObject.position.z
                );
        }
        else if (direction == MoveDirection.Up)
        {
            copyObject.position = new Vector3(
                copyObject.position.x,
                copyObject.position.y,
                copyObject.position.z + distance.z
                );
        }
        else if (direction == MoveDirection.Down)
        {
            copyObject.position = new Vector3(
                copyObject.position.x,
                copyObject.position.y,
                copyObject.position.z - distance.z
                );
        }


        // ��� �Ӽ� ���� �� ����
        DynamicBlock db = copyObject.GetComponent<DynamicBlock>();
        db.blockType = BlockType.GetTypeByDetail(td);
        db.blockTypeDetail = td;
        db.Refresh();

        //��� �߰�
        gol.Add(copyObject.gameObject);

        // �θ� ����
        copyObject.SetParent(target.transform.parent);

        // ������Ʈ �̸� ����
        copyObject.name = string.Format("Block ({0})", gol.Count - 1);

        // Ÿ�� �缳��
        target = copyObject.gameObject;

        // ī�޶� �̵�
        MoveCamera(direction);
        MoveCamera(direction);
    }

    void MoveCamera(MoveDirection direction)
    {
        if (direction == MoveDirection.None)
            return;

        if (direction == MoveDirection.Left)
        {
            mainCam.position = new Vector3(
                mainCam.position.x - (distance.x/2),
                mainCam.position.y,
                mainCam.position.z
                );
        }
        else if (direction == MoveDirection.Right)
        {
            mainCam.position = new Vector3(
                mainCam.position.x + (distance.x / 2),
                mainCam.position.y,
                mainCam.position.z
                );
        }
        else if (direction == MoveDirection.Up)
        {
            mainCam.position = new Vector3(
                mainCam.position.x,
                mainCam.position.y,
                mainCam.position.z + (distance.z / 2)
                );
        }
        else if (direction == MoveDirection.Down)
        {
            mainCam.position = new Vector3(
                mainCam.position.x,
                mainCam.position.y,
                mainCam.position.z - (distance.z / 2)
                );
        }
    }

    MoveDirection GetDirection(bool isCopyUI)
    {
        ButtonFlag L;
        ButtonFlag R;
        ButtonFlag U;
        ButtonFlag D;

        if (isCopyUI)
        {
            L = btnL.GetComponent<ButtonFlag>();
            R = btnR.GetComponent<ButtonFlag>();
            U = btnU.GetComponent<ButtonFlag>();
            D = btnD.GetComponent<ButtonFlag>();
        }
        else
        {
            L = btnMoveL.GetComponent<ButtonFlag>();
            R = btnMoveR.GetComponent<ButtonFlag>();
            U = btnMoveU.GetComponent<ButtonFlag>();
            D = btnMoveD.GetComponent<ButtonFlag>();
        }

        MoveDirection result;

        if (L.getFlag())
            result = MoveDirection.Left;
        else if (R.getFlag())
            result = MoveDirection.Right;
        else if (U.getFlag())
            result = MoveDirection.Up;
        else if (D.getFlag())
            result = MoveDirection.Down;
        else
            result = MoveDirection.None;

        L.setOff();
        R.setOff();
        U.setOff();
        D.setOff();

        L.MirrorSync(true);
        R.MirrorSync(true);
        U.MirrorSync(true);
        D.MirrorSync(true);

        return result;
    }

    void BtnCopyOnOFF()
    {
        if (target == null)
        {
            btnL.SetActive(false);
            btnR.SetActive(false);
            btnU.SetActive(false);
            btnD.SetActive(false);
        }
        else
        {
            btnL.SetActive(true);
            btnR.SetActive(true);
            btnU.SetActive(true);
            btnD.SetActive(true);
        }
    }

    void RefreshAllObject()
    {
        for(int i = 0; i < gol.Count; i++)
            gol[i].GetComponent<DynamicBlock>().Refresh();
    }

    void CheckRefreshBtn()
    {
        if (btnRefresh.getFlag())
        {
            RefreshAllObject();
            btnRefresh.setOff();
        }
    }
}
