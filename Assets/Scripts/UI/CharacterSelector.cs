using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Character
{
    Julie,
    PissanxBebe
}

public class CharacterSelector : MonoBehaviour
{
    public static List<Character> PlayerCharacters = new List<Character>() { Character.Julie, Character.Julie };

    [SerializeField] private List<Image> playerCharacterImages;
    [SerializeField] private TMP_Text selectingPlayerText;

    // currentlySelecting % totalPlayers = which player is selecting:
    // 0 = player one, 1 = player two
    private int _currentlySelectingImage = 0;
    private int _currentlySelectingChar = 0;
    
    private readonly Dictionary<string, Character> _characterNameDict = new Dictionary<string, Character>()
    {
        { Character.Julie.ToString(), Character.Julie },
        { Character.PissanxBebe.ToString(), Character.PissanxBebe }
    };

    public void SelectCharacter(Sprite charSprite)
    {
        playerCharacterImages[_currentlySelectingImage % playerCharacterImages.Count].sprite = charSprite;
        ++_currentlySelectingImage;
        // +1 to account for 0-indexing list
        selectingPlayerText.text = $"P{_currentlySelectingImage % playerCharacterImages.Count + 1}";
    }

    public void SelectCharacter(string charName)
    {
        PlayerCharacters[_currentlySelectingChar % PlayerCharacters.Count] = _characterNameDict[charName];
        ++_currentlySelectingChar;
    }

    private void OnDestroy()
    {
        Debug.Log($"Player One: {PlayerCharacters[0]}");
        Debug.Log($"Player Two: {PlayerCharacters[1]}");
    }
}
