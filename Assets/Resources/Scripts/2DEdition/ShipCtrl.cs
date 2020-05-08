using UnityEngine;
using System.Collections;

public class ShipCtrl : MonoBehaviour
{
    [SerializeField] private string shipName = "", description = "";
    [SerializeField] private Sprite spriteForMenu = null;
    [SerializeField] private int unlockLevel;
    [SerializeField] private bool blackShip = false;
    [SerializeField] private IslandCtrl islandCtrl = null;
    [SerializeField] private LifebuoyManager lifebuoy = null;
    [SerializeField] private Transform parent = null;
    [SerializeField] private SpriteRenderer body = null;
    [SerializeField] private TrailRenderer trail = null;
    [SerializeField] float angularSpeed = 1f, toRaidDuration = 1f, fromRaidDuration = 1f, OutOfScreenDestination = 5f;
    [SerializeField] private int level = 0;
    public ShipLevel[] levels;
    private RotationDirection direction = RotationDirection.Clockwise;
    private RaidStep raidStep = RaidStep.Free;
    private float raidTimer = 0f, startY = 0f,
        time = 0f, speedModifier = 1f, autoRaidAngle = 0f;
    private int blackMarkCount = 0;

    public string ShipName { get => shipName; }

    public string Description { get => description; }

    public int UnlockLevel { get => unlockLevel; }

    public int Level { get => level; }

    public Sprite SpriteForMenu { get => spriteForMenu; }

    private void Start()
    {
        body.transform.localScale = Vector3.zero;
        level = Island.Instance().GetParameter(shipName + "_Level", 1);
        if (blackShip)
            blackMarkCount = Island.Instance().GetParameter(shipName + "_BlackMark", 1);
        if (level > 0) Activate();
    }

    public void Activate()
    {
        lifebuoy.AddLifebuoy();

        body.transform.localScale = Vector3.zero;
        body.gameObject.LeanScale(Scale, 1f).setEase(LeanTweenType.easeOutBounce);
        trail.widthMultiplier = Scale.x;
        LeanTween.delayedCall(0.5f, () => trail.emitting = true);
    }

    private Vector3 Scale
    {
        get
        {
            if (level == 0) return Vector3.zero;
            int lvl = Mathf.Clamp(level - 1, 0, levels.Length - 1);
            return new Vector3(levels[lvl].size.x, levels[lvl].size.y, 1f);
        }
    }

    public int BlackMark { get => blackMarkCount; }

    public void AddBlackMark(int count)
    {
        blackMarkCount += count;
        Island.Instance().SetParameter(shipName + "_BlackMark", blackMarkCount);
    }

    public bool Unlocked
    {
        get => Island.Instance().Level >= unlockLevel;
    }

    public bool Black
    {
        get => blackShip;
    }

    public bool Exists
    {
        get => level > 0;
    }

    public int LevelsToUnlock
    {
        get
        {
            if (Island.Instance().Level >= unlockLevel) return 1000;
            else return Island.Instance().Level - unlockLevel;
        }
    }

    public BigDigit Cost
    {
        get => levels[level].cost;
    }

    public bool MaxGraded
    {
        get => level == levels.Length;
    }

    public float TimeInRaid
    {
        get
        {
            if (level == 0) return 0f;
            int lvl = Mathf.Clamp(level - 1, 0, levels.Length - 1);
            return levels[lvl].timeInRaid;
        }
    }

    public BigDigit Reward
    {
        get
        {
            if (level == 0) return BigDigit.zero;
            int lvl = Mathf.Clamp(level - 1, 0, levels.Length - 1);
            return levels[lvl].reward;
        }
    }

    public void LevelUp()
    {
        if (Black && BlackMark <= 0 || !Black && !Island.Instance().ChangeMoney(BigDigit.Reverse(Cost))) return;
        level++;
        if (Black)
        {
            blackMarkCount--;
            Island.Instance().SetParameter(shipName + "_BlackMark", blackMarkCount);
        }

        if (level == 1)
        {
            Activate();
            EventManager.SendEvent("ShipBought", shipName);
        }
        else
        {
            body.gameObject.LeanScale(Scale, 1f).setEase(LeanTweenType.easeOutBounce);
            trail.widthMultiplier = Scale.x;
            EventManager.SendEvent("ShipUpgraded", shipName, level);
        }

        Island.Instance().SetParameter(shipName + "_Level", level);
    }

