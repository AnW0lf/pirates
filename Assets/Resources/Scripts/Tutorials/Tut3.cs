using UnityEngine;
using UnityEngine.UI;

public class Tut3 : MonoBehaviour
{
    public GameObject ships;
    public PanelQuests quests;
    private Ship ship;

    private void OnEnable()
    {
        ship = ships.GetComponentInChildren<Ship>();

        if (Screen.safeArea.yMax != Screen.safeArea.height)
        {
            if (GetComponentInChildren<Text>()) GetComponentInChildren<Text>().enabled = false;
        }
    }

    private void Update()
    {
        if (ship.InRaid())
        {
            quests.opened = true;
            GetComponentInParent<Tutorial>().NextStage();
        }
    }
}
