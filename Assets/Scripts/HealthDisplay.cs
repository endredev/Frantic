using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{

    TextMeshProUGUI healthText = null;
    Player player;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        healthText.text = player.getHelath().ToString();
    }
}
