using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePresenter : MonoBehaviour
{
    [SerializeField]
    public Image GageImage;

    public void TimeUpdate(float time)
    {
        GageImage.fillAmount = time / 30f;
    }
}
