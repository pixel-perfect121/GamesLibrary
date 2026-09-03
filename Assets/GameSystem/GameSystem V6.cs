using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace _SaveManager
{
    public enum Method { Sync, Async }

    public static class SaveManager
    {
        public static GameData GameData { get; private set; } = new();
        private static readonly string path = Path.Combine(Application.persistentDataPath, "Save.json");

        private static readonly JsonSerializerOptions saveOptions = new()
        {
            IncludeFields = true, WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters = { new Vector2Converter(), new Vector3Converter(), new ColorConverter() }
        };

        private static readonly System.Threading.SemaphoreSlim saveLock = new(1, 1);

        private const int ATTEMPTS = 3, DELAY = 15;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() => RequestLoad(Method.Sync);

        public static void RequestSave(Method saveMethod)
        {
            switch (saveMethod)
            {
                case Method.Sync: GlobalSave(); break;
                case Method.Async: _ = GlobalSaveAsync(); break;
                default: Debug.LogError("Error while trying to save Game Data"); break;
            }
        }
        public static void RequestLoad(Method loadMethod)
        {
            switch (loadMethod)
            {
                case Method.Sync: GlobalLoad(); break;
                case Method.Async: _ = GlobalLoadAsync(); break;
                default: Debug.LogError("Error while trying to load Game Data"); break;
            }
        }

        public static void DeleteGameData() { GameData = new(); RequestSave(Method.Async); }

        #region Global Save&Load
        private static void GlobalSave()
        {
            int attempts = 0;
            while (attempts < ATTEMPTS)
            {
                try
                {
                    attempts++;
                    saveLock.Wait();

                    string json = JsonSerializer.Serialize(GameData, saveOptions);
                    File.WriteAllText(path, json); return;
                }
                catch (System.Exception /*message*/)
                {
                    Debug.LogError($"Saving failed, trying again: {attempts}");

                    if (attempts >= ATTEMPTS) Debug.Log("All Saving attempts failed, returning defaults");
                }
                finally { saveLock.Release(); }
            }
            GameData = new();
            File.WriteAllText(path, JsonSerializer.Serialize(new GameData(), saveOptions));
        }
        private static void GlobalLoad()
        {
            if (!File.Exists(path)) { GameData = new(); GlobalSave(); return; }

            int attempts = 0;
            while (attempts < ATTEMPTS)
            {
                try
                {
                    attempts++;
                    saveLock.Wait();

                    string json = File.ReadAllText(path);
                    GameData = JsonSerializer.Deserialize<GameData>(json, saveOptions) ?? new();
                    return;
                }
                catch (System.Exception /*message*/)
                {
                    Debug.LogError($"Loading failed, trying again: {attempts}");

                    if (attempts >= ATTEMPTS) Debug.Log("All loading attempts failed, returning defaults");
                }
                finally { saveLock.Release(); }
            }
            GameData = new(); GlobalSave();
        }

        private static async Task GlobalSaveAsync()
        {
            int attempts = 0;
            while (attempts < ATTEMPTS)
            {
                try
                {
                    attempts++;
                    await saveLock.WaitAsync();

                    string json = JsonSerializer.Serialize(GameData, saveOptions);
                    await File.WriteAllTextAsync(path, json); return;
                }
                catch (System.Exception /*message*/)
                {
                    Debug.LogError($"Saving failed, trying again: {attempts}");

                    if (attempts >= ATTEMPTS) Debug.Log("All Saving attempts failed, returning defaults");

                    await Task.Delay(DELAY);
                }
                finally { saveLock.Release(); }
            }
            GameData = new();
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(new GameData(), saveOptions));
        }
        private static async Task GlobalLoadAsync()
        {
            if (!File.Exists(path)) { GameData = new(); await GlobalSaveAsync(); return; }

            int attempts = 0;
            while (attempts < ATTEMPTS)
            {
                try
                {
                    attempts++;
                    await saveLock.WaitAsync();

                    string json = await File.ReadAllTextAsync(path);
                    GameData = JsonSerializer.Deserialize<GameData>(json, saveOptions) ?? new();
                    return;
                }
                catch (System.Exception /*message*/)
                {
                    Debug.LogError($"Loading failed, trying again: {attempts}");

                    if (attempts >= ATTEMPTS) Debug.Log("All loading attempts failed, returning defaults");

                    await Task.Delay(DELAY);
                }
                finally { saveLock.Release(); }
            }
            GameData = new(); await GlobalSaveAsync(); return;
        }
        #endregion

        #region JSON converters
        private class Vector2Converter : JsonConverter<Vector2>
        {
            public override Vector2 Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
            {
                float x = 0f, y = 0f;

                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) return new(x, y);

                    if (reader.TokenType != JsonTokenType.PropertyName) continue;

                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "x": x = reader.GetSingle(); break;
                        case "y": y = reader.GetSingle(); break;
                    }
                }
                throw new JsonException();
            }
            public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteNumber("x", value.x);
                writer.WriteNumber("y", value.y);

                writer.WriteEndObject();
            }
        }
        private class Vector3Converter : JsonConverter<Vector3>
        {
            public override Vector3 Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
            {
                float x = 0f, y = 0f, z = 0f;

                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) return new(x, y, z);
                    if (reader.TokenType != JsonTokenType.PropertyName) continue;

                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "x": x = reader.GetSingle(); break;
                        case "y": y = reader.GetSingle(); break;
                        case "z": z = reader.GetSingle(); break;
                    }
                }
                throw new JsonException();
            }
            public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteNumber("x", value.x);
                writer.WriteNumber("y", value.y);
                writer.WriteNumber("z", value.z);

                writer.WriteEndObject();
            }
        }
        private class ColorConverter : JsonConverter<Color>
        {
            public override Color Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
            {
                float r = 0f, g = 0f, b = 0f, a = 0f;

                if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject) return new(r, g, b, a);

                    if (reader.TokenType != JsonTokenType.PropertyName) continue;

                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "r": r = reader.GetSingle(); break;
                        case "g": g = reader.GetSingle(); break;
                        case "b": b = reader.GetSingle(); break;
                        case "a": a = reader.GetSingle(); break;
                    }
                }
                throw new JsonException();
            }
            public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteNumber("r", value.r);
                writer.WriteNumber("g", value.g);
                writer.WriteNumber("b", value.b);
                writer.WriteNumber("a", value.a);

                writer.WriteEndObject();
            }
        }
        #endregion
    }

    #region Data
    [System.Serializable]
    public class GameData
    {
        public Dictionary<string, GameInfo> GameInfoDictionary { get; set; } = new();
    }
    #endregion
}

