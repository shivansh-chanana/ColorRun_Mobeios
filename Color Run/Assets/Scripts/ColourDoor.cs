using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourDoor : MonoBehaviour
{
    public Material doorMat;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public string myTag;
    public Animator anim;

    private void Start()
    {
        skinnedMeshRenderer.material = doorMat;
        string[] newTag = myTag.Split('_');
        newTag[0] = "door_";
        gameObject.tag = newTag[0] + newTag[1];
    }
}
