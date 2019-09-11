using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowNewIsland : WindowBase
{
    [SerializeField] protected Text islandName;
    [SerializeField] protected Image islandImage;
    [SerializeField] protected string[] islandNames;
    [SerializeField] protected Sprite[] islandSprites;
    [SerializeField] protected List<int> levels;
    [SerializeField] protected ScrollManager sm;

    protected Island island;

    private void Awake()
    {
        island = Island.Instance;
    }

    public override void Open(object[] args)
    {
        int level = island.Level;
        if (levels.Contains(level))
        {
            base.Open(args);
            back.SetActive(true);
            rays.SetActive(true);
            int id = levels.IndexOf(level);
            islandName.text = islandNames[id];
            islandImage.sprite = islandSprites[id];
            StartCoroutine(Center());
        }
        else Close();
    }

    public override void Close()
    {
        base.Close();
        transform.parent.GetComponent<InterfaceIerarchy>().Next();
    }

    private IEnumerator Center()
    {
        bool Opened() { return transform.parent.GetComponent<InterfaceIerarchy>().Done; }
        yield return new WaitWhile(Opened);
        yield return new WaitForSeconds(0.5f);
        sm.Center();
    }
}
