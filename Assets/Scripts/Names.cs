using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Names : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("InputField").GetComponent<InputField>().ActivateInputField();
    }

    public void ConfirmName()
    {
        if (CheckName())
        {
            Camera.main.GetComponent<CameraTurnAndFace>().StartGame();
        }
        else
        {
            transform.Find("InputField").GetComponent<InputField>().ActivateInputField();
        }
    }

    private bool CheckName()
    {
        string name = transform.Find("InputField/Text").GetComponent<Text>().text;
        name = name.Trim();

        if (name != "")
        {
            LeaderboardDriver.Name = name;
            return true;
        }
        else
        {
            return false;
        }
    }
}
