using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPanel : MonoBehaviour
{
    public static TriggerPanel instance;

    private CanvasGroup CG => GetComponent<CanvasGroup>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show()
    {
        CG.alpha = 1;
        CG.interactable = true;
    }

    public void Hide()
    {
        CG.alpha = 0;
        CG.interactable = false;
    }


}
