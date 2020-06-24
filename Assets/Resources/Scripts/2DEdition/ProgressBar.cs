using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            if (!gameObject.activeSelf) return;
            if (coroutine != null) StopCoroutine(coroutine);
            if (_visible) coroutine = StartCoroutine(SetAlpha(1f, showDuration));
            else coroutine = StartCoroutine(SetAlpha(0f, hideDuration));
        }
    }

    public bool Force
    {
        get => _force;
        set
        {
            _force = value;

            if (_force) anim.Stop();
            else anim.Play();

            foreach (ImageAlpha image in images)
            {
                float alpha = image.Alpha;
                image.forced = _force;
                image.Alpha = alpha;
                image.image.gameObject.SetActive(_force ? image.enabledOnForsed : image.enabledOnNotForsed);
            }
            foreach (TextAlpha text in texts)
            {
                float alpha = text.Alpha;
                text.forced = _force;
                text.Alpha = alpha;
                text.text.gameObject.SetActive(_force ? text.enabledOnForsed : text.enabledOnNotForsed);
            }
        }
    }

    [SerializeField]
    private bool _visible = true;
    [SerializeField]
    private float showDuration = 0.3f, hideDuration = 0.3f;
    [SerializeField]
    private Image progress = null;
    [SerializeField]
    private Image progressForced = null;
    [SerializeField]
    private Animation anim = null;
    [SerializeField]
    private Text label = null, timer = null;
    [SerializeField]
    private ImageAlpha[] images;
    [SerializeField]
    private TextAlpha[] texts;

    private Coroutine coroutine = null;
    private bool _force = false;

    private void Start()
    {
        foreach (ImageAlpha image in images)
        {
            image.Alpha = 0f;
            image.forced = false;
            image.image.gameObject.SetActive(image.enabledOnNotForsed);
        }
        foreach (TextAlpha text in texts)
        {
            text.Alpha = 0f;
            text.forced = false;
            text.text.gameObject.SetActive(text.enabledOnNotForsed);
        }

        if (!Force) anim.Play();
    }

    private void OnEnable()
    {
        Visible = _visible;
    }

    public float Progress
    {
        get => progress.fillAmount;
        set
        {
            progress.fillAmount = value;
            progressForced.fillAmount = value;
        }
    }

    public string Label { get => label.text; set => label.text = value; }
    public int Timer
    {
        set
        {
            string min = (value / 60).ToString(), sec = (value % 60).ToString();
            if (sec.Length == 1) sec = "0" + sec;
            timer.text = string.Format("{0}:{1}", min, sec);
        }
    }

    private IEnumerator SetAlpha(float alpha, float duration)
    {
        if (images.Length + texts.Length == 0)
        {
            coroutine = null;
            yield break;
        }

        float alphaAverage = 0f;
        foreach (ImageAlpha image in images) alphaAverage += image.Alpha;
        foreach (TextAlpha text in texts) alphaAverage += text.Alpha;
        alphaAverage /= images.Length + texts.Length;

        float time = 0f;
        while (time < duration)
        {
            foreach (ImageAlpha image in images) image.Alpha = Mathf.Lerp(alphaAverage, alpha, time / duration);
            foreach (TextAlpha text in texts) text.Alpha = Mathf.Lerp(alphaAverage, alpha, time / duration);
            yield return null;
            time += Time.deltaTime;
        }

        foreach (ImageAlpha image in images) image.Alpha = alpha;
        foreach (TextAlpha text in texts) text.Alpha = alpha;

        coroutine = null;
    }
}

[System.Serializable]
class ImageAlpha
{
    public Image image;
    public Vector2 alphaVector;
    [System.NonSerialized]
    public bool forced = false;
    public bool enabledOnNotForsed = true, enabledOnForsed = true;

    public ImageAlpha(Image image, Vector2 alphaVector, bool forced, float alpha)
    {
        this.image = image;
        this.alphaVector = alphaVector;
        this.forced = forced;
        Alpha = alpha;
    }

    public float Alpha
    {
        get
        {
            return image.color.a / (forced ? alphaVector.y : alphaVector.x);
        }
        set
        {
            Color color = image.color;
            color.a = Mathf.Clamp01(value) * (forced ? alphaVector.y : alphaVector.x);
            image.color = color;
        }
    }

}

[System.Serializable]
class TextAlpha
{
    public Text text;
    public Vector2 alphaVector;
    [System.NonSerialized]
    public bool forced = false;
    public bool enabledOnNotForsed = true, enabledOnForsed = true;

    public TextAlpha(Text text, Vector2 alphaVector, bool forced)
    {
        this.text = text;
        this.alphaVector = alphaVector;
        this.forced = forced;
    }

    public float Alpha
    {
        get
        {
            return text.color.a / (forced ? alphaVector.y : alphaVector.x);
        }
        set
        {
            Color color = text.color;
            color.a = Mathf.Clamp01(value) * (forced ? alphaVector.y : alphaVector.x);
            text.color = color;
        }
    }
}
