using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class Example_FilePath : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		string Platform = "";
#if UNITY_EDITOR
		Platform = "EDITOR";
#elif UNITY_IPHONE
		Platform = "IPHONE";
#elif UNITY_ANDROID
		Platform = "ANDROID";
#endif
		Debug.Log(Platform);
		Debug.Log("dataPath: " + Application.dataPath);
		Debug.Log("persistentDataPath: " + Application.persistentDataPath);
		Debug.Log("streamingAssetsPath: " + Application.streamingAssetsPath);
		Debug.Log("temporaryCachePath: " + Application.temporaryCachePath);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
