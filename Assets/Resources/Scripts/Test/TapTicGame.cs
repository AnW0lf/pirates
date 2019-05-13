using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapTicGame : MonoBehaviour
{
    [SerializeField] Image tapticBackground = null;
    [SerializeField] Image tapticIcon = null;
    [SerializeField] Color backgroundOn = Color.green;
    [SerializeField] Color backgroundOff = Color.red;
    [SerializeField] Color iconOn = Color.green;
    [SerializeField] Color iconOff = Color.red;

    public enum VibrationType { WARNING, FAILURE, SUCCESS, LIGHT, MEDIUM, HEAVY, DEFAULT, VIBRATE, SELECTION}

    void Start()
    {
        if (tapticBackground != null) tapticBackground.color = Taptic.tapticOn ? backgroundOn : backgroundOff;
        if (tapticIcon != null)  tapticIcon.color = Taptic.tapticOn ? iconOn : iconOff;
    }

    public void TriggerTaptic(string type)
    {
        switch (type)
        {
            case "default":     Taptic.Default(); break;
            case "failure":     Taptic.Failure(); break;
            case "heavy":       Taptic.Heavy(); break;
            case "light":       Taptic.Light(); break;
            case "medium":      Taptic.Medium(); break;
            case "selection":   Taptic.Selection(); break;
            case "success":     Taptic.Success(); break;
            case "vibrate":     Taptic.Vibrate(); break;
            case "warning":     Taptic.Warning(); break;
            default: break;
        }
    }

    public void Toggle()
    {
        Taptic.tapticOn = !Taptic.tapticOn;
        if (tapticBackground != null) tapticBackground.color = Taptic.tapticOn ? backgroundOn : backgroundOff;
        if (tapticIcon != null) tapticIcon.color = Taptic.tapticOn ? iconOn : iconOff;
        Taptic.Selection();
    }
}
