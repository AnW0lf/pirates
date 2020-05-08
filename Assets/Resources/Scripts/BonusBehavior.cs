using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBehavior : MonoBehaviour
{
    [SerializeField] private BonusType type = BonusType.Material;
    [SerializeField] private float modifier = 0f;
    [SerializeField] private FlyingObject flying = null;
    [SerializeField] private SpriteRenderer sprite = null;
    [SerializeField] private new Collider2D collider2D = null;


    public BonusType Type { get => type; }
    public float Modifier { get => modifier; }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        gameObject.LeanScale(Vector3.one * 1.2f, 0.2f)
            .setOnComplete(() => gameObject.LeanScale(Vector3.one, 0.05f)
            .setOnComplete(() => GetComponent<Animation>().enabled = true));
    }

    public void Hide(string text)
    {
        collider2D.enabled = false;
        sprite.enabled = false;
        flying.gameObject.SetActive(true);
        flying.Fly(transform.position, transform.position + Vector3.up, 1.5f, text);
        Destroy(gameObject, 1.6f);
    }
}

public enum BonusType { Material, Money, Speed, Wheel}
