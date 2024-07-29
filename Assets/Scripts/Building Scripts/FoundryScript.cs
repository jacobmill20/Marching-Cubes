using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundryScript : MonoBehaviour, UtilityInterface
{
    public int cubitsPerSecond;

    public void StartUtility()
    {
        StartCoroutine(AddCubits());
    }

    IEnumerator AddCubits()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.cubits += cubitsPerSecond;
        StartCoroutine(AddCubits());
    }

    public void DisableUtility()
    {
        this.enabled = false;
    }
}
