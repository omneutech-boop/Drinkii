using UnityEngine;
using System.Collections.Generic;

public class LivesUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        GameManager.Instance.OnLivesChanged += RefreshHearts;
        BuildHearts();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLivesChanged -= RefreshHearts;
    }

    void BuildHearts()
    {
        foreach (var h in hearts) Destroy(h);
        hearts.Clear();

        for (int i = 0; i < GameManager.Instance.lives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
    }

    void RefreshHearts()
    {
        int currentLives = GameManager.Instance.lives;

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentLives);
        }
    }
}