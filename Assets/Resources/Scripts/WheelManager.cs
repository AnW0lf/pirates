using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    public Text wheelCounter;

    public static int wheelTokens;

    // Start is called before the first frame update
    void Start()
    {
        wheelTokens = 0;
    }

    // Update is called once per frame
    void Update()
    {
        wheelCounter.text = "Lifebuoys: " + wheelTokens + "/" + PlayerPrefs.GetFloat("GlobalSpins");
    }
}
