using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAnimControl : MonoBehaviour
{
    public List<GameObject> childObjects = new List<GameObject>();

    internal void AddInList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects.Add(transform.GetChild(i).gameObject);
        }
    }
}
