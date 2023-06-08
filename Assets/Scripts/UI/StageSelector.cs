using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum StageMap
{
    Cathedral,
    Ballroom
}

public class StageSelector : MonoBehaviour
{
    public static StageMap SelectedStageMap = StageMap.Cathedral;
    
    [SerializeField] private Image previewImage;
    [SerializeField] private Image curSelection;
    [SerializeField] private TMP_Text previewText;

    private readonly Dictionary<string, StageMap> _mapNameToStageMap = new Dictionary<string, StageMap>()
    {
        { StageMap.Cathedral.ToString(), StageMap.Cathedral },
        { StageMap.Ballroom.ToString(), StageMap.Ballroom }
    };

    public void UpdateSelectedName(string stage)
    {
        previewText.text = stage;
        SelectedStageMap = _mapNameToStageMap[stage];
    }

    public void UpdatePreviewImage(Image stageImage)
    {
        previewImage.sprite = stageImage.sprite;
    }

    public void UpdateSelectionBox(Image selectionBox)
    {
        curSelection.color = Color.clear;
        curSelection = selectionBox;
        curSelection.color = Color.yellow;
    }

    private void OnDestroy()
    {
        Debug.Log($"Map: {SelectedStageMap}");
    }
}
