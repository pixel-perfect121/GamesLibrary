using UnityEngine;

public enum DialogueType { Greetings, Farewell, }

[CreateAssetMenu(fileName = "GameDialogue", menuName = "ScriptableObjects/GameDialogue")]
public class GameDialogue : ScriptableObject
{
    [SerializeField] private Dialogue[] dialogues;
    private readonly System.Collections.Generic.Dictionary<DialogueType, Dialogue> dialogueDictionary = new();

    public string GetDialogue(DialogueType dialogueType)
    {
        if (dialogues == null || dialogues.Length == 0) return string.Empty;
        if (!dialogueDictionary.TryGetValue(dialogueType, out Dialogue dialogue) || dialogue == null) return string.Empty;

        return dialogue.GetDialogue();
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("Dialogue was not initialized");
            return;
        }

        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue == null)
            {
                Debug.LogWarning("Some dialogues were not initialized");
                continue;
            }

            dialogue.SetName();
        }
    }
    #endif

    void OnEnable()
    {
        if (dialogues == null || dialogues.Length == 0) return;

        dialogueDictionary.Clear();
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue == null)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Some dialogues were not initialized");
                #endif
                continue;
            }

            if (!dialogueDictionary.TryAdd(dialogue.dialogueType, dialogue))
            {
                #if UNITY_EDITOR
                Debug.LogWarning($"Duplicate dialogue found: {dialogue.dialogueType}");
                #endif
                continue;
            }
        }
    }

    [System.Serializable]
    private class Dialogue
    {
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        public void SetName() => name = dialogueType.ToString();
        #endif
        public DialogueType dialogueType;
        [SerializeField, TextArea(2, 5)] private string[] sentences;

        public string GetDialogue()
        {
            if (sentences == null || sentences.Length == 0) return string.Empty;

            return sentences[Random.Range(0, sentences.Length)];
        }
    }
}
