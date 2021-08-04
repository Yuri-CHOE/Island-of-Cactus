using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_mini : MonoBehaviour
{
    // �÷��̾�
    public Player owner = null;

    // �÷��̾� ������
    public Sprite face = null;

    //�̴ϰ��� ����
    public int score = 0;
    public bool plusScore = false;
    public bool minusScore = false;
    public bool scoreSetCheck = false;

    //�̴ϰ��� ���
    public int rank = 0;

    //�̴ϰ��� ���� ����
    public bool join = false;

    //���� �ڽ��� �������� Ȯ��
    public bool myTurn = false;

    //�÷��̾� death ������Ʈ
    public GameObject deathUi;

    //�÷��̾� ���ھ� �ؽ�Ʈ ������Ʈ
    public Text txtScore;

    void Start()
    {
        score = 0;
        txtScore.text = score.ToString();
    }

    void Update()
    {
        if (join)
        {
            // ��ü Ȱ��ȭ Ȥ�� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }

        turnCehck();
        scoreCheck();
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
            deathUi.gameObject.SetActive(false);
        }
        else
        {
            deathUi.gameObject.SetActive(true);
        }
    }

    void scoreCheck()
    {
        scoreSetCheck = false;

        if (plusScore)
        {
            score += 100;
            txtScore.text = score.ToString();
            scoreSetCheck = true;
            plusScore = false;
        }
    }
}