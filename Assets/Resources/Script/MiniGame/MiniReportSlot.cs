using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniReportSlot : MonoBehaviour
{
    public Player owner = null;

    [SerializeField]
    Text rankText = null;

    [SerializeField]
    Image border = null;


    [SerializeField]
    Image face = null;

    [SerializeField]
    Text score = null;
    
    [SerializeField]
    Text reward = null;

    [SerializeField]
    Text playerName = null;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Player player, Color rankColor, string _rankText)
    {
        Debug.Log("�̴ϰ��� :: ���� - �¾� ��û��-> " + player.name);

        // ������ �Է�
        owner = player;

        // ��� ǥ��
        rankText.text = _rankText;

        // ��� �׵θ�
        border.color = rankColor;

        // ĳ���� ������
        face.sprite = owner.face;

        // ����
        score.text = owner.miniInfo.score.ToString();

        // ����
        reward.text = owner.miniInfo.reward.ToString();

        // �÷��̾� �̸�
        playerName.text = owner.name;
    }
}
