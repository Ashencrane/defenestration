using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerOneHealthBar;
    [SerializeField] private Image playerTwoHealthBar;

    [SerializeField] private Sprite julieHealthBar;
    [SerializeField] private Sprite bebeHealthBar;

    private Dictionary<Character, Sprite> _characterHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        _characterHealthBar = new Dictionary<Character, Sprite>()
        {
            { Character.Julie, julieHealthBar },
            { Character.PissanxBebe, bebeHealthBar }
        };

        playerOneHealthBar.sprite = _characterHealthBar[CharacterSelector.PlayerCharacters[0]];
        playerTwoHealthBar.sprite = _characterHealthBar[CharacterSelector.PlayerCharacters[1]];
    }
}
