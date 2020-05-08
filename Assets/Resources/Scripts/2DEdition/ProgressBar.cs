using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public bool Visible
    {
        get => _visible; set
        {
            if (value != _visible)
            {
                _visible = value;
                if (_visible)
                {
                    progress.gameObject.LeanAlpha(1f, showDuration);
                    label.gameObject.LeanAlpha(1f, showDuration);
                    icon.gameObject.LeanAlpha(1f, showDuration);
                    background.gameObject.LeanAlpha(1f, showDuration);
                }
                else
                {
                    progress.gameObject.LeanAlpha(0f, hideDuration);
                    label.gameObject.LeanAlpha(0f, hideDuration);
                    icon.gameObject.LeanAlpha(0f, hideDuration);
                    background.gameObject.LeanAlpha(0f, hideDuration);
                }
            }

        }
    }

    [SerializeField]
    private bool _visible = true;
    [SerializeField]
    private SpriteRenderer icon = null, background = null;
    [SerializeField]
    private Transform progress = null;
    [SerializeField]
    private TextMesh label = null;
    [SerializeField]
    private float showDuration = 0.5f, hideDuration = 0.5f;

    public float Progress { get => progress.localScale.x; set => progress.localScale = new Vector3(Mathf.Clamp01(value), progress.localScale.y, progress.localScale.z); }

    public string Label { get => label.text; set => label.text = value; }

    private void Start()
    {
        if (_visible)
        {
            progress.gameObject.LeanAlpha(1f, 0f);
            label.gameObject.LeanAlpha(1f, 0f);
            icon.gameObject.LeanAlpha(1f, 0f);
            background.gameObject.LeanAlpha(1f, 0f);
        }
        else
        {
            progress.gameObject.LeanAlpha(0f, 0f);
            label.gameObject.LeanAlpha(0f, 0f);
            icon.gameObject.LeanAlpha(0f, 0f);
            background.gameObject.LeanAlpha(0f, 0f);
        }
    }
}
