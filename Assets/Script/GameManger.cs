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
    public static float curHP;
    float fadeTime = 2.5f;
    float scoreTime = 0;
    public TMP_Text count;

    public Image end;
    public GameObject gameOver;
    public GameObject button;
    public GameObject board;

    //엔딩조건
    public bool hitHand = false; 
    bool endScreen = true;
    bool isend = false;

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
        count.text = ((int)scoreTime).ToString();
        isend = false;
        curHP = maxHP;
        hpBar.value = curHP / maxHP;
        Debug.Log(hpBar.value);
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
            int w = 1;
            //for (int i = 0; i < result.Leaderboard.Count; i++)
            for(int i= result.Leaderboard.Count-1; i>=0; i--)
            {
                
                var curBoard = result.Leaderboard[i];
                if (curBoard.StatValue == (int)scoreTime)
                {
                    Debug.Log("color");
                    testText.color = new Color(255, 255, 0);
                }
                else
                {
                    testText.color = Color.white;
                    Debug.Log("no color");
                }
                testText.text += w + " " + curBoard.DisplayName + " " + curBoard.StatValue + "\n";
                w++;
                Debug.Log((int)scoreTime + "," + curBoard.StatValue);
            }
        },
        (error) => print("fail"));
    }

    private void Update()
    {
        if (curHP > 0 && !hitHand)
        {
            count.text = ((int)scoreTime).ToString();
            scoreTime += Time.deltaTime;
            curHP -= Time.deltaTime;
            HandleHp();
        }

        if(curHP > maxHP)
        {
            curHP = maxHP;
        }
       
        if ((curHP < 0 || hitHand || FogSpawn.startSpawnFogNum <=0) &&!isend ) //엔드 조건
        {
            Debug.Log(hitHand);
           
            //fillImg.gameObject.SetActive(false);
            //scoreToString = scoreTime.ToString("00.00");
            //scoreToString = scoreToString.Replace(".", ":");
            //nowScore.text = "score  " + scoreToString; 
            if (endScreen)
            {
               StartCoroutine(Fade());
               

            }

            isend = true;
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
        //if (scoreTime > dataBase.highScore)
        //{
        //    dataBase.highScore = scoreTime;
        //}

        //fillImg.gameObject.SetActive(false);
        //scoreToString = scoreTime.ToString("00.00");
        //scoreToString = scoreToString.Replace(".", ":");
        //nowScore.text = "score  " + scoreToString;
        
        //bestScoreToString = dataBase.highScore.ToString("00.00");
        //bestScoreToString = bestScoreToString.Replace(".", ":");
        //bestScore.text = "best    " + bestScoreToString;
        //SaveData();
        if(FogSpawn.startSpawnFogNum<=0)
        {
            gameOver.GetComponent<TMP_Text>().text = "GAME CLEAR";
            SetStat((int)scoreTime);
        }
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

        //yield return new WaitForSeconds(0.5f);
        gameOver.gameObject.SetActive(true);
        board.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //nowScore.gameObject.SetActive(true);
        //yield return new WaitForSeconds(0.5f);
        //bestScore.gameObject.SetActive(true);
        button.SetActive(true);
        //while(Time.timeScale >=0.05)
        //{
        //    Time.timeScale -= 0.08f;
        //    yield return new WaitForSeconds(0.01f);
        //    Debug.Log(Time.timeScale);

        //}
        GetLeaderboard();
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
        //var request = new UpdatePlayerStatisticsRequest
        //{
        //    Statistics = new List<StatisticUpdate>
        //{ new StatisticUpdate{StatisticName ="Rank", Value =a}}
        //};
       // PlayFabClientAPI.UpdatePlayerStatistics(request, ())
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName ="Rank", Value = a},
            }
        },
        (result) => { Debug.Log("저장"); },
        (error) => { test2.text = "저장 실패"; });


    }

    
}

[SerializeField]
public class DataBase
{
    public float highScore=0;
}
