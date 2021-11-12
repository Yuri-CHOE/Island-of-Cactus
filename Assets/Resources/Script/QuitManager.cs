using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitManager : MonoBehaviour
{
    public static Coroutine quitSequence = null;

    [SerializeField] Transform box = null;

    [SerializeField] InputAction escape  = null;
    //[SerializeField] InputActionMap escapeMap  = null;



    void Start()
    {
        //escapeMap.Enable();
        escape.Enable();

        // ȭ�� ���� ����
        Screen.sleepTimeout = 300;
    }


    void Update()
    {
        //if (escape.triggered)
        if (escape.triggered)
            Call();
    }

    public void Call()
    {
        // �ڽ� ȣ��
        box.gameObject.SetActive(true);
        Debug.Log("����� :: esc ����");
    }



    public void Quit()
    {
        if(quitSequence != null)
        {
            Debug.LogWarning("���� :: ���α׷� ���� ��� -> �̹� ������");
            return;
        }

        Debug.Log("���� :: ���α׷� ���� ��û��");

        // ���� ���� ����
        quitSequence = StartCoroutine(QuitSequence());
    }

    IEnumerator QuitSequence()
    {
        Debug.Log("���� :: ���α׷� ���� ���� ���۵�");
        yield return null;

        // ���� ���
        while (GameSaveStream.saveControl != null)
        {
            Debug.Log("���� :: ���� ���̺� �����");
            yield return null;
        }

        Debug.Log("���� :: ���α׷� ���� ���� �Ϸ��");

        // ����
        Application.Quit();
    }
}
