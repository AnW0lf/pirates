using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandSpriteController : MonoBehaviour
{
    public List<int> levels;
    public Sprite[] sprites;
    public GameObject changeSpriteEffectPref;
    public Vector3 effectScale = new Vector3(200f, 200f, 1f);
    public GameObject newShipWindow, wheelUpdateWindow;
    [SerializeField] private float sizeIncrease = 0.03f;

    private Image image;
    private Island island;
    private GameObject changeSpriteEffect;
    private bool change = false;
    private Animation anim;
    private RectTransform rect;
    private Vector2 startSizeDelta;

    private void Awake()
    {
        island = Island.Instance();
        image = GetComponent<Image>();
        anim = GetComponent<Animation>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        startSizeDelta = rect.sizeDelta;
        int mod()
        {
            for(int i = island.Level; i >= 0; i--)
            {
                if (levels.Contains(i))
                    return levels.IndexOf(i);
            }
            return 0;
        }
        rect.sizeDelta = Vector2.one * startSizeDelta * Mathf.Pow((1f + sizeIncrease), mod());
        EventManager.Subscribe("LevelUp", UpdateInfo);
        InitInfo();
        changeSpriteEffect = Instantiate(changeSpriteEffectPref, transform);
        changeSpriteEffect.transform.localScale = effectScale;
        changeSpriteEffect.SetActive(false);
    }

    public void ChangeSprite()
    {
        if (change)
        {
            change = false;
            StopAllCoroutines();
            StartCoroutine(Change());
        }
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (levels.Contains(island.Level) && sprites.Length > levels.IndexOf(island.Level))
        {
            change = true;
        }
    }

    private void InitInfo()
    {
        if (levels.Count > 0 && sprites.Length > 0)
        {
            bool setted = false;
            for (int i = island.Level; i > 0; i--)
            {
                if (levels.Contains(i) && sprites.Length > levels.IndexOf(i))
                {
                    image.sprite = sprites[levels.IndexOf(i)];
                    setted = true;
                    break;
                }
            }
            if (!setted)
            {
                image.sprite = sprites[0];
            }
        }
    }

    private IEnumerator Change()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);

        do
        {
            yield return wait;
        } while (newShipWindow.activeInHierarchy || wheelUpdateWindow.activeInHierarchy);
        if (changeSpriteEffect == null)
        {
            changeSpriteEffect = Instantiate(changeSpriteEffectPref, transform);
            changeSpriteEffect.transform.localScale = effectScale;
        }
        changeSpriteEffect.SetActive(false);
        yield return wait;
        image.sprite = sprites[levels.IndexOf(island.Level)];
        changeSpriteEffect.SetActive(true);
        anim.Play("UpgradeBonusPulse");
        rect.sizeDelta = Vector2.one * startSizeDelta * Mathf.Pow((1f + sizeIncrease), levels.IndexOf(island.Level));
        yield return wait;
        changeSpriteEffect.SetActive(false);
    }
}
