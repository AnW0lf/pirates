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
    public ScrollManager sm;

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
            StartCoroutine(Center());
        }
        else nsw.Open();
    }

    private IEnumerator Center()
    {
        System.Func<bool> active = delegate { return (nsw.window.activeInHierarchy || back.activeInHierarchy); };
        yield return new WaitWhile(active);
        yield return new WaitForSeconds(0.5f);
        sm.Center();
    }
}
