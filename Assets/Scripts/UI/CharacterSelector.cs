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
    public static List<Sprite> PlayerCharactersSprites = new List<Sprite>() { null, null };

    public static Dictionary<Character, int> CharacterIndices = new Dictionary<Character, int>()
    {
        { Character.Julie, 0 },
        { Character.PissanxBebe, 1 }
    };


    [SerializeField] private List<Image> playerCharacterImages;
    [SerializeField] private TMP_Text selectingPlayerText;
    [SerializeField] private Sprite defaultSprite;

    // currentlySelecting % totalPlayers = which player is selecting:
    // 0 = player one, 1 = player two
    private int _currentlySelectingImage = 0;
    private int _currentlySelectingChar = 0;
    
    private readonly Dictionary<string, Character> _characterNameDict = new Dictionary<string, Character>()
    {
        { Character.Julie.ToString(), Character.Julie },
        { Character.PissanxBebe.ToString(), Character.PissanxBebe }
    };

    private void Awake()
    {
        for (var i = 0; i < PlayerCharactersSprites.Count; ++i)
            PlayerCharactersSprites[i] = defaultSprite;
    }

    public void SelectCharacter(Sprite charSprite)
    {
        playerCharacterImages[_currentlySelectingImage % playerCharacterImages.Count].sprite = charSprite;
        PlayerCharactersSprites[_currentlySelectingImage % PlayerCharactersSprites.Count] = charSprite;
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
