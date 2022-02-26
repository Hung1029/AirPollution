using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeBtnImage : MonoBehaviour {

    public Image origineBtn_Img;
    public Image[] imageChanging;
    public bool onPressed;


	void Start () {
        //origineBtn_Img = GetComponent<Image>();
        onPressed = false;
	}
	
	void Update () {
        if (onPressed) origineBtn_Img.sprite = imageChanging[1].sprite; //1:pause
        else origineBtn_Img.sprite = imageChanging[0].sprite; //0:play
    }

    public void onButtonChangeImg()
    {
        onPressed = !onPressed;
    }

    

}
