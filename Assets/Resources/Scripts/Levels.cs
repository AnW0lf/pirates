using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public int startExp;
    public float expModifier;
    public GameObject levelProgress, levelUp;
    public Image levelBar;
    public Text levelNumber;

    private int maxExp;
    private Island island;

    public static int level;
    public static int curExp;

    private void Awake()
    {
        island = Island.Instance();
    }

    void Start()
    {
        SetLevel();
    }

    void Update()
    {
        levelNumber.text = "Level " + level;

        if (curExp < maxExp)
        {
            levelUp.SetActive(false);
            levelProgress.SetActive(true);
            levelBar.fillAmount = (float)curExp / (float)maxExp;
        }
        else
        {
            levelProgress.SetActive(false);
            levelUp.SetActive(true);
        }
    }

    private void SetLevel()
    {
        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);

        level = PlayerPrefs.GetInt("Level");

        curExp = PlayerPrefs.GetInt("Exp");
        maxExp = (int)(startExp * Mathf.Pow(expModifier, level));
    }

    public void LevelUp()
    {
        level += 1;
        PlayerPrefs.SetInt("Level", level);
        curExp = 0;
        maxExp = (int)(startExp * Mathf.Pow(expModifier, level));
    }
}
