using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapTicGame : MonoBehaviour
{
    [SerializeField] private Image tapticBackground = null;
    [SerializeField] private Image tapticIcon = null;
    [SerializeField] private Color backgroundOn = Color.green;
    [SerializeField] private Color backgroundOff = Color.red;
    [SerializeField] private Color iconOn = Color.green;
    [SerializeField] private Color iconOff = Color.red;

    private Island island;

    public enum VibrationType { WARNING, FAILURE, SUCCESS, LIGHT, MEDIUM, HEAVY, DEFAULT, VIBRATE, SELECTION}

    private void Awake()
    {
        island = Island.Instance();
    }

    void Start()
    {
        /*
#if UNITY_IOS
        if (UIDevice.currentDevice().valueForKey("_feedbackSupportLevel") == 0)
        {
        if (tapticBackground != null) tapticBackground.gameObject.SetActive(false);
        if (tapticIcon != null)  tapticIcon.gameObject.SetActive(false);
        }
#endif
*/
        island.InitParameter("TapTic", 0);
        Taptic.tapticOn = island.GetParameter("TapTic", 0) != 0;
        if (tapticBackground != null) tapticBackground.color = Taptic.tapticOn ? backgroundOn : backgroundOff;
        if (tapticIcon != null) tapticIcon.color = Taptic.tapticOn ? iconOn : iconOff;
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
        island.SetParameter("TapTic", Taptic.tapticOn ? 1 : 0);
        if (tapticBackground != null) tapticBackground.color = Taptic.tapticOn ? backgroundOn : backgroundOff;
        if (tapticIcon != null) tapticIcon.color = Taptic.tapticOn ? iconOn : iconOff;
        Taptic.Selection();
    }
}
