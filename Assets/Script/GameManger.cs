using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
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
    public GameObject button;

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

    public AudioSource fly;

    public TMP_Text testText;
    public TMP_Text test2;

    private void Start()
    {
        
        hpBar.value = curHP / maxHP;

        SAVE_DATA_DIRECTORY = Application.persistentDataPath + "/Saves/";
        Debug.Log(Application.persistentDataPath);
        //SaveData();
        //LoadData();
        Debug.Log("high: " + dataBase.highScore);

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataBase);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
        Debug.Log("save");

    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            dataBase = JsonUtility.FromJson<DataBase>(loadJson);
        }

        else
        {
            Debug.Log("없음");
        }
        
 
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = "Rank",
            MaxResultsCount = 10,
            ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true }
        };
        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var curBoard = result.Leaderboard[i];
                testText.text += i + 1 + " " + curBoard.DisplayName + " " + curBoard.StatValue + "\n";
            }
        },
        (error) => print("fail"));
    }

    private void Update()
    {
        if (curHP > 0 && !hitHand)
        {
            scoreTime += Time.deltaTime;
            curHP -= Time.deltaTime;
            HandleHp();
        }
       
        if (curHP < 0 || hitHand) //엔드 조건
        {
            Debug.Log(hitHand);
            SetStat((int)scoreTime);
            GetLeaderboard();
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
        hitHand = false;
        fly.Stop();
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
        button.SetActive(true);
        while(Time.timeScale >=0.05)
        {
            Time.timeScale -= 0.08f;
            yield return new WaitForSeconds(0.01f);
            Debug.Log(Time.timeScale);
            
        }
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
       
        Time.timeScale = 1f;
    }    

    public void Home()
    {
        
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }

    public void SetStat(int a)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName ="Rank", Value = a},
            }
        },
        (result) => { test2.text = "저장"; },
        (error) => { test2.text = "저장 실패"; });


    }

    
}

[SerializeField]
public class DataBase
{
    public float highScore=0;
}
