using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public string prefix = "", text = "", postfix = "";

    private string oldPrefix = "", oldText = "", oldPostfix = "", content;
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        UpdateText();
    }

    private void Update()
    {
        if(!oldPrefix.Equals(prefix) || !oldText.Equals(text) || !oldPostfix.Equals(postfix))
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        content = prefix + text + postfix;
        oldPrefix = prefix;
        oldPostfix = postfix;
        oldText = text;
        _text.text = content;
    }
}
