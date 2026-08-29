using UnityEngine;
using System.Collections;
using _InputManager;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI speakerText, conversationText;

    private bool isBusy;

    private void PrepareConversation(Conversation conversation)
    {
        if (conversation == null || conversation.conversations == null || conversation.conversations.Length == 0) return;

        StartCoroutine(StartConversation(conversation));
    }
    private IEnumerator StartConversation(Conversation conversation)
    {
        if (isBusy)
        {
            Debug.LogWarning("Conversation manager is busy");
            yield break;
        }
        if (speakerText == null || conversationText == null)
        {
            Debug.LogError("Null references");
            yield break;
        }

        isBusy = true;

        Debug.Log("Starting conversation");

        foreach (Conversation.ConversationData conversationData in conversation.conversations)
        {
            if (conversationData == null || conversationData.sentences == null || conversationData.sentences.Length == 0) continue;

            speakerText.text = conversationData.speaker.ToString();

            foreach (string sentence in conversationData.sentences)
            {
                conversationText.text = sentence;

                yield return new WaitUntil(() => InputManager.Input.Conversation.Next.triggered);
                yield return null;
            }
        }

        isBusy = false;
    }

    void OnEnable() { Conversation.Started += PrepareConversation; }
    void OnDisable() { Conversation.Started -= PrepareConversation; }
}
