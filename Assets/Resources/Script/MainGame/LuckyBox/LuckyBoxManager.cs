using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBoxManager : MonoBehaviour
{
    public static LuckyBoxManager script = null;


    [SerializeField]
    AudioSource audio = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    UnityEngine.UI.Text nameText = null;
    [SerializeField]
    UnityEngine.UI.Text infoText = null;

    //Player owner = null;

    Vector3 hidingPos = new Vector3();


    public bool isEnd { get { return animator.GetCurrentAnimatorStateInfo(0).IsName("end"); } }
    public bool isReady { get { return animator.GetBool("isReady"); } }

    // ���� ����
    public Coroutine coroutineOpen = null;


    void Awake()
    {
        // ��Ű�ڽ� ������Ʈ ���
        script = transform.GetComponent<LuckyBoxManager>();

        // �ʱ� ��ġ ���
        hidingPos = transform.position;

        //// ���� ����
        //audio.clip = AudioManager.script.osfxLuckybox;
    }

    
    // Update is called once per frame    
    void Update()
    {
        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);

        // �ʱ�ȭ ���� �ʾ����� ���� ������ �ִϸ��̼��� "idel"�� ���
        if (!animator.GetBool("isReady"))
            if (current.IsName("idel"))
            {
                // �ʱ�ȭ
                Clear();

                // ���� �ִϸ��̼� ����
                animator.SetBool("isReady", true);
            }
    }

    /// <summary>
    /// ��� ��ҷ� �̵�
    /// </summary>
    void Hide()
    {
        // �ʱ� ��ġ�� �̵�
         transform.position = hidingPos;
    }

    /// <summary>
    /// �ʱ�ȭ ���
    /// </summary>
    void Clear()
    {
        //owner = null;

        animator.SetBool("doOpen", false);
        animator.SetBool("isOpen", false);
        animator.SetBool("isReady", false);
        animator.SetBool("isCall", false);
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void ClearForced()
    {
        animator.SetTrigger("Clear");
    }


    public void GetLuckyBox(Player _owner)
    {
        // �ʱ�ȭ �ȉ����� ���� �ʱ�ȭ
        if (!isReady)
            ClearForced();

        // ��ȯ ��ǥ ����
        Vector3 pos = _owner.avatar.transform.position + Vector3.back * 2f;

        // ���� ����
        pos.y = 5f;

        // ������Ʈ ĳ���Ϳ��� ��ȯ
        transform.position = pos;

        // ���� ���
        audio.PlayOneShot(AudioManager.script.osfxLuckybox);

        // ���� �ִϸ��̼� ����
        animator.SetBool("isCall", true);
    }


    public void Open()
    {
        // �ʱ�ȭ �ȉ����� �ߴ�
        if (!isReady)
            return;

        // �ִϸ��̼� ����
        animator.SetBool("doOpen", true);
    }

    public IEnumerator WaitAndResult(LuckyBox _luckyBox, Player target)
    {

        // ���� ���� ���
        while (!isEnd)
        {
            //Debug.LogWarning("��Ű �ڽ� :: ���� ���� �����");
            yield return null;
        }

        //

        // ����� ���
        MessageBox mb = GameData.gameMaster.messageBox;
        //mb.PopUp(mb.pageSwitch.GetObject(3).GetComponent<UnityEngine.UI.MessageBox.MessageBoxRule>(), 3);     // ��Ű�ڽ� Ÿ���� ���� 3�� ====== ���� ���������� �����Ұ�
        mb.PopUp(MessageBox.Type.LuckyBox);


        // �޽��� �ڽ� Ȯ�� ���
        while (mb.gameObject.activeSelf)
        {
            //Debug.LogWarning("��Ű �ڽ� :: �޽��� �ڽ� Ȯ�� �����");
            yield return null;
        }

        // �����
        Hide();

        // ȿ�� ����
        yield return _luckyBox.Effect(target);
        Debug.Log("Lucky Box :: ȿ�� ����� = " + _luckyBox.name);

        // �̵� ���� ���
        while (target.movement.actNow.type != Action.ActionType.None || target.movement.actionsQueue.Count > 0)
        {
            yield return null;
        }

        // ���� ����
        BlockWork.isEnd = true;

        // �ʱ�ȭ ����
        ClearForced();
    }

    /// <summary>
    /// �޽��� �ڽ� ���� ����
    /// </summary>
    /// <param name="__index"></param>
    public void SetTextByIndex(int __index)
    {
        nameText.text = LuckyBox.table[__index].name;
        infoText.text = LuckyBox.table[__index].info;
    }
}
