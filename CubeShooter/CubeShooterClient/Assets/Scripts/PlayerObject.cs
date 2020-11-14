using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject
{
    public int Id { get; private set; }
    public string UserName { get; private set; }
    public Color Color { get; private set; }

    public PlayerObject (int _id, string _userName, Color _color)
    {
        Id = _id;
        UserName = _userName;
        Color = _color;
    }

    //public PlayerObject(int _id, string _userName, Vector3 _colorVector)
    //{
    //    Id = _id;
    //    UserName = _userName;
    //    Color = new Color(_colorVector.x, _colorVector.y, _colorVector.z);
    //}

    public void SetUserName(string _userName) => UserName = _userName;
    public void SetColor(Color _color) => Color = _color;
}
