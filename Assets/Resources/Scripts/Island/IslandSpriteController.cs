using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandSpriteController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject changeSpriteEffectPref;
    public Vector3 effectScale = new Vector3(200f, 200f, 1f);
    [SerializeField] private float sizeIncrease = 0.03f;
    [SerializeField] private int islandNumber = 1;

    private Image image;
    private Island island;
    private GameObject changeSpriteEffect;
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
        if (IslandSpriteLevel < sprites.Count)
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
        else if(sprites.Count > 0) image.sprite = sprites[sprites.Count - 1];
    }

    private IEnumerator Change()
    {
        WaitForSeconds wait = new WaitForSeconds(0.25f);
        
        if (changeSpriteEffect == null)
        {
            changeSpriteEffect = Instantiate(changeSpriteEffectPref, transform);
            changeSpriteEffect.transform.localScale = effectScale;
        }
        changeSpriteEffect.SetActive(false);
        yield return wait;
        image.sprite = sprites[IslandSpriteLevel++];
        island.SetParameter("IslandSpriteLevel_" + islandNumber, IslandSpriteLevel);
        changeSpriteEffect.SetActive(true);
        anim.Play("UpgradeBonusPulse");
        rect.sizeDelta = Vector2.one * startSizeDelta * Mathf.Pow((1f + sizeIncrease), IslandSpriteLevel);
        yield return wait;
        changeSpriteEffect.SetActive(false);
    }
}
