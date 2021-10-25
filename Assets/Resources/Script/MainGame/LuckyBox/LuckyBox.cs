using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBox
{
    public enum Type
    {
        None,
        Move,
        WorldEvent,
        MonsterWave,
        MiniGame,
        GetItem,
        StealItem,
    }

    // 테이블
    static List<LuckyBox> _table = new List<LuckyBox>();
    public static List<LuckyBox> table { get { return _table; } }


    // 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }




    // 럭키박스 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 럭키박스 카테고리
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // 럭키박스 이름
    string _name = null;
    public string name { get { return _name; } }
    
    // 럭키박스 정보
    string _info = null;
    public string info { get { return _info; } }

    // 효과
    public IocEffect effect = new IocEffect();

    // 럭키박스 레어도 (드랍률)
    int _rare = -1;
    public int rare { get { return _rare; } }



    // 생성자

    /// <summary>
    /// 사용 금지
    /// </summary>
    protected LuckyBox()
    {
        // 사용 금지
    }
    /// <summary>
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    protected LuckyBox(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 11)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기

        // 인덱스
        _index = int.Parse(strList[0]);

        // 카테고리
        _type = (Type)int.Parse(strList[1]);

        // 이름
        _name = loaclList[1];

        // 효과
        effect.Set(
        (IocEffect.Expiration)(int.Parse(strList[3])),
            int.Parse(strList[4]),
            (IocEffect.Target)(int.Parse(strList[5])),
            int.Parse(strList[6]),
            (IocEffect.What)(int.Parse(strList[7])),
            int.Parse(strList[8])
            );
        
        // 정보
        _info = loaclList[2].Replace("\\n", "\n").Replace("value", strList[8]);

        // 레어도
        _rare = int.Parse(strList[10]);       
    }



    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 럭키박스");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader luckyReader = new CSVReader(null, "LuckyBox.csv");
        CSVReader local = new CSVReader(null, "LuckyBox_local.csv", true, false);

        // 더미 생성
        table.Add(new LuckyBox());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < luckyReader.table.Count; i++)
        {
            table.Add(new LuckyBox(luckyReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }










    /// <summary>
    /// 럭키박스 효과
    /// </summary>
    /// <param name="targetPlayer_Or_null">작동자</param>
    /// <returns></returns>
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        Debug.LogError("Lucky Box :: 효과 호출됨 = " + name);

        // 타겟 리스트
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // 통합 효과
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // 개별 특수 효과
        yield return EachEffect(this, targetPlayer_Or_null, pl);
    }


    /// <summary>
    /// 럭키박스 효과
    /// </summary>
    /// <param name="__luckyBox"></param>
    /// <param name="user">작동자</param>
    /// <param name="filteredTarget">효과 대상</param>
    /// <returns></returns>
    public static IEnumerator EachEffect(LuckyBox __luckyBox, Player user, List<Player> filteredTarget)
    {
        switch (__luckyBox.index)
        {
            // 필요시 추가

            case 0:
                // 0번은 없음
                break;

            case 16:
                // 선인장 도적단
                {
                    // 잘못된 인원수 차단
                    if (filteredTarget.Count == 0)
                        break;

                    // 연출
                    // 미구현=============

                    ItemSlot slot = null;
                    for (int i = 0; i < filteredTarget.Count; i++)
                    {
                        // 아이템 없으면 중단
                        if (filteredTarget[i].inventoryCount <= 0)
                            break;

                        // 가져올 아이템 지정
                        slot = filteredTarget[i].infoUI.inventory[0];

                        // 강탈
                        filteredTarget[i].RemoveItem(slot);
                    }
                }

                break;

            case 17:
                // 중립 몬스터 돌격
                {
                    // 돌진 명령 - 자동화
                    yield return MonsterManager.script.Dash(user.location, __luckyBox.effect.where);

                    // 수동화 - 사용 안함
                    {
                        //// 소환
                        //MonsterManager.script.Call(user.location);

                        //// 애니메이션 작동
                        //MonsterManager.script.Work();

                        //// 카메라 포커싱
                        //MonsterManager.script.Focus();

                        //// 돌진 명령
                        //yield return MonsterManager.script.DashOnly(__luckyBox.effect.where);

                        //// 몬스터 숨김
                        //MonsterManager.script.Hide();
                    }

                    // 카메라 탈착
                    GameData.worldManager.cameraManager.CamMoveTo(Turn.now.avatar.transform, CameraManager.CamAngle.Top);
                    //GameData.worldManager.cameraManager.CamFree();

                }
                break;

            case 20:
                // 럭키 아이템
                {
                    // 럭키박스 드랍테이블
                    DropTable dropTable = new DropTable();

                    // 드랍테이블 셋팅
                    dropTable.rare = new List<int>();
                    for (int i = 0; i < Item.tableLuckyDrop.Count; i++)
                    {
                        dropTable.rare.Add(Item.tableLuckyDrop[i].rare);
                        Debug.Log("드랍 테이블 :: 추가됨 -> " + Item.tableLuckyDrop[i].rare);
                    }
                    Debug.Log("드랍 테이블 :: 목록 총량 ->" + dropTable.rare.Count);

                    int select = dropTable.Drop();
                    Debug.Log("럭키 아이템 :: 선택됨 -> " + Item.tableLuckyDrop[select].name);

                    // 지급
                    user.AddItem(Item.tableLuckyDrop[select], 1);
                }
                break;

            case 21:
                // 공공의 적
                {
                    // 코인 최다 플레이어
                    List<Player> best = new List<Player>();
                    Player current = null;

                    for (int i = 0; i < user.otherPlayers.Count; i++)
                    {
                        current = user.otherPlayers[i];

                        // 비교대상 없을 경우 즉시 지정
                        if (best.Count == 0)
                            best.Add(current);

                        // 코인 더 많을 경우 지정
                        else if (current.coin.Value > best[0].coin.Value)
                        {
                            best.Clear();
                            best.Add(current);
                        }

                        // 같을 경우
                        else if (current.coin.Value == best[0].coin.Value)
                        {
                            best.Add(current);
                        }
                    }

                    // 코인 강탈
                    for (int i = 0; i < best.Count; i++)
                        best[i].coin.subtract(__luckyBox.effect.value / best.Count);

                    // 코인 지급
                    user.coin.Add(__luckyBox.effect.value);
                }
                break;

            case 22:
                // 코인 최다 플레이어
                {
                    List<Player> least = new List<Player>();
                    Player current = null;

                    for (int i = 0; i < user.otherPlayers.Count; i++)
                    {
                        current = user.otherPlayers[i];

                        // 비교대상 없을 경우 즉시 지정
                        if (least.Count == 0)
                            least.Add(current);

                        // 코인 더 적을 경우 지정
                        else if (current.coin.Value < least[0].coin.Value)
                        {
                            least.Clear();
                            least.Add(current);
                        }

                        // 같을 경우
                        else if (current.coin.Value == least[0].coin.Value)
                        {
                            least.Add(current);
                        }
                    }

                    // 코인 강탈
                    user.coin.subtract(__luckyBox.effect.value);

                    // 코인 지급
                    for (int i = 0; i < least.Count; i++)
                        least[i].coin.Add(__luckyBox.effect.value / least.Count);
                }
                break;


        }

        yield return null;
    }
}
