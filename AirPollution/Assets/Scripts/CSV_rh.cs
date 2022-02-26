using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSV_rh
{
    static CSV_rh csv;
    public List<string[]> m_ArrayData;
    public string[] colArray;

    public List<string[]> data;
    public List<string[]> data2;

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
        return data2[row][col];
    }

    public int getInt(int row, int col)
    {
        return int.Parse(data2[row][col]);
    }

    public string[] GetRow(int row)
    {
        return data2[row];
    }

    public string[] GetCol(int col)
    {
        colArray = new string[data[0].Length];
        // Debug.Log("DO YOU HERE:" + col);
        for (int i = 0; i < 367; i++)
        {
            //Debug.Log("MY COL IS:"+m_ArrayData[i][col]);
            //Debug.Log("MY ARRAY SIZE IS:" + m_ArrayData.Count);
            colArray[i] = data2[i][col];
            // Debug.Log("MY COL IS:" + colArray[i]);
        }
        return colArray;
    }


    private CSV_rh()
    {
        m_ArrayData = new List<string[]>();
        data2 = new List<string[]>();
        data = new List<string[]>();

    }
    public void loadFile(string fileName)
    {
        data.Clear();
        TextAsset questdata = Resources.Load<TextAsset>(fileName);
        data.Add(questdata.text.Split('\n'));


        // Debug.Log("OOOOOOOOO" + data[0][0]);

        for (int i = 0; i < data[0].Length; i++)
        {
            for (int j = 0; j < 27; j++)
            {
                data2.Add(data[0][j].ToString().Split(','));
                Debug.Log(data2[i][j]);
            }
        }


        //for (int i = 0; i < data.Count; i++)
        //{
        //    data2.Add(data[i].ToString().Split(','));
        //}



        //for (int i = 0; i < 27; i++)
        //{
        //    for (int j = 0; j < 366; i++)
        //    {
        //        string line2 = data[i].ToString();
        //        data2[i][j] = line2.Split(',').ToString();
        //        Debug.Log("WOWOOW" + data2[i][j]);
        //        data2[i][j] = data[0][i].Split(',');
        //    }
        //    Debug.Log("LIne");
        //}
        //Debug.Log("WHO ARE　YOU");
    }
}
