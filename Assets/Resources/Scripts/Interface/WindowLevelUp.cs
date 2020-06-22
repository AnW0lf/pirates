using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowLevelUp : WindowBase
{
    [SerializeField] protected GameObject rewardObject, levelRewardObject;
    [SerializeField] protected Text textField;
    [SerializeField] protected List<IslandController> islandsList;
    [SerializeField] protected List<PiersUpgrade> piersList;
    [SerializeField] protected PanelQuests quest = null;
    [SerializeField] protected Image islandBackground = null, islandFill = null;
    [SerializeField] protected Text islandProgressText = null;
    [SerializeField] protected float islandProgressDuration = 1f;
    [SerializeField] protected Animation rewardText, rewardButton;

    protected int levelsToShip;
    protected Island island;
    protected BigDigit money;

    private void Awake()
    {
        island = Island.Instance();
        Opened = false;
    }

    public override void Open(object[] args)
    {
        base.Open(args);
        Reward();
        IslandProgress();
    }

    private void IslandProgress()
    {
        islandProgressText.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        islandProgressText.color = new Color(1f, 1f, 1f, 0f);
        int lessLevel = quest.Levels[quest.Levels.Length - 2].level,
            greaterLevel = quest.Levels[quest.Levels.Length - 1].level,
            curLevel = Island.Instance().Level,
            spriteId = 0;

        for (int i = 0; i < quest.Levels.Length; i++)
        {
            if (curLevel <= quest.Levels[i].level)
            {
                greaterLevel = quest.Levels[i].level;
                if (i == 0) lessLevel = 0;
                else lessLevel = quest.Levels[i - 1].level;
                spriteId = i + 1;
                break;
            }
        }

        float progress = (float)(curLevel - lessLevel) / (greaterLevel - lessLevel);
        float oldProgress = (float)(curLevel - lessLevel - 1) / (greaterLevel - lessLevel);
        Sprite sprite = null;
        int counter = 0;
        foreach (IslandController ic in islandsList)
        {
            IslandSpriteController isc = ic.GetComponent<IslandSpriteController>();
            if (counter + isc.sprites.Count > spriteId)
            {
                sprite = isc.sprites[spriteId - counter];
                break;
            }
            counter += isc.sprites.Count;
        }

        islandBackground.sprite = sprite;
        islandFill.sprite = sprite;
        islandFill.fillAmount = oldProgress;

        if (lessLevel / 25 < greaterLevel / 25)
            islandProgressText.text = string.Format("NEW ISLAND: {0}%", Mathf.RoundToInt(progress * 100f));
        else
            islandProgressText.text = string.Format("Island Up: {0}%", Mathf.RoundToInt(progress * 100f));

        LeanTween.delayedCall(0.4f, () =>
        {
            StartCoroutine(ProgressIsland(oldProgress, progress, islandProgressDuration));

            foreach (IslandController ic in islandsList)
            {
                if (progress == 1f)
                {
                    transform.parent.GetComponent<InterfaceIerarchy>().onDone +=
                        () => ic.GetComponent<IslandSpriteController>().ChangeSprite();
                }
                else
                {
                    transform.parent.GetComponent<InterfaceIerarchy>().onDone +=
                        () => ic.GetComponent<IslandSpriteController>().PlayEffect();
                }
            }

            LeanTween.delayedCall(islandProgressDuration * 1.1f, () =>
            {
                islandProgressText.color = new Color(1f, 1f, 1f, 1f);
                islandProgressText.gameObject.LeanScale(Vector3.one * 1.2f, 0.1f)
                .setOnComplete(() => islandProgressText.gameObject.LeanScale(Vector3.one, 0.05f));

                LeanTween.delayedCall(0.4f, () =>
                 {
                     rewardText.gameObject.SetActive(true);
                     rewardText.Play();
                     LeanTween.delayedCall(0.4f,
                         () =>
                         {
                             rewardButton.gameObject.SetActive(true);
                             rewardButton.Play("NewLevelButtonShow");
                             LeanTween.delayedCall(0.5f, () => rewardButton.Play("BonusPulse"));
                         });
                 });
            });
        });
    }

    private IEnumerator ProgressIsland(float oldProgress, float progress, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            islandFill.fillAmount = Mathf.Lerp(oldProgress, progress, time / duration);
            yield return null;
        }
    }

    public override void Close()
    {
        base.Close();
        rewardText.gameObject.SetActive(false);
        rewardButton.gameObject.SetActive(false);
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }

    public void AddLevelReward(float modifier)
    {
        if (modifier > 0)
            island.ChangeMoney(money * modifier);
        else if (modifier < 0)
            island.ChangeMoney(money * -modifier);
    }

    private void Reward()
    {
        money = BigDigit.zero;

        if (island.Level <= 25)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.Reward * island.Level * 16f);
            }
        }
        else if (island.Level > 25 && island.Level <= 50)
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.Reward * (island.Level - 25) * 16f);
            }
        }
        else
        {
            foreach (IslandController islandCont in islandsList)
            {
                if (islandCont.minLevel <= island.Level)
                    money += (islandCont.Reward * (island.Level - 50) * 16f);
            }
        }



        levelsToShip = 999;
        foreach (PiersUpgrade piers in piersList)
        {
            foreach (PierManager pier in piers.piers)
            {
                if (island.Level + 1 < pier.minLvl)
                {
                    if ((pier.minLvl - (island.Level + 1)) < levelsToShip)
                    {
                        levelsToShip = pier.minLvl - (island.Level + 1);
                    }
                }
            }
        }

        textField.text = string.Format("+{0}", money);

        if (money < 100f)
        {
            rewardObject.transform.localPosition = new Vector3(-180f, rewardObject.transform.localPosition.y, rewardObject.transform.localPosition.z);
            levelRewardObject.transform.localPosition = new Vector3(180f, levelRewardObject.transform.localPosition.y, levelRewardObject.transform.localPosition.z);
        }
        else if (money < 1000f)
        {
            rewardObject.transform.localPosition = new Vector3(-150f, rewardObject.transform.localPosition.y, rewardObject.transform.localPosition.z);
            levelRewardObject.transform.localPosition = new Vector3(150f, levelRewardObject.transform.localPosition.y, levelRewardObject.transform.localPosition.z);
        }
        else if (money < 10000f)
        {
            rewardObject.transform.localPosition = new Vector3(-90f, rewardObject.transform.localPosition.y, rewardObject.transform.localPosition.z);
            levelRewardObject.transform.localPosition = new Vector3(90f, levelRewardObject.transform.localPosition.y, levelRewardObject.transform.localPosition.z);
        }
        else
        {
            rewardObject.transform.localPosition = new Vector3(0f, rewardObject.transform.localPosition.y, rewardObject.transform.localPosition.z);
            levelRewardObject.transform.localPosition = new Vector3(0f, levelRewardObject.transform.localPosition.y, levelRewardObject.transform.localPosition.z);
        }
    }
}
