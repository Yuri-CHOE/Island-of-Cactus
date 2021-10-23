using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;


namespace CustomAI
{
    namespace MiniGame
    {
        public class MiniAI
        {
            public struct Piece
            {
                public Transform obj;
                public float value;

                public Piece(Transform _obj, float _value)
                {
                    obj = _obj;
                    value = _value;
                }
            }
            public struct Answer
            {
                // 답안
                public List<Piece> pieces;


                // 생성자
                public Answer(List<Piece> listOrNull)
                {
                    if (listOrNull == null)
                        pieces = new List<Piece>();
                    else
                        pieces = listOrNull;
                }

                // 기본값
                public static Answer none = new Answer();
            }
            public enum AnswerType
            {
                none,
                single,
                pair,
                multiple,
            }
            public struct Remember
            {
                public List<Piece> piece;
                public List<Answer> answer;
            }

            // 생성자
            protected MiniAI()
            {

            }
            public MiniAI(Player _owner)
            {
                owner = _owner;
                remember.piece = new List<Piece>();
                remember.answer = new List<Answer>();
            }


            // 소유자
            public Player owner = null;

            // 진행 제어
            public Coroutine workControl = null;


            // 지능
            public AIElement brain = AIElement.New();

            // 답안 학습
            public Remember remember = new Remember();

            // 공개된 답안
            public static List<Answer> opendAnswer = new List<Answer>();

            // 전체 선택지
            public static List<Piece> restPiece = new List<Piece>();


            // 초기화
            public void Clear()
            {
                workControl = null;
                brain = AIElement.New();
                remember.piece.Clear();
                remember.answer.Clear();
            }


            // 학습
            /// <summary>
            /// 정답을 추론할수 있는 단서
            /// </summary>
            /// <param name="piece">단서</param>
            public void Learn(Piece piece)
            {
                Debug.Log("미니 AI :: 학습 대상 -> " + piece.obj.name + " by " + owner.name);
                // 중복 방지
                if (remember.piece.Contains(piece))
                    return;

                remember.piece.Add(piece);
            }
            public static void LearnEveryAI(Piece piece)
            {
                for (int i = 0; i < MiniPlayerManager.entryAI.Count; i++)
                    MiniPlayerManager.entryAI[i].miniAi.Learn(piece);

            }
            /// <summary>
            /// 확정된 정답
            /// </summary>
            /// <param name="answer">정답</param>
            void Learn(Answer answer)
            {
                if (answer.pieces == null || answer.pieces.Count == 0)
                {
                    Debug.LogError("error :: 미니게임 답안 추가 불가능 -> 입력된 단서 없음");
                    Debug.Break();
                    return;
                }

                // 중복 방지
                if (remember.answer.Contains(answer))
                    return;

                // 단서 제외처리
                for (int i = 0; i < answer.pieces.Count; i++)
                    Consume(answer.pieces[i]);

                remember.answer.Add(answer);
            }
            public static void LearnEveryAI(Answer answer)
            {
                for (int i = 0; i < MiniPlayerManager.entryAI.Count; i++)
                    MiniPlayerManager.entryAI[i].miniAi.Learn(answer);

            }


            // 정보 소모
            /// <summary>
            /// 단서 제거
            /// </summary>
            /// <param name="piece"></param>
            void Consume(Piece piece)
            {
                if (remember.piece.Contains(piece))
                    remember.piece.Remove(piece);
            }
            /// <summary>
            /// 정답 제거
            /// </summary>
            /// <param name="answer"></param>
            public void Consume(Answer answer)
            {
                // 정답 제외처리
                if(remember.answer.Contains(answer))
                    remember.answer.Remove(answer);

                // 단서 제외처리
                for (int i = 0; i < answer.pieces.Count; i++)
                    Consume(answer.pieces[i]);
            }
            public static void ConsumeEveryAI(Answer answer)
            {
                for (int i = 0; i < MiniPlayerManager.entryAI.Count; i++)
                    MiniPlayerManager.entryAI[i].miniAi.Consume(answer);

            }
            public void SelectedAnswer(Answer answer)
            {
                // 제출
                MiniGameManager.answer = answer;
                MiniGameManager.isAnswerSubmit = true;

                // 로그
                Debug.Log("미니 AI :: " + owner.name + "의 답안 제출됨 -> " + MiniGameManager.answer);
            }
            /// <summary>
            /// 정답 공개 처리
            /// </summary>
            /// <param name="answer"></param>
            public static void OpenAnswer(Answer answer)
            {
                // 정답 및 단서 제외
                ConsumeEveryAI(answer);

                // 공개된 정답 추가
                opendAnswer.Add(answer);

                // 선택지 제거
                for (int i = 0; i < answer.pieces.Count; i++)
                    restPiece.Remove(answer.pieces[i]);
            }


