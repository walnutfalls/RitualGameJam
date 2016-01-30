using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;



namespace GameStateManagement
{
    /// <summary>
    /// Persistent global class allowing access to saved player data. 
    /// </summary>
    public abstract class PlayerData<T> : UnitySingleton<PlayerData<T>>
        where T : ICloneable
    {
        /// <summary>
        /// Concatenates the application's path (platform dependent) with a filename for our player data class
        /// </summary>
        public string PlayerInfoPath
        {
            get { return Application.persistentDataPath + "/playerInfo.dat"; }
        }
        
        /// <summary>
        /// Shown in editor: the default (start) data
        /// </summary>
        public T Data;

        private T _currentData;
        public T CurrentData
        { 
            get 
            {
                if (_currentData == null)
                {
                    Load(); //sets _currentData
                }
                return _currentData;
            }
            private set { _currentData = value; }
        }
        
        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(PlayerInfoPath);
            bf.Serialize(file, CurrentData);
            file.Flush();
            file.Close();
        }

        public void Load()
        {
            if (File.Exists(PlayerInfoPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(PlayerInfoPath, FileMode.Open);
                CurrentData = (T)bf.Deserialize(file);
                file.Close();
            }
            else
            {
                //Data should be serialized with useful initial values from the get-go. Save it to disk.               
                _currentData = (T)Data.Clone();
            }            
        }
        
        public void ResetData()
        {
            CurrentData = (T)Data.Clone();
            Save();
        }
    }
}
