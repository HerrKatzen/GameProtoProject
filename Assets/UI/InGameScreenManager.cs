using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace GameProtoProject
{
    public class InGameScreenManager : MonoBehaviour
    {
        GameObject m_PauseMenu;
        GameObject m_YouDiedScreen;
        bool m_isPaused = false;

        public TextMeshProUGUI scoreText;

        public void SwitchState()
        {
            m_isPaused = !m_isPaused;
            Time.timeScale = m_isPaused ? 0 : 1.0f;
            m_PauseMenu.SetActive(m_isPaused);
        }

        void Start()
        {
            m_PauseMenu = transform.Find("PauseMenu").gameObject;
            m_YouDiedScreen = transform.Find("YouDiedScreen").gameObject;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchState();
            }
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }


        public void SetScoreText()
        {
            int zombie = 0, phaser = 0, bird = 0;
            Dictionary<string, int> deadMap = StageHandler.Instance.DeadMap;
            deadMap.TryGetValue("Zombie", out zombie);
            deadMap.TryGetValue("Flying", out bird);
            deadMap.TryGetValue("Phaser", out phaser);
            int score = zombie + phaser * 2 + bird * 5;
            scoreText.text = string.Format("Score: {0}", score);
        }
        public void OnDeath()
        {
            SetScoreText();
            m_YouDiedScreen.SetActive(true);
        }
    }
}