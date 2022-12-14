using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class GameManger : MonoBehaviour
{
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Image fillImg;

    //체력 및 시간
    public float maxHP = 10;
    public float curHP = 10;
    float fadeTime = 2.5f;
    float scoreTime = 0;

    public Image end;
    public GameObject gameOver;

    //엔딩조건
    public bool hitHand = false; 
    bool endScreen = true;

    //데이터
    DataBase dataBase = new DataBase();
    string SAVE_DATA_DIRECTORY;
    string SAVE_FILENAME = "/DataBase.txt";

    //UI
    public TextMeshProUGUI nowScore;
    public TextMeshProUGUI bestScore;
    string scoreToString;
    string bestScoreToString;


    private void Start()
    {
        
        hpBar.value = curHP / maxHP;

        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        Debug.Log(Application.dataPath);
        LoadData();
        Debug.Log("high: " + dataBase.highScore);

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataBase);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
        Debug.Log("save");

    }

    public void LoadData()
    {
        string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
        dataBase = JsonUtility.FromJson<DataBase>(loadJson);
        
 
    }

    private void Update()
    {
        if(curHP >0)
            scoreTime += Time.deltaTime;

        curHP -= Time.deltaTime;
        HandleHp();
        if (curHP < 0 || hitHand)
        {
            //fillImg.gameObject.SetActive(false);
            //scoreToString = scoreTime.ToString("00.00");
            //scoreToString = scoreToString.Replace(".", ":");
            //nowScore.text = "score  " + scoreToString; 
            if (endScreen)
            {
                StartCoroutine(Fade());
            }            
        }
    }

    void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, curHP / maxHP, Time.deltaTime * 10);
    }

    public IEnumerator Fade()
    {
        if (scoreTime > dataBase.highScore)
        {
            dataBase.highScore = scoreTime;
        }

        fillImg.gameObject.SetActive(false);
        scoreToString = scoreTime.ToString("00.00");
        scoreToString = scoreToString.Replace(".", ":");
        nowScore.text = "score  " + scoreToString;
        
        bestScoreToString = dataBase.highScore.ToString("00.00");
        bestScoreToString = bestScoreToString.Replace(".", ":");
        bestScore.text = "best    " + bestScoreToString;
        SaveData();

        endScreen = false;
        end.gameObject.SetActive(true);
        Color alpha = end.color;

        float time = 0f;


        while (alpha.a < 0.8f)
        {
            time += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            end.color = alpha;
            yield return null;
        }
        time = 0f;

        yield return new WaitForSeconds(0.5f);
        gameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        nowScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bestScore.gameObject.SetActive(true);
        //end.gameObject.SetActive(false);
        //yield return null;
    }
}

[SerializeField]
public class DataBase
{
    public float highScore=0;
}
