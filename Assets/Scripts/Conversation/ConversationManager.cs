using UnityEngine;
using System.Collections;
using _InputManager;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI speakerText, conversationText;
    [SerializeField, Range(0.001f, 0.1f)] private float typeSpeed;

    private readonly System.Func<bool> Condition = () => InputManager.Input.Conversation.Next.triggered;
    private WaitForSeconds typeSpeedDelay; private WaitUntil condition; private bool isBusy;
    // Maybe use a Queue<Conversation> to handle multiple queues, Enqueue when isBusy = true

    void Awake() { typeSpeedDelay = new(typeSpeed); condition = new(Condition); }

    private void PrepareConversation(Conversation conversation)
    {
        if (conversation == null || conversation.HasStarted) return;
        if (conversation.Conversations == null || conversation.Conversations.Length == 0) return;

        StartCoroutine(StartConversation(conversation));

        IEnumerator StartConversation(Conversation conversation)
        {
            if (isBusy || speakerText == null || conversationText == null) yield break;
    
            isBusy = true;

            foreach (Conversation.ConversationData conversationData in conversation.Conversations)
            {
                if (conversationData == null || conversationData.Sentences == null || conversationData.Sentences.Length == 0) continue;

                speakerText.text = conversationData.Speaker.ToString();

                foreach (string sentence in conversationData.Sentences)
                {
                    conversationText.maxVisibleCharacters = 0;
                    conversationText.text = sentence;
                    for (int i = 0; i < sentence.Length; i++)
                    {
                        conversationText.maxVisibleCharacters++;
                        yield return typeSpeedDelay;
                    }

                    yield return condition; yield return null;
                }
            }

            isBusy = false;
        }
    }

    void OnEnable() { Conversation.Started += PrepareConversation; }
    void OnDisable() { Conversation.Started -= PrepareConversation; }
}
