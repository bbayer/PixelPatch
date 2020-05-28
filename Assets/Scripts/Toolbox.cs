using UnityEngine;
using UnityEditor;
using System.Collections;


public class Toolbox : MonoBehaviour
{
    public bool hideMouse;
    // Use this for initialization
    void Start()
    {
        Texture2D px = new Texture2D(2, 2);
        px.SetPixels(new Color[] { new Color(0,0,0,.5f), new Color(0, 0, 0, .5f) , new Color(0, 0, 0, .5f) , new Color(0, 0, 0, .5f) });
        Cursor.SetCursor(px, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = !hideMouse;
        if (Input.GetKeyUp(KeyCode.S)) {

            StartCoroutine(CaptureScreen());
        }
    }

    IEnumerator CaptureScreen() {
        yield return new WaitForEndOfFrame();
        int count = PlayerPrefs.GetInt("screenshot_count", 0);
        string filename = "capture_" + count + "_"+Screen.width+"_"+Screen.height+".png";
        ScreenCapture.CaptureScreenshot(filename);
        PlayerPrefs.SetInt("screenshot_count", count + 1);

    }
}
