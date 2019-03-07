using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gamekit2D;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour, IDataPersister
{
    public Dialogue dialogue = new Dialogue();

    public bool triggerOnce = false;

    protected bool hasTriggered = false;
    protected bool Triggering;
    protected bool m_CanExecuteButtons;
    protected DialogueManager dialogueManager;

    public DataSettings dataSettings;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        //PersistentDataManager.LoadAllData();
    }

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }
    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_CanExecuteButtons = true;
        Triggering = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_CanExecuteButtons = false;
        if (Triggering)
        {
            dialogueManager.EndDialogue();
            if (triggerOnce)
            {
                hasTriggered = true;
                gameObject.SetActive(false);
                Save();
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_CanExecuteButtons && PlayerInput.Instance.Interact.Down)
        {
            if (Triggering)
            {
                Triggering = false;
                dialogueManager.StartDialogue(dialogue);
            }
            else
            {
                if (dialogueManager.DisplayNextSentence())
                {
                    if (triggerOnce)
                    {
                        hasTriggered = true;
                        gameObject.SetActive(false);
                        Save();
                    }
                    Triggering = true;
                }
            }
        }
    }

    public void Save()
    {
        PersistentDataManager.SetDirty(this);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<bool>(hasTriggered);
    }

    public void LoadData(Data data)
    {
        hasTriggered = (data as Data<bool>).value;
    }
}
