using System;
using UnityEngine;

public class Player
{
    private PlayerData data = null;
    public PlayerData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    private PlayerBehaviour behaviour = null;
    public PlayerBehaviour Behaviour
    {
        get
        {
            return behaviour;
        }
        set
        {
            behaviour = value;
        }
    }
}

