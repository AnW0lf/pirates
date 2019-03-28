using UnityEngine;

public class Tut3 : MonoBehaviour
{
    public PierManager pier;
    private Island island;

    private void Awake()
    {
        island = Island.Instance();
    }

    private void Update()
    {
        if (island.GetParameter(pier.shipName + "_" + pier.detailName1, 0) > 0)
            GetComponentInParent<Tutorial>().NextStage();
    }
}
