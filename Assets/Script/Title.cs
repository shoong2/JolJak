using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    //public void ClickStart()
    //{
    //    SceneManager.LoadScene("Main");
    //}
    //

    public AudioSource titleAudio;
    public void CoruStart()
    {
        StartCoroutine(ClickStart());
        titleAudio.Stop();
    }

    public IEnumerator ClickStart()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Main");
    }
}
