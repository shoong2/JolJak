using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
public class PlayFabLogin : MonoBehaviour
{
    public TMP_InputField name_Input;
    public TMP_InputField PW_Input;
    public TMP_InputField Email_Input;
    public TMP_Text ErrorText;

    string name;
    string password;
    string email;

    public Camera captureCamera;


    public void Capture()
    {
        // ī�޶� ���� �ؽ�ó�� �������մϴ�.
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // �ؽ�ó�� ����Ʈ �迭�� ��ȯ�մϴ�.
        byte[] bytes = screenShot.EncodeToPNG();

        // ���Ϸ� �����մϴ�.
        File.WriteAllBytes("Assets/CapturedImage.png", bytes);

        Debug.Log("capture");

    }


//public void PW_Value_Changed()
//{
//    password = PW_Input.text.ToString();
//}

//public void Email_Value_Changed()
//{
//    email = Email_Input.text.ToString();
//}

//public void Start()
//{
//    //if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
//    //{
//    //    /*
//    //    Please change the titleId below to your own titleId from PlayFab Game Manager.
//    //    If you have already set the value in the Editor Extensions, this can be skipped.
//    //    */
//    //    PlayFabSettings.staticSettings.TitleId = "30C95";
//    //}
//    PlayFabSettings.TitleId = "30C95";
//    Debug.Log("hi");


//}

public void Login()
    {
        ErrorText.text = "Login.....";
        var request = new LoginWithEmailAddressRequest { Email = Email_Input.text, Password = PW_Input.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        ErrorText.text = "Login.....";
        SceneManager.LoadScene("Main");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        ErrorText.text = error.GenerateErrorReport();
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = Email_Input.text, Password = PW_Input.text, Username = name_Input.text,
        DisplayName = name_Input.text};
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("���� ����");
        ErrorText.text = "Success Register!";
    }

    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("���� ����");
        Debug.LogWarning(error.GenerateErrorReport());
        ErrorText.text = error.GenerateErrorReport();
    }

    public void SetStat(int a)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate {StatisticName ="test", Value = a},
            }
        }, 
        (result) => { ErrorText.text = "����"; },
        (error)=> { ErrorText.text = "���� ����"; });

       
    }

    public void GetStat()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                ErrorText.text = "";
                foreach (var eachStat in result.Statistics)
                    ErrorText.text += eachStat.StatisticName + " : " + eachStat.Value + "\n";
            },
            (error) => { ErrorText.text = "�� �ҷ����� ����"; });
    }

}
