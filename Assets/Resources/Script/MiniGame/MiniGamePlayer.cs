using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ����
public class MiniGamePlayer : MonoBehaviour
{
    // �÷��̾�
    Player _owner = null;
    public Player owner { get { return _owner; } set { SetOwner(value); } }

    // ������ UI
    [SerializeField]
    Image icon = null;

    // �̴ϰ��� ����
    ref MiniScore info { get { return ref owner.miniInfo; } }

    // �̴ϰ��� ���� �߰���
    public int scorePlus = 0;

    // �ѹ��� �߰��� ����
    int scoreDot = 0;


    // ���� �ڽ�
    public Image scoreBox = null;

    // ����
    [SerializeField] Color deadColor = new Color();

    // ���� �ؽ�Ʈ
    public Text txtScore = null;

    // �� ǥ�� ����
    public CanvasGroup turnImg = null;

    // �� ��ũ
    float blinkValue = 0f;
    float blinkSpeed = 2f;


    // �� Ȯ��
    public bool isMyTurn { get { return owner == MiniGameManager.script.mpm.turnNow; } }


    void Start()
    {

    }

    void Update()
    {
        // ���� �� ����
        //if (MiniGameManager.progress != ActionProgress.Working)
        if (!MiniGameManager.script.isGameStart)
            return;

        // ���� ó��
        if (scorePlus != 0)
        {
            // ����
            if (scorePlus < -10)
                scoreDot = -10;
            else if (scorePlus > 10)
                scoreDot = 10;
            else
                scoreDot = scorePlus;

            // ������ ��ŭ ����
            scorePlus -= scoreDot;

            // ������ * ���� ����� ��ŭ ���� �ݿ�
            //info.score += scoreDot;
            info.ScorePlus(scoreDot);

            // ���� ������Ʈ
            ScoreTextSync(info.score);
        }

        //// ��� ó��
        //if (owner.miniInfo.isDead)
        //{
        //    // ���� ����
        //    scoreBox.color = deadColor;

        //    // ��ũ��Ʈ ��Ȱ��
        //    enabled = false;
        //}

        // �� ǥ��
        if (isMyTurn)
        {
            // �� ����
            blinkValue += Time.deltaTime * blinkSpeed;

            // �� ����
            blinkValue %= 2f;

            // ���� �ݿ�
            // 1 - blinkValue : ( -1 -> 0 -> 1 -> -1 ) �� ��ȯ
            // ���밪 ó�� Mathf.Abs() : ( 1 -> 0 -> 1 -> 1 ) �� ��ȯ
            turnImg.alpha = Mathf.Abs(1f - blinkValue);
        }
    }

    public void SetOwner(Player __owner)
    {
        // ���� ���
        _owner = __owner;

        // ������ �̴ϰ��� ���� ��ũ
        info = _owner.miniInfo;

        // ������ �̴ϰ��� UI ���
        _owner.miniPlayerUI = this;

        // ������ ���
        icon.sprite = owner.character.GetIcon();

        // ���� ���� ����
        if (!info.join)
        {
            // ��ü Ȱ��ȭ Ȥ�� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
        else
        {
            // ���� ��ũ
            ScoreTextSync(info.score);

            // AI �÷��̾� �ڵ� �غ�
            if (_owner.type == Player.Type.AI)
                _owner.miniInfo.isReady = true;
        }
    }

    void ScoreTextSync(int value)
    {
        //txtScore.text = info.score.ToString();
        txtScore.text = value.ToString();
    }

    /// <summary>
    /// �̴ϰ��� �÷��� ���� ��Ż (���)
    /// </summary>
    public void Death()
    {
        // ��� ó��
        info.isDead = true;

        // UI �ݿ�
        scoreBox.color = deadColor;

        // ��ũ��Ʈ ��Ȱ��
        enabled = false;
    }

    public void BlinkOff()
    {
        blinkValue = 1f;
        turnImg.alpha = 0f;
    }

}

/// <summary>
/// ����
/// </summary>
//public class MiniGamePlayer : MonoBehaviour
//{
//    // �÷��̾�
//    public Player owner = null;

//    // �÷��̾� ������
//    public Sprite face = null;

//    //�̴ϰ��� ����
//    public int score = 0;
//    public bool plusScore = false;
//    public bool minusScore = false;
//    public bool scoreSetCheck = false;

//    //�̴ϰ��� ���
//    public int rank = 0;

//    //�̴ϰ��� ���� ����
//    public bool join = false;

//    //���� �ڽ��� �������� Ȯ��
//    public bool myTurn = false;

//    //�÷��̾� death ������Ʈ
//    public GameObject deathUi;

//    //�÷��̾� ���ھ� �ؽ�Ʈ ������Ʈ
//    public Text txtScore;


//    void Start()
//    {
//        score = 0;
//        txtScore.text = score.ToString();
//    }

//    void Update()
//    {
//        if (join)
//        {
//            // ��ü Ȱ��ȭ Ȥ�� ��Ȱ��ȭ
//            gameObject.SetActive(false);
//        }

//        turnCehck();
//        scoreCheck();
//    }

//    public void LoadFace()
//    {
//        // ������ �ε�
//        Debug.Log(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));
//        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));

//        // �̹��� ��ȿ �˻�
//        if (temp == null)
//        {
//            // �⺻ ������ ��ü ó��
//            Debug.Log(@"UI/playerInfo/player");
//            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
//        }

//        // ���� ���� ó��
//        if (temp == null)
//            Debug.Log("�ε� ���� :: UI/playerInfo/player");
//        // ������ ����
//        else
//            face = temp;
//    }

//    void turnCehck()
//    {
//        if (myTurn)
//        {
//            deathUi.gameObject.SetActive(false);
//        }
//        else
//        {
//            deathUi.gameObject.SetActive(true);
//        }
//    }

//    void scoreCheck()
//    {
//        scoreSetCheck = false;

//        if (plusScore)
//        {
//            score += 100;
//            txtScore.text = score.ToString();
//            scoreSetCheck = true;
//            plusScore = false;
//        }
//    }
//}