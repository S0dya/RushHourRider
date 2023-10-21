using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreTrigger : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    bool isUsed;


    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isUsed)
        {
            GameMenuUI.I.AddScore(enemy.scoreAmountToAdd);
        }
    }
}
