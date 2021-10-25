using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRuleManager : MonoBehaviour
{
    // 퀵 스크립트
    public static GameRuleManager script = null;


    // 지역
    [Header("area")]
    [SerializeField]
    Text areaText = null;
    int areaLimit = 1;

    // 상세 지역
    [Header("section")]
    [SerializeField]
    Text sectionText = null;
    int sectionLimit = 1;

    // 최대 사이클 수
    [Header("cycle")]
    [SerializeField]
    InputField cycleMaxInput = null;
    [SerializeField]
    Slider cycleMaxSlider = null;

    // 플레이 인원 수
    [Header("player")]
    [SerializeField]
    InputField playerCountInput = null;
    [SerializeField]
    Slider playerCountSlider = null;



    // Start is called before the first frame update
    void Start()
    {
        // 퀵등록
        script = this;

        // 초기 셋팅
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
            // 값 저장
            CycleSet((int)cycleMaxSlider.value);

            // 싱크
            cycleMaxInput.text = GameRule.cycleMax.ToString();
            //cycleMaxText.text = cycleMaxInput.text;
        }
        else if(inputField.transform.parent.name == "Player")
        {
            // 값 저장
            PlayerSet((int)playerCountSlider.value);

            // 싱크
            playerCountInput.text = GameRule.playerCount.ToString();
            //playerCountText.text = playerCountInput.text;
        }
    }

    public void SyncToSlider(Slider slider)
    {
        if (slider.transform.parent.name == "Cycle")
        {
            // 싱크
            cycleMaxSlider.value = int.Parse(cycleMaxInput.text);
        }
        else if (slider.transform.parent.name == "Player")
        {
            // 싱크
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

        Debug.Log("game rule :: cycle 값 변경됨 => " +GameRule.cycleMax );

        cycleMaxInput.text = GameRule.cycleMax.ToString();
    }

    public void PlayerSet(int value)
    {
        GameRule.playerCount = Limiter(value, (int)playerCountSlider.minValue, (int)playerCountSlider.maxValue);

        Debug.Log("game rule :: player 값 변경됨 => " + GameRule.playerCount);

        playerCountInput.text = GameRule.playerCount.ToString();
    }



}
