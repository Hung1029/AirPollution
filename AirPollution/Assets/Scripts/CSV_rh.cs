using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSV_rh
{
    static CSV_rh csv;
    public List<string[]> m_ArrayData;
    public string[] colArray;


    public static CSV_rh GetInstance()
    {
        if (csv == null)
        {
            csv = new CSV_rh();
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


    private CSV_rh()
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
}
