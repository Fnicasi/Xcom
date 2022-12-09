using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    public void HideMesh()
    {
        meshRenderer.enabled = false;
    }
    public void ShowMesh()
    {
        meshRenderer.enabled = true;
    }   
}
