using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class CSVReader
{
    // 데이터
    public List<string[]> table = new List<string[]>();

    // 파일 경로
    public string path = null;

    // 저장 가능여부
    public bool isReadOnly = true;
    // 사본을 다뤄야 할 경우는?
    //  ㄴ Resources는 패킹 후에는 읽기 전용
    //  ㄴ 유저 데이터나 세이브 데이터 등 수정이 필요한 파일의 경우 앱 데이터 폴더에서 관리해야함
    //  ㄴ 이럴땐 _isReadOnly 를 false로 할것


    // 데이터 위치
    static string basicPath = Application.dataPath + @"/Resources" + @"/Data";   // 프로젝트디렉토리/Assets/Resources/Data
    static string copyPath = Application.persistentDataPath + @"/Data";           // 기기별 데이터 폴더/Data


    // 생성자
    protected CSVReader()
    {
        // 사용 금지
    }
    /// <summary>
    /// 선언과 동시에 파일 읽어오기
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    /// <param name="lineSeparator">라인 구분자</param>
    public CSVReader(string fileName, bool _isReadOnly, char dataSeparator, char lineSeparator)
    {
        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(fileName, dataSeparator, lineSeparator);
    }
    /// <summary>
    /// 선언과 동시에 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public CSVReader(string fileName, bool _isReadOnly, char dataSeparator)
    {
        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(fileName, dataSeparator);
    }
    /// <summary>
    /// 선언과 동시에 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    public CSVReader(string fileName, bool _isReadOnly)
    {
        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(fileName);
    }
    /// <summary>
    /// 선언과 동시에 읽기 전용으로 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    public CSVReader(string fileName)
    {
        // 저장 가능 여부 설정
        isReadOnly = true;

        // 파일 읽기
        ReadFile(fileName);
    }


    /// <summary>
    /// 폴더 존재를 체크하고 없으면 생성
    /// </summary>
    /// <param name="_path">/세부 경로</param>
    static void CheckPath(string _path)
    {
        Debug.Log("폴더 체크 :: " + @_path);

        if (!Directory.Exists(@_path))
        {
            Directory.CreateDirectory(@_path);

            Debug.Log("폴더 생성 :: " + @_path);
        }
    }


    /// <summary>
    /// 파일 체크 후 없으면 원본에서 복사
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    void CheckFile(string fileName, bool _isReadOnly)
    {
        // 원본 폴더 체크
        CheckPath(basicPath);

        // 사본 폴더 체크
        CheckPath(copyPath);

        // 파일 경로 설정
        if (_isReadOnly)
            path = basicPath + fileName;
        else
            path = copyPath + fileName;
        Debug.Log("파일 체크 :: " + @path);

        // 파일 체크 및 복사
        FileInfo fi = new FileInfo(@path);
        if (fi.Exists)
        {
            Debug.Log("파일 확인됨");
        }
        else
        {
            Debug.Log("파일 누락");
            if (!_isReadOnly)       // 데이터 저장이 필요할 경우
            {
                Debug.Log("파일 복사됨 :: " + @path);
                new FileInfo(@basicPath + @fileName).CopyTo(@path);
            }
        }
    }


    /// <summary>
    /// 읽어온 데이터 및 경로 초기화
    /// </summary>
    void Reset()
    {
        // 초기화
        table.Clear();
        path = null;
        isReadOnly = true;
    }


    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    /// <param name="lineSeparator">라인 구분자</param>
    public void ReadFile(string fileName, char dataSeparator, char lineSeparator)
    {
        // 초기화
        Reset();

        // 폴더 및 파일 체크
        CheckFile(fileName, isReadOnly);

        // 파일 누락 방지
        if (path == null)
            return;

        // 열기
        FileStream fs = new FileStream(@path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        Debug.Log("File open :: " + @path);

        // 라인 단위 읽어오기
        List<string> strLine = new List<string>();
        string tempLine = null;
        if (lineSeparator == '\n')
        {
            // 기본 라인 구분자 (\n)
            while ((tempLine = sr.ReadLine()) != null)
                strLine.Add(tempLine);
        }
        else
        {
            // 특수 라인 구분자
            tempLine = sr.ReadToEnd();
            strLine.AddRange(tempLine.Split(lineSeparator));
        }

        // 라인을 데이터 단위로 분할
        for (int i = 0; i < strLine.Count; i++)
            table.Add(strLine[i].Split(dataSeparator));

        // 닫기
        sr.Close();
        fs.Close();
        Debug.Log("File close :: " + @path);
    }
    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// 라인 구분자 = '\n'
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public void ReadFile(string fileName, char dataSeparator)
    {
        ReadFile(fileName, dataSeparator, '\n');
    }
    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public void ReadFile(string fileName)
    {
        ReadFile(fileName, ',', '\n');
    }


    public void Save(char dataSeparator, char lineSeparator)
    {
        // 읽기 전용일 경우
        if (!isReadOnly)
        {
            Debug.Log("파일 저장 불가 :: 읽기 전용");
            return;
        }

        // 저장할 라인이 없을 경우
        if(table.Count <= 0)
        {
            Debug.Log("파일 저장 불가 :: 데이터 없음");
            return;
        }

        // 저장할 데이터가 없을 경우
        if (table[0].Length <= 0)
        {
            Debug.Log("파일 저장 불가 :: 데이터 없음");
            return;
        }

        Debug.Log("파일 저장 시작 :: " + @path);

        // 테이블 구성
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < table.Count; i++)
        {
            // 라인 구성
            sb.Append(table[i][0]);
            for (int j = 1; j < table[i].Length; j++)
            {
                sb.Append(dataSeparator).Append(table[i][j]);
            }

            // 개행 문자 삽입
            if (i == table.Count - 1)
                if (lineSeparator == '\n')
                    sb.Append('\r').Append('\n');
                else
                    sb.Append(lineSeparator);
        }

        // 파일 작성
        FileStream fs = new FileStream(@path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(sb);

        // 파일 닫기
        fs.Close();
        sw.Close();
    }
    public void Save()
    {
        Save(',', '\n');
    }

    /*
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    */

}
