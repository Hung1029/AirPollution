using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PS_SunStarManager : MonoBehaviour
{
    public float latitude = 23.4558622f;  // 北回歸線車站
    public float longitude = 120.2555355f;
    public GameObject sunObject;
    public float sunOrbitRadius = 300;
    public GameObject sun_light;
    //public SnowManager snow_system; // 陸地跟極地場景，根據月份設定 snow system 中的相關參數

    public Slider months_slider;
    public Slider hours_slider;

    private Vector3 _sunpos;
    private Quaternion _sunrot;

    private Vector3 _sunlightrot;
    private Light _sunlightComponent;

    public float _fyear = 2017;
    public float _fmonth = 1;
    public float _fday = 2;
    public float _fhour = 0.05f;

    private Color32[] _sunColor = new Color32[20];  // 儲存 0 到 90 度角的太陽顏色
    private Color32 _curSunColor;

    // change shader's properties
    private Renderer rendLow;
    private Renderer rendHigh;

    public bool months_play = false;
    public bool hours_play = false;
    public Button months_playButton;
    public Button hours_playButton;

    // 產生夜空所需要的資料
    public bool North_Pole = false;

    // Use this for initialization
    public void Start()
    {
        months_slider = GameObject.Find("months_slider").GetComponent<Slider>();
        hours_slider = GameObject.Find("hours_slider").GetComponent<Slider>();
        if (!sunObject)
        {
            // sunObject = GameObject.Find("SunObject");
            // 如果還是沒有，後面的程式碼會避開所有對於 sunObject 的計算
        }

        /*if (!snow_system)
        {
            // snow_system = GameObject.Find("Snow System").GetComponent<SnowManager>();
            // 如果還是沒有，後面的程式碼會避開所有對於 snow_system 的參數設定
        }*/

        if (!sun_light) sun_light = GameObject.Find("sun light");
        //Adds a listener to the main slider and invokes a method when the value changes.
        months_slider.onValueChanged.AddListener(delegate { MonthValueChangeCheck(); });
        hours_slider.onValueChanged.AddListener(delegate { HourValueChangeCheck(); });

        // 設定太陽在不同角度上的顏色
        _sunColor[0] = new Color32(19, 24, 98, 80);      // 0 度
        _sunColor[1] = new Color32(255, 137, 14, 80);   // 5 度
        _sunColor[2] = new Color32(255, 214, 181, 80);   // 10 度
        _sunColor[3] = new Color32(255, 226, 202, 80);   // 15 度
        _sunColor[4] = new Color32(255, 232, 213, 80);   // 20 度
        _sunColor[5] = new Color32(255, 235, 218, 80);   // 25 度
        _sunColor[6] = new Color32(255, 238, 223, 80);   // 30 度
        _sunColor[7] = new Color32(255, 240, 226, 80);   // 35 度
        _sunColor[8] = new Color32(255, 241, 229, 80);   // 40 度
        _sunColor[9] = new Color32(255, 242, 230, 80);   // 45 度
        _sunColor[10] = new Color32(255, 243, 231, 80);   // 50 度
        _sunColor[11] = new Color32(255, 243, 232, 80);   // 55 度
        _sunColor[12] = new Color32(255, 244, 234, 80);   // 60 度
        _sunColor[13] = new Color32(255, 245, 236, 80);   // 65 度
        _sunColor[14] = new Color32(255, 246, 237, 80);   // 70 度
        _sunColor[15] = new Color32(255, 247, 239, 80);   // 75 度
        _sunColor[16] = new Color32(255, 248, 241, 80);   // 80 度
        _sunColor[17] = new Color32(255, 249, 243, 80);   // 85 度
        _sunColor[18] = new Color32(255, 251, 245, 80);   // 90 度

        _sunrot.Set(0, 0, 0, 1);
        CalculateSunPosition(_fyear, _fmonth, _fday, _fhour - 8.0f, latitude, longitude, ref _sunpos);
        if (sunObject) sunObject.transform.localPosition = _sunpos;
        sun_light.transform.eulerAngles = new Vector3(_sunlightrot.x, _sunlightrot.y, _sunlightrot.z);

        if (_sunlightrot.x < -20) _curSunColor = _sunColor[0];
        else if (_sunlightrot.x < -4) _curSunColor = Color32.Lerp(_sunColor[0], _sunColor[1], (_sunlightrot.x + 20) / 16);
        else if (_sunlightrot.x < 2) _curSunColor = Color32.Lerp(_sunColor[1], _sunColor[2], (Mathf.Sin((_sunlightrot.x + 4) / 6.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 15) _curSunColor = Color32.Lerp(_sunColor[2], _sunColor[3], (Mathf.Sin((_sunlightrot.x - 2.0f) / 13.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 20) _curSunColor = Color32.Lerp(_sunColor[3], _sunColor[4], (_sunlightrot.x - 15.0f) / 5.0f);
        else _curSunColor = _sunColor[(int)(_sunlightrot.x / 5)];

        rendHigh = GameObject.Find("HighHemiSphere").GetComponent<MeshRenderer>();
        rendHigh.material.SetColor("_CloudColor", _curSunColor);
        rendLow = GameObject.Find("LowHemiSphere").GetComponent<MeshRenderer>();
        rendLow.material.SetColor("_CloudColor", _curSunColor);

        // 改變太陽的顏色
        _sunlightComponent = sun_light.GetComponent<Light>();
        _sunlightComponent.color = _curSunColor;


        // 設定雪地的參數
        // season 
        //    計算方式 1 月到 2 月，從 4.0 ~ 2.0，3 月到 9 月從 0 到 2.35，10 到12月從 2.35 到 4.0
        // snow 
        //    計算方式 1 月到 2 月，從 1.0 ~ 0，3 月到 9 月都是 0，10 到 12月 從 0 到 1.0
        // 
        /*if (!North_Pole)
        {
            if (snow_system)
            {
                if (_fmonth < 3)
                {
                    snow_system.snowValueOld = 1.0f - (_fmonth - 1) / 2.0f - _fday / 60.0f;
                    snow_system.seasonValueOld = 4.0f - (_fmonth - 1) * 2.0f / 2.0f - _fday / 30.0f;
                }
                else if (_fmonth < 10)
                {
                    snow_system.snowValueOld = 0;
                    snow_system.seasonValueOld = (_fmonth - 3) * 2.35f / 7.0f + _fday * 2.35f / 210.0f;
                }
                else
                {
                    snow_system.snowValueOld = (_fmonth - 10.0f) / 3.0f + _fday / 90.0f; ;
                    snow_system.seasonValueOld = 2.35f + (_fmonth - 10.0f) * 1.65f / 3.0f + _fday * 1.65f / 90.0f;
                }
                snow_system.seasonValue = snow_system.seasonValueOld;
                snow_system.snowValue = snow_system.snowValueOld;
                Shader.SetGlobalFloat("_Snow_Amount", Mathf.Clamp(snow_system.snowValue * snow_system.snowGrassValue, 0, snow_system.snowGrassValueMax));
                Shader.SetGlobalFloat("_Season", Mathf.Clamp(snow_system.seasonValue, 0, 4));
                foreach (var item in snow_system.materials)
                {
                    if (item != null)
                    {
                        item.SetFloat("_Snow_Amount", snow_system.snowValue);
                        item.SetFloat("_Season", snow_system.seasonValue);
                    }
                }
                if (snow_system.snowValue > snow_system.particleTurnOn)
                {
                    foreach (var item in snow_system.particles)
                    {
                        if (item != null && !item.gameObject.activeSelf)
                            item.gameObject.SetActive(true);
                    }
                }
                else
                {
                    foreach (var item in snow_system.particles)
                    {
                        if (item != null && item.gameObject.activeSelf)
                            item.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            snow_system.snowValue = snow_system.snowValueOld = 2.0f;
            snow_system.seasonValue = snow_system.seasonValueOld = 4.0f;
            Shader.SetGlobalFloat("_Snow_Amount", Mathf.Clamp(snow_system.snowValue * snow_system.snowGrassValue, 0, snow_system.snowGrassValueMax));
            Shader.SetGlobalFloat("_Season", Mathf.Clamp(snow_system.seasonValue, 0, 4));
            foreach (var item in snow_system.materials)
            {
                if (item != null)
                {
                    item.SetFloat("_Snow_Amount", snow_system.snowValue);
                    item.SetFloat("_Season", snow_system.seasonValue);
                }
            }
            if (snow_system.snowValue > snow_system.particleTurnOn)
            {
                foreach (var item in snow_system.particles)
                {
                    if (item != null && !item.gameObject.activeSelf)
                        item.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var item in snow_system.particles)
                {
                    if (item != null && item.gameObject.activeSelf)
                        item.gameObject.SetActive(false);
                }
            }
        }*/

    }

    // Invoked when the value of the slider changes.
    public void MonthValueChangeCheck()
    {
        _fmonth = (int)months_slider.value / 14 + 1;
        _fday = ((int)(months_slider.value - (_fmonth - 1) * 14) + 1) * 2;
        CalculateSunPosition(_fyear, _fmonth, _fday, _fhour - 8.0f, latitude, longitude, ref _sunpos);
        if (sunObject) sunObject.transform.localPosition = _sunpos;
        sun_light.transform.Rotate(_sunlightrot);
        sun_light.transform.eulerAngles = new Vector3(_sunlightrot.x, _sunlightrot.y, _sunlightrot.z);

        if (_sunlightrot.x < -20) _curSunColor = _sunColor[0];
        else if (_sunlightrot.x < -4) _curSunColor = Color32.Lerp(_sunColor[0], _sunColor[1], (_sunlightrot.x + 20) / 16);
        else if (_sunlightrot.x < 2) _curSunColor = Color32.Lerp(_sunColor[1], _sunColor[2], (Mathf.Sin((_sunlightrot.x + 4) / 6.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 15) _curSunColor = Color32.Lerp(_sunColor[2], _sunColor[3], (Mathf.Sin((_sunlightrot.x - 2.0f) / 13.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 20) _curSunColor = Color32.Lerp(_sunColor[3], _sunColor[4], (_sunlightrot.x - 15.0f) / 5.0f);
        else _curSunColor = _sunColor[(int)(_sunlightrot.x / 5)];

        // 設定兩層雲的顏色
        if (rendHigh) rendHigh.material.SetColor("_CloudColor", _curSunColor);
        if (rendLow) rendLow.material.SetColor("_CloudColor", _curSunColor);

        // 設定太陽的顏色
        if (_sunlightComponent) _sunlightComponent.color = _curSunColor;

        // 設定雪地的參數
        /*if (snow_system && !North_Pole)
        {
            if (_fmonth < 3)
            {
                snow_system.snowValueOld = 1.0f - (_fmonth - 1) / 2.0f - _fday / 60.0f;
                snow_system.seasonValueOld = 4.0f - (_fmonth - 1) * 2.0f / 2.0f - _fday / 30.0f;
            }
            else if (_fmonth < 10)
            {
                snow_system.snowValueOld = 0;
                snow_system.seasonValueOld = (_fmonth - 3) * 2.35f / 7.0f + _fday * 2.35f / 210.0f;
            }
            else
            {
                snow_system.snowValueOld = (_fmonth - 10.0f) / 3.0f + _fday / 90.0f; ;
                snow_system.seasonValueOld = 2.35f + (_fmonth - 10.0f) * 1.65f / 3.0f + _fday * 1.65f / 90.0f;
            }
            snow_system.seasonValue = snow_system.seasonValueOld;
            snow_system.snowValue = snow_system.snowValueOld;
            Shader.SetGlobalFloat("_Snow_Amount", Mathf.Clamp(snow_system.snowValue * snow_system.snowGrassValue, 0, snow_system.snowGrassValueMax));
            Shader.SetGlobalFloat("_Season", Mathf.Clamp(snow_system.seasonValue, 0, 4));
            foreach (var item in snow_system.materials)
            {
                if (item != null)
                {
                    item.SetFloat("_Snow_Amount", snow_system.snowValue);
                    item.SetFloat("_Season", snow_system.seasonValue);
                }
            }
            if (snow_system.snowValue > snow_system.particleTurnOn)
            {
                foreach (var item in snow_system.particles)
                {
                    if (item != null && !item.gameObject.activeSelf)
                        item.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var item in snow_system.particles)
                {
                    if (item != null && item.gameObject.activeSelf)
                        item.gameObject.SetActive(false);
                }
            }
        }*/
    }
    public void HourValueChangeCheck()
    {
        _fhour = hours_slider.value;
        CalculateSunPosition(_fyear, _fmonth, _fday, _fhour - 8.0f, latitude, longitude, ref _sunpos);
        if (sunObject) sunObject.transform.localPosition = _sunpos;
        sun_light.transform.eulerAngles = new Vector3(_sunlightrot.x, _sunlightrot.y, _sunlightrot.z);

        if (_sunlightrot.x < -20) _curSunColor = _sunColor[0];
        else if (_sunlightrot.x < -4) _curSunColor = Color32.Lerp(_sunColor[0], _sunColor[1], (_sunlightrot.x + 20) / 16);
        else if (_sunlightrot.x < 2) _curSunColor = Color32.Lerp(_sunColor[1], _sunColor[2], (Mathf.Sin((_sunlightrot.x + 4) / 6.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 15) _curSunColor = Color32.Lerp(_sunColor[2], _sunColor[3], (Mathf.Sin((_sunlightrot.x - 2.0f) / 13.0f) * Mathf.PI * 0.5f));
        else if (_sunlightrot.x < 20) _curSunColor = Color32.Lerp(_sunColor[3], _sunColor[4], (_sunlightrot.x - 15.0f) / 5.0f);
        else _curSunColor = _sunColor[(int)(_sunlightrot.x / 5)];

        // 設定兩層雲的顏色
        if (rendHigh) rendHigh.material.SetColor("_CloudColor", _curSunColor);
        if (rendLow) rendLow.material.SetColor("_CloudColor", _curSunColor);

        // 設定太陽的顏色
        if (_sunlightComponent) _sunlightComponent.color = _curSunColor;
    }

    public double CorrectAngle(double angleInRadians)
    {
        if (angleInRadians < 0)
        {
            double d = (-angleInRadians) / (2 * Mathf.PI);
            int r = (int)(d);
            double t = (d - r) * (2 * Mathf.PI);
            return 2 * Mathf.PI - t;
        }
        else if (angleInRadians > 2 * Mathf.PI)
        {
            double d = angleInRadians / (2 * Mathf.PI);
            int r = (int)(d);
            double t = (d - r) * (2 * Mathf.PI);
            return t;
        }
        else
        {
            return angleInRadians;
        }
    }

    //
    // 給予時間跟所在的經緯度，算出目前太陽在天空的位置
    // 回傳的 sunpos 就是 x,y,z 座標
    // 預設的年是 2017 年 1 月 1 日 0 時
    // 
    public void CalculateSunPosition(float fyear, float fmonth, float fday, float fhour, double latitude, double longitude, ref Vector3 sunpt)
    {
        double Deg2Rad = Mathf.PI / 180.0;
        double Rad2Deg = 180.0 / Mathf.PI;

        // Number of days from J2000.0.  
        double julianDate = 367 * fyear -
            (int)((7.0 / 4.0) * (fyear + (int)((fmonth + 9.0) / 12.0))) +
            (int)((275.0 * fmonth) / 9.0) + fday - 730531.5;

        double julianCenturies = julianDate / 36525.0;

        // Sidereal Time  
        double siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

        double siderealTimeUT = siderealTimeHours + (366.2422 * (double)fhour / 365.2422);

        double siderealTime = siderealTimeUT * 15 + longitude;

        // Refine to number of days (fractional) to specific time.  
        julianDate += (double)fhour / 24.0;
        julianCenturies = julianDate / 36525.0;

        // Solar Coordinates  
        double meanLongitude = CorrectAngle(Deg2Rad * (280.466 + 36000.77 * julianCenturies));

        double meanAnomaly = CorrectAngle(Deg2Rad * (357.529 + 35999.05 * julianCenturies));

        double equationOfCenter = Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
            Mathf.Sin((float)meanAnomaly) + 0.02 * Mathf.Sin((float)(2 * meanAnomaly)));

        double elipticalLongitude = CorrectAngle(meanLongitude + equationOfCenter);
        double obliquity = (23.439 - 0.013 * julianCenturies) * Deg2Rad;

        // Right Ascension  
        double rightAscension = Mathf.Atan2(
            Mathf.Cos((float)obliquity) * Mathf.Sin((float)elipticalLongitude),
            Mathf.Cos((float)elipticalLongitude));
        double declination = Mathf.Asin(
            Mathf.Sin((float)rightAscension) * Mathf.Sin((float)obliquity));

        // Horizontal Coordinates  
        double hourAngle = CorrectAngle(siderealTime * Deg2Rad) - rightAscension;

        if (hourAngle > Mathf.PI)
        {
            hourAngle -= 2 * Mathf.PI;
        }

        double altitude = Mathf.Asin(Mathf.Sin((float)(latitude * Deg2Rad)) *
            Mathf.Sin((float)declination) + Mathf.Cos((float)(latitude * Deg2Rad)) *
            Mathf.Cos((float)declination) * Mathf.Cos((float)hourAngle));

        // Nominator and denominator for calculating Azimuth  
        // angle. Needed to test which quadrant the angle is in.  
        double aziNom = -Mathf.Sin((float)hourAngle);
        double aziDenom =
            Mathf.Tan((float)declination) * Mathf.Cos((float)(latitude * Deg2Rad)) -
            Mathf.Sin((float)(latitude * Deg2Rad)) * Mathf.Cos((float)hourAngle);

        double azimuth = Mathf.Atan((float)(aziNom / aziDenom));

        if (aziDenom < 0) // In 2nd or 3rd quadrant  
        {
            azimuth += Mathf.PI;
        }
        else if (aziNom < 0) // In 4th quadrant  
        {
            azimuth += 2 * Mathf.PI;
        }

        // 轉換成 sun light 的旋轉角度
        _sunlightrot.Set((float)(altitude * Rad2Deg), (float)((azimuth - Mathf.PI * 0.5) * Rad2Deg), 0);

        // 轉換成座標
        double azim = 180 - azimuth * Rad2Deg;
        if (azim < 0) azim += 360;
        azim = azim * Deg2Rad;
        double zeni = (90.0 - altitude * Rad2Deg) * Deg2Rad;

        //  _sunlightrot.Set((float)(zeni*Rad2Deg), (float)(azim*Rad2Deg), 0);

        sunpt.z = -sunOrbitRadius * Mathf.Sin((float)zeni) * Mathf.Sin((float)azim);
        sunpt.y = sunOrbitRadius * Mathf.Cos((float)zeni);
        sunpt.x = -sunOrbitRadius * Mathf.Sin((float)zeni) * Mathf.Cos((float)azim);
    }

    // Update is called once per frame
    void Update()
    {
        AutoPlay();
    }

    void AutoPlay()
    {
        if (months_play)
        {
            months_slider.value += Time.deltaTime * 10;
            if (months_slider.value >= 167) months_slider.value = 0f;
            if (hours_playButton) hours_playButton.interactable = false;
        }
        else
        {
            if (hours_playButton) hours_playButton.interactable = true;
        }

        if (hours_play)
        {
            hours_slider.value += Time.deltaTime * 1f;
            if (hours_slider.value >= 23.8) hours_slider.value = 0.05f;
            if (months_playButton) months_playButton.interactable = false;
        }
        else
        {
            if (months_playButton) months_playButton.interactable = true;
        }
    }

    public void MonthsPlay()
    {
        months_play = !months_play;
        hours_slider.interactable = !hours_slider.interactable;

    }
    public void HoursPlay()
    {
        hours_play = !hours_play;
        months_slider.interactable = !months_slider.interactable;
    }
}