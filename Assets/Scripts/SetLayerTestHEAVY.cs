using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayerTestHEAVY : MonoBehaviour
{
    void Start()
    {
        GameObject root = CreateHeirarchy();
        float startTime = Time.realtimeSinceStartup;
        SetLayerOnAll(root, 5);
        float totalTimeMs = (Time.realtimeSinceStartup - startTime) * 1000;
        print("Set layer on all time: " + totalTimeMs + "ms");
        startTime = Time.realtimeSinceStartup;
        SetLayerOnAllRecursive(root, 4);
        totalTimeMs = (Time.realtimeSinceStartup - startTime) * 1000;
        print("Set layer on all recursive time: " + totalTimeMs + "ms");
    }

    static GameObject CreateHeirarchy()
    {
        GameObject root = new GameObject();

        GameObject[] children = new GameObject[100];
        for (int i = 0; i < 100; i++)
        {
            GameObject child = new GameObject();
            child.transform.parent = root.transform;
            children[i] = child;
        }

        GameObject[] grandchildren = new GameObject[1000];
        for (int i = 0; i < 1000; i++)
        {
            GameObject grandchild = new GameObject();
            grandchild.transform.parent = children[Random.Range(0, 99)].transform;
            grandchildren[i] = grandchild;
        }


        for (int i = 0; i < 10000; i++)
        {
            GameObject greatgrandchild = new GameObject();
            greatgrandchild.transform.parent = grandchildren[Random.Range(0, 999)].transform;
        }

        return root;
    }

    static void SetLayerOnAll(GameObject obj, int layer)
    {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layer;
        }
    }

    static void SetLayerOnAllRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerOnAllRecursive(child.gameObject, layer);
        }
    }
}
