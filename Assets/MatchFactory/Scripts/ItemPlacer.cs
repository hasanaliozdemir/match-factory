using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class ItemPlacer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private List<ItemLevelData> itemDatas;

    [Button]
    private void DebugSomething()
    {
        Debug.Log("Debugging ItemPlacer");
    }
}
