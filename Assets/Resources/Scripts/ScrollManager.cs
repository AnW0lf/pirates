using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public int level;
    public Canvas game;
    public float speed = 15f;

    private RectTransform rect;
    private Island island;
    private float unit = 2436f;
    private int childCount;

    private void Awake()
    {
        island = Island.Instance;
        rect = GetComponent<RectTransform>();

        childCount = transform.childCount;
        float sizeY = childCount * unit;
        Vector2 pos = new Vector3(rect.localPosition.x, sizeY - (unit * (1 + island.Level / level)), rect.localPosition.z);
        rect.sizeDelta = new Vector2(.0f, sizeY);
        rect.localPosition = pos;
    }

    private void Start()
    {
        foreach (RectTransform child in rect)
        {
            child.sizeDelta = new Vector2(game.GetComponent<RectTransform>().sizeDelta.x, child.sizeDelta.y);
        }
    }

    public void Center()
    {
        StartCoroutine(GoToCenter());
    }

    private IEnumerator GoToCenter()
    {
        childCount = transform.childCount;
        float sizeY = childCount * unit;
        Vector3 newPos = new Vector3(rect.localPosition.x, sizeY - (unit * (1 + island.Level / level)), rect.localPosition.z);
        Vector3 direction = newPos - rect.localPosition;
        float err = speed * 4f;

        System.Func<bool> move = delegate
        {
            direction = newPos - rect.localPosition;
            transform.Translate(direction.normalized * Time.fixedDeltaTime * speed);
            return Mathf.Abs(direction.magnitude) > err;
        };

        yield return new WaitWhile(move);
        rect.localPosition = newPos;
    }
}
