using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private int unlockLevel = 3;
    [SerializeField] private Button btn;
    [SerializeField] private Transform opened = null, closed = null;
    [SerializeField] private VerticalLayoutGroup group = null;
    [SerializeField] private float switchDuration = 0.2f, scrollDuration = 0.5f;
    [SerializeField] private Vector2 spacingVector = Vector2.zero;
    [SerializeField] private Button[] islandButtons;
    [SerializeField] private Color selected, unselected;
    [SerializeField] private RectTransform content = null;

    private bool isOpened = false;
    private Coroutine coroutine = null;
    private int curId = -1, id = 0, click = 0;
    private bool scrolling = false;

    private void Start()
    {
        CheckLevel();
        if (!btn.gameObject.activeSelf)
            EventManager.Subscribe("LevelUp", CheckLevel);
    }

    private void CheckLevel(object[] arg0)
    {
        CheckLevel();
    }

    private void CheckLevel()
    {
        if (Island.Instance().Level < unlockLevel) return;
        btn.gameObject.SetActive(true);
        group.gameObject.SetActive(true);
        EventManager.Unsubscribe("LevelUp", CheckLevel);
    }

    public void Close()
    {
        opened.localScale = Vector3.one;
        closed.localScale = Vector3.up + Vector3.forward;
        opened.LeanScaleX(0f, switchDuration / 2f).setOnComplete(() => opened.gameObject.SetActive(false));
        LeanTween.delayedCall(switchDuration / 2f, () =>
        {
            closed.gameObject.SetActive(true);
            closed.LeanScaleX(1f, switchDuration / 2f).setOnComplete(() => btn.interactable = true);
        });
        isOpened = false;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(SwitchGroup(spacingVector.y, switchDuration));
    }

    public void Open()
    {
        closed.localScale = Vector3.one;
        opened.localScale = Vector3.up + Vector3.forward;
        closed.LeanScaleX(0f, switchDuration / 2f).setOnComplete(() => closed.gameObject.SetActive(false));
        LeanTween.delayedCall(switchDuration / 2f, () =>
        {
            opened.gameObject.SetActive(true);
            opened.LeanScaleX(1f, switchDuration / 2f).setOnComplete(() => btn.interactable = true);
        });
        isOpened = true;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(SwitchGroup(spacingVector.x, switchDuration));

        EventManager.SendEvent("WorldOpened");
    }

    public void Switch()
    {
        click++;
        if (isOpened) Close();
        else Open();
    }

    public void SelectIsland(int id)
    {
        click++;
        SetColor(id);
        StartCoroutine(ScrollTo(id, scrollDuration));
    }

    private void SetColor(int id)
    {
        foreach (Button islandButton in islandButtons)
        {
            var colors = islandButton.colors;
            colors.normalColor = unselected;
            islandButton.colors = colors;
        }

        {
            var colors = islandButtons[id].colors;
            colors.normalColor = selected;
            islandButtons[id].colors = colors;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            click--;
            if (click < 0)
            {
                click = 0;
                if (isOpened)
                    Close();
            }
        }
        if (scrolling) return;
        id = Mathf.RoundToInt((content.anchoredPosition.y / content.sizeDelta.y) * 4f);
        if (curId != id)
        {
            curId = id;
            SetColor(id);
        }
    }

    private IEnumerator ScrollTo(int id, float duration)
    {
        scrolling = true;
        float startY = content.anchoredPosition.y,
            finishY = content.sizeDelta.y * (0.25f * id);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Vector2 pos = content.anchoredPosition;
            pos.y = Mathf.Lerp(startY, finishY, time / duration);
            content.anchoredPosition = pos;
            yield return null;
        }
        scrolling = false;
    }

    private IEnumerator SwitchGroup(float spacing, float duration)
    {
        float startSpacing = group.spacing;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            group.spacing = Mathf.Lerp(startSpacing, spacing, time / duration);
            yield return null;
        }

        coroutine = null;
    }
}
