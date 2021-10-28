using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //// �����
    public static MonsterManager script = null;

    // ���� ������
    public List<Transform> monsterPrefab = new List<Transform>();


    // �̵� ��Ʈ�ѷ�
    public CharacterMover movement = null;

    // ���� �ִϸ�����
    Animator animator = null;


    void Awake()
    {
        script = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ����
        Create(0);

        // �����
        Hide();

        // ũ�� ����
        movement.bodyObject.localScale = new Vector3(5f,5f,-5f);

        // ���� ����
        movement.bodyObject.Rotate(new Vector3(0f, 180f, 0f));


        // ����� ���� �÷��̾� ���
        movement.owner = Player.system.Monster;
        movement.owner.movement = movement;
        movement.owner.avatarBody = movement.bodyObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Create(int index)
    {
        // ������ �۵� �ߴ�
        if (index > monsterPrefab.Count)
        {
            Debug.LogError("error :: ���� �ε��� �ʰ�");
            return;
        }
        else if (monsterPrefab[index] == null)
        {
            Debug.LogError("error :: �ش� ���� ����");
            return;
        }

        // ���� ���� �ı�
        if (movement.bodyObject != null)
            Remove();

        // ����
        movement.bodyObject = Transform.Instantiate(
            monsterPrefab[index],
            transform
            );

        // �ִϸ����� ���
        animator = movement.bodyObject.GetComponent<Animator>();
    }

    public void Remove()
    {
        // ����� ���� ��� �ߴ�
        if (movement.bodyObject == null)
            return;

        // ���� ========�̱���
        // StartCoroutine ���� ���� ��� ������Ű�� ��, ������ �ʿ� �����Ƿ� ���� ���� ���ʿ�

        // �ı�
        Debug.Log("���� :: ���� ��û�� -> " + movement.bodyObject);
        Transform.Destroy(movement.bodyObject);

        movement.bodyObject = null;
        animator = null;
    }

    /// <summary>
    /// ���� �ִϸ��̼� Ȱ��ȭ
    /// </summary>
    public void Work()
    {
        Work(true);
    }
    /// <summary>
    /// ���� �ִϸ��̼��� Ȱ�� ���θ� ����
    /// ������ ���� ���� �ʿ�
    /// </summary>
    /// <param name="isActivate">true = Ȱ��, false = ��Ȱ��</param>
    public void Work(bool isActivate)
    {
        // ������ �۵� �ߴ�
        if (movement.bodyObject == null)
        {
            Debug.LogError("error :: ���� ���� �ȵ�");
            return;
        }
        else if (animator == null)
        {
            Debug.LogError("error :: Animator ������Ʈ�� �������� ����");
            return;
        }

        // Ȱ�� ����
        animator.SetBool("work", isActivate);
    }

    public void Hide()
    {
        // ��Ȱ��ȭ
        movement.bodyObject.gameObject.SetActive(false);


        // ��ġ ����
        movement.bodyObject.position = new Vector3(0, -10, 0);

        // �ִϸ��̼� off
        Work(false);
    }


    /// <summary>
    /// Ư�� ��ġ�� ���� ȣ��
    /// </summary>
    /// <param name="position"></param>
    public void Call(int blockIndex)
    {
        // �ε��� ����
        movement.location = blockIndex;
        movement.owner.location = blockIndex;

        // �̵�
        movement.transform.position = BlockManager.script.GetBlock(blockIndex).transform.position;

        // Ȱ��ȭ
        movement.bodyObject.gameObject.SetActive(true);
    }

    public void Focus()
    {
        // ��� ���� ��� �ߴ�
        if (movement == null)
            return;

        // ī�޶� ��Ŀ��
        GameData.worldManager.cameraManager.CamMoveTo(movement.transform, CameraManager.CamAngle.Top);
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    /// <returns></returns>
    public IEnumerator DashOnly(int blockCount)
    {
        // ���� üũ
        if (movement.bodyObject != null)
        {
            // �ӵ� ����
            movement.moveSpeed = 10f;

            // ��ȹ
            movement.PlanMoveBy(blockCount);
                       
            // �׼� ���� ���
            while (movement.actionsQueue.Count > 0 || movement.actNow.type == Action.ActionType.None)
            {
                yield return null;
            }

        }
        else
        {
            Debug.LogError("error :: ���� ����� �����ؾ���");
            Debug.Break();
        }
    }

    /// <summary>
    /// ��ȯ �� ���� ���� ����
    /// ��ȯ -> �ִϸ��̼� -> ��Ŀ�� -> ���� -> ���� -> ����
    /// </summary>
    /// <param name="blockIndex">��ȯ ����</param>
    /// <param name="blockCount">���� ĭ��</param>
    /// <returns></returns>
    public IEnumerator Dash(int blockIndex, int blockCount)
    {
        // ��ȯ
        Call(blockIndex);

        // �ִϸ��̼� �۵�
        Work();

        // ī�޶� ��Ŀ��
        script.Focus();

        // ���� ���
        yield return DashOnly(blockCount);

        // ���� ����
        Hide();

        //// ���� ���� - ��Ȱ������ ���� ��Ȱ��
        //Remove();

    }


}
