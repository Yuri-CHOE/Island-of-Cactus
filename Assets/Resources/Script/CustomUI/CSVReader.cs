using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class CSVReader
{
    // 데이터
    public List<List<string>> table = new List<List<string>>();

    // 파일 경로
    public string path = null;

    // 복사본 사용 여부
    public bool isCopyFile = false;
    // 사본을 다뤄야 할 경우는?
    //  ㄴ Resources는 패킹 후에는 읽기 전용
    //  ㄴ 유저 데이터나 세이브 데이터 등 수정이 필요한 파일의 경우 앱 데이터 폴더에서 관리해야함
    //  ㄴ 이럴땐 _isReadOnly 를 false로 할것

    // 저장 불가능여부
    public bool isReadOnly = true;


    // 데이터 위치
    public static string basicPath = Application.dataPath + @"/Resources" + @"/Data";   // 프로젝트디렉토리/Assets/Resources/Data
    public static string copyPath = Application.persistentDataPath + @"/Data";           // 기기별 데이터 폴더/Data


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
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly, char dataSeparator, char lineSeparator)
    {
        // 원본 폴더 체크
        CheckPath(basicPath);

        // 사본 폴더 체크
        CheckPath(copyPath);

        // 복사본 사용 여부 설정
        isCopyFile = _isCopyFile;

        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(subPathOrNull, fileName, dataSeparator, lineSeparator);
    }
    /// <summary>
    /// 선언과 동시에 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly, char dataSeparator)
    {
        // 원본 폴더 체크
        CheckPath(basicPath);

        // 사본 폴더 체크
        CheckPath(copyPath);

        // 복사본 사용 여부 설정
        isCopyFile = _isCopyFile;

        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(subPathOrNull, fileName, dataSeparator);
    }
    /// <summary>
    /// 선언과 동시에 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    /// <param name="_isReadOnly">저장할 필요 없다면 true</param>
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly)
    {
        // 원본 폴더 체크
        CheckPath(basicPath);

        // 사본 폴더 체크
        CheckPath(copyPath);

        // 복사본 사용 여부 설정
        isCopyFile = _isCopyFile;

        // 저장 가능 여부 설정
        isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(subPathOrNull, fileName);
    }
    /// <summary>
    /// 선언과 동시에 읽기 전용으로 파일 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="fileName">/세부 경로/파일명.확장자</param>
    public CSVReader(string subPathOrNull, string fileName)
    {
        // 원본 폴더 체크
        CheckPath(basicPath);

        // 사본 폴더 체크
        CheckPath(copyPath);

        //// 복사본 사용 여부 설정
        //isCopyFile = _isCopyFile;

        //// 저장 가능 여부 설정
        //isReadOnly = !_isReadOnly;

        // 파일 읽기
        ReadFile(subPathOrNull, fileName);
    }


    /// <summary>
    /// 폴더 존재를 체크하고 없으면 생성
    /// </summary>
    /// <param name="_path">/세부 경로</param>
    public static void CheckPath(string _path)
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
    public static bool CheckFile(string _path, string subPathOrNull, string fileName, bool _isCopyFile)
    {
        Debug.Log("파일 체크 :: " + @_path);

        bool result = true;

        // 파일 체크 및 복사
        FileInfo fi = new FileInfo(@_path);
        if (fi.Exists)
        {
            Debug.Log("파일 확인됨");
        }
        else
        {
            Debug.LogWarning("파일 누락");
            if (_isCopyFile)       // 데이터 저장이 필요할 경우
            {
                string path_basic = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

                Debug.Log("파일 복사 요청됨 :: " + @path_basic);
                CheckPath(string.Format("{0}/{1}", copyPath, subPathOrNull));
                FileInfo temp = new FileInfo(@path_basic);
                if (temp.Exists)
                {
                    Debug.Log("원본 파일 확인됨");
                    temp.CopyTo(@_path);
                    Debug.Log("파일 복사됨 :: \t" + @_path + "\n\t\t" + temp.FullName);
                }
                else
                {
                    result = false;
                    Debug.LogError("원본 파일 누락됨");
                }
            }
        }

        return result;
    }
    /// <summary>
    /// 파일 체크
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="fileName"></param>
    public static bool CheckFile(string _path, string fileName)
    {
        // 경로 체크
        CheckPath(@_path);

        string fullPath = _path + '/' + fileName;

        Debug.Log("파일 체크 :: " + @fullPath);

        // 파일 체크 및 복사
        FileInfo fi = new FileInfo(@fullPath);
        bool result = fi.Exists;
        if (result)
            Debug.Log("파일 확인됨");
        else
            Debug.LogWarning("파일 누락 :: " + fullPath);
        return result;
    }


    /// <summary>
    /// 읽어온 데이터 및 경로 초기화
    /// </summary>
    void Reset()
    {
        // 초기화
        table.Clear();
        path = null;
        isCopyFile = false;
        isReadOnly = true;
    }


    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// </summary>
    /// <param name="subPathOrNull">세부 경로</param>
    /// <param name="fileName">파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    /// <param name="lineSeparator">라인 구분자</param>
    public void ReadFile(string subPathOrNull, string fileName, char dataSeparator, char lineSeparator)
    {
        // 초기화
        //Reset();

        // 파일명 누락 방지
        if (fileName == null)
            return;

        //// 경로 설정
        //if (isCopyFile)
        //    path = string.Format("{0}/{1}/{2}", copyPath, subPathOrNull, fileName);
        //else
        //    path = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

        //// 파일 체크
        //CheckFile(path, fileName, isCopyFile);

        //// 경로 설정
        //if (isCopyFile)
        //    path = string.Format("{0}/{1}/{2}", copyPath, subPathOrNull, fileName);
        //else
        //    path = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

        //// 파일 체크
        //// 누락시 자동 원본 복사
        //// 복사 실패시 중단
        //if(!CheckFile(path, subPathOrNull, fileName, isCopyFile))
        //    return;


        // 경로 설정
        if (isCopyFile)
            path = string.Format("{0}/{1}", copyPath, subPathOrNull);
        else
            path = string.Format("{0}/{1}", basicPath, subPathOrNull);

        string fullPath = string.Format("{0}/{1}", path, fileName);

        // 파일 체크
        if (!CheckFile(path, fileName))
        {
            // 복사본일 경우 원본 복사 시도
            if (isCopyFile)
            {
                string pathBasic = string.Format("{0}/{1}", basicPath, subPathOrNull);
                // 원본 체크
                if (CheckFile(@pathBasic, fileName))
                {
                    // 원본 복사
                    string pathBasicFile = string.Format("{0}/{1}", pathBasic, fileName);

                    FileInfo temp = new FileInfo(@pathBasicFile);
                    temp.CopyTo(@fullPath);
                    Debug.LogWarning("파일 복사됨 :: \t" + @pathBasicFile);
                }
                // 원본 파일 없음
                else
                {
                    Debug.LogError("error :: 파일 없음\n" + fullPath);
                    return;
                }
            }
            // 파일 없음
            else
            {
                Debug.LogError("error :: 파일 없음\n" + fullPath);
                Debug.Break();
                return;
            }
        }

        path = @fullPath;


        // 열기
        //FileStream fs = new FileStream(@path, FileMode.Open);
        StreamReader sr = new StreamReader(@path);
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
        {
            List<string> dataTemp = new List<string>();
            dataTemp.AddRange(strLine[i].Split(dataSeparator));

            table.Add(dataTemp);
        }

        // 닫기
        sr.Close();
        //fs.Close();
        Debug.Log("File close :: " + @path);
    }
    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// 라인 구분자 = '\n'
    /// </summary>
    /// <param name="subPathOrNull">세부 경로</param>
    /// <param name="fileName">파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public void ReadFile(string subPathOrNull, string fileName, char dataSeparator)
    {
        ReadFile(subPathOrNull, fileName, dataSeparator, '\n');
    }
    /// <summary>
    /// 세부경로 및 파일명으로 읽어오기
    /// 라인 구분자 = '\n'
    /// 데이터 구분자 = ','
    /// </summary>
    /// <param name="subPathOrNull">세부 경로</param>
    /// <param name="fileName">파일명.확장자</param>
    /// <param name="dataSeparator">데이터 구분자</param>
    public void ReadFile(string subPathOrNull, string fileName)
    {
        ReadFile(subPathOrNull, fileName, ',', '\n');
    }


    public void Save(char dataSeparator, char lineSeparator)
    {
        // 읽기 전용일 경우
        if (!isCopyFile)
        {
            Debug.Log("파일 저장 불가 :: 원본 파일 수정 불가");
            return;
        }

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
        if (table[0].Count <= 0)
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
            for (int j = 1; j < table[i].Count; j++)
            {
                sb.Append(dataSeparator).Append(table[i][j]);
            }

            // 개행 문자 삽입
            if (i < table.Count - 1)
                if (lineSeparator == '\n')
                    sb.Append('\r').Append('\n');
                else
                    sb.Append(lineSeparator);
            else
                Debug.LogError("개행 종료");
        }

        // 파일 초기화
        FileStream fs = new FileStream(@path, FileMode.Create);
        fs.Close();

        // 파일 작성
        StreamWriter sw = new StreamWriter(@path);
        sw.Write(sb);

        // 파일 닫기
        //fs.Close();
        sw.Close();
    }
    public void Save(char dataSeparator)
    {
        Save(dataSeparator, '\n');
    }
    public void Save()
    {
        Save(',', '\n');
    }

    /// <summary>
    /// 라인 구분 없는 스트링을 파일로 저장
    /// </summary>
    /// <param name="subPathOrNull">생략 가능, 고정 경로 : Assets/Resources/Data/</param>
    /// <param name="fileName">이미 존재하지 않는 파일명</param>
    /// <param name="data">저장 내용</param>
    public static void SaveNew(string subPathOrNull, string fileName, bool _isCopyFile, bool useOverWrite, string data)
    {
        Debug.Log("저장 요청됨");

        // 파일명 없을 경우 중단
        if (fileName == null)
            return;

        // 데이터 없을 경우 중단
        if (data == null)
            return;

        // 경로 조합
        string __path = null;

        if (_isCopyFile)
            __path = copyPath;
        else
            __path = basicPath;

        if (subPathOrNull != null)
            __path += '/' + @subPathOrNull;

        // 폴더 체크
        CheckPath(@__path);

        string _file = __path + '/' + fileName;

        // 파일 체크 및 복사
        FileInfo fi = new FileInfo(@_file);
        if (fi.Exists && !useOverWrite)
        {
            Debug.LogError("저장 실패 :: 파일 중복");
        }
        else
        {


            // 파일 초기화
            FileStream fs = new FileStream(@_file, FileMode.Create);
            fs.Close();

            // 생성
            //StreamWriter sw = fi.CreateText();
            StreamWriter sw = new StreamWriter(@_file);

            // 작성
            //sw.WriteLine(data);
            sw.Write(data);

            // 닫기
            sw.Close();

            Debug.LogWarning("저장 성공 :: " + @_file);
        }
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
