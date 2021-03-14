using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameSceneCtlr : MonoBehaviour
{
    [SerializeField]
    PlayerCtlr Player;
    [SerializeField]
    ScoreCtlr ScoreCtlr;
    [SerializeField]
    GameObject OnPreStartUI;
    [SerializeField]
    GameObject OnResultUI;
    [SerializeField]
    SEPlayer SEPlayer;
    [SerializeField]
    GameObject BGMObj;

    private TimePresenter timePresenter;
    private AudioSource bgmSource;
    private PlayableDirector director;
    private bool inGame = false;
    private float gameTime = 30f;
    private void Start()
    {
        timePresenter = GetComponent<TimePresenter>();
        bgmSource = BGMObj.GetComponent<AudioSource>();
        director = GetComponent<PlayableDirector>();
    }
    void Update()
    {
        if (inGame)
        {
            gameTime -= Time.deltaTime;
            if (gameTime <= 0)
            {
                gameTime = 0;
                CreateResultScene();
                inGame = false;
            }
            timePresenter.TimeUpdate(gameTime);
        }

    }

    public void StartGame()
    {
        SEPlayer.PlaySound(1);
        OnPreStartUI.SetActive(false);
        Player.IPSetPause(false);
        startTimer();
    }

    public void CreateResultScene()
    {
        bgmSource.volume /= 2;
        //Player.IPSetPause(true);
        Player.InResult();
        StartCoroutine("setResultWait");
        SEPlayer.PlaySound(5);
        //animation
        director.Play();
    }
    public void GotoTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    IEnumerator setResultWait()
    {
        yield return new WaitForSeconds(4f);
        OnResultUI.SetActive(true);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(ScoreCtlr.Score);
    }

    private void startTimer()
    {
        inGame = true;
    }

}
