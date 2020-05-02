using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public bool Visible { get => _visible; set
        {
            _visible = value;
            progressBar.SetActive(_visible);
        }
    }

    [SerializeField]
    private bool _visible;
    [SerializeField]
    private GameObject progressBar;
    [SerializeField]
    private Image progress;
    [SerializeField]
    private Text label;

    public float Progress { get => progress.fillAmount; set => progress.fillAmount = value; }

    public string Label { get => label.text; set => label.text = value; }
}
