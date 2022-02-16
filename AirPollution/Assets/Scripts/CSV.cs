using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSV
{
    static CSV csv;
    public List<string[]> m_ArrayData;

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
        catch {
            Debug.Log("file don't finded!");
            return;
        }
        string line; //暫存每一行數據
        while ((line = sr.ReadLine()) != null) {
            m_ArrayData.Add(line.Split(','));
        }
        sr.Close();
        sr.Dispose();
        }
}
