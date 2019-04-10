using UnityEngine;

public class Tut3 : MonoBehaviour
{
    public GameObject ships;

    private Ship ship;

    private void OnEnable()
    {
        ship = ships.GetComponentInChildren<Ship>();
    }

    private void Update()
    {
        if (ship.InRaid())
            GetComponentInParent<Tutorial>().NextStage();
    }
}