namespace _InputManager
{
    public enum GameMap { }
    public enum Condition { Up, Down, Held }

    public static class InputManager
    {
        public static GameControls Input { get; private set; } = new();
        public static InputActionAsset Asset => Input.asset;

        public static Keyboard Keyboard { get; private set; } = Keyboard.current;
        public static Mouse Mouse { get; private set; } = Mouse.current;
        public static Gamepad Gamepad { get; private set; } = Gamepad.current;
        public static Joystick Joystick { get; private set; } = Joystick.current;
        private static InputActionRebindingExtensions.RebindingOperation operation = new();

        public static KeyModifiers Modifiers { get; private set; } = new();

        private const string SAVEKEY = "Rebinds";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Input.Enable(); Asset.Enable();

            LoadKeybinds();

            InputSystem.onDeviceChange += DetectDevice;
            static void DetectDevice(InputDevice device, InputDeviceChange change)
            {
                foreach (InputDevice dv in InputSystem.devices)
                { if (dv == null) Debug.Log("No device detected, please connect a device."); }

                if (device is Keyboard ckb)
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added: Keyboard = ckb; break;
                        case InputDeviceChange.Removed: Keyboard = null; break;
                        case InputDeviceChange.Reconnected: Keyboard = ckb; break;
                        case InputDeviceChange.Disconnected: Keyboard = null; break;
                    }
                }
                if (device is Mouse cms)
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added: Mouse = cms; break;
                        case InputDeviceChange.Removed: Mouse = null; break;
                        case InputDeviceChange.Reconnected: Mouse = cms; break;
                        case InputDeviceChange.Disconnected: Mouse = null; break;
                    }
                }
                if (device is Gamepad cgp)
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added: Gamepad = cgp; break;
                        case InputDeviceChange.Removed: Gamepad = null; break;
                        case InputDeviceChange.Reconnected: Gamepad = cgp; break;
                        case InputDeviceChange.Disconnected: Gamepad = null; break;
                    }
                }
                if (device is Joystick cjs)
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added: Joystick = cjs; break;
                        case InputDeviceChange.Removed: Joystick = null; break;
                        case InputDeviceChange.Reconnected: Joystick = cjs; break;
                        case InputDeviceChange.Disconnected: Joystick = null; break;
                    }
                }
            }
        }

        #region Map settings
        public static void EnableMap(GameMap map) { Asset.FindActionMap(map.ToString()).Enable(); }
        public static void DisableMap(GameMap map) { Asset.FindActionMap(map.ToString()).Disable(); }
        public static void EnableOnly(GameMap map) { DisableAll(); EnableMap(map); }
        public static void DisableOnly(GameMap map) { EnableAll(); DisableMap(map); }
        public static void EnableAll() { foreach (InputActionMap map in Asset.actionMaps) map.Enable(); }
        public static void DisableAll() { foreach (InputActionMap map in Asset.actionMaps) map.Disable(); }
        public static bool IsItOn(GameMap map) { return Asset.FindActionMap(map.ToString()).enabled; }
        #endregion

        #region Get functions
        public static bool GetKeyboardKey(Key key, Condition condition)
        {
            if (Keyboard == null) return false;
            //if (key is Key.Escape) return false;

            return condition switch
            {
                Condition.Up => Keyboard[key].wasReleasedThisFrame,
                Condition.Down => Keyboard[key].wasPressedThisFrame,
                Condition.Held => Keyboard[key].isPressed,
                _ => false
            };
        }
        public static bool AnyKeyboardKey(Condition condition)
        {
            if (Keyboard == null) return false;
            //if (key is Key.Escape) return false;

            return condition switch
            {
                Condition.Up => Keyboard.anyKey.wasReleasedThisFrame,
                Condition.Down => Keyboard.anyKey.wasPressedThisFrame,
                Condition.Held => Keyboard.anyKey.isPressed,
                _ => false
            };
        }
        public static bool GetGamepadButton(GamepadButton button, Condition condition)
        {
            if (Gamepad == null) return false;

            return condition switch
            {
                Condition.Up => Gamepad[button].wasReleasedThisFrame,
                Condition.Down => Gamepad[button].wasPressedThisFrame,
                Condition.Held => Gamepad[button].isPressed,
                _ => false
            };
        }
        public static bool AnyGamepadButton(Condition condition)
        {
            if (Gamepad == null) return false;

            foreach (GamepadButton button in System.Enum.GetValues(typeof(GamepadButton)))
            {
                switch (condition)
                {
                    case Condition.Up: if (Gamepad[button].wasReleasedThisFrame) return true; break;
                    case Condition.Down: if (Gamepad[button].wasPressedThisFrame) return true; break;
                    case Condition.Held: if (Gamepad[button].isPressed) return true; break;
                    default: return false;
                }
            }
            return false;
        }
        public static bool GetMouseButton(MouseButton button, Condition condition)
        {
            if (Mouse == null) return false;

            switch (condition)
            {
                case Condition.Up:
                    {
                        return button switch
                        {
                            MouseButton.Left => Mouse.leftButton.wasReleasedThisFrame,
                            MouseButton.Right => Mouse.rightButton.wasReleasedThisFrame,
                            MouseButton.Middle => Mouse.middleButton.wasReleasedThisFrame,
                            MouseButton.Forward => Mouse.forwardButton.wasReleasedThisFrame,
                            MouseButton.Back => Mouse.backButton.wasReleasedThisFrame,
                            _ => false,
                        };
                    }
                case Condition.Down:
                    {
                        return button switch
                        {
                            MouseButton.Left => Mouse.leftButton.wasPressedThisFrame,
                            MouseButton.Right => Mouse.rightButton.wasPressedThisFrame,
                            MouseButton.Middle => Mouse.middleButton.wasPressedThisFrame,
                            MouseButton.Forward => Mouse.forwardButton.wasPressedThisFrame,
                            MouseButton.Back => Mouse.backButton.wasPressedThisFrame,
                            _ => false,
                        };
                    }
                case Condition.Held:
                    {
                        return button switch
                        {
                            MouseButton.Left => Mouse.leftButton.isPressed,
                            MouseButton.Right => Mouse.rightButton.isPressed,
                            MouseButton.Middle => Mouse.middleButton.isPressed,
                            MouseButton.Forward => Mouse.forwardButton.isPressed,
                            MouseButton.Back => Mouse.backButton.isPressed,
                            _ => false,
                        };
                    }
            }
            return false;
        }
        public static bool AnyMouseButton(Condition condition)
        {
            if (Mouse == null) return false;

            foreach (InputControl control in Mouse.allControls)
            {
                if (control is not ButtonControl button) continue;

                switch (condition)
                {
                    case Condition.Up: if (button.wasReleasedThisFrame) return true; break;
                    case Condition.Down: if (button.wasPressedThisFrame) return true; break;
                    case Condition.Held: if (button.isPressed) return true; break;
                    default: return false;
                }
            }
            return false;
        }
        public static float GetAxisRaw(string axisName)
        {
            if (Keyboard == null) return 0f;

            switch (axisName)
            {
                case "Horizontal":
                    {
                        if (GetKeyboardKey(Key.LeftArrow, Condition.Held) || GetKeyboardKey(Key.A, Condition.Held)) return -1f;
                        if (GetKeyboardKey(Key.RightArrow, Condition.Held) || GetKeyboardKey(Key.D, Condition.Held)) return 1f;
                        if (Gamepad != null)
                        {
                            if (GetGamepadButton(GamepadButton.DpadLeft, Condition.Held) || Gamepad.leftStick.left.isPressed) return -1f;
                            if (GetGamepadButton(GamepadButton.DpadRight, Condition.Held) || Gamepad.leftStick.right.isPressed) return 1f;
                        }
                        break;
                    }
                case "Vertical":
                    {
                        if (GetKeyboardKey(Key.UpArrow, Condition.Held) || GetKeyboardKey(Key.W, Condition.Held)) return 1f;
                        if (GetKeyboardKey(Key.DownArrow, Condition.Held) || GetKeyboardKey(Key.S, Condition.Held)) return -1f;
                        if (Gamepad != null)
                        {
                            if (GetGamepadButton(GamepadButton.DpadUp, Condition.Held) || Gamepad.leftStick.up.isPressed) return 1f;
                            if (GetGamepadButton(GamepadButton.DpadDown, Condition.Held) || Gamepad.leftStick.down.isPressed) return -1f;
                        }
                        break;
                    }
                default: return 0f;
            }
            return 0f;
        }
        public static Vector3 MousePosition(Camera cam = null, bool ignoreZ = true)
        {
            if (Mouse == null) return Vector3.zero;
            Vector3 mousePosition = Mouse.position.ReadValue();

            if (cam == null) return Mouse.position.ReadValue();
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(mousePosition);

            if (cam.orthographic) return new(worldMousePosition.x, worldMousePosition.y, ignoreZ ? 0f : cam.transform.position.z);

            Ray ray = cam.ScreenPointToRay(mousePosition);
            Plane plane = new(Vector3.forward, Vector3.zero);
            if (plane.Raycast(ray, out float distance)) return new(ray.GetPoint(distance).x, ray.GetPoint(distance).y, ignoreZ ? 0f : cam.transform.position.z);

            return Vector3.zero;
        }
        #endregion

        #region Rebind
        public static void Rebind(InputAction action, int index, System.Action OnStart = null, System.Action OnComplete = null, bool includeMouse = false)
        {
            if (operation != null) { operation.Cancel(); operation.Dispose(); }

            action.Disable(); OnStart?.Invoke();

            operation = action.PerformInteractiveRebinding(index).WithControlsExcluding(includeMouse ? " " : "Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .WithTimeout(5f)
                .OnComplete(op =>
                {
                    action.Enable(); op.Dispose();
                    SaveKeybinds(); OnComplete?.Invoke();
                })
                .OnCancel(op => { action.Enable(); op.Dispose(); OnComplete?.Invoke(); })
                .Start();
        }

        public static InputAction FindAction(InputActionReference actionRef) { return Input.FindAction(actionRef.name); }

        private static void SaveKeybinds() { PlayerPrefs.SetString(SAVEKEY, Input.SaveBindingOverridesAsJson()); }
        private static void LoadKeybinds()
        {
            string json = PlayerPrefs.GetString(SAVEKEY, string.Empty);
            if (!string.IsNullOrEmpty(json)) Input.LoadBindingOverridesFromJson(json);
        }
        public static void ResetBindings() { PlayerPrefs.DeleteKey(SAVEKEY); Input.RemoveAllBindingOverrides(); }
        #endregion

        public struct KeyModifiers
        {
            public bool LeftControl => GetKeyboardKey(Key.LeftCtrl, Condition.Held);
            public bool RightControl => GetKeyboardKey(Key.RightCtrl, Condition.Held);

            public bool LeftShift => GetKeyboardKey(Key.LeftShift, Condition.Held);
            public bool RightShift => GetKeyboardKey(Key.RightShift, Condition.Held);

            public bool LeftAlt => GetKeyboardKey(Key.LeftAlt, Condition.Held);
            public bool RightAlt => GetKeyboardKey(Key.RightAlt, Condition.Held);
        }
    }
}

