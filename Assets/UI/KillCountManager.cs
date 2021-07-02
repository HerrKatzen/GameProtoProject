using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace GameProtoProject
{
    public class KillCountManager : MonoBehaviour
    {
        TextMeshProUGUI m_PhaserKillCountText;
        TextMeshProUGUI m_FlyingKillCountText;
        TextMeshProUGUI m_ZombieKillCountText;

        // Start is called before the first frame update
        void Start()
        {
            m_PhaserKillCountText = transform.Find("PhaserKillCount").GetComponent<TextMeshProUGUI>();
            m_ZombieKillCountText = transform.Find("ZombieKillCount").GetComponent<TextMeshProUGUI>();
            m_FlyingKillCountText = transform.Find("FlyingKillCount").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            int phaser = 0, zombie = 0, flying = 0;
            StageHandler.Instance.DeadMap.TryGetValue("Phaser", out phaser);
            StageHandler.Instance.DeadMap.TryGetValue("Zombie", out zombie);
            StageHandler.Instance.DeadMap.TryGetValue("Flying", out flying);
            m_PhaserKillCountText.text = phaser.ToString();
            m_ZombieKillCountText.text = zombie.ToString();
            m_FlyingKillCountText.text = flying.ToString();
        }
    }
}
