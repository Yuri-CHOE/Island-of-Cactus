using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class CSVReader
{
    // ������
    public List<string[]> table = new List<string[]>();

    // ���� ���
    public string path = null;

    // ���� ���ɿ���
    public bool isReadOnly = true;
    // �纻�� �ٷ�� �� ����?
    //  �� Resources�� ��ŷ �Ŀ��� �б� ����
    //  �� ���� �����ͳ� ���̺� ������ �� ������ �ʿ��� ������ ��� �� ������ �������� �����ؾ���
    //  �� �̷��� _isReadOnly �� false�� �Ұ�


    // ������ ��ġ
    static string basicPath = Application.dataPath + @"/Resources" + @"/Data";   // ������Ʈ���丮/Assets/Resources/Data
    static string copyPath = Application.persistentDataPath + @"/Data";           // ��⺰ ������ ����/Data


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
    public CSVReader(string fileName, bool _isReadOnly, char dataSeparator, char lineSeparator)
    {
        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(fileName, dataSeparator, lineSeparator);
    }
    /// <summary>
    /// ����� ���ÿ� ���� �о����
    /// ���� ������ = '\n'
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    /// <param name="dataSeparator">������ ������</param>
    public CSVReader(string fileName, bool _isReadOnly, char dataSeparator)
    {
        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(fileName, dataSeparator);
    }
    /// <summary>
    /// ����� ���ÿ� ���� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="_isReadOnly">������ �ʿ� ���ٸ� true</param>
    public CSVReader(string fileName, bool _isReadOnly)
    {
        // ���� ���� ���� ����
        isReadOnly = !_isReadOnly;

        // ���� �б�
        ReadFile(fileName);
    }
    /// <summary>
    /// ����� ���ÿ� �б� �������� ���� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    public CSVReader(string fileName)
    {
        // ���� ���� ���� ����
        isReadOnly = true;

        // ���� �б�
        ReadFile(fileName);
    }


    /// <summary>
    /// ���� ���縦 üũ�ϰ� ������ ����
    /// </summary>
    /// <param name="_path">/���� ���</param>
    static void CheckPath(string _path)
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
    void CheckFile(string fileName, bool _isReadOnly)
    {
        // ���� ���� üũ
        CheckPath(basicPath);

        // �纻 ���� üũ
        CheckPath(copyPath);

        // ���� ��� ����
        if (_isReadOnly)
            path = basicPath + fileName;
        else
            path = copyPath + fileName;
        Debug.Log("���� üũ :: " + @path);

        // ���� üũ �� ����
        FileInfo fi = new FileInfo(@path);
        if (fi.Exists)
        {
            Debug.Log("���� Ȯ�ε�");
        }
        else
        {
            Debug.Log("���� ����");
            if (!_isReadOnly)       // ������ ������ �ʿ��� ���
            {
                Debug.Log("���� ����� :: " + @path);
                new FileInfo(@basicPath + @fileName).CopyTo(@path);
            }
        }
    }


    /// <summary>
    /// �о�� ������ �� ��� �ʱ�ȭ
    /// </summary>
    void Reset()
    {
        // �ʱ�ȭ
        table.Clear();
        path = null;
        isReadOnly = true;
    }


    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    /// <param name="lineSeparator">���� ������</param>
    public void ReadFile(string fileName, char dataSeparator, char lineSeparator)
    {
        // �ʱ�ȭ
        Reset();

        // ���� �� ���� üũ
        CheckFile(fileName, isReadOnly);

        // ���� ���� ����
        if (path == null)
            return;

        // ����
        FileStream fs = new FileStream(@path, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
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
            table.Add(strLine[i].Split(dataSeparator));

        // �ݱ�
        sr.Close();
        fs.Close();
        Debug.Log("File close :: " + @path);
    }
    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// ���� ������ = '\n'
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    public void ReadFile(string fileName, char dataSeparator)
    {
        ReadFile(fileName, dataSeparator, '\n');
    }
    /// <summary>
    /// ���ΰ�� �� ���ϸ����� �о����
    /// ���� ������ = '\n'
    /// ������ ������ = ','
    /// </summary>
    /// <param name="fileName">/���� ���/���ϸ�.Ȯ����</param>
    /// <param name="dataSeparator">������ ������</param>
    public void ReadFile(string fileName)
    {
        ReadFile(fileName, ',', '\n');
    }


    public void Save(char dataSeparator, char lineSeparator)
    {
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
        if (table[0].Length <= 0)
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
            for (int j = 1; j < table[i].Length; j++)
            {
                sb.Append(dataSeparator).Append(table[i][j]);
            }

            // ���� ���� ����
            if (i == table.Count - 1)
                if (lineSeparator == '\n')
                    sb.Append('\r').Append('\n');
                else
                    sb.Append(lineSeparator);
        }

        // ���� �ۼ�
        FileStream fs = new FileStream(@path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(sb);

        // ���� �ݱ�
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