namespace _AudioManager
{
    public enum SoundType { }

    public static class AudioManager
    {
        private static readonly Dictionary<SoundType, AudioBook> audioDictionary = new();

        private static bool isSwitching;

        public static void PlaySFX(SoundType soundType, AudioSource source)
        {
            if (!Available(soundType, out AudioBook audioBook) || audioBook.isMusic) return;

            source.clip = GetClipRandom(soundType);
            source.pitch = Random.Range(1f - audioBook.pitch, 1f + audioBook.pitch);
            source.volume = audioBook.volume; source.loop = audioBook.loop; source.Play();
        }
        public static IEnumerator PlayMusic(SoundType soundType, AudioSource musicSource)
        {
            if (!Available(soundType, out AudioBook audioBook) || !audioBook.isMusic) yield break;

            isSwitching = true;

            float elapsed = 0f;
            if (musicSource.isPlaying)
            {
                float currentVolume = audioBook.volume;
                while (elapsed <= 1f)
                {
                    musicSource.volume = Mathf.Lerp(currentVolume, 0f, elapsed);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                musicSource.volume = 0f; musicSource.Stop();
            }

            musicSource.loop = audioBook.loop;
            musicSource.clip = audioBook.clips[Random.Range(0, audioBook.clips.Length)];
            musicSource.Play();

            elapsed = 0f;
            while (elapsed <= 1f)
            {
                musicSource.volume = Mathf.Lerp(0f, audioBook.volume, elapsed);
                elapsed += Time.deltaTime;
                yield return null;
            }
            musicSource.volume = audioBook.volume;

            isSwitching = false;
        }
        public static IEnumerator StopMusic(AudioSource musicSource)
        {
            if (isSwitching) yield break;

            isSwitching = true;

            float elapsed = 0;
            if (musicSource.isPlaying)
            {
                float currentVolume = musicSource.volume;
                while (elapsed <= 1f)
                {
                    musicSource.volume = Mathf.Lerp(currentVolume, 0f, elapsed);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                musicSource.volume = 0f; musicSource.Stop();
            }

            isSwitching = false;
        }

        public static AudioClip GetClipByName(SoundType soundType, string clipName)
        {
            if (!Available(soundType, out AudioBook audioBook)) return null;

            for (int i = 0; i < audioBook.clips.Length; i++)
                if (audioBook.clips[i].name == clipName) return audioBook.clips[i];

            return null;
        }
        public static AudioClip GetClipRandom(SoundType soundType)
        {
            if (!Available(soundType, out AudioBook audioBook)) return null;

            return audioBook.clips[Random.Range(0, audioBook.clips.Length)];
        }

        private static bool Available(SoundType soundType, out AudioBook audioBook)
        {
            audioBook = null;

            if (isSwitching) return false;
            if (!audioDictionary.TryGetValue(soundType, out audioBook) ||
                audioBook.clips.Length == 0) return false;

            return true;
        }

        #region Get&Set volume
        //public static void SetMasterVolume(float audioLevel)
        //{
        //    if (AudioMixer == null) return;

        //    AudioMixer.SetFloat(MASTERVOLUME, Mathf.Log10(audioLevel) * 20f);
        //}
        //public static void SetMusicVolume(float audioLevel)
        //{
        //    if (AudioMixer == null) return;

        //    AudioMixer.SetFloat(MUSICVOLUME, Mathf.Log10(audioLevel) * 20f);
        //}
        //public static void SetSFXVolume(float audioLevel)
        //{
        //    if (AudioMixer == null) return;

        //    AudioMixer.SetFloat(SFXVOLUME, Mathf.Log10(audioLevel) * 20f);
        //}

        //public static void SetVolume(string mixerName, float audioLevel)
        //{
        //    if (AudioMixer == null) return;

        //    AudioMixer.SetFloat(mixerName, Mathf.Log10(audioLevel) * 20f);
        //}
        #endregion

        public static void Register(AudioBook book)
        {
            if (audioDictionary.ContainsKey(book.soundType)) return;

            audioDictionary.Add(book.soundType, book);
        }
        public static void Unregister(AudioBook book)
        {
            if (!audioDictionary.ContainsKey(book.soundType)) return;

            audioDictionary.Remove(book.soundType);
        }
    }

    [System.Serializable]
    public class AudioBook
    {
        public SoundType soundType;
        public AudioClip[] clips;

        [Range(0f, 1f)] public float volume, pitch;
        public bool loop, isMusic;
    }
}

namespace _UIManager
{
    public enum PanelType { }

