using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowController : MonoBehaviour
{
    public TextManager titleTM, upLevelTM, raidTimeTM, rewardTM, detailLevelTM, bonusTM, upBtnTM, fadeLevelTM, descriptionTM;
    public Image icon, miniIcon, profitIcon;
    public GameObject windowFade, iconFade, titleFade, cost, rewardEffectPref, characteristics;
    public Vector3 effectScale = new Vector3(120f, 120f, 1f);
    public Button exitBtn, upgradeBtn;
    public Text costTxt;

    public Sprite body, sail, gun, coin, sandclock;

    public Animation timeAnim, coinAnim;

    private PierManager pier;
    private Island island;
    private GameObject rewardEffect;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void OnEnable()
    {
        EventManager.Subscribe("ChangeMoney", UpdateInfo);
        EventManager.Subscribe("AddExp", UpdateInfo);
        EventManager.Subscribe("LevelUp", UpdateInfo);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("ChangeMoney", UpdateInfo);
        EventManager.Unsubscribe("AddExp", UpdateInfo);
        EventManager.Unsubscribe("LevelUp", UpdateInfo);
        Destroy(rewardEffect);
    }

    private void UpdateInfo(object[] arg0)
    {
        UpdateInfo();
    }

    public void GenerateMenu(PierManager pier)
    {
        this.pier = pier;

        UpdateInfo();
        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(pier.Upgrade);
        upgradeBtn.onClick.AddListener(UpdateInfo);
        upgradeBtn.onClick.AddListener(BonusPulse);
    }

    public void BonusPulse()
    {
        float a = 0f;
        int b = 0;
        if (pier.detailCurrentLvl1 < pier.detailMaxLvl1)
        {
            a = pier.detailChangeRaidTime1;
            b = pier.detailChangeReward1;
        }
        else if (pier.detailCurrentLvl2 < pier.detailMaxLvl2)
        {
            a = pier.detailChangeRaidTime2;
            b = pier.detailChangeReward2;
        }
        else if (pier.detailCurrentLvl3 < pier.detailMaxLvl3)
        {
            a = pier.detailChangeRaidTime3;
            b = pier.detailChangeReward3;
        }
        if(a != 0f && b == 0f)
        {
            timeAnim.Play();
            if (rewardEffect != null) Destroy(rewardEffect);
            rewardEffect = Instantiate(rewardEffectPref, timeAnim.transform);
            rewardEffect.transform.localScale = effectScale;
        }
        else if (a == 0f && b != 0f)
        {
            coinAnim.Play();
            if (rewardEffect != null) Destroy(rewardEffect);
            rewardEffect = Instantiate(rewardEffectPref, coinAnim.transform);
            rewardEffect.transform.localScale = effectScale;
        }
    }

    private void UpdateInfo()
    {

        if (pier.black)
        {
            if (pier.GetBlackMark() > 0 && !pier.shipExist)
                NotBought();
            else if (!pier.shipExist)
                Locked();
            else if (pier.maxLvl)
                MaxLevel();
            else
                Bought();
        }
        else if (pier.minLvl > island.Level)
            Locked();
        else if (!pier.shipExist)
            NotBought();
        else if (pier.maxLvl)
            MaxLevel();
        else
            Bought();

        if (pier.minLvl <= island.Level && !pier.maxLvl && (!pier.black && pier.GetUpgradeCost() <= island.Money || pier.black && pier.GetBlackMark() > 0))
            upgradeBtn.interactable = true;
        else
            upgradeBtn.interactable = false;
    }

    private void Locked()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, CheckRange(pier.GetReward()));
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, pier.shipDescription);
        if (pier.black)
            SetState(upBtnTM, "Catch unlock in Lucky Wheel");
        else
            SetState(upBtnTM, pier.minLvl.ToString(), "LEVEl ");

        if (!icon.sprite.Equals(pier.spriteForMenu))
            icon.sprite = pier.spriteForMenu;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (iconFade.activeInHierarchy)
            iconFade.SetActive(false);
        if (!windowFade.activeInHierarchy)
            windowFade.SetActive(true);
        if (!titleFade.activeInHierarchy)
            titleFade.SetActive(true);
        if (upgradeBtn.interactable)
            upgradeBtn.interactable = false;

        icon.color = Color.black;
        cost.SetActive(false);
        characteristics.gameObject.SetActive(false);
    }

    private void NotBought()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, CheckRange(pier.GetReward()));
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, pier.shipDescription);

        if (!pier.black)
        {
            SetState(upBtnTM, "Unlock\n");
            cost.SetActive(true);
            costTxt.text = pier.GetUpgradeCost().ToString();
        }
        else if (pier.GetBlackMark() > 0)
        {
            SetState(upBtnTM, "Unlock");
            cost.SetActive(false);
        }
        else
        {
            SetState(upBtnTM, "Catch upgrade in Lucky Wheel");
            cost.SetActive(false);
        }

        SetState(fadeLevelTM, pier.minLvl.ToString(), "LEVEL ");
        characteristics.gameObject.SetActive(false);

        if (!icon.sprite.Equals(pier.spriteForMenu))
            icon.sprite = pier.spriteForMenu;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (!iconFade.activeInHierarchy)
            iconFade.SetActive(true);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
        if (pier.black)
            icon.color = Color.black;
        else
            icon.color = Color.white;
    }

    private void Bought()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        int curLvl = pier.detailCurrentLvl1 + pier.detailCurrentLvl2 + pier.detailCurrentLvl3 + 1;
        SetState(upLevelTM, curLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, CheckRange(pier.GetReward()));

        if (!miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);
        if (!icon.sprite.Equals(pier.spriteForMenu))
            icon.sprite = pier.spriteForMenu;

        float a = 0f;
        int b = 0;
        if (pier.detailCurrentLvl1 < pier.detailMaxLvl1)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl1.ToString() + "/" + pier.detailMaxLvl1, "HULL ");
            a = pier.detailChangeRaidTime1;
            b = pier.detailChangeReward1;
            miniIcon.sprite = pier.detailMiniature1;
        }
        else if (pier.detailCurrentLvl2 < pier.detailMaxLvl2)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl2.ToString() + "/" + pier.detailMaxLvl2, "SAIL ");
            a = pier.detailChangeRaidTime2;
            b = pier.detailChangeReward2;
            miniIcon.sprite = pier.detailMiniature2;
        }
        else if (pier.detailCurrentLvl3 < pier.detailMaxLvl3)
        {
            SetState(detailLevelTM, pier.detailCurrentLvl3.ToString() + "/" + pier.detailMaxLvl3, "GUNS ");
            a = pier.detailChangeRaidTime3;
            b = pier.detailChangeReward3;
            miniIcon.sprite = pier.detailMiniature3;
        }

        string bonus = (a == 0f ? "" : (a < 0f ? "" : "+") + a.ToString())
                + (b == 0 ? "" : (b < 0f ? "" : " +") + b.ToString());
        SetState(bonusTM, bonus);
        profitIcon.gameObject.SetActive(true);
        profitIcon.sprite = a != 0 ? sandclock : coin;

        descriptionTM.gameObject.SetActive(false);
        characteristics.gameObject.SetActive(true);

        if (!pier.black)
        {
            SetState(upBtnTM, "Upgrade\n");
            cost.SetActive(true);
            costTxt.text = pier.GetUpgradeCost().ToString();
        }
        else if (pier.GetBlackMark() > 0)
        {
            SetState(upBtnTM, "Upgrade");
            cost.SetActive(false);
        }
        else
        {
            SetState(upBtnTM, "Catch upgrade in Lucky Wheel");
            cost.SetActive(false);
        }

        if (iconFade.activeInHierarchy)
            iconFade.SetActive(false);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);

        icon.color = Color.white;
    }

    private void MaxLevel()
    {
        SetState(titleTM, pier.shipName);
        int maxLvl = pier.detailMaxLvl1 + pier.detailMaxLvl2 + pier.detailMaxLvl3 + 1;
        SetState(upLevelTM, maxLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, pier.GetRaidTime().ToString(), "", "s");
        SetState(rewardTM, CheckRange(pier.GetReward()));
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, "Choice of the true corsair");
        SetState(upBtnTM, "MAX LEVEL");

        if (!icon.sprite.Equals(pier.spriteForMenu))
            icon.sprite = pier.spriteForMenu;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (!iconFade.activeInHierarchy)
            iconFade.SetActive(true);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
        if (upgradeBtn.interactable)
            upgradeBtn.interactable = false;

        icon.color = Color.white;
        cost.SetActive(false);

        descriptionTM.gameObject.SetActive(false);
        characteristics.gameObject.SetActive(true);
    }

    private void SetState(TextManager tm, string text, string prefix = "", string postfix = "")
    {
        if (!tm.gameObject.activeInHierarchy)
            tm.gameObject.SetActive(true);
        tm.text = text;
        tm.prefix = prefix;
        tm.postfix = postfix;
    }

    private string CheckRange(int value)
    {
        if (value < 10000)
        {
            return value.ToString();
        }
        else
        {
            float v = value, degree;
            for (degree = 0; v > 1000f; degree++, v /= 1000f) ;

            string str = v.ToString();
            str = str.Length >= 5 ? str.Substring(0, 5) : str;
            str = str.Replace(',', '.');
            for (; str.Length < 5; str = str.Contains(".") ? str += "0" : str += ".") ;

            switch (degree)
            {
                case 0: return str;
                case 1: return str + "K";
                case 2: return str + "M";
                case 3: return str + "B";
                case 4: return str + "T";
                case 5: return str + "q";
                case 6: return str + "Q";
                case 7: return str + "s";
                case 8: return str + "S";
                default: return str + "?";
            }
        }
    }
}
