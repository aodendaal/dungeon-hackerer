using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    [SerializeField]
    private GameObject debugPanel;

    private InputField debugInput;
    private Text debugText;

    private Dictionary<string, Action<string[]>> debugActions;

	// Use this for initialization
	void Start () {
        debugPanel.SetActive(false);

        debugInput = debugPanel.transform.GetChild(0).GetComponent<InputField>();
        debugText = debugPanel.transform.GetChild(1).GetComponent<Text>();

        debugActions = new Dictionary<string, Action<string[]>>();
        debugActions.Add("hello", SayHello);
        debugActions.Add("say", Say);
        debugActions.Add("teleport", Teleport);
        debugActions.Add("nextlevel", NextLevel);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            debugPanel.SetActive(!debugPanel.activeSelf);

            if (debugPanel.activeSelf)
            {
                DungeonData.inDebugMode = true;
                debugInput.text = string.Empty;
                debugInput.Select();
                debugInput.ActivateInputField();
            }
            else
            {
                DungeonData.inDebugMode = false;
            }
        }
	}

    public void EnterCommand_EndEdit()
    {
        if (debugInput.text != string.Empty)
        {
            if (debugInput.text == "`")
            {
                return;
            }

            DebugMessage(debugInput.text);

            var words = debugInput.text.Split(' ');

            var firstWord = words[0];

            if (debugActions.ContainsKey(firstWord))
            {
                debugActions[firstWord.ToLower()](words);
            }
            else
            {
                DebugMessage("Unknown command");
            }

            debugInput.text = string.Empty;
        }

        debugInput.Select();
        debugInput.ActivateInputField();
    }

    private void DebugMessage(string message)
    {
        debugText.text += "\n" + message;

    }

    private void SayHello(string[] args)
    {
        DebugMessage("Hello, World!");
    }


    private void Say(string[] args)
    {
        if (args.Length == 1)
        {
            DebugMessage("Invalid parameters");
        }

        var message = string.Empty;
        for (var i = 1; i < args.Length; i++)
        {
            message += " " + args[i];
        }
        message = message.Substring(1, message.Length - 1);

        DebugMessage("\"" + message + "\"");
    }

    private void Teleport(string[] args)
    {
        if (args.Length != 2)
        {
            DebugMessage("Invalid parameters");
        }

        if (args[1].ToLower() == "entrance")
        {
            var player = GameObject.Find("Player");

            var position = DungeonData.GetEntrancePosition(DungeonData.currentLevel);

            player.transform.position = position + new Vector3(0f, 2.5f, 0f);

            DebugMessage("Teleporting to entrance");
        }
        else if (args[1].ToLower() == "exit")
        {
            var player = GameObject.Find("Player");

            var position = DungeonData.GetExitPosition(DungeonData.currentLevel);

            player.transform.position = position + new Vector3(0f, 2.5f, 0f);

            DebugMessage("Teleporting to exit");
        }
        else
        {
            DebugMessage("Unknown location");
        }
    }

    private void NextLevel(string[] args)
    {
        var player = GameObject.Find("Player");

        var position = DungeonData.GetExitPosition(DungeonData.currentLevel);

        player.transform.position = position + new Vector3(0f, 2.5f, 0f);

        DebugMessage("Teleporting to next level");
    }
}
