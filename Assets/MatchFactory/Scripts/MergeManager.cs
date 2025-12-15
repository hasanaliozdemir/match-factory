using System;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class MergeManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float goUpDistance = 0.15f;
    [SerializeField] private float goUpDuration = 0.15f;
    [SerializeField] private LeanTweenType goUpEasing = LeanTweenType.easeInOutSine;
    [SerializeField] private float mergeDuration = 0.3f;

    void Awake()
    {
        ItemSpotManager.mergeStarted += OnMergeStarted;
    }



    void OnDestroy()
    {
        ItemSpotManager.mergeStarted -= OnMergeStarted;
    }

    private void OnMergeStarted(List<Item> items)
    {
        // All items moves to up and merges in middle
        for (int i = 0; i < items.Count; i++)
        {
            Vector3 targetPos = items[i].transform.position + items[i].transform.up * goUpDistance;

            Action callback = null;

            if (i == 0)
            {
                callback = () => SmashItems(items);
            }

            LeanTween.move(items[i].gameObject, targetPos, goUpDuration)
                .setEase(goUpEasing)
                .setOnComplete(callback);
        }
    }

    private void SmashItems(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
