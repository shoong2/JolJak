using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Image fillImg;

    float maxHP = 50;
    float curHP = 50;

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
        }
    }

    void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, curHP / maxHP, Time.deltaTime * 10);
    }
}
