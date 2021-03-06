using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    private bool upper = true;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine("startWithRandomWait");
    }

    void Update()
    {
        Vector3 dir = Vector3.up * 0.01f;
        if (!upper) dir *= -1f;
        transform.Translate(dir);
    }

    IEnumerator startWithRandomWait()
    {
        float wait = Random.Range(0.1f, 0.6f);
        yield return new WaitForSeconds(wait);
        StartCoroutine("changeAnimDir");
    }

    IEnumerator changeAnimDir()
    {
        yield return new WaitForSeconds(0.7f);
        upper = !upper;
        StartCoroutine("changeAnimDir");
    }
}
