using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IDZ.Game
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameSets = new List<GameObject>();
        [SerializeField]private Canvas canvas;
        int selectedGameIndex;
        private void Awake()
        {
            DataManager.Instance.Load(selectedGameIndex,GameConstants.SelectedGameIndex);
           Instantiate(gameSets[selectedGameIndex],canvas.transform);
        }

    }
}

