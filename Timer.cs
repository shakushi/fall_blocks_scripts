using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public Image GageImage;
    [SerializeField]
    public GameSceneCtlr GameSceneCtlr;

    private bool isPause = true;
    private float gameTime = 30f;

    public void StartTimer()
    {
        isPause = false;
    }
    public void SetPause(bool value)
    {
        isPause = value;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPause)
        {
            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                gameTime = 0;
                GameSceneCtlr.CreateResultScene();
                isPause = true;
            }
            GageImage.fillAmount = gameTime / 30f;
        }

    }
}
