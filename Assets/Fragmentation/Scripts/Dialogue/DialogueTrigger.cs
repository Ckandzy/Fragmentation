using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gamekit2D;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue = new Dialogue();
    bool isFirstExecute;
    bool m_CanExecuteButtons;
    protected DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        m_CanExecuteButtons = true;
        isFirstExecute = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_CanExecuteButtons = false;
        dialogueManager.EndDialogue();
    }

    private void FixedUpdate()
    {
        if (m_CanExecuteButtons && PlayerInput.Instance.Interact.Down)
        {
            if (isFirstExecute)
            {
                isFirstExecute = false;
                dialogueManager.StartDialogue(dialogue);
            }
            else
            {
                if (dialogueManager.DisplayNextSentence())
                    isFirstExecute = true;
            }
        }
    }
}
