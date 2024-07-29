using System.Collections;
using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    public GameObject[] Objects;
    
    private int idx;
    private GameObject newguy;

    private void Start()
    {
        idx = 0;
        StartCoroutine("TakePic");
    }

    private void Update()
    {
    }

    IEnumerator TakePic()
    {
        newguy = Instantiate(Objects[idx]);
        newguy.SetActive(true);

        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(Objects[idx].name + ".png");

        yield return new WaitForSeconds(0.5f);
        idx++;
        newguy.SetActive(false);

        if(idx < Objects.Length)
            StartCoroutine("TakePic");
    }
}