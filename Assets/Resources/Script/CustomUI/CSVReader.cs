using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class CSVReader
{
    // ������
    public List<List<string>> table = new List<List<string>>();

    // ���� ���
    public string path = null;

    // ���纻 ��� ����
    public bool isCopyFile = false;
    // �纻�� �ٷ�� �� ����?
    //  �� Resources�� ��ŷ �Ŀ��� �б� ����
    //  �� ���� �����ͳ� ���̺� ������ �� ������ �ʿ��� ������ ��� �� ������ �������� �����ؾ���
    //  �� �̷��� _isReadOnly �� false�� �Ұ�

    // ���� �Ұ��ɿ���
    public bool isReadOnly = true;


    // ������ ��ġ
    public static string basicPath = Application.dataPath + @"/Resources" + @"/Data";   // ������Ʈ���丮/Assets/Resources/Data
    public static string copyPath = Application.persistentDataPath + @"/Data";           // ��⺰ ������ ����/Data


    // ������
    protected CSVReader()
    {
        // ��� ����
    }
    /// <summary>
    /// ����� ���ÿ� ���� �о����
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    /// <param name="dataSeparator">������ ������</param>
    /// <param name="lineSeparator">���� ������</param>
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly, char dataSeparator, char lineSeparator)
    {
        // ���� ���� üũ
        CheckPath(basicPath);

        // �纻 ���� üũ
        CheckPath(copyPath);

        // ���纻 ��� ���� ����
        isCopyFile = _isCopyFile;

        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(subPathOrNull, fileName, dataSeparator, lineSeparator);
    }
    /// <summary>
    /// ����� ���ÿ� ���� �о����
    /// ���� ������ = '\n'
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    /// <param name="dataSeparator">������ ������</param>
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly, char dataSeparator)
    {
        // ���� ���� üũ
        CheckPath(basicPath);

        // �纻 ���� üũ
        CheckPath(copyPath);

        // ���纻 ��� ���� ����
        isCopyFile = _isCopyFile;

        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(subPathOrNull, fileName, dataSeparator);
    }
    /// <summary>
    /// ����� ���ÿ� ���� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    public CSVReader(string subPathOrNull, string fileName, bool _isCopyFile, bool _isReadOnly)
    {
        // ���� ���� üũ
        CheckPath(basicPath);

        // �纻 ���� üũ
        CheckPath(copyPath);

        // ���纻 ��� ���� ����
        isCopyFile = _isCopyFile;

        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(subPathOrNull, fileName);
    }
    /// <summary>
    /// ����� ���ÿ� �б� �������� ���� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    public CSVReader(string subPathOrNull, string fileName)
    {
        // ���� ���� üũ
        CheckPath(basicPath);

        // �纻 ���� üũ
        CheckPath(copyPath);

        //// ���纻 ��� ���� ����
        //isCopyFile = _isCopyFile;

        //// ���� ���� ���� ����
        //isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(subPathOrNull, fileName);
    }


    /// <summary>
    /// ���� ���縦 üũ�ϰ� ������ ����
    /// </summary>
    /// <param name="_path">/���� ���</param>
    public static void CheckPath(string _path)
    {
        Debug.Log("���� üũ :: " + @_path);

        if (!Directory.Exists(@_path))
        {
            Directory.CreateDirectory(@_path);

            Debug.Log("���� ���� :: " + @_path);
        }
    }


    /// <summary>
    /// ���� üũ �� ������ �������� ����
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    public static bool CheckFile(string _path, string subPathOrNull, string fileName, bool _isCopyFile)
    {
        Debug.Log("���� üũ :: " + @_path);

        bool result = true;

        // ���� üũ �� ����
        FileInfo fi = new FileInfo(@_path);
        if (fi.Exists)
        {
            Debug.Log("���� Ȯ�ε�");
        }
        else
        {
            Debug.LogWarning("���� ����");
            if (_isCopyFile)       // ������ ������ �ʿ��� ���
            {
                string path_basic = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

                Debug.Log("���� ���� ��û�� :: " + @path_basic);
                CheckPath(string.Format("{0}/{1}", copyPath, subPathOrNull));
                FileInfo temp = new FileInfo(@path_basic);
                if (temp.Exists)
                {
                    Debug.Log("���� ���� Ȯ�ε�");
                    temp.CopyTo(@_path);
                    Debug.Log("���� ����� :: \t" + @_path + "\n\t\t" + temp.FullName);
                }
                else
                {
                    result = false;
                    Debug.LogError("���� ���� ������");
                }
            }
        }

        return result;
    }
    /// <summary>
    /// ���� üũ
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="fileName"></param>
    public static bool CheckFile(string _path, string fileName)
    {
        // ��� üũ
        CheckPath(@_path);

        string fullPath = _path + '/' + fileName;

        Debug.Log("���� üũ :: " + @fullPath);

        // ���� üũ �� ����
        FileInfo fi = new FileInfo(@fullPath);
        bool result = fi.Exists;
        if (result)
            Debug.Log("���� Ȯ�ε�");
        else
            Debug.LogWarning("���� ���� :: " + fullPath);
        return result;
    }


    /// <summary>
    /// �о�� ������ �� ��� �ʱ�ȭ
    /// </summary>
    void Reset()
    {
        // �ʱ�ȭ
        table.Clear();
        path = null;
        isCopyFile = false;
        isReadOnly = true;
    }


    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// </summary>
    /// <param name="subPathOrNull">���� ���</param>
    /// <param name="fileName">���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    /// <param name="lineSeparator">���� ������</param>
    public void ReadFile(string subPathOrNull, string fileName, char dataSeparator, char lineSeparator)
    {
        // �ʱ�ȭ
        //Reset();

        // ���ϸ� ���� ����
        if (fileName == null)
            return;

        //// ��� ����
        //if (isCopyFile)
        //    path = string.Format("{0}/{1}/{2}", copyPath, subPathOrNull, fileName);
        //else
        //    path = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

        //// ���� üũ
        //CheckFile(path, fileName, isCopyFile);

        //// ��� ����
        //if (isCopyFile)
        //    path = string.Format("{0}/{1}/{2}", copyPath, subPathOrNull, fileName);
        //else
        //    path = string.Format("{0}/{1}/{2}", basicPath, subPathOrNull, fileName);

        //// ���� üũ
        //// ������ �ڵ� ���� ����
        //// ���� ���н� �ߴ�
        //if(!CheckFile(path, subPathOrNull, fileName, isCopyFile))
        //    return;


        // ��� ����
        if (isCopyFile)
            path = string.Format("{0}/{1}", copyPath, subPathOrNull);
        else
            path = string.Format("{0}/{1}", basicPath, subPathOrNull);

        string fullPath = string.Format("{0}/{1}", path, fileName);

        // ���� üũ
        if (!CheckFile(path, fileName))
        {
            // ���纻�� ��� ���� ���� �õ�
            if (isCopyFile)
            {
                string pathBasic = string.Format("{0}/{1}", basicPath, subPathOrNull);
                // ���� üũ
                if (CheckFile(@pathBasic, fileName))
                {
                    // ���� ����
                    string pathBasicFile = string.Format("{0}/{1}", pathBasic, fileName);

                    FileInfo temp = new FileInfo(@pathBasicFile);
                    temp.CopyTo(@fullPath);
                    Debug.LogWarning("���� ����� :: \t" + @pathBasicFile);
                }
                // ���� ���� ����
                else
                {
                    Debug.LogError("error :: ���� ����\n" + fullPath);
                    return;
                }
            }
            // ���� ����
            else
            {
                Debug.LogError("error :: ���� ����\n" + fullPath);
                Debug.Break();
                return;
            }
        }

        path = @fullPath;


        // ����
        //FileStream fs = new FileStream(@path, FileMode.Open);
        StreamReader sr = new StreamReader(@path);
        Debug.Log("File open :: " + @path);

        // ���� ���� �о����
        List<string> strLine = new List<string>();
        string tempLine = null;
        if (lineSeparator == '\n')
        {
            // �⺻ ���� ������ (\n)
            while ((tempLine = sr.ReadLine()) != null)
                strLine.Add(tempLine);
        }
        else
        {
            // Ư�� ���� ������
            tempLine = sr.ReadToEnd();
            strLine.AddRange(tempLine.Split(lineSeparator));
        }

        // ������ ������ ������ ����
        for (int i = 0; i < strLine.Count; i++)
        {
            List<string> dataTemp = new List<string>();
            dataTemp.AddRange(strLine[i].Split(dataSeparator));

            table.Add(dataTemp);
        }

        // �ݱ�
        sr.Close();
        //fs.Close();
        Debug.Log("File close :: " + @path);
    }
    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// ���� ������ = '\n'
    /// </summary>
    /// <param name="subPathOrNull">���� ���</param>
    /// <param name="fileName">���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    public void ReadFile(string subPathOrNull, string fileName, char dataSeparator)
    {
        ReadFile(subPathOrNull, fileName, dataSeparator, '\n');
    }
    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="subPathOrNull">���� ���</param>
    /// <param name="fileName">���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    public void ReadFile(string subPathOrNull, string fileName)
    {
        ReadFile(subPathOrNull, fileName, ',', '\n');
    }


    public void Save(char dataSeparator, char lineSeparator)
    {
        // �б� ������ ���
        if (!isCopyFile)
        {
            Debug.Log("���� ���� �Ұ� :: ���� ���� ���� �Ұ�");
            return;
        }

        // �б� ������ ���
        if (!isReadOnly)
        {
            Debug.Log("���� ���� �Ұ� :: �б� ����");
            return;
        }

        // ������ ������ ���� ���
        if(table.Count <= 0)
        {
            Debug.Log("���� ���� �Ұ� :: ������ ����");
            return;
        }

        // ������ �����Ͱ� ���� ���
        if (table[0].Count <= 0)
        {
            Debug.Log("���� ���� �Ұ� :: ������ ����");
            return;
        }

        Debug.Log("���� ���� ���� :: " + @path);

        // ���̺� ����
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < table.Count; i++)
        {
            // ���� ����
            sb.Append(table[i][0]);
            for (int j = 1; j < table[i].Count; j++)
            {
                sb.Append(dataSeparator).Append(table[i][j]);
            }

            // ���� ���� ����
            if (i < table.Count - 1)
                if (lineSeparator == '\n')
                    sb.Append('\r').Append('\n');
                else
                    sb.Append(lineSeparator);
            else
                Debug.LogError("���� ����");
        }

        // ���� �ʱ�ȭ
        FileStream fs = new FileStream(@path, FileMode.Create);
        fs.Close();

        // ���� �ۼ�
        StreamWriter sw = new StreamWriter(@path);
        sw.Write(sb);

        // ���� �ݱ�
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
    /// ���� ���� ���� ��Ʈ���� ���Ϸ� ����
    /// </summary>
    /// <param name="subPathOrNull">���� ����, ���� ��� : Assets/Resources/Data/</param>
    /// <param name="fileName">�̹� �������� �ʴ� ���ϸ�</param>
    /// <param name="data">���� ����</param>
    public static void SaveNew(string subPathOrNull, string fileName, bool _isCopyFile, bool useOverWrite, string data)
    {
        Debug.Log("���� ��û��");

        // ���ϸ� ���� ��� �ߴ�
        if (fileName == null)
            return;

        // ������ ���� ��� �ߴ�
        if (data == null)
            return;

        // ��� ����
        string __path = null;

        if (_isCopyFile)
            __path = copyPath;
        else
            __path = basicPath;

        if (subPathOrNull != null)
            __path += '/' + @subPathOrNull;

        // ���� üũ
        CheckPath(@__path);

        string _file = __path + '/' + fileName;

        // ���� üũ �� ����
        FileInfo fi = new FileInfo(@_file);
        if (fi.Exists && !useOverWrite)
        {
            Debug.LogError("���� ���� :: ���� �ߺ�");
        }
        else
        {


            // ���� �ʱ�ȭ
            FileStream fs = new FileStream(@_file, FileMode.Create);
            fs.Close();

            // ����
            //StreamWriter sw = fi.CreateText();
            StreamWriter sw = new StreamWriter(@_file);

            // �ۼ�
            //sw.WriteLine(data);
            sw.Write(data);

            // �ݱ�
            sw.Close();

            Debug.LogWarning("���� ���� :: " + @_file);
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
