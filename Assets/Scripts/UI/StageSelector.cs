using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StageSelector : MonoBehaviour
{
    public static string SelectedStage = "Cathedral";
    
    [SerializeField] private Image previewImage;
    [SerializeField] private Image curSelection;
    [SerializeField] private TMP_Text previewText;

    public void UpdateSelectedName(string stage)
    {
        previewText.text = stage;
        SelectedStage = stage;
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

    
}
