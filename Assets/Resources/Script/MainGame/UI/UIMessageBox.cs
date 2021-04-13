using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageBox : MonoBehaviour
{
    public Image m_box;
    public GameObject Innerbox;

    public GameObject buttonList;

    //public List<GameObject> btn;

    //[SerializeField]
    //List<string> btnSetName = new List<string>();
    //List<bool[]> btnSet = new List<bool[]>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void addBtnSet(string name, bool[] bArr)
    //{
    //    if(bArr.Length != btn.Count || bArr.Length < 0)
    //    {
    //        Debug.Log("Error :: Out of Range ( button : 0~" + (btn.Count-1) + ") -> Array : " + (bArr.Length-1));
    //        return;
    //    }

    //    btnSetName.Add(name);
    //    btnSet.Add(bArr);
    //}


    //public void setUp(int number)
    //{
    //    if (number >= btnSet.Count || number < 0)
    //    {
    //        Debug.Log("Error :: Out of Range ( button set Lixt : 0~" + (btnSet.Count - 1) + ") -> Array : " + number);
    //        return;
    //    }

    //    Debug.Log("Excute :: Button list setup -> [" + number + "]" + btnSetName[number]);
    //}





    void popUp()
    {

    }
}
