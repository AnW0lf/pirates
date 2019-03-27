using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingWheelReward : MonoBehaviour
{
    public Text text;
    public Image image;

    private void Update()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
