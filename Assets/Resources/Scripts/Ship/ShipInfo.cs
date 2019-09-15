using UnityEngine;

[CreateAssetMenu(fileName = "NewShipInfo", menuName = "Ships/ShipInfo")]
public class ShipInfo : ScriptableObject
{
    [Header("Основное")]
    new public string name = "NewShip";
    public int unlockLevel = 1, gradeLevel = 1;
    public Sprite icon = null;
    [Header("Стоимость")]
    public BigDigit startPrice = new BigDigit(0d, 0);
    [Header("Время рейда")]
    public float raidTime = 1f;
    [Header("Награда")]
    public BigDigit reward = new BigDigit(0d, 0);
    [Header("Характеристики корабля")]
    public float speed = 200f;
    public float distance = 400f;
}
