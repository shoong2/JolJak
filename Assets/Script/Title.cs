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

    public void GuestBook()
    {
        SceneManager.LoadScene("GuestBook");
    }

    public void Cam()
    {
        SceneManager.LoadScene("Cam");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (titleAudio.isPlaying)
                titleAudio.Stop();
            else
                titleAudio.Play();
        }
    }
}
