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

    private Image image;
    private Island island;
    private GameObject changeSpriteEffect;
    private bool change = false;
    private Animation anim;

    private void Awake()
    {
        island = Island.Instance();
        image = GetComponent<Image>();
        anim = GetComponent<Animation>();
    }

    private void Start()
    {
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
        WaitForSeconds wait = new WaitForSeconds(0.5f);

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
        yield return wait;
        changeSpriteEffect.SetActive(false);
    }
}
