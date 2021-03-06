using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    public AudioClip sound5;

    AudioSource audioSource;

    public void PlaySound(int num)
    {
        switch (num)
        {
            case 1:
                audioSource.PlayOneShot(sound1);
                break;
            case 2:
                audioSource.PlayOneShot(sound2);
                break;
            case 3:
                audioSource.PlayOneShot(sound3);
                break;
            case 4:
                audioSource.PlayOneShot(sound4);
                break;
            case 5:
                audioSource.PlayOneShot(sound5);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

}
