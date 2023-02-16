using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int sideValue;
    public bool isActive;
    public MeshRenderer meshRenderer;


    private void OnTriggerStay(Collider other)
    {
        isActive = true;

    }
    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }

}
