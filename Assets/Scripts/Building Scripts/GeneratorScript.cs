using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour, UtilityInterface
{
    public int powerPerSecond;

    public void StartUtility()
    {
        StartCoroutine("AddPower");
    }

    IEnumerator AddPower()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.power += powerPerSecond;
        StartCoroutine("AddPower");
    }

    public void DisableUtility()
    {
        this.enabled = false;
    }
}
