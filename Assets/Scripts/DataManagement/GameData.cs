using UnityEngine;
using System;

[Serializable] // 클래스를 직렬화 시키기 위한 문구
public class GameData : MonoBehaviour
{
    public int score;
    public int bestScore;
    public float bgmVolume;
    public string playerName;

    public Vector3 playerPosition;

    public GameData()
    {
        score = 0;
        bestScore = 0;
        bgmVolume = 0.5f;
        playerName = "Player";
        playerPosition = new Vector3(0,1,0);
    }
}
