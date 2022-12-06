using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
    }
    private static Mouse instance; //We will only have one instance of this class, so we will make it static for ease of access

    [SerializeField]
    public LayerMask mousePlaneLayerMask;

    // Update is called once per frame
    void Update()
    {
        transform.position = Mouse.GetPosition();
    }

    //Method to get the position where we have the mouse on the plane
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //We will perform a raycast from the camera to the mouse position and store it as ray
        //The raycastHit struct will have all the information of the point hit by the raycast
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask); //We will only seek collition with the layerMask mousePlane
        return raycastHit.point;
    }
}
