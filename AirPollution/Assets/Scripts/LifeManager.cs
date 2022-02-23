using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public enum LifeType
    {
       Fadeout, Fadein, Default
    }
    public float lifestate;
    public LifeType lifetype;
    public GameObject target;
    private MeshRenderer mesh;
    public float floatColorChangeSpeed=0.5f;
    public Color defaultColor;
    static LifeManager lifemanager;
    
    // Start is called before the first frame update
    void Start()
    {
       
        mesh=target.GetComponent<MeshRenderer>();       
        defaultColor = target.GetComponent<MeshRenderer>().material.color;
    }

  
    public static LifeManager GetInstance()
    {
        if (lifemanager == null)
        {
            lifemanager = new LifeManager();
        }
        return lifemanager;
    }
    // Update is called once per frame
    void Update()
    {
        lifestate = PS_SunStarManager.nowState;

        if (lifestate < 30) {
            lifetype = LifeType.Fadein;

        }
        else  {
            lifetype = LifeType.Fadeout;
        }

        switch (lifetype)
        {
            case LifeType.Fadein:
                   
                    StartFadein();
                break;
            case LifeType.Fadeout:
                    
                    StartFadeout();
                break;
            default:
                   
                break;
        }
    }

    public void StartFadein() {

        //if (target.activeInHierarchy) { 
        //    target.SetActive(true);
        //    Debug.Log("HELLOOOO!");
        //}

        //for (float t = 0f; t < 1.0f; t += Time.deltaTime)
        //{
        //     mesh.material.color = Color.Lerp(defaultColor, Color.clear, 1.0f * Time.deltaTime);
            
        //}
        mesh.enabled = true; 
        // _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, floatColorChangeSpeed * Time.deltaTime);   
    }

    public void StartFadeout()
    {
        //for (float t = 0f; t < 1.0f; t += Time.deltaTime)
        //{
        //    mesh.material.color = Color.Lerp(Color.clear, defaultColor, 1.0f * Time.deltaTime);
        //}
         mesh.enabled = false; 
        
        //target.SetActive(false);
        
    }
}
