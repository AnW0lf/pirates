using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public void LevelUp()
    {
        Island.Instance.LevelUp();
    }

    public void AddMoney(int exponent)
    {
        Island.Instance.ChangeMoney(new BigDigit(1, exponent));
    }

    public void AddPremium(int exponent)
    {
        Island.Instance.ChangePremium(new BigDigit(1, exponent));
    }

    public void AddSpin(int a)
    {
        Island.Instance.ChangeLifebuoy(a);
    }

    public void Resetting()
    {
        Island.Instance.Resetting();
    }
}
