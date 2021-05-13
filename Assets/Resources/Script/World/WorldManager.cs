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

       

    // 월드 파일 저장 경로
    static string _subRoot = "World";
    public static string subRoot { get { return _subRoot; } }

    // 월드 파일 확장자
    static string _extension = ".iocw";     
    public static string extension { get { return @_extension; } }

    // 월드 파일 종료 문자
    static char _endSplit = '#';
    public static char endSplit { get { return _endSplit; } }

    // 월드 파일 테이블 구문 문자
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
        // 확장자 생략 처리
        string fName;
        if (filename.Contains(extension))
            fName = filename;
        else
            fName = filename + extension;

        // 파일 읽기
        CSVReader worldFileReader = new CSVReader(subRoot, fName, true, tableSplit, endSplit);

        worldFile = worldFileReader.table[0];
    }
}
