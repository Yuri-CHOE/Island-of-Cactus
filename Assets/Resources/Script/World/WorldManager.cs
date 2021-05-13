using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("ManagerScript")]
    public CameraManager cameraManager;
    public GroundManager groundManager;
    public BlockManager blockManager;
    public DecorManager decorManager;

    public static List<string> worldFile = new List<string>();

       

    // ���� ���� ���� ���
    static string _subRoot = "World";
    public static string subRoot { get { return _subRoot; } }

    // ���� ���� Ȯ����
    static string _extension = ".iocw";     
    public static string extension { get { return @_extension; } }

    // ���� ���� ���� ����
    static char _endSplit = '#';
    public static char endSplit { get { return _endSplit; } }

    // ���� ���� ���̺� ���� ����
    static char _tableSplit = '$';
    public static char tableSplit { get { return _tableSplit; } }
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void BuildWorld(string filename)
    {
        // Ȯ���� ���� ó��
        string fName;
        if (filename.Contains(extension))
            fName = filename;
        else
            fName = filename + extension;

        // ���� �б�
        CSVReader worldFileReader = new CSVReader(subRoot, fName, true, tableSplit, endSplit);

        worldFile = worldFileReader.table[0];
    }
}