            // 추론
            /// <summary>
            /// 추론 가능 여부 (warning :: 정답여부 아님)
            /// </summary>
            /// <returns>추론 가능 여부</returns>
            public bool CanThink()
            {
                if (MiniGameManager.answerType == AnswerType.single)
                    return remember.piece.Count >= 1;
                else if (MiniGameManager.answerType == AnswerType.pair)
                    return remember.piece.Count >= 2;
                else if (MiniGameManager.answerType == AnswerType.multiple)
                    return remember.piece.Count >= 2;
                else
                    return false;
            }
            public bool IsSameValue(Piece piece_1, Piece piece_2)
            {
                return piece_1.value == piece_2.value;
            }
            /// <summary>
            /// true = 같은 값의 서로 다른 오브젝트
            /// </summary>
            /// <param name="piece_1"></param>
            /// <param name="piece_2"></param>
            /// <returns></returns>
            public bool IsSameValueEach(Piece piece_1, Piece piece_2)
            {
                return !IsSameObject(piece_1, piece_2) && IsSameValue(piece_1, piece_2);
            }
            public bool IsSameObject(Piece piece_1, Piece piece_2)
            {
                return piece_1.obj == piece_2.obj;
            }
            public bool IsSameAbsolute(Piece piece_1, Piece piece_2)
            {
                return (piece_1.obj == piece_2.obj) && (piece_1.value == piece_2.value);
            }



            // 게임 수행
            public IEnumerator Work(Minigame minigame)
            {
                // 게임 수행
                switch (minigame.index)
                {
                    // 기본 로직 - 답 찾기
                    /*
                     * step 1
                     * CanThink() -> 추론이 가능하면
                     *      단서(remember.piece) 조합하여 정답 확인
                     * 
                     * step 2
                     * 단서 조합 결과가 정답이면
                     *      Learn(Answer answer) -> 정답 목록 추가 및 단서목록 제거
                     *      
                     * step 3
                     * 정답(remember.answer)이 있고 지능확률 적중하면
                     *      정답 선택
                     *      종료
                     *      
                     * step 4
                     * 오답 선택
                     * 
                     * step 5
                     * 제출
                     */

                    case 1: // 카드 짝맞추기
                        {
                            Piece temp;
                            List<Piece> pl = new List<Piece>();
                            List<Answer> newAnswer = new List<Answer>();

                            // step 1 - 단서 조합
                            if (CanThink())
                            {

                                // 단서 전체 스캔
                                for (int i = 0; i <  remember.piece.Count; i++)
                                {
                                    temp = remember.piece[i];
                                    // 대상 비교
                                    for (int j = i + 1; j < remember.piece.Count; j++)
                                    {
                                        // 같은값의 다른 오브젝트일 경우
                                        if (IsSameValueEach(temp, remember.piece[j]))
                                        {
                                            // 정답으로 판단
                                            pl.Add(temp);
                                            pl.Add(temp);
                                            newAnswer.Add(new Answer(pl));
                                        }
                                    }
                                    pl.Clear();
                                }


                                // step 2 - 정답으로 승격
                                while (newAnswer.Count > 0)
                                    Learn(newAnswer[0]);

                            }

                            Answer selectTemp;

                            // step 3 - 답안 작성

                            float intelCut = Random.Range(0.00f, 1.00f);

                            // 정답을 보유중이고 요구 지능 이상일경우
                            if ((remember.answer.Count > 0) && (brain.intelligence.valueStatic >= intelCut))
                            {
                                // 선택
                                selectTemp = remember.answer[   Random.Range(0, remember.answer.Count)  ];
                            }

                            // step 4 - 답안 작성
                            else
                            {
                                // 랜덤 선택
                                pl.Clear();
                                int rand = Random.Range(0, restPiece.Count);
                                pl.Add(restPiece[rand]);

                                //인접 선택
                                pl.Add(restPiece[(rand + 1) % restPiece.Count]);

                                selectTemp = new Answer(pl);
                            }

                            // step 5 - 제출
                            SelectedAnswer(selectTemp);
                        }
                        break;
                }

                yield return null;

                // 종료 처리
                workControl = null;                
            }
        }
    }
}