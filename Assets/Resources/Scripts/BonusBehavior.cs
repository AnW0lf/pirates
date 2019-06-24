using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBehavior : MonoBehaviour
{
    public bool bonusMoney, bonusMaterial, bonusSpeed, bonusWheel;
    public int modifier;

    public string textOnClick;
    public GameObject textOnClickObject;

    private GameObject _textOnClick;

    //private void OnMouseUp()
    //{
    //    _textOnClick = Instantiate(textOnClickObject, gameObject.transform);
    //    _textOnClick.GetComponent<Text>().text = textOnClick;
    //    _textOnClick.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    //}

}
