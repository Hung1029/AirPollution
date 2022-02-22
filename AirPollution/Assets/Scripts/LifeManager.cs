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
    public float floatColorChangeSpeed=2.0f;
    public Color defaultColor;
    static LifeManager lifemanager;
    // Start is called before the first frame update
    void Start()
    {
       
        mesh=target.GetComponent<MeshRenderer>();
        defaultColor = target.GetComponent<MeshRenderer>().material.color;
      
        //mesh = target.GetComponent<MeshRenderer>();
        //defaultColor = target.GetComponent<MeshRenderer>().material.color;
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
        //Debug.Log(PS_SunStarManager.nowState);

        if (lifestate < 25) {
            lifetype = LifeType.Fadein;

        }
        else  {
            lifetype = LifeType.Fadeout;
        }

        switch (lifetype)
        {
            case LifeType.Fadein:
                print("Hello");
                StartFadein();
                break;
            case LifeType.Fadeout:
                print("Bye");
                StartFadeout();
                break;
            default:
                print("Nothing");
                break;
        }
    }

    public void StartFadein() {
     
            mesh.material.color = Color.Lerp(Color.clear,defaultColor , 1.0f);
            // _rawImage.color = Color.Lerp(_rawImage.color, Color.clear, floatColorChangeSpeed * Time.deltaTime);
        
     }

    public void StartFadeout()
    {
            mesh.material.color = Color.Lerp(defaultColor,Color.clear , 1.0f);
        
    }
}
