using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    GameManager _gm;
    Vector2 _startPos;
    Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "PlayerGoalDetection")
        {

            _gm._aiScore++;
            _rb.velocity = Vector2.zero;
            transform.position = _startPos;
        }
        else if (collision.gameObject.name == "OpponentGoalDetection")
        {
            _gm._playerScore++;
            _rb.velocity = Vector2.zero;
            transform.position = _startPos;
        }
    }
}
