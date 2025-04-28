using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CodeInput : MonoBehaviour
{
    public BasicGM gameManager;


    public List<char> CorrectCode = new List<char> { 'c', 'b', 'd', 'a' };

    public List<char> PlayerInput = new List<char>();

    private HashSet<char> allowedLetters = new HashSet<char> { 'c', 'b', 'd', 'a' };

    public TMP_Text inputDisplayText;

    private Dictionary<char, string> symbolMapping = new Dictionary<char, string>
    {
        { 'c', "■" },
        { 'b', "▲" },
        { 'd', "★" },
        { 'a', "⚪" }
    };

    private void Update()
    {
        for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
        {
            if (Input.GetKeyDown(key))
            {
                char inputChar = char.ToLower((char)key);

                if (allowedLetters.Contains(inputChar))
                {
                    AddToInput(inputChar);
                }
                break;
            }
        }
    }

    public void AddToInput(char letter)
    {
        PlayerInput.Add(letter);
        Debug.Log($"[Input] Added: {letter}");

        UpdateInputDisplay();

        if (PlayerInput.Count == CorrectCode.Count)
        {
            CheckInput();
        }
    }

    public void CheckInput()
    {
        for (int i = 0; i < CorrectCode.Count; i++)
        {
            if (PlayerInput[i] != CorrectCode[i])
            {
                Debug.Log("[Check] Incorrect code. Try again.");
                PlayerInput.Clear();
                UpdateInputDisplay();
                return;
            }
        }

        Debug.Log("[Check] Correct code --- You escaped!!");
        gameManager.ShowEndScreen(true);
        PlayerInput.Clear();
        UpdateInputDisplay();
    }

    private void UpdateInputDisplay()
    {
        if (inputDisplayText == null) return;

        if (PlayerInput.Count == 0)
        {
            inputDisplayText.text = "Code:";
        }
        else
        {
            inputDisplayText.text = "Code:";

            for (int i = 0; i < PlayerInput.Count; i++)
            {
                if (symbolMapping.ContainsKey(PlayerInput[i]))
                {
                    inputDisplayText.text += symbolMapping[PlayerInput[i]];
                }
                else
                {
                    inputDisplayText.text += PlayerInput[i];
                }

                //if (i != PlayerInput.Count - 1)
                    //inputDisplayText.text += " → ";
            }
        }
    }
}
