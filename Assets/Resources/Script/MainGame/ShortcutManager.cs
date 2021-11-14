using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
    public static ShortcutManager script = null;

    [SerializeField]
    CameraManager cm = null;

    // ���� �Ա� ���
    public int shortCutIn = -1;
    DynamicBlock shortCutInObj = null;

    // ���� �ⱸ ���
    public int shortCutOut = -1;
    DynamicBlock shortCutOutObj = null;

    // ���� �ȳ� �ؽ�Ʈ
    public UnityEngine.UI.Text infoText = null;
    string infoT = "�������� �̿��� �� �ֽ��ϴ�";
    string infoF = "����� �����Ͽ� �̿��� �� �����ϴ�";

    // ���� ���� �ؽ�Ʈ
    public UnityEngine.UI.Text shortcutPrice = null;

    // ���� ����
    public int price = 50;

    // �����
    public Player customer = null;
    int customerCoin = 0;

    // ���� ���� ����
    bool _canUse = false;
    public bool canUse { get { return _canUse; } }

    // ��� ����
    [SerializeField]
    Color colorT = new Color();
    [SerializeField]
    Color colorF = new Color();

    // ���� ���
    Coroutine endWait = null;


    private void Awake()
    {
        // �� ���
        script = this;
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �ε��� �ʵ� ���
    public void SetUp()
    {
        // �� �ƿ� �ε��� ���
        for (int i = 0; i < BlockManager.script.blockCount; i++)
        {
            DynamicBlock temp = BlockManager.script.GetBlock(i).GetComponent<DynamicBlock>();

            // �� ���
            if (temp.blockTypeDetail == BlockType.TypeDetail.shortcutIn)
            {
                shortCutIn = i;

                shortCutInObj = temp;

                transform.position = shortCutInObj.transform.position;
            }

            // �ƿ� ���
            else if (temp.blockTypeDetail == BlockType.TypeDetail.shortcutOut)
            {
                shortCutOut = i;

                shortCutOutObj = temp;
            }
        }
    }


    public void CallShortcutUI(Player _customer)
    {
        // �÷��̾� ���Է½� �ߴ�
        if (_customer == null)
        {
            Debug.LogError("��ũ��Ʈ ���� :: _customer �� ���� null �Դϴ�");
            Debug.Break();
            return;
        }

        // �ʱ�ȭ
        Clear();

        // ����� ����
        customer = _customer;

        // ����� ���μ� Ȯ��
        customerCoin = customer.coin.Value;

        // UI ����
        shortcutPrice.text = price.ToString();
        CheckColor();

        // ����� ���� - ���� ��
        GameMaster.script.messageBox.InputControl(Player.me == Turn.now);
        GameMaster.script.messageBox.btnUse.interactable = (customer == Player.me);

        // UI ȣ��
        GameMaster.script.messageBox.PopUp(MessageBox.Type.ShortCut);

    }


    /// <summary>
    /// ���� ���� ���� �Ǵ� �� ���� ����
    /// </summary>
    void CheckColor()
    {
        // ���� ����� ���
        if (customerCoin >= price)
        {
            // �ȳ� �ؽ�Ʈ
            infoText.text = infoT;
            infoText.color = colorT;

            // ���� ����
            shortcutPrice.color = colorT;

            // ���� ���� ����
            _canUse = true;
        }
        // ���� ������ ���
        else
        {
            // �ȳ� �ؽ�Ʈ
            infoText.text = infoF;
            infoText.color = colorF;

            // ���� ����
            shortcutPrice.color = colorF;

            // ���� ���� ����
            _canUse = false;
        }
    }



    public void Use()
    {
        // ���� �Ұ��ɽ� ����
        if (!_canUse)
            return;

        // ī�޶� ����
        cm.CamMoveTo(customer.avatar.transform, CameraManager.CamAngle.Top);

        // ���� �� �̵� ��û
        Pay();

        // �޽��� �ڽ� �ݱ�
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);

        // ���� ��� �ߴ�
        if (endWait != null)
            StopCoroutine(endWait);

        // �̵� ��� �� ���� ����
        endWait = StartCoroutine(Waiting());
    }


    /// <summary>
    /// ������ �̿뿡 ���� ���� �� �̵���û
    /// </summary>
    /// <param name="customer">������</param>
    void Pay()
    {
        // ��� ����
        customer.coin.subtract(price);
        Debug.Log("���� ���� :: " + price);

        // �̵� ó��
        customer.movement.MoveSet(shortCutOutObj.transform.position, 5f, true);
    }


    public void Clear()
    {
        // �ʱ�ȭ
        customer = null;
        customerCoin = 0;

        _canUse = false;
    }

    IEnumerator Waiting()
    {
        // �̵� �Ϸ� ���
        while (customer.movement.isBusy)
        {
            yield return null;
        }

        // ī�޶� ����
        cm.CamFree();

        // ��ǥ ����
        customer.movement.location = shortCutOut;

        // ��ħ �ؼ�
        customer.movement.AvatarOverFix();

        // �̵� ���� ����
        GameMaster.script.messageBox.Out();
    }

}
