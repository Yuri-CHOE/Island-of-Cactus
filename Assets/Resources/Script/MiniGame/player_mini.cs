using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_mini : MonoBehaviour
{
    public static player_mini instance = null;

    // �÷��̾�
    public Player owner = null;

    // �÷��̾� ������
    public Sprite face = null;

    //�̴ϰ��� ����
    public int score = 0;
    public bool plusScore = false;
    public bool minusScore = false;

    //�̴ϰ��� ���
    public int rank = 0;

    //�̴ϰ��� ���� ����
    public bool join = true;

    //���� �ڽ��� �������� Ȯ��
    public bool myTurn = false;

    //�÷��̾� death ������Ʈ
    public GameObject deathUi;

    //�÷��̾� ���ھ� �ؽ�Ʈ ������Ʈ
    public Text txtScore;

    void Awake()
    {
      if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        // ��ü Ȱ��ȭ Ȥ�� ��Ȱ��ȭ
        gameObject.SetActive(join);
    }

    void Start()
    {
        deathUi = GetComponent<GameObject>();
        score = 0;
        txtScore.text = score.ToString();
    }

    void Update()
    {
        turnCehck();
    }

    public void LoadFace()
    {
        // ������ �ε�
        Debug.Log(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"UI/playerInfo/player");
            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        }

        // ���� ���� ó��
        if (temp == null)
            Debug.Log("�ε� ���� :: UI/playerInfo/player");
        // ������ ����
        else
            face = temp;
    }

    void turnCehck()
    {
        if (myTurn)
        {
            if (plusScore)
            {
                score += 100;
                txtScore.text = score.ToString();
            }
            deathUi.gameObject.SetActive(false);

        }
        else
        {
            deathUi.gameObject.SetActive(true);
        }
    }

}