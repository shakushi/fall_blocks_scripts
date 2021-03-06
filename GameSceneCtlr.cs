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
    Timer Timer;
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

    private AudioSource bgmSource;
    private PlayableDirector director;
    private void Start()
    {
        bgmSource = BGMObj.GetComponent<AudioSource>();
        director = GetComponent<PlayableDirector>();
    }

    public void StartGame()
    {
        SEPlayer.PlaySound(1);
        OnPreStartUI.SetActive(false);
        Player.IPSetPause(false);
        Timer.StartTimer();
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
}
