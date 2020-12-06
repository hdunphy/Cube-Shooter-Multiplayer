using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject
{
    public int Id { get; private set; }
    public string UserName { get; private set; }
    public Color Color { get; private set; }
    public bool IsReady { get; private set; }

    public PlayerObject (int _id, string _userName, Color _color, bool _isReady)
    {
        Id = _id;
        UserName = _userName;
        Color = _color;
        IsReady = _isReady;
    }

    //public void SetUserName(string _userName) => UserName = _userName;
    //public void SetColor(Color _color) => Color = _color;
    //public void SetIsReadyIcon(bool _isReady) => IsReady = _isReady;
}
