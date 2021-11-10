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

    public static List<TextAsset> worldAsset = new List<TextAsset>();

    public static List<string> worldFile = new List<string>();

       

    // 월드 파일 저장 경로
    static string _subRoot = "World";
    public static string subRoot { get { return _subRoot; } }

    // 월드 파일 확장자
    //static string _extension = ".iocw";     
    static string _extension = ".json";     
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


    //public static void BuildWorld(string filename)
    //{
    //    // 확장자 생략 처리
    //    string fName;
    //    if (filename.Contains(extension))
    //        fName = filename;
    //    else
    //        fName = filename + extension;

    //    // 월드명 등록
    //    GameData.SetWorldFileName(fName);
    //    Debug.Log("맵 파일 :: " + fName);

    //    // 파일 읽기
    //    CSVReader worldFileReader = new CSVReader(subRoot, fName, false, true, tableSplit, endSplit);

    //    worldFile = worldFileReader.table[0];
    //}
    public static void BuildWorld(int area,int section)
    {
        if (area == 1 && section == 1)
        {
            BuildWorld(worldAsset[0]);
            return;
        }
        if (area == 1 && section == 2)
        {
            BuildWorld(worldAsset[1]);
            return;
        }
        if (area == 1 && section == 3)
        {
            BuildWorld(worldAsset[2]);
            return;
        }
    }
    public static void BuildWorld(TextAsset textAsset)
    {
        try
        {
            Debug.Log("월드 :: 사용할 맵파일 -> " + textAsset.name);

            // 파일 읽기
            CSVReader worldFileReader = new CSVReader(textAsset, tableSplit, endSplit);

            worldFile = worldFileReader.table[0];
        }
        catch { Debug.LogError("error :: TextAsset 로드 실패 누락 -> " + textAsset.name); Debug.Break(); }

    }
}
