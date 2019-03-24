using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    public Text wheelCounter;
    public float posOn, posOff;
    public static int wheelTokens;

    public bool IsOpen { get; private set; }
    private Transform parent;

    private void Awake()
    {
        parent = transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        wheelTokens = 0;
        IsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        wheelCounter.text = "Lifebuoys: " + wheelTokens + "/" + PlayerPrefs.GetFloat("GlobalSpins");
        //parent.Translate(Vector3.forward * (parent.position.y - (IsOpen ? posOn: posOff)) * Time.deltaTime);
    }

    private void OnMouseUpAsButton()
    {
        IsOpen = !IsOpen;
    }

    private void OnMouseDrag()
    {
        parent.position = Input.mousePosition;
    }
}
