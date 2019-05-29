using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandSpriteController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject changeSpriteEffectPref, changeSpriteTextPref;
    public Vector3 effectScale = new Vector3(200f, 200f, 1f);
    [SerializeField] private float sizeIncrease = 0.03f;
    [SerializeField] private int islandNumber = 1, minLevel = 0, maxLevel = 0;

    private Image image;
    private Island island;
    private GameObject changeSpriteEffect, changeSpriteText;
    private Animation anim;
    private RectTransform rect;
    private Vector2 startSizeDelta;
    public int IslandSpriteLevel { get; private set; }

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
        island.InitParameter("IslandSpriteLevel_" + islandNumber, 0);
        IslandSpriteLevel = island.GetParameter("IslandSpriteLevel_" + islandNumber, 0);
        rect.sizeDelta = Vector2.one * startSizeDelta * Mathf.Pow((1f + sizeIncrease), IslandSpriteLevel);
        InitInfo();
        changeSpriteEffect = Instantiate(changeSpriteEffectPref, transform);
        changeSpriteEffect.transform.localScale = effectScale;
        changeSpriteEffect.SetActive(false);
    }

    public void ChangeSprite()
    {
        if (minLevel <= island.Level && maxLevel >= island.Level && IslandSpriteLevel < sprites.Count)
        {
            StopAllCoroutines();
            StartCoroutine(Change());
        }
    }

    private void InitInfo()
    {
        if (sprites.Count > IslandSpriteLevel)
        {
            image.sprite = sprites[IslandSpriteLevel];
        }
        else if (sprites.Count > 0) image.sprite = sprites[sprites.Count - 1];
    }

    private IEnumerator Change()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        if (changeSpriteEffect == null)
        {
            changeSpriteEffect = Instantiate(changeSpriteEffectPref, transform);
            changeSpriteEffect.transform.localScale = effectScale;
        }
        changeSpriteEffect.SetActive(false);
        yield return wait;
        IslandSpriteLevel++;
        if (sprites.Count > IslandSpriteLevel)
        {
            image.sprite = sprites[IslandSpriteLevel];
        }
        else if (sprites.Count > 0) image.sprite = sprites[sprites.Count - 1];
        island.SetParameter("IslandSpriteLevel_" + islandNumber, IslandSpriteLevel);
        changeSpriteEffect.SetActive(true);
        anim.Play("UpgradeBonusPulse");
        if (changeSpriteText == null)
        {
            changeSpriteText = Instantiate(changeSpriteTextPref, transform);
        }
        rect.sizeDelta = Vector2.one * startSizeDelta * Mathf.Pow((1f + sizeIncrease), IslandSpriteLevel);
        yield return wait;
        //changeSpriteEffect.SetActive(false);
    }

    private void Update()
    {
        if (changeSpriteText != null)
        {
            if (!changeSpriteText.GetComponent<Animation>().isPlaying)
            {
                Destroy(changeSpriteText);
            }
        }
    }
}
