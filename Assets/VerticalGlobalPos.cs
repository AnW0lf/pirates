using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalGlobalPos : MonoBehaviour
{

    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //if (gameObject.transform.parent.transform.parent.transform.eulerAngles.z > 100f)
        //{
        //    gameObject.transform.eulerAngles += new Vector3(0f, 0f, 180f);
        //}
    }
}
