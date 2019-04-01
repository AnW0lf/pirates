using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandController : MonoBehaviour
{
    public int minLevel;
    public float delay = 0.5f, modifier;

    private Island island;
    private bool clicked = false, active = false;
    private Animation anim;

    private void Awake()
    {
        island = Island.Instance();
        anim = GetComponent<Animation>();
    }

    private void Start()
    {
        if (island.Level >= minLevel)
        {
            StartCoroutine(GenerateMoney());
            active = true;
        }
    }

    private void Update()
    {
        if(!active && island.Level >= minLevel)
        {
            StartCoroutine(GenerateMoney());
            active = true;
        }
    }

    public void Click()
    {
        clicked = true;
    }

    private IEnumerator GenerateMoney()
    {
        float time = clicked ? delay / 2f : delay;
        clicked = false;
        yield return new WaitForSeconds(time);
        anim.Play();
        island.ChangeMoney((int)(island.Level * island.Level * modifier));
        StartCoroutine(GenerateMoney());
    }
}
