using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarForBoss : MonoBehaviour
{
    [SerializeField] private HealthforBoss playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;
    void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    // Update is called once per frame
    void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}