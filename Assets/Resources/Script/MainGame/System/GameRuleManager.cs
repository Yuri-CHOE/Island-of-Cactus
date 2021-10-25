using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRuleManager : MonoBehaviour
{
    // �� ��ũ��Ʈ
    public static GameRuleManager script = null;


    // ����
    [Header("area")]
    [SerializeField]
    Text areaText = null;
    int areaLimit = 1;

    // �� ����
    [Header("section")]
    [SerializeField]
    Text sectionText = null;
    int sectionLimit = 1;

    // �ִ� ����Ŭ ��
    [Header("cycle")]
    [SerializeField]
    InputField cycleMaxInput = null;
    [SerializeField]
    Slider cycleMaxSlider = null;

    // �÷��� �ο� ��
    [Header("player")]
    [SerializeField]
    InputField playerCountInput = null;
    [SerializeField]
    Slider playerCountSlider = null;



    // Start is called before the first frame update
    void Start()
    {
        // �����
        script = this;

        // �ʱ� ����
        AreaChange(0);
        SectionChange(0);
        cycleMaxSlider.value = GameRule.cycleMax;
        playerCountSlider.value = GameRule.playerCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    int Limiter(int value, int min, int max)
    {
        if (value < min)
            value = min;
        else if (value > max)
            value = max;
        
        return value;
    }


    public void SyncToInputField(InputField inputField)
    {
        if(inputField.transform.parent.name == "Cycle")
        {
            // �� ����
            CycleSet((int)cycleMaxSlider.value);

            // ��ũ
            cycleMaxInput.text = GameRule.cycleMax.ToString();
            //cycleMaxText.text = cycleMaxInput.text;
        }
        else if(inputField.transform.parent.name == "Player")
        {
            // �� ����
            PlayerSet((int)playerCountSlider.value);

            // ��ũ
            playerCountInput.text = GameRule.playerCount.ToString();
            //playerCountText.text = playerCountInput.text;
        }
    }

    public void SyncToSlider(Slider slider)
    {
        if (slider.transform.parent.name == "Cycle")
        {
            // ��ũ
            cycleMaxSlider.value = int.Parse(cycleMaxInput.text);
        }
        else if (slider.transform.parent.name == "Player")
        {
            // ��ũ
            playerCountSlider.value = int.Parse(playerCountInput.text);

        }
    }



    public void AreaChange(int value)
    {
        GameRule.area = Limiter(GameRule.area + value, 1, areaLimit);

        areaText.text = GameRule.area.ToString();
    }

    public void SectionChange(int value)
    {
        GameRule.section = Limiter(GameRule.section + value, 1, sectionLimit);

        sectionText.text = GameRule.section.ToString();
    }

    public void CycleSet(int value)
    {
        GameRule.cycleMax = Limiter(value, (int)cycleMaxSlider.minValue, (int)cycleMaxSlider.maxValue);

        Debug.Log("game rule :: cycle �� ����� => " +GameRule.cycleMax );

        cycleMaxInput.text = GameRule.cycleMax.ToString();
    }

    public void PlayerSet(int value)
    {
        GameRule.playerCount = Limiter(value, (int)playerCountSlider.minValue, (int)playerCountSlider.maxValue);

        Debug.Log("game rule :: player �� ����� => " + GameRule.playerCount);

        playerCountInput.text = GameRule.playerCount.ToString();
    }



}
