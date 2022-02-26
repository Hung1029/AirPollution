using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using LitJson;

public class GetKaohsiungData : MonoBehaviour
{
    public static GetKaohsiungData Instance { get; private set; }
    public enum  DataType { 
        NowData,HistoryData,Default
    }
    public DataType datatype;

    
    public TextMesh outputArea;
    public TextMesh outputTem;
    public TextMesh outputRain;
    public TextMesh outputDate;

    //List<string> inside = new List<string> { "新北市", "臺北市", "桃園市", "新竹市", "臺中市", "臺南市", "高雄市", "花蓮縣" };
    //public Dropdown dropdownCounty;
    string uri_data = "https://data.epa.gov.tw/api/v1/aqx_p_488?format=json&limit={0}&api_key={1}&filters=County,EQ,{2}|SiteName,EQ,{3}|Pollutant,EQ,細懸浮微粒";
    string limit = "1";
    string api = "9be7b239-557b-4c10-9775-78cadfc555e9";
    public string county = "高雄市";
    public string sitename = "前金";
    int i = 0;

    string uri_rain = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-D355501B-3D14-481D-AAAD-90B786822049&format=JSON&locationName=%E9%AB%98%E9%9B%84%E5%B8%82&elementName=PoP&sort=time";
    string uri_tem = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-D355501B-3D14-481D-AAAD-90B786822049&format=JSON&locationName=%E9%AB%98%E9%9B%84%E5%B8%82&elementName=MinT,MaxT&sort=time";

    [Serializable]
    public class DATA
    {
        public string SiteName; //測站名稱
        public string County; //縣市
        public string AQI; //空氣品質指標
        public string Status; //狀態
        public string PM2_5; //細懸浮微粒(μg/m3)
        public string PM2_5_AVG; //細懸浮微粒移動平均值(μg/m3)

        public string DataCreationDate; //資料發布時間
        public string Longitude; //經度
        public string Latitude; //緯度
    }
    public class RAIN_DATA
    {
        public string Rain;
    }
    public class TEM_DATA
    {
        public string Max_Tem;
        public string Min_Tem;
    }
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        datatype = DataType.Default;
        outputArea = GameObject.Find("AirDataPm").GetComponent<TextMesh>();
        outputTem = GameObject.Find("AirDataTem").GetComponent<TextMesh>();
        outputRain = GameObject.Find("AirDataWet").GetComponent<TextMesh>();
        outputDate = GameObject.Find("AirDataTime").GetComponent<TextMesh>();
    }

    void Update()
    {
        switch (datatype) 
        {
            case DataType.NowData:
                print("NowType");
                StartGetNowData();
                break;

            case DataType.HistoryData:
                print("HistoryType"); 
                break;

            case DataType.Default:
                print("DataType Default");
                break;
        }
    }

      public void StartGetNowData() {
        StartCoroutine(GetData_Coroutine());
        StartCoroutine(GetWea_Coroutine());
        StartCoroutine(GetTem_Coroutine());
        StartCoroutine(GetNowTime_Coroutine());
        datatype = DataType.Default;
    }

    // void GetData() => StartCoroutine(GetData_Coroutine());

    IEnumerator GetData_Coroutine()
    {
        //outputArea.text = "Loading...";
        string uri = String.Format(uri_data, limit, api, county, sitename);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                outputArea.text = request.error;
            else
            {
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                if (jsonData["records"].Count == 0)
                {
                    outputArea.text = "Sorry, there is no result for your request!";
                }
                else
                {
                    List<DATA> json = new List<DATA>();
                    json.Clear();
                    Debug.Log("Clear");

                    for (i = 0; i < 1; i++)
                    {
                        json.Add(new DATA()
                        {
                            SiteName = jsonData["records"][i]["SiteName"].ToString(),
                            County = jsonData["records"][i]["County"].ToString(),
                            Status = jsonData["records"][i]["Status"].ToString(),
                            AQI = jsonData["records"][i]["AQI"].ToString(),
                            PM2_5 = jsonData["records"][i]["PM2.5"].ToString(),
                            PM2_5_AVG = jsonData["records"][i]["PM2.5_AVG"].ToString(),
                            DataCreationDate = jsonData["records"][i]["DataCreationDate"].ToString(),
                            Longitude = jsonData["records"][i]["Longitude"].ToString(),
                            Latitude = jsonData["records"][i]["Latitude"].ToString()
                        });
                    }


                    Debug.Log(json[0].SiteName);
                    outputArea.text = //"測站名稱: " + json[0].SiteName + Environment.NewLine +
                    //    "縣市: " + json[0].County + Environment.NewLine +
                    //    "狀態: " + json[0].Status + Environment.NewLine +
                    //    "空氣品質指標: " + json[0].AQI + Environment.NewLine +
                    //    "細懸浮微粒(μg/m3): " + json[0].PM2_5 + Environment.NewLine +
                    json[0].PM2_5_AVG;
                    //"緯度: " + json[0].Longitude + Environment.NewLine +
                    //"經度: " + json[0].Latitude + Environment.NewLine +
                    //"資料發布時間: " + json[0].DataCreationDate;
                }
            }
        }
    }

    IEnumerator GetWea_Coroutine()
    {
       // outputRain.text = "Loading...";

        using (UnityWebRequest request = UnityWebRequest.Get(uri_rain))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                outputArea.text = request.error;
            else
            {
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                if (jsonData["records"].Count == 0)
                {
                    outputRain.text = "empty";
                }
                else
                {
                    List<RAIN_DATA> json_data = new List<RAIN_DATA>();
                    json_data.Clear();
                    Debug.Log("Clear");

                    json_data.Add(new RAIN_DATA()
                    {
                        Rain = jsonData["records"]["location"][0]["weatherElement"][0]["time"][0]["parameter"]["parameterName"].ToString(),
                    });

                    Debug.Log("降雨機率:" + json_data[0].Rain);
                    outputRain.text = json_data[0].Rain + "%";
                }
            }
        }
    }

    IEnumerator GetTem_Coroutine()
    {
       // outputTem.text = "Loading...";

        using (UnityWebRequest request = UnityWebRequest.Get(uri_tem))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                outputArea.text = request.error;
            else
            {
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                if (jsonData["records"].Count == 0)
                {
                    outputTem.text = "empty";
                }
                else
                {
                    List<TEM_DATA> json_data = new List<TEM_DATA>();
                    json_data.Clear();
                    Debug.Log("Clear");
                    json_data.Add(new TEM_DATA()
                    {
                        Max_Tem = jsonData["records"]["location"][0]["weatherElement"][1]["time"][0]["parameter"]["parameterName"].ToString(),
                        Min_Tem = jsonData["records"]["location"][0]["weatherElement"][0]["time"][0]["parameter"]["parameterName"].ToString()
                    });
                    Debug.Log("溫度:" + json_data[0].Max_Tem + json_data[0].Min_Tem);
                    var average = (int.Parse(json_data[0].Max_Tem) + int.Parse(json_data[0].Min_Tem)) / 2;
                    outputTem.text = average.ToString() + "℃";
                }
            }
        }
    }

    IEnumerator GetNowTime_Coroutine()
    {
        outputDate.text = DateTime.Now.Month.ToString() +"/"+ DateTime.Now.Day.ToString()+"/"+ DateTime.Now.Year.ToString();
        //_fhour.value = DateTime.Now.Hour;

        yield return null;
    }
}
