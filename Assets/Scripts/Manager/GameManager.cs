using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int _playerScore = 0, _aiScore = 0;
    public float _timer = 120f;
    public Text _scoreText;
    public string _score;

    // Update is called once per frame
    void Update()
    {

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            Debug.Log("game over");
        }

        _score = _playerScore.ToString() + "    -    " + _aiScore.ToString();

        _scoreText.text = _score.ToString();
    }
}
