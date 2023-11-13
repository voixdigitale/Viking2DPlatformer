using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Vector2 _startingPos;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _deathVFX;

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
        Instantiate(_deathVFX, transform.position, Quaternion.identity);
        _model.SetActive(false);
        StartCoroutine(BackToStart());        
    }

    private IEnumerator BackToStart() {
        yield return new WaitForSeconds(_respawnDelay);

        _model.SetActive(true);
        _movement.SetPlatformMode(false);
        transform.position = _startingPos;
    }
}
