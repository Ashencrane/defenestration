using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private RawImage victoryImg;
    [SerializeField] private RawImage defeatImg;
    [SerializeField]
    Texture[] victoryArt;
    [SerializeField]
    Texture[] defeatArt;

    public int winner = -1; // 0 = P1 winner, 1 = P2 winner

    public void SetVictoryText(string text)
    {
        victoryText.text = text;
    }

    public void SetWinner(int winner) // 0 = P1, 1 = P2
    {
        this.winner = winner;
        Character winnerCharacter = CharacterSelector.PlayerCharacters[winner];
        Character loserCharacter = CharacterSelector.PlayerCharacters[winner == 1 ? 0 : 1];
        victoryImg.texture = victoryArt[CharacterSelector.CharacterIndices[winnerCharacter]];
        defeatImg.texture = defeatArt[CharacterSelector.CharacterIndices[loserCharacter]];
    }
    
    public void ReplayLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
