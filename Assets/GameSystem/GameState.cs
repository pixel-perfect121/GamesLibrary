/// <summary>All available states the game can be in, can be changed through GameState class.</summary>
public enum GameStates { Resumed, Paused, Dialogue, }

/// <summary>Class responsible for storing and changing the state of the game.</summary>
public sealed class GameState
{
    #region Singleton
    /// <summary>Private constructor to ensure no one can instantiate a copy of the class.</summary>
    private GameState() { }
    /// <summary>Initialized instance of the class.</summary>
    private static readonly GameState shared = new();
    /// <summary>Provides ease of access to the class.</summary>
    public static GameState Shared => shared;
    #endregion

    /// <summary>Current state of the game, only changed inside its containing class.</summary>
    public GameStates CurrentState { get; private set; }

    /// <summary>Event carrying information about the current state.</summary>
    public event System.Action<GameStates> Changed;
    /// <summary>Event carrying information about the previous and current state.</summary>
    public event System.Action<GameStates, GameStates> Transitioned;

    /// <summary>Allows changing the state of the game and invokes both Changed and Transitioned events.</summary>
    /// <param name="newState">The next state that the game will be in.</param>
    /// <remarks>If newState is already the current state of the game, nothing will happen.</remarks>
    public void ChangeState(GameStates newState)
    {
        if (newState == CurrentState) return;

        GameStates previousState = CurrentState;
        CurrentState = newState;

        Changed?.Invoke(CurrentState);
        Transitioned?.Invoke(previousState, CurrentState);
    }
}
