using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneCtlr : MonoBehaviour
{
    public void GoToGameMain()
    {
        SceneManager.LoadScene("GameMain");
    }
}