    private void GetReward()
    {
        islandCtrl.RewardRaid(Reward);
    }

    private void Update()
    {
        if (level == 0) return;
        switch (raidStep)
        {
            case RaidStep.Free:
                {
                    float angle = angularSpeed * Time.deltaTime;
                    parent.eulerAngles += Vector3.forward * angle * (int)direction;
                    autoRaidAngle += angle;
                    if (autoRaidAngle >= 180f) GoToRaid();
                }
                break;
            case RaidStep.ToRaid:
                {
                    time += Time.deltaTime;

                    Vector3 bodyPos = body.transform.localPosition;
                    bodyPos.y = Mathf.Lerp(startY, OutOfScreenDestination * (int)direction, time / raidTimer);
                    body.transform.localPosition = bodyPos;
                    if (time >= raidTimer)
                    {
                        raidStep = RaidStep.InRaid;
                        time = TimeInRaid / speedModifier;
                    }
                }
                break;
            case RaidStep.InRaid:
                {
                    time -= Time.deltaTime;

                    if (time <= 0f)
                    {
                        if (direction == RotationDirection.Clockwise) direction = RotationDirection.Contrclockwise;
                        else direction = RotationDirection.Clockwise;
                        Vector3 bodyScale = body.transform.localScale;
                        bodyScale.y = -bodyScale.y;
                        body.transform.localScale = bodyScale;
                        PrepareFromRaid();
                        raidStep = RaidStep.FromRaid;
                    }
                }
                break;
            case RaidStep.FromRaid:
                {
                    time += Time.deltaTime;

                    Vector3 bodyPos = body.transform.localPosition;
                    bodyPos.y = Mathf.Lerp(startY, 0f, time / raidTimer);
                    body.transform.localPosition = bodyPos;
                    if (time >= raidTimer)
                    {
                        raidStep = RaidStep.Free;
                        GetReward();
                    }
                }
                break;
        }
    }

    private void OnMouseUpAsButton()
    {
        GoToRaid();
    }

    private void GoToRaid()
    {
        PrepareToRaid();
        raidStep = RaidStep.ToRaid;
        autoRaidAngle = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TrailTrigger") trail.emitting = true;
        else if (raidStep != RaidStep.FromRaid && collision.tag == "Bonus")
        {
            BonusBehavior bonus = collision.GetComponent<BonusBehavior>();
            switch (bonus.Type)
            {
                case BonusType.Material:
                    Island.Instance().ExpUp(Reward * bonus.Modifier);
                    bonus.Hide(string.Format("+{0}", Reward.ToString()));
                    break;
                case BonusType.Money:
                    Island.Instance().ChangeMoney(Reward * bonus.Modifier);
                    bonus.Hide(string.Format("{0}", Reward.ToString()));
                    break;
                case BonusType.Speed:
                    speedModifier += bonus.Modifier;
                    bonus.Hide(string.Format("-{0} SEC", TimeInRaid / speedModifier));
                    break;
                case BonusType.Wheel:
                    lifebuoy.AddLifebuoy();
                    bonus.Hide(string.Format("+{0}", bonus.Modifier));
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TrailTrigger") trail.emitting = false;
    }

    private void PrepareToRaid()
    {
        if (raidStep == RaidStep.ToRaid || raidStep == RaidStep.InRaid) return;

        if (raidStep == RaidStep.FromRaid) GetReward();

        float bodyY = body.transform.localPosition.y;

        if (raidStep == RaidStep.Free)
            raidTimer = toRaidDuration;
        else
            raidTimer = Mathf.Abs(bodyY) / OutOfScreenDestination * toRaidDuration + toRaidDuration;

        speedModifier = 1f;
        startY = bodyY;
        time = 0f;
    }

    private void PrepareFromRaid()
    {
        float bodyY = body.transform.localPosition.y;
        raidTimer = fromRaidDuration;
        startY = bodyY;
        time = 0f;
    }

}

public enum RotationDirection { Clockwise = -1, Contrclockwise = 1 }
public enum RaidStep { Free, ToRaid, InRaid, FromRaid }

[System.Serializable]
public class ShipLevel
{
    public Vector2 size;
    public float timeInRaid;
    public BigDigit reward, cost;

    public ShipLevel(Vector2 size, float timeInRaid, BigDigit reward, BigDigit cost)
    {
        this.size = size;
        this.timeInRaid = timeInRaid;
        this.reward = reward;
        this.cost = cost;
    }
}
