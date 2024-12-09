using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float _rangerDefence, _speed, _jumpHeight;

    public bool _canShoot, _canJump, _isGrounded;

    [SerializeField] private Transform _defence, _groundCheck;
    private GameObject _ball;
    private Rigidbody2D _rb;
    private Rigidbody2D _rbBall;
    public LayerMask _groundLayer;

    float _ballX = 0.01f;
    float _ballY = 0.01f;

    [SerializeField] private AIMovementStats AIMovementStats;

    private void Start()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball");
        _rb = GetComponent<Rigidbody2D>();
        _rbBall = _ball.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Shoot();
        Jump();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.1f, _groundLayer);
        //Debug.DrawRay(_groundCheck.position, Vector3.up, Color.red);
    }

    public void Move()
    {
        if(Mathf.Abs(_ball.transform.position.x - transform.position.x) < _rangerDefence)
        {
            if(_ball.transform.position.x > transform.position.x)
            {
                _rb.velocity = new Vector2(Time.deltaTime * _speed, _rb.velocity.y);
            }
            else 
                _rb.velocity = new Vector2(-Time.deltaTime * _speed, _rb.velocity.y);
        }
        else
        {
            if (transform.position.x > _defence.position.x)
                _rb.velocity = new Vector2(-Time.deltaTime * _speed, _rb.velocity.y);
            else
                _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
    }

    public void Shoot()
    {
        if(_canShoot)
        {
            _rbBall.AddForce(new Vector2(-_ballX, _ballY));
        }
    }

    public void Jump()
    {
        if(_canJump && _isGrounded == true)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpHeight);
        }
    }
}
