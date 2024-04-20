using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IDZ.Game
{
    public class SelectionManager : MonoBehaviour
    {
    
        public void OnGameSelectionButtonClicked(int index)
        {
            SceneManager.LoadScene(1);
            DataManager.Instance.SaveData(index, GameConstants.SelectedGameIndex);
            Debug.Log(index);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}


