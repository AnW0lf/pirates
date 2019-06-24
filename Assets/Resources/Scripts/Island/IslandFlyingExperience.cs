using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandFlyingExperience : MonoBehaviour
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
        count.text = "+" + value.ToString();
        anim.SetTrigger("Fly");
    }
}
