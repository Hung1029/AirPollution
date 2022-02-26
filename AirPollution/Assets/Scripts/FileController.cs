using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FileController : MonoBehaviour
{
    public static FileController Instance { get; private set; }
    public List <int[][]> Pm25Array;
    public string[] results,results_rh,results_tem;
    public enum ShowArray { 
        DayRow,YearCol,Default
    }
    public ShowArray showarray;

    public int defaultNum=3;
    public enum City { 
        Taipei,Kaohsiung
    }
    public City city;
    // Start is called before the first frame update
    void Start()
    {
        switch (city) {
            case City.Taipei:
                CSV.GetInstance().loadFile("taipei_pm26");
                CSV_rh.GetInstance().loadFile("taipei_rh");
                CSV_tem.GetInstance().loadFile("taipei_temp");
                //CSV.GetInstance().loadFile(Application.dataPath + "/Csv", "taipei_pm26.csv");
                // CSV_rh.GetInstance().loadFile(Application.dataPath + "/Csv", "taipei_rh.csv");
                // CSV_tem.GetInstance().loadFile(Application.dataPath + "/Csv", "taipei_temp.csv");

                break;
            case City.Kaohsiung:
                //CSV.GetInstance().loadFile(Application.dataPath + "/Csv", "kaohsiung_pm25.csv");
                //CSV_rh.GetInstance().loadFile(Application.dataPath + "/Csv", "kaohsiung_rh.csv");
               // CSV_tem.GetInstance().loadFile(Application.dataPath + "/Csv", "kaohsiung_temp.csv");
                CSV.GetInstance().loadFile("kaohsiung_pm25");
                CSV_rh.GetInstance().loadFile("kaohsiung_rh");
                CSV_tem.GetInstance().loadFile("kaohsiung_temp");

                break;
        }
       
        // Debug.Log("getInt: " + CSV.GetInstance().getRow(5));
        
    }
    private void Awake()
    {
        Instance = this;
    }

    void ShowData(int num) 
    {
        if (showarray == ShowArray.DayRow)
        {
            int counts = CSV.GetInstance().GetRow(num).Length;
            results = CSV.GetInstance().GetRow(num);

            int counts_rh = CSV_rh.GetInstance().GetRow(num).Length;
            results_rh = CSV_rh.GetInstance().GetRow(num);

            int counts_tem = CSV_tem.GetInstance().GetRow(num).Length;
            results_tem = CSV_tem.GetInstance().GetRow(num);

            //Debug.Log("COUNTS: " + results[num]);
            //for (int i = 0; i < counts; i++)
            //{
            //    Debug.Log(i + "Row HERE  " + results[i]);
            //}
            showarray = ShowArray.Default;
        }
        else if (showarray == ShowArray.YearCol) {
            
            int counts = CSV.GetInstance().GetCol(num).Length;
            results = CSV.GetInstance().GetCol(num);

            int counts_rh = CSV_rh.GetInstance().GetRow(num).Length;
            results_rh = CSV_rh.GetInstance().GetRow(num);

            int counts_tem = CSV_tem.GetInstance().GetRow(num).Length;
            results_tem = CSV_tem.GetInstance().GetRow(num);

            //Debug.Log("COUNTS: " + results[num]);
            //for (int i = 0; i < counts; i++)
            //{
            //    Debug.Log(i + "Col HERE  " + results[i]);
            //}
            showarray = ShowArray.Default;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        switch (showarray)
        {
            case ShowArray.DayRow:
                ShowData(defaultNum);
                break;
            case ShowArray.YearCol:
                ShowData(defaultNum);
                break;
            case ShowArray.Default:
                //print("ShowArray Default");
                break;
        }
    }
}
