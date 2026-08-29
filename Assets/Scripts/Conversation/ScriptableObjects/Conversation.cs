using UnityEngine;

public enum Speaker { NONE, Player, Villager, Merchant, RandomNPC, }

[CreateAssetMenu(fileName = "Conversation", menuName = "ScriptableObjects/Conversation")]
public class Conversation : ScriptableObject
{
    public ConversationData[] conversations;
    public static event System.Action<Conversation> Started;

    public void StartConversation() => Started?.Invoke(this);

#if UNITY_EDITOR
    void OnValidate()
    {
        if (conversations == null || conversations.Length == 0)
        {
            Debug.LogWarning("Conversations was not initialized", this);
            return;
        }

        foreach (ConversationData conversation in conversations)
        {
            if (conversation.speaker == Speaker.NONE)
                Debug.LogWarning("A conversation speaker was set to NONE", this);

            conversation.SetName();
        }
    }
#endif

    [System.Serializable]
    public class ConversationData
    {
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        public void SetName() => name = speaker.ToString();
        #endif
        public Speaker speaker;
        [TextArea(3, 6)] public string[] sentences;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Conversation))]
    private class ConversationEditor : UnityEditor.Editor
    {
        private Conversation Conversation => (Conversation)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);

            GUILayout.Label("Editor Utilities");

            if (GUILayout.Button("Start Conversation"))
                Conversation.StartConversation();
        }
    }
#endif
}
