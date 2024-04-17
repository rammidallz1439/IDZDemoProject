using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace IDZ
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void SaveData<T>(T content, string fileName)
        {
            string directoryPath = Application.persistentDataPath;
            string filePath = Path.Combine(directoryPath, fileName);


            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fileStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, content);
            }
        }
        public void Load<T>(T data, string fileName)
        {
            string dathPath = Application.persistentDataPath;
            string filePath = Path.Combine(dathPath, fileName);
            if (Directory.Exists(dathPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (File.Exists(filePath))
                {
                    FileStream fileStream = File.Open(filePath, FileMode.Open);
                    data = (T)formatter.Deserialize(fileStream);
                    fileStream.Close();
                }



            }
            else
            {
                Directory.CreateDirectory(dathPath);
                Debug.LogWarning("Save file not found. Using default values.");
            }

        }
    }
}
