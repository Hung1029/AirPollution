using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 LookAt;
    public float speed;
   // public Vector3 Center;

    private void Awake()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //taipei lookat (0,45,0) , speed 10f
        transform.LookAt(LookAt);
        transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
    }
}
