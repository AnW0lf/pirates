using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialAnim : MonoBehaviour
{
    public string planeName;

    private void OnEnable()
    {
        EventManager.Subscribe(planeName + "AddMaterial", Anim);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(planeName + "AddMaterial", Anim);
    }

    private void Anim(object[] arg0)
    {
        if (arg0.Length > 0)
        {
            Transform child = transform.GetChild(0);
            if (!child.gameObject.activeInHierarchy)
                child.gameObject.SetActive(true);
            child.SetAsLastSibling();
            child.GetComponent<Text>().text = "+" + CheckRange((int)arg0[0]);
            child.GetComponent<Animation>().Stop();
            child.GetComponent<Animation>().Play();
        }
    }

    private string CheckRange(int v)
    {
        float money = v;
        int degree = 0;
        while (money > 1000)
        {
            degree++;
            money /= 1000;
        }
        string str = money.ToString();
        str = str.Length >= 4 ? str.Substring(0, 4) : str;
        switch (degree)
        {
            case 0:
                return str;
            case 1:
                return str + "K";
            case 2:
                return str + "M";
            case 3:
                return str + "B";
            case 4:
                return str + "T";
            case 5:
                return str + "Q";
            default:
                return str + "A";
        }
    }
}
