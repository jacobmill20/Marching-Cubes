using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFlip : MonoBehaviour
{
    public GameObject[] pages;

    public int idx;

    public void BackPage()
    {
        if (idx - 1 >= 0)
        {
            pages[idx].SetActive(false);
            pages[idx - 1].SetActive(true);
            idx -= 1;
        }
    }

    public void ForwardPage()
    {
        if (idx + 1 < pages.Length)
        {
            pages[idx].SetActive(false);
            pages[idx + 1].SetActive(true);
            idx += 1;
        }
    }
}
