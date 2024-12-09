using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private AI _ai;
    private Rigidbody2D _rb;
    Vector3 _lastVelocity;

    private void Start()
    {
        _playerMovement = _playerMovement.GetComponent<PlayerMovement>();
        _ai = _ai.GetComponent<AI>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _lastVelocity = _rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _playerMovement._canKick = true;
        }
        if(collision.gameObject.tag == "AIShoot")
        {
            _ai._canShoot = true;
        }
        if (collision.gameObject.tag == "AIJump")
        {
            _ai._canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerMovement._canKick = false;
        }
        if (collision.gameObject.tag == "AIShoot")
        {
            _ai._canShoot = false;
        }
        if (collision.gameObject.tag == "AIJump")
        {
            _ai._canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = _lastVelocity.magnitude;
        var direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);

        _rb.velocity = direction * Mathf.Max(speed, -1f);
    }
}