    public static class UIManager
    {
        private static readonly Dictionary<PanelType, Panel> panelDictionary = new();

        private static PanelType currentPanel;
        private static bool isSwitching;

        public static void Initialize(PanelType mainMenuPanel)
        {
            foreach (var pairs in panelDictionary)
            {
                if (pairs.Value == null) continue;

                pairs.Value.canvasGroup.alpha = 0f;
                pairs.Value.canvasGroup.gameObject.SetActive(false);
            }

            if (!panelDictionary.TryGetValue(mainMenuPanel, out Panel mainMenu)) return;

            mainMenu.canvasGroup.alpha = 1f;
            mainMenu.canvasGroup.gameObject.SetActive(true);
            currentPanel = mainMenuPanel;
        }

        public static void Switch(PanelType nextPanel, CanvasGroup fadePanel, System.Action OnStart = null, System.Action OnComplete = null)
        {
            if (!Available(nextPanel, out Panel current, out Panel next)) return;

            isSwitching = true;

            fadePanel.gameObject.SetActive(true); fadePanel.blocksRaycasts = true;
            OnStart?.Invoke();

            foreach (var pairs in panelDictionary) pairs.Value?.canvasGroup.gameObject.SetActive(false);

            current.canvasGroup.gameObject.SetActive(false); current.canvasGroup.alpha = 0f;

            next.canvasGroup.alpha = 1f; next.canvasGroup.gameObject.SetActive(true);
            if (next.defaultButton != null) OnComplete?.Invoke();

            currentPanel = next.panelType;

            fadePanel.blocksRaycasts = false; fadePanel.gameObject.SetActive(false);

            isSwitching = false;
        }
        public static IEnumerator SwitchFade(PanelType nextPanel, CanvasGroup fadePanel, float fadeDuration, float holdDuration, System.Action OnStart = null, System.Action OnComplete = null)
        {
            if (!Available(nextPanel, out Panel current, out Panel next)) yield break;

            isSwitching = true;

            fadePanel.gameObject.SetActive(true); fadePanel.blocksRaycasts = true;

            OnStart?.Invoke();

            float elapsed = 1f;
            while (elapsed >= 0f)
            {
                current.canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed);
                elapsed -= Time.deltaTime / (fadeDuration != 0f ? fadeDuration : 0.001f);
                yield return null;
            }

