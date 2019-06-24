using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandFlyingCoin : MonoBehaviour
{
    [SerializeField]
    private Text count;
    private Animator anim;
    private bool hasText = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (count != null || count == null && transform.childCount > 0 && (count = transform.GetChild(0).GetComponent<Text>()) != null)
            hasText = true;
    }

    public void Fly(BigDigit value)
    {
        if (!hasText) return;
        count.text = value.ToString();
        if (value > 999)
        {
            transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
        }
        else if (value > 99)
        {
            transform.localPosition = new Vector3(-25f, transform.localPosition.y, transform.localPosition.z);
        }
        else if (value > 9)
        {
            transform.localPosition = new Vector3(-50f, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(-70f, transform.localPosition.y, transform.localPosition.z);
        }
        anim.SetTrigger("Fly");
    }
}
