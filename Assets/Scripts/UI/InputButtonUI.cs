using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Player player;
    public int direction;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.StartButtonInput(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.StopButtonInput(direction);
    }
}
