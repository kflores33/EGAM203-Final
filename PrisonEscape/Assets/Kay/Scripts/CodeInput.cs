using UnityEngine;
using System.Collections.Generic;

public class CodeInput : MonoBehaviour
{
    public BasicGM gameManager;

    
    public List<char> CorrectCode;

    public List<char> PlayerInput = new List<char>();
    private void Start()
    {
        CorrectCode = new List<char> { 'c', 'b', 'd','a' };
    }

    private void Update()
    {
        
        for (KeyCode key = KeyCode.A; key <= KeyCode.Z; key++)
        {
            if (Input.GetKeyDown(key))
            {
                AddToInput((char)key);
                break;
            }
        }
    }

    public void AddToInput(char letter)
    {
        PlayerInput.Add(letter);
        Debug.Log($"Added letter: {letter}");

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
                Debug.Log("Incorrect code");
                PlayerInput.Clear();
                return;
            }
        }

        Debug.Log("Correct code --- You escaped!!");
        gameManager.ShowEndScreen(true);
        PlayerInput.Clear();
    }
}

