using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadbutt : MonoBehaviour
{
    [SerializeField] GameObject _ball;
    public float _ballX;
    public float _ballY;
    // Start is called before the first frame update
    void Start()
    {
        _ball = GameObject.FindGameObjectWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            //_ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(_ballX, _ballY));
        }
    }
}
