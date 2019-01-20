using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCloth : MonoBehaviour {

    public Transform target;    // 目标的模型，要求其骨骼已经存在
    public Transform source;    // 源模型，所有的可以替换的部件都在这上面

    // 模型资源，对应上面的source
    Dictionary<string, Dictionary<string, Transform>> data = new Dictionary<string, Dictionary<string, Transform>>();
    // 目标骨架，对应上面的target
    Transform[] hips;
    // 目标皮肤，替换这里面的内容就行了
    Dictionary<string, SkinnedMeshRenderer> targetSmr = new Dictionary<string, SkinnedMeshRenderer>();

    // 初始化时的皮肤
    string[,] avatarStr = new string[,] { { "coat", "003" }, { "hair", "003" }, { "pant", "003" }, { "hand", "003" }, { "foot", "003" }, { "head", "003" } };

    // Use this for initialization
    void Start()
    {
        // 获取资源的所有皮肤
        SkinnedMeshRenderer[] parts = source.GetComponentsInChildren<SkinnedMeshRenderer>();

        // 初始化data，将资源添加到Dictionary容器中
        foreach (SkinnedMeshRenderer part in parts)
        {
            string[] partName = part.name.Split('-');
            if (!data.ContainsKey(partName[0]))
            {
                data.Add(partName[0], new Dictionary<string, Transform>());

                // 初始化targetSmr，添加骨架上的皮肤类，但当前的皮肤类内容是空的
                GameObject partObj = new GameObject();
                partObj.name = partName[0];
                partObj.transform.parent = target;
                partObj.transform.localPosition = part.transform.localPosition;
                partObj.transform.localRotation = part.transform.localRotation;
                targetSmr.Add(partName[0], partObj.AddComponent<SkinnedMeshRenderer>());
            }
            data[partName[0]].Add(partName[1], part.transform);
        }

        // 初始化hips，获取所有的骨骼，在Unity已经添加
        hips = target.GetComponentsInChildren<Transform>();

        // 初始化皮肤
        int length = avatarStr.GetLength(0);
        for (int i = 0; i < length; ++i)
        {
            changeMesh(avatarStr[i, 0], avatarStr[i, 1]);
        }
    }

    // 改变部件
    public void changeMesh(string part, string item)
    {
        SkinnedMeshRenderer smr = data[part][item].GetComponent<SkinnedMeshRenderer>();    //获取当前要替换的皮肤，这是源

        // 获取target上与source对应的骨骼，这边千万不能直接把骨骼赋值进去了
        List<Transform> bones = new List<Transform>();
        foreach (Transform bone in smr.bones)
        {
            foreach (Transform hip in hips)
            {
                if (hip.name != bone.name)
                {
                    continue;
                }
                bones.Add(hip);
                break;
            }
        }

        // 这边是目标，进行替换
        targetSmr[part].sharedMesh = smr.sharedMesh;    //替换皮肤
        targetSmr[part].bones = bones.ToArray();    //替换骨骼
        targetSmr[part].materials = smr.materials;  //替换材质
    }

}
