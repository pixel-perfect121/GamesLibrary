using UnityEngine;

public enum Speaker { NONE, Player, Villager, Merchant, RandomNPC, }

[CreateAssetMenu(fileName = "Conversation", menuName = "ScriptableObjects/Conversation")]
public class Conversation : ScriptableObject
{
    [field: SerializeField] public ConversationData[] Conversations { get; private set; }

    public static event System.Action<Conversation> Started;
    [HideInInspector] public bool HasStarted { get; private set; }

    public void StartConversation() { Started?.Invoke(this); HasStarted = true; }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Conversations == null || Conversations.Length == 0)
        {
            Debug.LogWarning("Conversations was not initialized", this);
            return;
        }

        foreach (ConversationData conversation in Conversations)
        {
            if (conversation == null) Debug.LogWarning($"{conversation.Speaker} is null", this);
            if (conversation.Speaker == Speaker.NONE) Debug.LogWarning("A conversation speaker was set to NONE", this);

            conversation.SetName();
        }
    }
#endif

    [System.Serializable]
    public class ConversationData
    {
        #if UNITY_EDITOR
        [SerializeField, HideInInspector] private string name;
        public void SetName() => name = Speaker.ToString();
        #endif
        [field: SerializeField] public Speaker Speaker { get; private set; }
        [field: SerializeField, TextArea(3, 6)] public string[] Sentences { get; private set; }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Conversation))]
    private class ConversationEditor : UnityEditor.Editor
    {
        private Conversation Conversation => target as Conversation;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);

            GUILayout.Label("Editor Utilities");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Conversation"))
            {
                if (Conversation.HasStarted)
                {
                    Debug.LogWarning($"Conversation named <b>{Conversation.name}</b> has already started");
                    return;
                }

                Conversation.StartConversation();
                Debug.Log($"Conversation named <b>{Conversation.name}</b> has started");
            }
            if (GUILayout.Button("Reset conversation"))
            {
                if (!Conversation.HasStarted)
                {
                    Debug.LogWarning($"Conversation named <b>{Conversation.name}</b> has already been reset");
                    return;
                }

                Conversation.HasStarted = false;
                Debug.Log($"Conversation named <b>{Conversation.name}</b> has been reset");
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
}
