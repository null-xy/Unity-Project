using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertMesh : MonoBehaviour {
    [ContextMenu("Convert to regular mesh")]
	void Convert()
    {
        SkinnedMeshRenderer skinnedMeshender = GetComponent<SkinnedMeshRenderer>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.sharedMesh = skinnedMeshender.sharedMesh;
        meshRenderer.sharedMaterials = skinnedMeshender.sharedMaterials;

        DestroyImmediate(skinnedMeshender);
        DestroyImmediate(this);
    }
}
