using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    public bool Visible
    {
        get => _visible;
        set
        {
            if(_visible != value)
            {
                _visible = value;
                if (_visible)
                {
                    text.gameObject.LeanAlpha(1f, showDuration);
                    icon.gameObject.LeanAlpha(1f, showDuration);
                }
                else
                {
                    text.gameObject.LeanAlpha(0f, hideDuration);
                    icon.gameObject.LeanAlpha(0f, hideDuration);
                }
            }
        }
    }

    [SerializeField]
    private bool _visible = true;
    [SerializeField]
    private float showDuration = 0.5f, hideDuration = 0.5f;
    [SerializeField]
    private TextMesh text;
    [SerializeField]
    private SpriteRenderer icon;
    [SerializeField]
    private LeanTweenType ease = LeanTweenType.linear;

    private void Start()
    {
        if (_visible)
        {
            text.gameObject.LeanAlpha(1f, 0f);
            icon.gameObject.LeanAlpha(1f, 0f);
        }
        else
        {
            text.gameObject.LeanAlpha(0f, 0f);
            icon.gameObject.LeanAlpha(0f, 0f);
        }
    }

    public void Fly(Vector3 startPos, Vector3 endPos, float duration)
    {
        transform.position = startPos;
        Visible = true;
        gameObject.LeanMove(endPos, Mathf.Max(0f, duration)).setEase(ease);
        LeanTween.delayedCall(Mathf.Max(0f, duration - hideDuration), () => Visible = false);
    }

    public void Fly(Vector3 startPos, Vector3 endPos, float duration, LeanTweenType ease)
    {
        transform.position = startPos;
        Visible = true;
        gameObject.LeanMove(endPos, Mathf.Max(0f, duration)).setEase(ease);
        LeanTween.delayedCall(Mathf.Max(0f, duration - hideDuration), () => Visible = false);
    }
}
