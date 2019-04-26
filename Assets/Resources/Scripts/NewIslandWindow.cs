using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewIslandWindow : MonoBehaviour
{
    public Text islandName;
    public Image islandImage;
    public string[] islandNames;
    public Sprite[] islandSprites;
    public List<int> levels;
    public GameObject back, fade;
    public NewShipWindow nsw;

    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    public void Open()
    {
        int level = island.Level;
        if (levels.Contains(level))
        {
            back.SetActive(true);
            fade.SetActive(true);
            int id = levels.IndexOf(level);
            islandName.text = islandNames[id];
            islandImage.sprite = islandSprites[id];
        }
        else nsw.Open();
    }
}
