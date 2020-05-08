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

    private ShipCtrl ship;
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
        if (ship != null)
            UpdateInfo();
    }

    public void GenerateMenu(ShipCtrl ship)
    {
        this.ship = ship;
        print(ship.ShipName);
        UpdateInfo();
        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(ship.LevelUp);
        upgradeBtn.onClick.AddListener(UpdateInfo);
        upgradeBtn.onClick.AddListener(BonusPulse);
    }

    public void BonusPulse()
    {
        float a = 0f;
        long b = 0;
        //if (ship.detailCurrentLvl1 < ship.detailMaxLvl1)
        //{
        //    a = ship.detailChangeRaidTime1;
        //    b = ship.detailChangeReward1;
        //}
        //else if (ship.detailCurrentLvl2 < ship.detailMaxLvl2)
        //{
        //    a = ship.detailChangeRaidTime2;
        //    b = ship.detailChangeReward2;
        //}
        //else if (ship.detailCurrentLvl3 <= ship.detailMaxLvl3)
        //{
        //    a = ship.detailChangeRaidTime3;
        //    b = ship.detailChangeReward3;
        //}
        if (a != 0f && b == 0f)
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

        if (ship.Black)
        {
            if (ship.BlackMark > 0 && !ship.Exists)
                NotBought();
            else if (!ship.Exists)
                Locked();
            else if (ship.MaxGraded)
                MaxLevel();
            else
                Bought();
        }
        else
        if (!ship.Unlocked)
            Locked();
        else if (!ship.Exists)
            NotBought();
        else if (ship.MaxGraded)
            MaxLevel();
        else
            Bought();

        if (ship.Unlocked && !ship.MaxGraded && (!ship.Black && ship.Cost <= island.Money || ship.Black && ship.BlackMark > 0))
            upgradeBtn.interactable = true;
        else
            upgradeBtn.interactable = false;
    }

    private void Locked()
    {
        SetState(titleTM, ship.ShipName);
        int maxLvl = ship.levels.Length;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, ship.TimeInRaid.ToString(), "", "s");
        SetState(rewardTM, ship.Reward.ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, ship.Description);
        if (ship.Black)
            SetState(upBtnTM, "Catch unlock in Lucky Wheel");
        else
            SetState(upBtnTM, ship.UnlockLevel.ToString(), "LEVEl ");

        if (!icon.sprite.Equals(ship.SpriteForMenu))
            icon.sprite = ship.SpriteForMenu;
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
        SetState(titleTM, ship.ShipName);
        int maxLvl = ship.levels.Length;
        SetState(upLevelTM, "0/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, ship.TimeInRaid.ToString(), "", "s");
        SetState(rewardTM, ship.Reward.ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, ship.Description);

        if (!ship.Black)
        {
            SetState(upBtnTM, "Unlock\n");
            cost.SetActive(true);
            costTxt.text = ship.Cost.ToString();
        }
        else if (ship.BlackMark > 0)
        {
            SetState(upBtnTM, "Unlock");
            cost.SetActive(false);
        }
        else
        {
            SetState(upBtnTM, "Catch upgrade in Lucky Wheel");
            cost.SetActive(false);
        }

        SetState(fadeLevelTM, ship.UnlockLevel.ToString(), "LEVEL ");
        characteristics.gameObject.SetActive(false);

        if (!icon.sprite.Equals(ship.SpriteForMenu))
            icon.sprite = ship.SpriteForMenu;
        if (miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);

        if (!iconFade.activeInHierarchy)
            iconFade.SetActive(true);
        if (windowFade.activeInHierarchy)
            windowFade.SetActive(false);
        if (titleFade.activeInHierarchy)
            titleFade.SetActive(false);
        if (ship.Black)
            icon.color = Color.black;
        else
            icon.color = Color.white;
    }

    private void Bought()
    {
        SetState(titleTM, ship.ShipName);
        int maxLvl = ship.levels.Length;
        int curLvl = ship.Level;
        SetState(upLevelTM, curLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, ship.TimeInRaid.ToString(), "", "s");
        SetState(rewardTM, ship.Reward.ToString());

        if (!miniIcon.gameObject.activeInHierarchy)
            miniIcon.gameObject.SetActive(false);
        if (!icon.sprite.Equals(ship.SpriteForMenu))
            icon.sprite = ship.SpriteForMenu;

        float a = 0f;
        long b = 0;
        /*if (ship.detailCurrentLvl1 < ship.detailMaxLvl1)
        {
            SetState(detailLevelTM, ship.detailCurrentLvl1.ToString() + "/" + ship.detailMaxLvl1, "HULL ");
            a = ship.detailChangeRaidTime1;
            b = ship.detailChangeReward1;
            miniIcon.sprite = ship.detailMiniature1;
        }
        else if (ship.detailCurrentLvl2 < ship.detailMaxLvl2)
        {
            SetState(detailLevelTM, ship.detailCurrentLvl2.ToString() + "/" + ship.detailMaxLvl2, "SAIL ");
            a = ship.detailChangeRaidTime2;
            b = ship.detailChangeReward2;
            miniIcon.sprite = ship.detailMiniature2;
        }
        else if (ship.detailCurrentLvl3 < ship.detailMaxLvl3)
        {
            SetState(detailLevelTM, ship.detailCurrentLvl3.ToString() + "/" + ship.detailMaxLvl3, "GUNS ");
            a = ship.detailChangeRaidTime3;
            b = ship.detailChangeReward3;
            miniIcon.sprite = ship.detailMiniature3;
        }*/

        string bonus = (a == 0f ? "" : (a < 0f ? "" : "+") + a.ToString())
                + (b == 0 ? "" : (b < 0f ? "" : " +") + b.ToString());
        SetState(bonusTM, bonus);
        profitIcon.gameObject.SetActive(true);
        profitIcon.sprite = a != 0 ? sandclock : coin;

        descriptionTM.gameObject.SetActive(false);
        characteristics.gameObject.SetActive(true);

        if (!ship.Black)
        {
            SetState(upBtnTM, "Upgrade\n");
        cost.SetActive(true);
        costTxt.text = ship.Cost.ToString();
        }
        else if (ship.BlackMark > 0)
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
        SetState(titleTM, ship.ShipName);
        int maxLvl = ship.levels.Length;
        SetState(upLevelTM, maxLvl.ToString() + "/" + maxLvl.ToString(), "Level ");
        SetState(raidTimeTM, ship.TimeInRaid.ToString(), "", "s");
        SetState(rewardTM, ship.Reward.ToString());
        detailLevelTM.gameObject.SetActive(false);
        bonusTM.gameObject.SetActive(false);
        profitIcon.gameObject.SetActive(false);
        SetState(descriptionTM, "Choice of the true corsair");
        SetState(upBtnTM, "MAX LEVEL");

        if (!icon.sprite.Equals(ship.SpriteForMenu))
            icon.sprite = ship.SpriteForMenu;
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
