using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CodeInput : MonoBehaviour
{
    public List<int> CorrectCode;

    public List<int> PlayerInput = new List<int>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddToInput(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddToInput(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddToInput(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AddToInput(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AddToInput(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AddToInput(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AddToInput(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            AddToInput(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            AddToInput(9);
        }
    }

    public void AddToInput(int number)
    {
        PlayerInput.Add(number);

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
        Debug.Log("Correct code---You eacaped!!");
        PlayerInput.Clear();
    }
}
