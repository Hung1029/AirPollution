using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FileController : MonoBehaviour
{
    public List <int[]> Pm25Array;
    // Start is called before the first frame update
    void Start()
    {
        CSV.GetInstance().loadFile(Application.dataPath + "/Csv", "taipei_pm25.csv");
        //Debug.Log("getInt: " + CSV.GetInstance().getInt(5,5));
        
        //for (int i = 1; i < 13; i++) {
        //    for (int j = 1; j < 366; j++) {
        //       // Pm25Array[i][j] = CSV.GetInstance().getInt(i+2, j);
        //      //  Debug.Log("getInt: " + CSV.GetInstance().getInt(2, 2));
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

}
}
