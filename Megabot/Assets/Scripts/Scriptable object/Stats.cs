using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Stats", menuName = "Scriptable objects/stat")]
public class Stats : ScriptableObject
{
    public float health;
    public float attack;
    public float moveSpeed;
    public float jumpForce;
}
