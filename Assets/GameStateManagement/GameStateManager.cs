using System.Collections.Generic;
using UnityEngine;

namespace GameStateManagement
{
    public class GameStateManager : UnitySingleton<GameStateManager>
    {
        private Stack<IGameState> _gameStateStack;        

        public IGameState CurrentGameState { get; private set; }
        
        #region MonoBehaviour
        public override void Awake()
        {
            base.Awake();

            _gameStateStack = new Stack<IGameState>();
            
            Application.logMessageReceived += HandleLog;
            
            //Initial State 
            CurrentGameState = GameStateBase<InitialGameState>.Instance;
            _gameStateStack.Push(CurrentGameState);
            
            CurrentGameState = _gameStateStack.Peek();
        }

        void OnLevelWasLoaded(int level)
        {            
            if(CurrentGameState != null)
                CurrentGameState.OnLevelLoaded(level);
        }

        /// <summary>
        /// If we are in a game state and application is paused, pause the game. 
        /// </summary>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                CurrentGameState.Pause();
            }
        }
        #endregion
       

        /// <summary>
        /// Calls the EnterState() of state we're transfering to, and pushes new state onto stack.
        /// </summary>
        /// <typeparam name="TGameState">State to transfer to</typeparam>
        public void TransitionToGameState<TGameState>()
            where TGameState : IGameState
        {
            if (_gameStateStack == null) //uninitialized GameStateManager
                _gameStateStack = new Stack<IGameState>();

            CurrentGameState.Pause();
            
            CurrentGameState = GameStateBase<TGameState>.Instance;
            CurrentGameState.EnterState();
            CurrentGameState.StateStaticInitialize();

            _gameStateStack.Push(CurrentGameState);            
        }
        

        /// <summary>
        /// Transitions back to previous state, discarding the current state. 
        /// </summary>
        public void PopGameStateStack()
        {
            IGameState previousState = _gameStateStack.Pop();
            CurrentGameState = _gameStateStack.Peek();

            CurrentGameState.ReturnToStateFrom(previousState);
            CurrentGameState.Resume();

            //clean up after returning so that no info that CurrentGameState might need is disposed.
            previousState.CleanUp();
        }
        

        /// <summary>
        /// Make sure that if someone throws an error in the editor, application quits. This enforces a
        /// clean game without hidden exceptions, while making it easier to deal with one issue at a time.        
        /// If something non-game-killing should be noted to the user, please use a warning.
        /// </summary>
        void HandleLog(string logString, string stackTrace, LogType type)
        {
#if UNITY_EDITOR
            if ((type == LogType.Error || type == LogType.Exception) && Debug.isDebugBuild)
            {
                Application.Quit();
            }
#endif
        }

    }
}
