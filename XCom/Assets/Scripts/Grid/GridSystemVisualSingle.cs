using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{ //This script controls each cell of the grid
    [SerializeField] MeshRenderer meshRenderer;

    public void HideMesh()
    {
        meshRenderer.enabled = false;
    }
    public void ShowMesh(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material; //Set the color
    }   
}
