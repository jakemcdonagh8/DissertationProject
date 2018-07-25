using System;
using UnityEngine;

/*
 * The properties that each GameObject is capable of having. Please note that not every gamebobject in a given scene needs to have
 * all the variables used as a requirement. 
 */

public class Properties : MonoBehaviour
{
    public String objectName;
    public String description;
    public TileManagement CurrentTile;
    public Material colour;
}