            current.canvasGroup.alpha = 0f; current.canvasGroup.gameObject.SetActive(false);

            yield return new WaitForSeconds(holdDuration != 0f ? holdDuration : 0.001f);

            next.canvasGroup.gameObject.SetActive(true);

            elapsed = 0;
            while (elapsed <= 1f)
            {
                next.canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed);
                elapsed += Time.deltaTime / (fadeDuration != 0f ? fadeDuration : 0.001f);
                yield return null;
            }

            next.canvasGroup.alpha = 1f;
            if (next.defaultButton != null) OnComplete?.Invoke();

            currentPanel = next.panelType;

            fadePanel.blocksRaycasts = false; fadePanel.gameObject.SetActive(false);

            isSwitching = false;
        }
        public static IEnumerator SwitchCrossfade(PanelType nextPanel, CanvasGroup fadePanel, float fadeDuration, System.Action OnStart = null, System.Action OnComplete = null)
        {
            if (!Available(nextPanel, out Panel current, out Panel next)) yield break;

            isSwitching = true;

            fadePanel.gameObject.SetActive(true); fadePanel.blocksRaycasts = true;

            OnStart?.Invoke();

            next.canvasGroup.gameObject.SetActive(true); next.canvasGroup.alpha = 0f;

            float elapsed = 1f;
            while (elapsed >= 0f)
            {
                current.canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed);
                next.canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed);
                elapsed -= Time.deltaTime / (fadeDuration != 0f ? fadeDuration : 0.001f);
                yield return null;
            }

