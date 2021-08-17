using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // ī�� ���
    public List<PairingCard> deck = new List<PairingCard>();

    // ���õ� ī��
    public PairingCard card_1;
    public PairingCard card_2;

    // ���� ó���� ��� ��
    int completePair = 0;

    // ����
    [SerializeField]
    GameObject ending = null;



    //public Mouse_Click mouse_Click1;
    //public Mouse_Click mouse_Click2;
    //public Scenes_mini num_player;
    //public manage_Player managePlayer;
    //public string num1, num2, name1, name2;
    //public int player, /*z = 0,*/ x, number;

    public void Init()
    {
        // �ش� ������ ���� ���� ���� - �ʼ�
        MiniGameManager.script.scoreRiseValue = 10;

        // ī�� ����
        CardSetUp();
    }

    void Awake()
    {
        //    // �ش� ������ ���� ���� ���� - �ʼ�
        //    MiniGameManager.script.scoreRiseValue = 10;

        //num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�   
        //managePlayer = GameObject.Find("Game").GetComponent<manage_Player>();
    }

    void Start()
    {
        //player = num_player.member_num;
        //num1 = "";
        //num2 = "";
        //x = 1;
        //number = 1;

        //// ī�� ����
        //CardSetUp();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (x == 3)
    //    {
    //        mouse_Click1 = GameObject.Find(name1).GetComponent<Mouse_Click>();
    //        mouse_Click2 = GameObject.Find(name2).GetComponent<Mouse_Click>();
    //        Debug.Log("Ȯ��" + num1 + " " + num2);
    //        if (num1 == num2)
    //        {
    //            mouse_Click2.i = 4;
    //            mouse_Click1.i = 4;
    //            z += 1;

    //            if (z == 9)
    //            {
    //                GameObject.Find("Canvas").transform.Find("Ending").gameObject.SetActive(true);
    //            }
    //            managePlayer.plusScore = true;
    //        }
    //        else
    //        {
    //            mouse_Click2.i = 2;
    //            mouse_Click1.i = 2;
    //        }
    //        managePlayer.turn = true;
    //        x = 1;
    //    }

    //    if (z == 9)         //ī��¦�� �� ���߾���
    //    {
    //        managePlayer.scoreSetChecking();
    //        managePlayer.ranking = true;
    //        z = 0;
    //    }
    //}

    //public void num1_set(string number, string card_name)
    //{
    //    //ù��° ī�� ���ڿ� ī�� ��ȣ
    //    num1 = number;
    //    name1 = card_name;
    //}

    //public void num2_set(string number, string card_name)
    //{
    //    //�ι�° ī�� ���ڿ� ī�� ��ȣ
    //    num2 = number;
    //    name2 = card_name;
    //}


    //------------------------------------------------------------------------------------

    void Update()
    {
        // �ι�° ī�� ���� ��
        if (card_2 != null)
        {
            //// ���� ����
            //if (card_2.animator.GetCurrentAnimatorStateInfo(0).IsName(""))
            //    // ��
            //    PairCheck();

            // ������ �Ϸ��
            if (card_2.animator.GetCurrentAnimatorStateInfo(0).IsName("aniWait"))
            {
                // üũ �Ϸ� ó��
                card_1.SetAniStateCheckFinish();
                card_2.SetAniStateCheckFinish();

                // �ʱ�ȭ
                PairClear();

                // ��� ī�� ¦ ������ ���
                if (completePair == 9)
                    Ending();
                // ���� �ƴҰ��
                else
                    MiniGameManager.script.mpm.NextTurn();
            }
        }
    }


    public void CardSetUp()
    {
        // �� Ȧ�� ����
        if (deck.Count % 2 != 0)
        {
            Debug.LogError("error :: Ȧ�� �� -> " + deck.Count);
            Debug.Break();
            return;
        }


        // �� ���� ----------------------------

        // ��� ��
        int count = deck.Count / 2;
        Debug.Log("�� ���� :: �� ���� -> " + deck.Count);
        Debug.Log("�� ���� :: ��� ���� -> " + count);

        // ī�� ���� ��� ����
        List<int> cardNum = new List<int>();
        for (int i = 0; i < count; i++)
        {
            // ����̹Ƿ� 2�� ���
            cardNum.Add(i + 1);
            cardNum.Add(i + 1);
        }


        // �� ��� ----------------------------

        // ���
        int shuffledIndex = -1;
        for (int i = 0; i < deck.Count; i++)
        {
            // �ε��� ���
            deck[i].Index = i;

            // ���� ī�� ����
            shuffledIndex = Random.Range(0, cardNum.Count);

            // ī�� ���� ���
            deck[i].cardNum = cardNum[shuffledIndex];

            // ī�� ���� ǥ��
            deck[i].numText.text = deck[i].cardNum.ToString();
            Debug.Log("ī�� :: ���� ������ -> " + deck[i].numText.text);

            // ���õ� ���� ����
            cardNum.RemoveAt(shuffledIndex);
        }
    }


    public void PairSelect(PairingCard clickedCard)
    {
        // õ��° ����
        if (card_1 == null)
            card_1 = clickedCard;

        // �ι�° ����
        else if (card_2 == null)
        {
            // ù��° ���õ� ī�尡 �ƴ� ��� ���� ó��
            if(clickedCard != card_1)
                card_2 = clickedCard;
        }

        // ����° ���� (�ʹ� ���� ����)
        else
        {
            Debug.LogWarning("warning :: ���� ó�� �����");
            return;
        }

        // ī�� �ӽ� ����
        Debug.Log("ī�� ���õ� :: ī�� ���� -> " + clickedCard.cardNum);
        clickedCard.SetAniStateOpen();

        // ��� üũ
        if (card_2 != null)
            PairCheck();
    }

    public void PairClear()
    {
        card_1 = null;
        card_2 = null;
    }

    bool PairCheck()
    {
        //// ���� �� �˸�
        //if (card_1 == null)
        //{
        //    Debug.LogError("error :: ī�� �� ���� -> card_1 is null");
        //    Debug.Break();
        //    return;
        //}
        //if (card_2 == null)
        //{
        //    Debug.LogError("error :: ī�� �� ���� -> card_2 is null");
        //    Debug.Break();
        //    return;
        //}

        // �� ��ġ ����
        bool result = card_1.cardNum == card_2.cardNum;

        // ��ġ
        if (result)
        {
            // ���� ó��
            card_1.SetAniStateRelease();
            card_2.SetAniStateRelease();

            completePair++;

            // ���� �߰�
            MiniGameManager.script.ScoreAdd(1);
        }
        // ����ġ
        else
        {
            // ���� ó�� ����
        }

        Debug.Log(
            string.Format(
                "ī�� �� :: {0} -> {1} == {2}",
                result.ToString(),
                card_1.numText.text,
                card_2.numText.text
                )
            );

        return result;
    }

    void Ending()
    {
        // �ʱ�ȭ
        completePair = 0;

        // ���� ���� ����
        MiniGameManager.script.Ending(ending);
    }
}
