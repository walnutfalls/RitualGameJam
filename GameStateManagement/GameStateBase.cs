using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GameStateManagement
{
    /// <summary>
    /// Game state abstract class (with many default empty methods)
    /// </summary>
    /// <typeparam name="TGameState">This will usually be the class that inherits GameState.</typeparam>
    public abstract class GameStateBase<TGameState> : IGameState
    {
        private static TGameState _instance;

        public event Action OnEnter;        

        public static string GameStateName { get; protected set; }

        public static TGameState Instance
        {
            get
            {
                if (_instance == null)                
                    _instance = ConstructGameStateInstance();
                
                return _instance;
            }
        }


        #region IGameState
        
        /// <summary>
        /// basic implementation, with a boolean flag
        /// </summary>        
        public bool IsPaused { get; private set; }


        /// <summary>
        /// Function for handling state-entering logic that can go
        /// right in the state class, as opposed to other controllers. 
        /// </summary>
        public abstract void StateStaticInitialize();      
        
        /// <summary>
        /// Functionality for returning to this state from any other state
        /// </summary>
        /// <param name="state">A non-specific IGameState child type object.</param>
        public virtual void ReturnToStateFrom(IGameState state) { }

        public virtual void Pause()
        {
            IsPaused = true;
        }

        public virtual void Resume() 
        {
            IsPaused = false;
        }
        
        /// <summary>
        /// Clean up function. Don't put transition code here - put it in EnterState() or ReturnToState().
        /// </summary>
        public virtual void CleanUp() { }

        public virtual void OnLevelLoaded(int level) { }
        
        public bool Is<T>()
            where T : IGameState
        {
            if (this.GetType() == typeof(T))
                return true;

            return false;
        }

        public virtual void EnterState()
        {
            if (OnEnter != null) OnEnter();
        }
        #endregion

        
        //private methods:
        private static TGameState ConstructGameStateInstance()
        {
            Type stateType = typeof(TGameState);
            ConstructorInfo ctor = stateType.GetConstructor(new Type[] { });
            object instance = ctor.Invoke(new object[] { });

            (instance as GameStateBase<TGameState>).IsPaused = false;
            
            return (TGameState)instance;
        }

      
    }
}
