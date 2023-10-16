using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTriigger : MonoBehaviour
{
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelManager.I.CreateNewLevel();
            Destroy(gameObject);
        }
    }
}
