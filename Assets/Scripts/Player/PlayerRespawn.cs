using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Vector2 startingPos;

    private PlayerMovement _movement;

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        _movement.SetPlatformMode(false);
        transform.position = startingPos;
    }
}
