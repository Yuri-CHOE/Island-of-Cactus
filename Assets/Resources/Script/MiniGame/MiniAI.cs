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

                public static bool operator ==(Piece p1, Piece p2) => (p1.obj == p2.obj && p1.value == p2.value);
                public static bool operator !=(Piece p1, Piece p2) => (p1.obj == p2.obj && p1.value == p2.value) ? false : true;
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

                public bool Contains(Piece piece)
                {
                    if(pieces != null)
                        for (int i = 0; i < pieces.Count; i++)
                            if (pieces[i] == piece)
                                return true;

                    return false;
                }

                bool Same(Answer answer)
                {
                    return Same(answer.pieces);
                }
                bool Same(List<Piece> pieceList)
                {
                    if (pieces != null && pieceList != null)
                        if (pieces.Count == pieceList.Count)
                            for (int i = 0; i < pieces.Count; i++)
                                for (int j = 0; j < pieceList.Count; j++)
                                    if (pieces[i] == pieceList[j])
                                        return true;

                    return false;
                }

                // �⺻��
                public static Answer none = new Answer();

                public static bool operator ==(Answer a1, Answer a2) => a1.Same(a2);
                public static bool operator !=(Answer a1, Answer a2) => !a1.Same(a2);
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
                bool check = false;
                for (int i = 0; i < remember.piece.Count; i++)
                {
                    if (remember.piece[i] == piece)
                        return;
                }

                // ���� ���� �ܼ� �ߺ� ����
                for (int i = 0; i < remember.answer.Count; i++)
                {
                    if (remember.answer[i].Contains(piece))
                        return;

                    //for (int j = 0; j < remember.answer[i].pieces.Count; j++)
                    //    if (remember.answer[i].pieces[j] == piece)
                    //        return;
                }

                // ������ ��� ���� �ܼ� �ߺ� ����
                for (int i = 0; i < opendAnswer.Count; i++)
                {
                    if (opendAnswer[i].Contains(piece))
                        return;

                    //for (int j = 0; j < opendAnswer[i].pieces.Count; j++)
                    //    if (opendAnswer[i].pieces[j] == piece)
                    //        return;
                }

                //// �����
                //string deb = string.Format("\n����� :: �ܼ� ��� ��ȸ");
                //for (int i = 0; i < remember.piece.Count; i++)
                //{
                //    deb = string.Format(deb + "\n�ܼ�{0} -> {1} = {2} -> �ߺ�����({3})", 
                //        i, 
                //        remember.piece[i].obj.name, 
                //        remember.piece[i].value, 
                //        remember.piece[i] == piece);
                //}
                //deb = string.Format(deb + "\n�߰� �ܼ� -> {0} = {1}", 
                //    piece.obj.name, 
                //    piece.value);
                //Debug.LogWarning(deb);

                // �ܼ� ���
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
                Debug.Log("�̴� AI :: ���� �߰� ��û��");

                if (answer.pieces == null || answer.pieces.Count == 0)
                {
                    Debug.LogError("error :: �̴ϰ��� ��� �߰� �Ұ��� -> �Էµ� �ܼ� ����");
                    Debug.LogError(answer.pieces);
                    Debug.LogError(answer.pieces.Count);
                    Debug.Break();
                    return;
                }

                // �ߺ� ����
                for (int i = 0; i < remember.answer.Count; i++)
                    if (remember.answer[i] == answer)
                    {
                        Debug.LogWarning("�̴� AI :: �ߺ� �������� �źε�");
                        return;
                    }

                // �ܼ� ����ó��
                for (int i = 0; i < answer.pieces.Count; i++)
                    Consume(answer.pieces[i]);

                //// �����
                //Debug.LogWarning(answer.pieces.Count);
                //string deb = string.Format("\n����� :: ���� ��� ��ȸ");
                //for (int i = 0; i < remember.answer.Count; i++)
                //{
                //    deb = string.Format(deb + "\n����{0} -> {1} = {2}  + {2} = {3} -> �ߺ�����({4})", 
                //        i, 
                //        remember.answer[i].pieces[0].obj.name, 
                //        remember.answer[i].pieces[0].value,
                //        remember.answer[i].pieces[1].obj.name,
                //        remember.answer[i].pieces[1].value,
                //        remember.answer[i] == answer);
                //}
                //deb = string.Format(deb + "\n����� :: �߰� ���� -> {0} = {1} + {2} = {3}", 
                //    answer.pieces[0].obj.name, 
                //    answer.pieces[0].value, 
                //    answer.pieces[1].obj.name, 
                //    answer.pieces[1].value);
                //for (int i = 0; i < opendAnswer.Count; i++)
                //{
                //    deb = string.Format(deb + "\n������ ����{0} -> {1} = {2}  + {2} = {3} -> �ߺ�����({4})",
                //        i,
                //        opendAnswer[i].pieces[0].obj.name,
                //        opendAnswer[i].pieces[0].value,
                //        opendAnswer[i].pieces[1].obj.name,
                //        opendAnswer[i].pieces[1].value,
                //        (opendAnswer[i] == answer));
                //}
                //Debug.LogWarning(deb);


                // ���� ���
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
                Debug.Log("�̴� AI :: �ܼ� ���� ��û�� -> " + owner.name);
                
                for (int i = 0; i < remember.piece.Count; i++)
                    if (remember.piece[i] == piece)
                    {
                        Debug.Log("�̴� AI :: �ܼ� ���� �õ� -> ����=" + (remember.piece[i] == piece));
                        //remember.piece.Remove(piece);
                        remember.piece.RemoveAt(i);
                        break;
                    }
            }
            /// <summary>
            /// ���� ����
            /// </summary>
            /// <param name="answer"></param>
            public void Consume(Answer answer)
            {
                Debug.Log("�̴� AI :: ���� ���� ��û�� -> " + owner.name);

                // ���� ����ó��
                for (int i = 0; i < remember.answer.Count; i++)
                    if (remember.answer[i] == answer)
                    {
                        Debug.Log("�̴� AI :: ���� ���� �õ� -> ����=" + (remember.answer[i] == answer));
                        //remember.answer.Remove(answer);
                        remember.answer.RemoveAt(i);
                        break;
                    }

                // �ܼ� ����ó��
                for (int i = 0; i < answer.pieces.Count; i++)
                    Consume(answer.pieces[i]);
            }
            public static void ConsumeEveryAI(Answer answer)
            {
                Debug.Log("�̴� AI :: ���� ����ó�� ��û��");

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
                                            Debug.Log(string.Format("�̴� AI :: ���� �߷� ���� -> {0} ({1},{2} = {3},{4})", owner.name, temp.obj.name, temp.value, remember.piece[j].obj.name, remember.piece[j].value));

                                            // �������� �Ǵ�
                                            pl.Add(temp);
                                            pl.Add(remember.piece[j]);
                                            newAnswer.Add(new Answer(new List<Piece>(pl)));
                                        }
                                    }
                                    pl.Clear();
                                }


                                // step 2 - �������� �°�
                                while (newAnswer.Count > 0)
                                {
                                    // ���� �н�
                                    Learn(newAnswer[0]);

                                    // ��� ����
                                    newAnswer.RemoveAt(0);
                                }

                            }

                            Answer selectTemp;

                            // step 3 - ��� �ۼ�

                            float intelCut = Random.Range(0.00f, 0.50f);

                            // ������ �������̰� �䱸 ���� �̻��ϰ��
                            if ((remember.answer.Count > 0) && (brain.intelligence.valueStatic >= intelCut))
                            {
                                Debug.Log("�̴� AI :: ���� ���� = " + remember.answer.Count + "��Ʈ -> " + owner.name);

                                if (brain.intelligence.valueStatic < intelCut)
                                    Debug.Log("�̴� AI :: ���� ȸ�ǵ�");

                                // ����
                                selectTemp = remember.answer[   Random.Range(0, remember.answer.Count)  ];
                            }

                            // step 4 - ���� �ۼ�
                            else
                            {
                                // ���� ����
                                pl.Clear();
                                int rand = Random.Range(0, restPiece.Count);
                                pl.Add(restPiece[rand]);

                                // �ܼ� ��Ȯ��
                                for (int i = 0; i < remember.piece.Count; i++)
                                {
                                    // �������� �ٸ� ������Ʈ�� ���
                                    if (IsSameValueEach(remember.piece[i], restPiece[rand]))
                                    {
                                        // �������� �Ǵ�
                                        pl.Add(remember.piece[i]);
                                        break;
                                    }
                                }

                                // ���н� ���� ����
                                if(pl.Count == 1)
                                    pl.Add(restPiece[(rand + 1) % restPiece.Count]);

                                selectTemp = new Answer(new List<Piece>(pl));
                            }

                            // step 5 - ����
                            SelectedAnswer(selectTemp);

                            Debug.LogWarning(
                            string.Format("�̴� AI :: {0}�� ���� �����-> {1} = {2} + {3} = {4}",
                    owner.name,
                    selectTemp.pieces[0].obj.name,
                    selectTemp.pieces[0].value,
                    selectTemp.pieces[1].obj.name,
                    selectTemp.pieces[1].value));
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