using UnityEngine;
using System.Collections;
using _InputManager;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI speakerText, conversationText;

    private event System.Func<bool> Condition = () => InputManager.Input.Conversation.Next.triggered;

    [SerializeField, Range(0.001f, 0.1f)] private float typeSpeed;
    private WaitForSeconds delay;
    private WaitUntil condition;
    private bool isBusy;

    void Awake()
    {
        delay = new(typeSpeed);
        condition = new(Condition);
    }

    private void PrepareConversation(Conversation conversation)
    {
        if (conversation == null || conversation.conversations == null || conversation.conversations.Length == 0) return;

        StartCoroutine(StartConversation(conversation));
    }
    private IEnumerator StartConversation(Conversation conversation)
    {
        if (isBusy || speakerText == null || conversationText == null) yield break;

        isBusy = true;

        foreach (Conversation.ConversationData conversationData in conversation.conversations)
        {
            if (conversationData == null || conversationData.sentences == null || conversationData.sentences.Length == 0) continue;

            speakerText.text = conversationData.speaker.ToString();

            foreach (string sentence in conversationData.sentences)
            {
                conversationText.maxVisibleCharacters = 0;
                conversationText.text = sentence;
                for (int i = 0; i < sentence.Length; i++)
                {
                    conversationText.maxVisibleCharacters++;
                    yield return delay;
                }

                yield return condition; yield return null;
            }
        }

        isBusy = false;
    }

    void OnEnable() { Conversation.Started += PrepareConversation; }
    void OnDisable() { Conversation.Started -= PrepareConversation; }
}
