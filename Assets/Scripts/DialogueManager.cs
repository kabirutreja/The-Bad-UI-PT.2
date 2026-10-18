using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueBlock
    {
        public string characterName;
        public string[] sentences;
        public DialogueBlock(string name, string[] lines)
        {
            characterName = name;
            sentences = lines;
        }
    }

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.02f;
    public TextMeshProUGUI nameText;
    private Queue<DialogueBlock> dialogueQueue = new Queue<DialogueBlock>();
    private string[] currentSentences;
    private int sentenceIndex;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && dialogueBox.activeSelf)
        {
            ContinueDialogue();
        }
    }

    public void StartButton()
    {
        dialogueQueue.Clear();

        dialogueQueue.Enqueue(new DialogueBlock("System", new string[] {
            "Game is currently under maintenance", "Start will be available soon", "Try again later."
        }));
        dialogueQueue.Enqueue(new DialogueBlock("PIP (:P)", new string[] {
            "Oh, That's just a formality!", "Try LOAD!"
        }));
        AdvanceQueue();
    }

    private void AdvanceQueue()
    {
        if (dialogueQueue.Count == 0)
        {
            dialogueBox.SetActive(false);
            return;
        }

        DialogueBlock nextBlock = dialogueQueue.Dequeue();
        StartDialogue(nextBlock.characterName, nextBlock.sentences);
    }

    private void StartDialogue(string characterName, string[] newSentences)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentSentences = newSentences;
        sentenceIndex = 0;
        nameText.text = characterName;
        dialogueBox.SetActive(true);
        DisplaySentence();
    }

    void DisplaySentence()
    {
        typingCoroutine = StartCoroutine(TypeSentence(currentSentences[sentenceIndex]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
    }

    public void ContinueDialogue()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentences[sentenceIndex];
            isTyping = false;
            typingCoroutine = null;
            return;
        }

        sentenceIndex++;
        
        if (sentenceIndex >= currentSentences.Length)
        {
            AdvanceQueue();
            return;
        }

        DisplaySentence();
    }
    public void LoadButton()
    {
        dialogueQueue.Clear();
        dialogueQueue.Enqueue(new DialogueBlock("System", new string[] { "Are you DUMBBB?", "How do you...", "Load a game that never existed." }));
        
        dialogueQueue.Enqueue(new DialogueBlock("PIP (T_T)", new string[] {
            "Ha! Good question!", "Try Quit?"
        }));
        AdvanceQueue();
    }
    public void QuitButton()
    {
        dialogueQueue.Clear();
        dialogueQueue.Enqueue(new DialogueBlock("PIP (:’(", new string[] { "You want to quit?", "Okay...", "I won't irritate you anymore." }));
        
        dialogueQueue.Enqueue(new DialogueBlock("PIP (._.)", new string[] {
            "Just Kidding!", "You can't quit yet", "You need to adjust the settings first", "plizzz...."
        }));
        AdvanceQueue();
    }

    public void QuestionMark()
    {
        dialogueQueue.Clear();
        dialogueQueue.Enqueue(new DialogueBlock("PIP (T_T)", new string[] { "Welcome, I'm PIP, your Personal Interface Partner", "Let's get you started", "Try all the buttons you see", "Some works and some don't.." }));
        AdvanceQueue();
    }
}
