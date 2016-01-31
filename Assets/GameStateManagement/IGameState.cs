namespace GameStateManagement
{
    public interface IGameState
    {
        void StateStaticInitialize();

        /// <summary>
        /// Methods for determining if state is paused.
        /// </summary>
        bool IsPaused { get; }


        /// <summary>
        /// Use this method to transition into the game state, set everything up...etc.
        /// </summary>
        void EnterState();

        /// <summary>
        /// In the stack of game states, when the top state is popped, this method will be called to "return" to this state. 
        /// </summary>
        /// <param name="state"></param>
        void ReturnToStateFrom(IGameState state);

        /// <summary>
        /// Pauses the state. Called when the game or application are paused. 
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the state. Called when the game or application are resumed.
        /// </summary>
        void Resume();
        
        /// <summary>
        /// Called by GameStateManager when a Unity scene is loaded.
        /// </summary>
        void OnLevelLoaded(int level);       

        /// <summary>
        /// Clean up function. When state is left for good. Don't put transition code here - put it in EnterState() or ReturnToState().
        /// </summary>
        void CleanUp();

        /// <summary>
        /// Utility function for checking type of Game State
        /// </summary>
        bool Is<TGameState>()
            where TGameState : IGameState;
    }
}
