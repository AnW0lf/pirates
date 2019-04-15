using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOnClickBehavior : MonoBehaviour
{
    private void Update()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
