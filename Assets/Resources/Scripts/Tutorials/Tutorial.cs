using UnityEngine;

public class Tutorial : MonoBehaviour, ITutorial
{
    public Tutorial link;

    protected bool isBegin = false;

    public virtual void Begin()
    {
        isBegin = true;
    }

    public virtual bool ConditionOut()
    {
        if (isBegin)
            return true;
        return false;
    }

    public virtual void Next()
    {
        if(link) link.Begin();
        Destroy(gameObject);
    }
}
