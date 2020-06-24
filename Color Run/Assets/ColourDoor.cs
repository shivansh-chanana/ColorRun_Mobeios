using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourDoor : MonoBehaviour
{
    public Material doorMat;
    public MeshRenderer meshRenderer;
    public string myTag;

    private void Start()
    {
        meshRenderer.material = doorMat;
        string[] newTag = myTag.Split('_');
        newTag[0] = "door_";
        gameObject.tag = newTag[0] + newTag[1];
    }
}
