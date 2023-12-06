using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
public class Cam : MonoBehaviour
{
    public RawImage display;
    WebCamTexture camTexture;
    int currentIndex = 0;

    [Header("캡처")]
    public Camera captureCamera;
    string savePath;
    string fileName;
    int count;
    public TMP_Text num;
    int captureCount = 3;
    public GameObject button;
    AudioSource shutter;
    public AudioSource focus;

    public GameObject[] objects;
    int objectsNum;

    public GameObject[] backgrounds;
    public Image bg;
    int bgNum;

    public Vector3[] colors;
    int colorNum;
    public TMP_Text gf;
    void Start()
    {
        colorNum = 0;
        objectsNum = 0;
        bgNum = 0;
        shutter = GetComponent<AudioSource>();
        fileName = "photo.png";
        savePath = Application.persistentDataPath + "/PhotoBook/";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        count = 1;

        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }
        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        display.texture = camTexture;
        camTexture.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
    public void Capture()
    {
        shutter.Play();
        //CaptureCounting();
        // 카메라를 렌더 텍스처로 렌더링합니다.
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // 텍스처를 바이트 배열로 변환합니다.
        byte[] bytes = screenShot.EncodeToPNG();


        // 파일로 저장합니다.
        while (File.Exists(savePath + fileName))
        {
            fileName = $"/Photo_{count}.png";
            //savePath = Path.Combine(savePath, fileName);
            count++;
        }
        File.WriteAllBytes(savePath + fileName, bytes);

        Debug.Log("capture");
        
    }

    public void CaptureCounting()
    {
        StartCoroutine(CaptureCount());
    }

    IEnumerator CaptureCount()
    {
        button.SetActive(false);
        num.gameObject.SetActive(true);
        num.text = 3.ToString();
        focus.Play();
        yield return new WaitForSecondsRealtime(1f);
        num.text = 2.ToString();
        focus.Play();
        yield return new WaitForSecondsRealtime(1f);
        num.text = 1.ToString();
        focus.Play();
        yield return new WaitForSecondsRealtime(1f);
        num.gameObject.SetActive(false);

        Capture();
        yield return new WaitForSeconds(1f);
        button.SetActive(true);
    }

    public void ObjectButton()
    {
        for(int i=0; i<objects.Length; i++)
        {
            objects[i].SetActive(objectsNum == i);
        }
        objectsNum++;
        if (objectsNum == objects.Length+1)
            objectsNum = 0;
    }

    public void BackgroundB()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(bgNum == i);
        }
        bgNum++;
        if (bgNum == backgrounds.Length + 1)
            bgNum = 0;
    }

    public void BgColor()
    {
        if (colorNum == 1)
            gf.color = Color.black;
        else
            gf.color = Color.white;

        bg.color = new Color(colors[colorNum].x/255, colors[colorNum].y/255, colors[colorNum].z/255);
       
        colorNum++;
        if (colorNum == colors.Length)
            colorNum = 0;
    }
}
