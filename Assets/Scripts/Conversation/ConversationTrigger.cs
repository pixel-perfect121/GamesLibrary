using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{
    [SerializeField] private Conversation conversation;

    public void TriggerConversation()
    {
        if (conversation == null) return;

        conversation.StartConversation();
        if (conversation.HasStarted) conversation = null;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ConversationTrigger))]
    private class ConversationTriggerEditor : UnityEditor.Editor
    {
        private ConversationTrigger ConversationTrigger => (ConversationTrigger)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);

            GUILayout.Label("Editor Utilities");

            if (GUILayout.Button("Trigger Conversation"))
            {
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    Debug.LogWarning("Cannot start conversation while not playing");
                    return;
                }

                ConversationTrigger.TriggerConversation();
            }
        }
    }
#endif
}
