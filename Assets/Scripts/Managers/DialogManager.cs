using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    public Animator animator;
    public int maxSentences;

    public bool SkipToIndex;
    public int lastIndex;

    public bool isDisplayingDialog;

    private void Awake()
    {
        instance = this;
    }

    public Queue<string> sentences;
    public int index;

    public delegate void OnEndDialog();
    public OnEndDialog onEndDialog;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        index = 0;
    }

    public void StartDialog (Dialog dialog)
    {
        isDisplayingDialog = true;
        index = 0;
        animator.SetBool("IsOpen", true);
        nameText.text = dialog.name;
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        index++;
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void DisplayNextSentenceEvent(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextSentence();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void EndDialog()
    {
        isDisplayingDialog = false;
        animator.SetBool("IsOpen", false);
        if(onEndDialog != null)
            onEndDialog.Invoke();
    }
    public void SkipDialog()
    {
        if (SkipToIndex)
        {
            index = lastIndex;
            return;
        }
        for (int i = index; i < maxSentences; i++)
        {
            DisplayNextSentence();
        }
    }
}

