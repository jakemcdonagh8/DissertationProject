using System;
using UnityEngine;

public class Player_Piece_Properties : MonoBehaviour
{
    // Properties that belong to a player piece. This is added onto a GameObject as a component.
    public String belongsTo;
    public float InitialHP;
    public float HP;
    public int movementSpeed;
    public int attackRange;
    public int attackDamage;
    public Boolean activated;
    public Boolean hasSpawnedUnit;
}
