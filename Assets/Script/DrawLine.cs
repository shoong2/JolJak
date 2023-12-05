using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;

    LineRenderer lr;
    EdgeCollider2D col;
    List<Vector2> points = new List<Vector2>();
    List<Vector3> erasers = new List<Vector3>();
    public Camera captureCamera;

    string savePath;
    string fileName;

    int count;

    //public GameObject whitePrefab;
    Color color;
    enum State { pencil,erase,paint};
    State state = State.pencil;

    public FlexibleColorPicker fcp;
    RaycastHit2D hit;
    

    private void Start()
    {
        color = Color.black;
        fileName = "GuestBook.png";
        savePath = Application.persistentDataPath + "/GeustBook/";
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        //savePath = Path.Combine(savePath, fileName);
        count = 1;
        //lr.SetColors(new Color(0, 1, 2,0));
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (state == State.erase)
            {
                
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //레이캐스트 정보를 저장할 구조체
                 RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Line"));
                for (int i = 0; i < hit.Length; i++)
                {
                    Destroy(hit[i].collider.gameObject);
                }

            }
            else if(state == State.pencil)
            {
                GameObject go = Instantiate(linePrefab);
                go.transform.position = new Vector3(0, 0, 0);
                lr = go.GetComponent<LineRenderer>();
                col = go.GetComponent<EdgeCollider2D>();
                lr.startColor = color;
                lr.endColor = color;
                points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                lr.positionCount = 1;
                lr.SetPosition(0, points[0]);
            }
        }
        else if(Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (state == State.pencil)
            {
                if (Vector2.Distance(points[points.Count - 1], pos) > 0.1f)
                {
                    points.Add(pos);
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, pos);
                    col.points = points.ToArray();
                }
            }
            //else if(state == State.erase)
            //{
            //    if (Vector2.Distance(erasers[erasers.Count - 1], pos) > 0.1f)
            //    {
            //        GameObject e = Instantiate(whitePrefab);
            //        e.transform.position = new Vector3(erase_object.transform.position.x, erase_object.transform.position.y, 0);
            //        erasers.Add(e.transform.position);
            //    }
            //}
        }
        else if(Input.GetMouseButtonUp(0))
        {
            points.Clear();
            if(state ==State.paint)
            {
                color = fcp.color;
            }

        }
    }


    public void Capture()
    {
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
        while(File.Exists(savePath+fileName))
        {
            fileName = $"/GuestBook_{count}.png";
            //savePath = Path.Combine(savePath, fileName);
            count++;
        }
        File.WriteAllBytes(savePath+fileName, bytes);

        Debug.Log("capture");

    }

    public void Erase()
    {
        //erase_object.SetActive(true);
        state = State.erase;
        fcp.gameObject.SetActive(false);
    }

    public void Paint()
    {
        state = State.paint;
        fcp.gameObject.SetActive(true);
    }

    public void Pencil()
    {
        state = State.pencil;
        //erase_object.SetActive(false);
        fcp.gameObject.SetActive(false);
    }

    public void ResetGuestBook()
    {
        SceneManager.LoadScene("GuestBook");
    }
}
