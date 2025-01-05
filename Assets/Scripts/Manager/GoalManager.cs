using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    GameManager _gm;
    Vector2 _startPos;
    Rigidbody2D _rb;
    [SerializeField] GameObject _resetObj;

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        //StartCoroutine(InitObj());
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
            //ResetManager.Instance.ResetPos();
            //_resetObj.SetActive(true);
            //StartCoroutine(ResetObj());
        }
        else if (collision.gameObject.name == "OpponentGoalDetection")
        {
            _gm._playerScore++;
            _rb.velocity = Vector2.zero;
            transform.position = _startPos;
            //ResetManager.Instance.ResetPos();
            //_resetObj.SetActive(true);
            //StartCoroutine(ResetObj());
        }
    }

    IEnumerator ResetObj()
    {
        yield return new WaitForSeconds(1);
        _resetObj.SetActive(false);
    }

    IEnumerator InitObj()
    {
        _resetObj.SetActive(true);
        yield return new WaitForSeconds(1);
        _resetObj.SetActive(false);
    }
}
