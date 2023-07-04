using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Sword
{
    Default,
    CupHiltRapier,
    HKNail,
    PolishSaber,
    RifleBayonet,
    SurtrSword,
    SwordOfDios
}

public class WeaponSelector : MonoBehaviour
{
    public static List<Sword> PlayerSwords = new List<Sword> { Sword.Default, Sword.Default };
    public static List<Sprite> PlayerSwordsSprites = new List<Sprite>() { null, null };
    public static Dictionary<Sword, int> SwordIndices = new Dictionary<Sword, int>
    {
        { Sword.Default, 0 },
        { Sword.CupHiltRapier, 1},
        { Sword.HKNail, 2 },
        { Sword.PolishSaber, 3},
        { Sword.RifleBayonet, 4 },
        { Sword.SurtrSword, 5},
        { Sword.SwordOfDios, 6 }
    };

    [SerializeField] private List<Image> playerWeaponImages;
    [SerializeField] private TMP_Text selectingPlayerText;
    [SerializeField] private Sprite defaultSword;
    [SerializeField] private List<Image> playerSprites;
    
    // currentlySelecting % totalPlayers = which player is selecting:
    // 0 = player one, 1 = player two
    private int _currentlySelectingImage = 0;
    private int _currentlySelectingWeapon = 0;
    
    private readonly Dictionary<string, Sword> _swordNameDict = new Dictionary<string, Sword>();

    private void Awake()
    {
        for (var i = 0; i < PlayerSwordsSprites.Count; ++i)
            PlayerSwordsSprites[i] = defaultSword;

        foreach (Sword s in Enum.GetValues(typeof(Sword)))
            _swordNameDict[s.ToString()] = s;

        // set selected sprites
        for (var i = 0; i < playerSprites.Count; ++i)
            playerSprites[i].sprite = CharacterSelector.PlayerCharactersSprites[i];
    }

    public void SelectWeapon(Sprite weaponSprite)
    {
        playerWeaponImages[_currentlySelectingImage % playerWeaponImages.Count].sprite = weaponSprite;
        PlayerSwordsSprites[_currentlySelectingImage % PlayerSwordsSprites.Count] = weaponSprite;
        ++_currentlySelectingImage;
        // +1 to account for 0-indexing list
        selectingPlayerText.text = $"P{_currentlySelectingImage % playerWeaponImages.Count + 1}";
    }

    public void SelectWeapon(string weaponName)
    {
        Debug.Log($"Player {_currentlySelectingWeapon % PlayerSwords.Count + 1}: {weaponName}");
        PlayerSwords[_currentlySelectingWeapon % PlayerSwords.Count] = _swordNameDict[weaponName];
        ++_currentlySelectingWeapon;
    }

    private void OnDestroy()
    {
        Debug.Log($"Player One: {PlayerSwords[0]}");
        Debug.Log($"Player Two: {PlayerSwords[1]}");
    }
}
