using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCtlr : MonoBehaviour
{
    [SerializeField]
    public TMP_Text ScoreText;
    [SerializeField]
    public TMP_Text SubScoreText;

    private int score = 0;
    public int Score
    {
        get { return score; }
        private set { score = value; }
    }

    private bool subscore_active = false;
    private float startTime;
    private float deleteTime = 1.5f;

    public void AddScore(int num, float bonus)
    {
        int addvalue = num * (int)(1000 * bonus);
        Score += addvalue;
        if(bonus != 1f)
        {
            SubScoreText.color = new Color(0.7f, 0.54f, 0f);
        }
        else
        {
            SubScoreText.color = Color.black;
        }
        SubScoreText.text = "+" + addvalue.ToString();
        subscore_active = true;
        startTime = Time.time;
    }

    private void Update()
    {
        ScoreText.text = Score.ToString("D8");
        if (subscore_active && Time.time - startTime > deleteTime)
        {
            subscore_active = false;
            SubScoreText.text = "";
        }
    }
}
