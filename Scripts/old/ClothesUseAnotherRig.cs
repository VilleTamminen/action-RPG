using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesUseAnotherRig : MonoBehaviour
{
    //Doesn't work
    //Code from How to have swappable Character Clothing - 3D Model & Meshes / Textures
    //it takes character model and assigns it's bones to this clothes' bones. 

    /*
    public GameObject character_model;
    void Start()
    {
        SkinnedMeshRenderer targetRenderer = character_model.GetComponent<SkinnedMeshRenderer>();
        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in targetRenderer.bones)
        {
            boneMap[bone.name] = bone;
        }

        SkinnedMeshRenderer thisRenderer = GetComponent<SkinnedMeshRenderer>();
        Transform[] boneArray = thisRenderer.bones;
        for (int idx = 0; idx < boneArray.Length; ++idx)
        {
            string boneName = boneArray[idx].name;
            if (boneMap.TryGetValue(boneName, out boneArray[idx]) == false)
            {
                Debug.LogError("failed to get bone: " + boneName);
                Debug.Break();
            }
        }
        thisRenderer.bones = boneArray; //take effect
    } */

    //Doesn't work
    //Second try with same kind script
    public SkinnedMeshRenderer TargetMeshRenderer;
    void Start()
    {
        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in TargetMeshRenderer.bones)
            boneMap[bone.gameObject.name] = bone;


        SkinnedMeshRenderer myRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();

        Transform[] newBones = new Transform[myRenderer.bones.Length];
        for (int i = 0; i < myRenderer.bones.Length; ++i)
        {
            GameObject bone = myRenderer.bones[i].gameObject;
            if (!boneMap.TryGetValue(bone.name, out newBones[i]))
            {
                Debug.Log("Unable to map bone \"" + bone.name + "\" to target skeleton.");
                break;
            }
        }
        myRenderer.bones = newBones;

    }
}
