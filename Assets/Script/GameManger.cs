using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Image fillImg;

    float maxHP = 10;
    float curHP = 10;
    float fadeTime = 2.5f;

    public Image end;
    public GameObject gameOver;
    bool endScreen = true;
    
    private void Start()
    {
        hpBar.value = curHP / maxHP;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    curHP -= 10;
        //}
        curHP -= Time.deltaTime;
        HandleHp();
        if (curHP < 0)
        {
            fillImg.gameObject.SetActive(false);
            if(endScreen)
                StartCoroutine(Fade());
            
        }
    }

    void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, curHP / maxHP, Time.deltaTime * 10);
    }

    public IEnumerator Fade()
    {
        endScreen = false;
        end.gameObject.SetActive(true);
        Color alpha = end.color;

        float time = 0f;

        //while (alpha.a > 0f)
        //{
        //    time += Time.deltaTime / fadeTime;
        //    alpha.a = Mathf.Lerp(1, 0, time);
        //    end.color = alpha;
        //    yield return null;
        //}

        //time = 0f;

        while (alpha.a < 0.8f)
        {
            time += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            end.color = alpha;
            yield return null;
        }
        time = 0f;
        //float fadeCount = 0;
        //while(fadeCount <1.0f)
        //{
        //    fadeCount += 0.01f;
        //    yield return new WaitForSeconds(0.01f);
        //    end.color = new Color(0, 0, 0, fadeCount);
        //}
        //endScreen = false;

        yield return new WaitForSeconds(1.0f);
        gameOver.gameObject.SetActive(true);

        //end.gameObject.SetActive(false);
        //yield return null;
    }
}
