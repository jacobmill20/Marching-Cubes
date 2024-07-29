using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private MeshRenderer[] mesh;
    private int colorIdx;
    private Color oldColor;

    private void Start()
    {
        mesh = transform.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            if (mesh[i].material.IsKeywordEnabled("_EMISSION"))
            {
                oldColor = mesh[i].material.GetColor("_EmissionColor");
                colorIdx = i;
            }
        }
    }
    private void Update()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition();
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].material.EnableKeyword("_EMISSION");
        }

        if (BuildingSystem.current.CanBePlaced(GetComponent<PlaceableObject>()))
        {
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].material.SetColor("_EmissionColor", Color.green);
            }
        }
        else
        {
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].material.SetColor("_EmissionColor", Color.red);
            }
        }
    }

    public void RemoveDrag()
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            if(i == colorIdx)
            {
                mesh[i].material.SetColor("_EmissionColor", oldColor);
            } else
                mesh[i].material.DisableKeyword("_EMISSION");
        }
        Destroy(this);
    }
}
