using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player m_player;

    public Player Player { get => m_player; private set => m_player = value; }

    public override void Awake()
    {
        MakeSingleton(false);
    }
}
