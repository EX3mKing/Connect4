using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    //public GameObject one;
    //public GameObject two;
    //public float speed;

    //private float xtarget;
    
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;
    private MeshRenderer rend;
    
    private void Start()
    {
        //xtarget = -Mathf.Abs(one.transform.localScale.x * 10);
        rend = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        /*
        one.transform.position += Vector3.left * (speed * Time.deltaTime);
        two.transform.position += Vector3.left * (speed * Time.deltaTime);
        
        if (one.transform.position.x <= xtarget)
        {
            one.transform.position = new Vector3(two.transform.position.x + two.transform.localScale.x * 10f, one.transform.position.y, one.transform.position.z);
        }
        
        if (two.transform.position.x <= xtarget)
        {
            two.transform.position = new Vector3(one.transform.position.x + one.transform.localScale.x * 10f, two.transform.position.y, two.transform.position.z);
        }*/

        rend.material.mainTextureOffset += new Vector2(Time.deltaTime * scrollSpeedX, Time.deltaTime * scrollSpeedY);
    }
}
