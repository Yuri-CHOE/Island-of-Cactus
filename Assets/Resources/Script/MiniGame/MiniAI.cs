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
                // ���
                public List<Piece> pieces;


                // ������
                public Answer(List<Piece> listOrNull)
                {
                    if (listOrNull == null)
                        pieces = new List<Piece>();
                    else
                        pieces = listOrNull;
                }

                // �⺻��
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

            // ������
            protected MiniAI()
            {

            }
            public MiniAI(Player _owner)
            {
                owner = _owner;
                remember.piece = new List<Piece>();
                remember.answer = new List<Answer>();
            }


            // ������
            public Player owner = null;

            // ���� ����
            public Coroutine workControl = null;


            // ����
            public AIElement brain = AIElement.New();

            // ��� �н�
            public Remember remember = new Remember();

            // ������ ���
            public static List<Answer> opendAnswer = new List<Answer>();

            // ��ü ������
            public static List<Piece> restPiece = new List<Piece>();


            // �ʱ�ȭ
            public void Clear()
            {
                workControl = null;
                brain = AIElement.New();
                remember.piece.Clear();
                remember.answer.Clear();
            }


            // �н�
            /// <summary>
            /// ������ �߷��Ҽ� �ִ� �ܼ�
            /// </summary>
            /// <param name="piece">�ܼ�</param>
            public void Learn(Piece piece)
            {
                Debug.Log("�̴� AI :: �н� ��� -> " + piece.obj.name + " by " + owner.name);
                // �ߺ� ����
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
            /// Ȯ���� ����
            /// </summary>
            /// <param name="answer">����</param>
            void Learn(Answer answer)
            {
                if (answer.pieces == null || answer.pieces.Count == 0)
                {
                    Debug.LogError("error :: �̴ϰ��� ��� �߰� �Ұ��� -> �Էµ� �ܼ� ����");
                    Debug.Break();
                    return;
                }

                // �ߺ� ����
                if (remember.answer.Contains(answer))
                    return;

                // �ܼ� ����ó��
                for (int i = 0; i < answer.pieces.Count; i++)
                    Consume(answer.pieces[i]);

                remember.answer.Add(answer);
            }
            public static void LearnEveryAI(Answer answer)
            {
                for (int i = 0; i < MiniPlayerManager.entryAI.Count; i++)
                    MiniPlayerManager.entryAI[i].miniAi.Learn(answer);

            }


            // ���� �Ҹ�
            /// <summary>
            /// �ܼ� ����
            /// </summary>
            /// <param name="piece"></param>
            void Consume(Piece piece)
            {
                if (remember.piece.Contains(piece))
                    remember.piece.Remove(piece);
            }
            /// <summary>
            /// ���� ����
            /// </summary>
            /// <param name="answer"></param>
            public void Consume(Answer answer)
            {
                // ���� ����ó��
                if(remember.answer.Contains(answer))
                    remember.answer.Remove(answer);

                // �ܼ� ����ó��
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
                // ����
                MiniGameManager.answer = answer;
                MiniGameManager.isAnswerSubmit = true;

                // �α�
                Debug.Log("�̴� AI :: " + owner.name + "�� ��� ����� -> " + MiniGameManager.answer);
            }
            /// <summary>
            /// ���� ���� ó��
            /// </summary>
            /// <param name="answer"></param>
            public static void OpenAnswer(Answer answer)
            {
                // ���� �� �ܼ� ����
                ConsumeEveryAI(answer);

                // ������ ���� �߰�
                opendAnswer.Add(answer);

                // ������ ����
                for (int i = 0; i < answer.pieces.Count; i++)
                    restPiece.Remove(answer.pieces[i]);
            }


            // �߷�
            /// <summary>
            /// �߷� ���� ���� (warning :: ���俩�� �ƴ�)
            /// </summary>
            /// <returns>�߷� ���� ����</returns>
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
            /// true = ���� ���� ���� �ٸ� ������Ʈ
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



            // ���� ����
            public IEnumerator Work(Minigame minigame)
            {
                // ���� ����
                switch (minigame.index)
                {
                    // �⺻ ���� - �� ã��
                    /*
                     * step 1
                     * CanThink() -> �߷��� �����ϸ�
                     *      �ܼ�(remember.piece) �����Ͽ� ���� Ȯ��
                     * 
                     * step 2
                     * �ܼ� ���� ����� �����̸�
                     *      Learn(Answer answer) -> ���� ��� �߰� �� �ܼ���� ����
                     *      
                     * step 3
                     * ����(remember.answer)�� �ְ� ����Ȯ�� �����ϸ�
                     *      ���� ����
                     *      ����
                     *      
                     * step 4
                     * ���� ����
                     * 
                     * step 5
                     * ����
                     */

                    case 1: // ī�� ¦���߱�
                        {
                            Piece temp;
                            List<Piece> pl = new List<Piece>();
                            List<Answer> newAnswer = new List<Answer>();

                            // step 1 - �ܼ� ����
                            if (CanThink())
                            {

                                // �ܼ� ��ü ��ĵ
                                for (int i = 0; i <  remember.piece.Count; i++)
                                {
                                    temp = remember.piece[i];
                                    // ��� ��
                                    for (int j = i + 1; j < remember.piece.Count; j++)
                                    {
                                        // �������� �ٸ� ������Ʈ�� ���
                                        if (IsSameValueEach(temp, remember.piece[j]))
                                        {
                                            // �������� �Ǵ�
                                            pl.Add(temp);
                                            pl.Add(temp);
                                            newAnswer.Add(new Answer(pl));
                                        }
                                    }
                                    pl.Clear();
                                }


                                // step 2 - �������� �°�
                                while (newAnswer.Count > 0)
                                    Learn(newAnswer[0]);

                            }

                            Answer selectTemp;

                            // step 3 - ��� �ۼ�

                            float intelCut = Random.Range(0.00f, 1.00f);

                            // ������ �������̰� �䱸 ���� �̻��ϰ��
                            if ((remember.answer.Count > 0) && (brain.intelligence.valueStatic >= intelCut))
                            {
                                // ����
                                selectTemp = remember.answer[   Random.Range(0, remember.answer.Count)  ];
                            }

                            // step 4 - ��� �ۼ�
                            else
                            {
                                // ���� ����
                                pl.Clear();
                                int rand = Random.Range(0, restPiece.Count);
                                pl.Add(restPiece[rand]);

                                //���� ����
                                pl.Add(restPiece[(rand + 1) % restPiece.Count]);

                                selectTemp = new Answer(pl);
                            }

                            // step 5 - ����
                            SelectedAnswer(selectTemp);
                        }
                        break;
                }

                yield return null;

                // ���� ó��
                workControl = null;                
            }
        }
    }
}