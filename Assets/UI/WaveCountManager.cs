using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameProtoProject
{
    public class WaveCountManager : MonoBehaviour
    {

        void Update()
        {
            int wave = StageHandler.Instance.waveCounter;
            TextMeshProUGUI textObject = transform.GetComponent<TextMeshProUGUI>();
            if (wave < 1) { textObject.text = ""; return; }
            textObject.text = string.Format("WAVE\n{0}", wave);
        }
    }
}
