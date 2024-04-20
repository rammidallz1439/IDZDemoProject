using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IDZ.Game
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameSets = new List<GameObject>();
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;
        int selectedGameIndex;
        GameObject currentObj;

        public static Action SwitchNextButtonevent;
        public static Action<bool> ResultPopupEvent;

        private void Awake()
        {
            selectedGameIndex = DataManager.Instance.Load<int>(GameConstants.SelectedGameIndex);
            currentObj = Instantiate(gameSets[selectedGameIndex], canvas.transform);
           

        }
        private void OnEnable()
        {
            SwitchNextButtonevent += SwitchnextButton;
            ResultPopupEvent += OpenResultPopup;
          
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                Application.Quit();
            }
        }
        private void SwitchnextButton()
        {
          
            nextButton.SetActive(false);
        }
        private void OpenResultPopup(bool isWon)
        {
            resultPanel.SetActive(true);
            resultPanel.transform.SetAsLastSibling();
            if(isWon)
            {
                resultText.text = GameConstants.WonText;
            }
            else
            {
                resultText.text = GameConstants.LostText;
            }
        }
      
        private void OnDisable()
        {
            SwitchNextButtonevent -= SwitchnextButton;
            ResultPopupEvent -= OpenResultPopup;
        }


        public void OnNextButton()
        {
         
            selectedGameIndex = DataManager.Instance.Load<int>(GameConstants.SelectedGameIndex);
            Destroy(currentObj);
            if(selectedGameIndex < 1)
            {
                currentObj = Instantiate(gameSets[selectedGameIndex + 1], canvas.transform);
                selectedGameIndex++;
                DataManager.Instance.SaveData(selectedGameIndex, GameConstants.SelectedGameIndex);
            }
            else
            {
                currentObj = Instantiate(gameSets[0], canvas.transform);
                selectedGameIndex = 0;
                DataManager.Instance.SaveData(selectedGameIndex, GameConstants.SelectedGameIndex);
            }
      
           

            resultPanel.SetActive(false );
        }
        public void OnResetButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

      
    }
}

