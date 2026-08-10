using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PowerUpUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotContainer;
    public List<PowerUpData> allPowerUpTypes; // the 5 power-ups only, assigned in Inspector

    private List<GameObject> spawnedSlots = new List<GameObject>();

    void Start()
    {
        GameManager.Instance.OnPowerUpInventoryChanged += RefreshSlots;
        BuildSlots();
        RefreshSlots();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPowerUpInventoryChanged -= RefreshSlots;
    }

    void BuildSlots()
    {
        foreach (var powerUp in allPowerUpTypes)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);

            Image icon = slot.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = powerUp.sprite;

            Button button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => GameManager.Instance.ActivatePowerUp(powerUp));

            spawnedSlots.Add(slot);
        }
    }

    void RefreshSlots()
    {
        Debug.Log("RefreshSlots called, slot count: " + spawnedSlots.Count);
        for (int i = 0; i < allPowerUpTypes.Count; i++)
        {
            int count = GameManager.Instance.GetPowerUpCount(allPowerUpTypes[i]);
            TextMeshProUGUI countText = spawnedSlots[i].transform.Find("CountText").GetComponent<TextMeshProUGUI>();
            countText.text = count.ToString();
        }
    }
}