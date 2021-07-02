using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{

    public class HealthBarManager : MonoBehaviour
    {
        public CharacterProperties properties;
        public GameObject m_HeartPrefab;
        public Sprite m_FullHeartSprite;
        public Sprite m_HalfHeartSprite;
        public Sprite m_EmptyHeartSprite;

        public int m_HeartCount = 10;
        float m_Unit;
        List<GameObject> m_Hearts = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            m_Unit = properties.maxHealth / m_HeartCount / 2;
            for (int i = 0; i < m_HeartCount; i++)
            {

                GameObject heart = Instantiate(m_HeartPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                heart.transform.parent = this.transform;
                heart.transform.localPosition = new Vector3(0, -1.5f * i, 0);
                m_Hearts.Add(heart);
            }
        }

        // Update is called once per frame
        void Update()
        {
            float hp = properties.health;
            for (int i = 0; i < m_HeartCount; i++)
            {
                GameObject heart = m_Hearts[i];
                if (hp > m_Unit * (2 * i + 1))
                {
                    heart.GetComponent<SpriteRenderer>().sprite = m_FullHeartSprite;

                }
                else if (hp > m_Unit * 2 * i)
                {
                    heart.GetComponent<SpriteRenderer>().sprite = m_HalfHeartSprite;
                }
                else
                {
                    heart.GetComponent<SpriteRenderer>().sprite = m_EmptyHeartSprite;
                }
            }
        }
    }
}