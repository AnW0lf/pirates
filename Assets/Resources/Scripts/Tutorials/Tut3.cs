using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tut3 : MonoBehaviour
{
    public GameObject ships;
    public PanelQuests quests;
    public GameObject text;
    private Ship ship;

    private void OnEnable()
    {
        StartCoroutine(SpriteOn());

        ship = ships.GetComponentInChildren<Ship>();

        if (Screen.safeArea.yMax != Screen.safeArea.height)
        {
            if (GetComponentInChildren<Text>()) GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void Update()
    {
        if (ship.InRaid)
        {
            quests.opened = true;
            GetComponentInParent<Tutorial>().NextStage();
        }
    }

    IEnumerator SpriteOn()
    {
        yield return new WaitForSeconds(4f);
        while(text.GetComponent<Text>().color.a < 1f)
        {
            text.GetComponent<Text>().color += new Color(0f, 0f, 0f, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
