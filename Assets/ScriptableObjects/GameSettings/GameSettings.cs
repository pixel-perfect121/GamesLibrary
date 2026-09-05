using UnityEngine;
using System.IO;
using System.Text.Json;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(0f, 1f)] public float volume;
    public bool fullScreen;

    private string path;
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true, IncludeFields = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

#if UNITY_EDITOR
    void OnValidate()
    {
        path = Path.Combine(Application.persistentDataPath, "settings.json");

        var @object = new { Volume = volume, Fullscreen = fullScreen };

        string json = JsonSerializer.Serialize(@object, options);
        File.WriteAllText(path, json);
    }
#endif
}
