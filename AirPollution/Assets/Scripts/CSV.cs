using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSV
{
    static CSV csv;
    public List<string[]> m_ArrayData;
    public string[] colArray;
   

    public static CSV GetInstance()
    {
        if (csv == null)
        {
            csv = new CSV();
        }
        return csv;
    }

    public string getString(int row, int col) 
    {
        return m_ArrayData[row][col];
    }

    public int getInt(int row, int col)
    {
        return int.Parse(m_ArrayData[row][col]);
    }

    public string[] GetRow(int row)
    {
        return m_ArrayData[row];
    }

    public string[] GetCol(int col)
    {
        colArray = new string[m_ArrayData.Count];
        Debug.Log("DO YOU HERE:" + col);
        for (int i = 0; i < m_ArrayData.Count; i++)
        {
            //Debug.Log("MY COL IS:"+m_ArrayData[i][col]);
            //Debug.Log("MY ARRAY SIZE IS:" + m_ArrayData.Count);
            colArray[i] = m_ArrayData[i][col];
           // Debug.Log("MY COL IS:" + colArray[i]);
        }
        return colArray;
    }


    private CSV() 
    { 
        m_ArrayData = new List<string[]>();
    }

    public void loadFile(string path, string fileName)
    {
        m_ArrayData.Clear(); //初始化數據
        StreamReader sr = null; //保存讀取文件後的原始數據
        try
        {
            sr = File.OpenText(path + "//" + fileName);
            Debug.Log("file finded!");
        }
        catch
        {
            Debug.Log("file don't finded!");
            return;
        }
        string line; //暫存每一行數據
        while ((line = sr.ReadLine()) != null)
        {
            m_ArrayData.Add(line.Split(','));
        }
        sr.Close();
        sr.Dispose();
    }

    //public string[][] m_arrayData;

    //public void LoadFile(string path, string fileName)
    //{
    //    讀取新文件前保證之前的數據為空
    //    m_arrayData = new string[0][];
    //    string fillPath = path + "/" + fileName;

    //    解析每一行的數據
    //      string[] lineArray;
    //    try
    //    {
    //        注意編碼方式，這裡用的是Windows系統自定義的編碼方式Encoding.Default，其實也就是GB2312（簡體中文），編碼不對應的話解析出來很可能是亂碼
    //       lineArray = File.ReadAllLines(fillPath, Encoding.Default);
    //        Debug.Log("file finded!");
    //    }
    //    catch
    //    {
    //        Debug.Log("file do not find!");
    //        return;
    //    }

    //    m_arrayData = new string[lineArray.Length][];
    //    for (int i = 0; i < lineArray.Length; i++)
    //    {
    //        CSV格式的文件採用英文逗號作為分隔符
    //        m_arrayData[i] = lineArray[i].Split(',');
    //    }
    //}


}