            current.canvasGroup.alpha = 0f; current.canvasGroup.gameObject.SetActive(false);

            next.canvasGroup.alpha = 1f;
            if (next.defaultButton != null) OnComplete?.Invoke();

            currentPanel = next.panelType;

            fadePanel.blocksRaycasts = false; fadePanel.gameObject.SetActive(false);

            isSwitching = false;
        }
        public static IEnumerator DipToBlack(PanelType nextPanel, CanvasGroup fadePanel, float fadeDuration, float holdDuration, System.Action OnStart = null, System.Action OnComplete = null)
        {
            if (!Available(nextPanel, out Panel current, out Panel next)) yield break;

            isSwitching = true;

            fadePanel.gameObject.SetActive(true); fadePanel.blocksRaycasts = true;

            OnStart?.Invoke();

            float elapsed = 0f;
            while (elapsed <= 1f)
            {
                fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed);
                elapsed += Time.deltaTime / (fadeDuration != 0f ? fadeDuration : 0.001f);
                yield return null;
            }
            fadePanel.alpha = 1f;

            current.canvasGroup.alpha = 0f; current.canvasGroup.gameObject.SetActive(false);
            next.canvasGroup.gameObject.SetActive(true); next.canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(holdDuration != 0f ? holdDuration : 0.001f);

            elapsed = 1f;
            while (elapsed >= 0f)
            {
                fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed);
                elapsed -= Time.deltaTime / (fadeDuration != 0f ? fadeDuration : 0.001f);
                yield return null;
            }

            if (next.defaultButton != null) OnComplete?.Invoke();

            currentPanel = next.panelType;

            fadePanel.alpha = 0f; fadePanel.blocksRaycasts = true; fadePanel.gameObject.SetActive(false);

            isSwitching = false;
        }

        public static Panel GetPanel(PanelType panelType)
        {
            if (!panelDictionary.TryGetValue(panelType, out Panel panel)) return null;

            return panel;
        }

        private static bool Available(PanelType nextPanel, out Panel current, out Panel next)
        {
            current = next = null;

            if (isSwitching) return false;
            if (!panelDictionary.TryGetValue(currentPanel, out current) || current == null) return false;
            if (!panelDictionary.TryGetValue(nextPanel, out next) || next == null) return false;
            if (current == next) return false;

            return true;
        }

        public static void Register(Panel panel)
        {
            if (panelDictionary.ContainsKey(panel.panelType)) return;

            panelDictionary.Add(panel.panelType, panel);
        }
        public static void Unregister(Panel panel)
        {
            if (!panelDictionary.ContainsKey(panel.panelType)) return;

            panelDictionary.Remove(panel.panelType);
        }
    }

    [System.Serializable]
    public class Panel
    {
        public PanelType panelType;
        public CanvasGroup canvasGroup;
        public GameObject defaultButton;
    }
}

namespace _Inheritance
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool persistInScenes = true;
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }

            Instance = this as T;
            if (persistInScenes) DontDestroyOnLoad(gameObject);
        }
    }

    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public T Owner { get; private set; }

        public StateMachine(T owner) => Owner = owner;

        public void ChangeState(State<T> nextState)
        {
            CurrentState?.Exit();
            CurrentState = nextState;
            CurrentState?.Enter();
        }
        public void ProcessState() => CurrentState?.Process();
    }

    #region Abstracts
    public abstract class State<T>
    {
        protected StateMachine<T> StateMachine { get; private set; }
        protected T Owner => StateMachine.Owner;

        public State(StateMachine<T> stateMachine) => StateMachine = stateMachine;

        public abstract void Enter();
		public virtual void Process() { }
		public virtual void LateProcess() { }
		public virtual void FixedProcess() { }
        public abstract void Exit();
    }
    #endregion
}
