using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameProtoProject
{
    public class MainMenuManager : MonoBehaviour
    {
        Dictionary<int, string> m_DifficultyMap = new Dictionary<int, string>() { { 0, "Easy" }, { 1, "Medium" }, { 2, "Hard" }, { 3, "Endless" } };

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SelectLevel(string name)
        {
            SceneManager.LoadScene(name);
            Time.timeScale = 1.0f;
        }

        public void OnDropdownSelect(int key)
        {
            string difficulty = "Easy";
            m_DifficultyMap.TryGetValue(key, out difficulty);
            GlobalSettings.Instance.Difficulty = difficulty;
        }

    }
}

