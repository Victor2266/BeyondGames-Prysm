using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Mirror;

public class ChatBehaviour : NetworkBehaviour
{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    private float timeStamp;//this calculates delta time for the cooldown
    private float coolDownPeriod = 3f;

    public string Username;

    [SerializeField] private int MaxChatLogChar = 150;

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);
        CmdChangeUserName(PlayerNameInput.DisplayName);
        OnMessage += HandleNewMessage;
    }

    [ClientCallback]

    private void OnDestroy()
    {
        SendMsg(Username + " has left the game.");

        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(String message)
    {
        chatText.text += message;

        if (chatText.text.Length > MaxChatLogChar)
        {
            chatText.text = chatText.text.Substring(chatText.text.Length - MaxChatLogChar);
        }

        chatUI.SetActive(true);
        timeStamp = Time.time + 3 * coolDownPeriod;
    }

    [Command]
    public void CmdChangeUserName(string User)
    {
        Username = User;
    }
    public void SendMsg(string message)
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        if (message.Length > 100)
        {
            message = message.Substring(0, 100);
        }

        inputField.ActivateInputField();
        
        CmdSendMessage(message);

        inputField.text = string.Empty;

        timeStamp = Time.time + coolDownPeriod;
    }
    public void ExtendOpenTime(string discard)
    {
        timeStamp = Time.time + 3 * coolDownPeriod;
    }
    private void Update()
    {
        if (hasAuthority)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                chatUI.SetActive(true);
                timeStamp = Time.time + 3 * coolDownPeriod;
                inputField.ActivateInputField();
            }
        }
        
        if (timeStamp <= Time.time)
        {
            chatUI.SetActive(false);
        }
    }
    [Command]

    private void CmdSendMessage(string message)
    {
        if (string.IsNullOrEmpty(Username))
        {
            RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
        }
        else
        {
            RpcHandleMessage($"[{Username}]: {message}");
        }
    }

    [ClientRpc]

    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
    
}
