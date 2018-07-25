using UnityEngine;

public class UnitManager : MonoBehaviour {

    // Manager variables
    public ColourManager colourManager;

    // Variables that correspond to the 6 main buttons that are used to spawn in new units for both players.
    public GameObject vikingHunter;
    public GameObject vikingArcher;
    public GameObject vikingDragon;

    public GameObject marauderHunter;
    public GameObject marauderArcher;
    public GameObject marauderDragon;

    // This function is called whenever a user clicks on a yellow tile GameObject. This function instantiates and creates a new GameObject.
    public void CreateUnit(GameObject tile, GameObject unitPiece)
    {

        GameObject unit = Instantiate(unitPiece, new Vector3(tile.GetComponent<Properties>().transform.position.x, 0, tile.GetComponent<Properties>().transform.position.z), Quaternion.identity) as GameObject;

        unit.name = unitPiece.name;

        unit.GetComponent<Properties>().CurrentTile = tile.GetComponent<Properties>().CurrentTile;
        tile.GetComponent<TileManagement>().CurrentPiece = unit;

        unit.GetComponent<Player_Piece_Properties>().activated = true;
        if (unit.GetComponent<Player_Piece_Properties>().belongsTo == "Player1")
        {
            unit.GetComponent<Renderer>().material.color = colourManager.darkRed;
        }
        else
        {
            unit.GetComponent<Renderer>().material.color = colourManager.darkBlue;
        }
    }

    // This function is called whenever a unit is taken out of the board. This is used to ensure that the Traitor piece is consistently changing sides during the game.
    public void ChangeTraitorSide(GameObject gameobject, GameObject prevPiece)
    {
        object[] pieces = GameObject.FindGameObjectsWithTag("Player_Piece");

        foreach (GameObject traitorPiece in pieces)
        {
            if (traitorPiece.GetComponent<Properties>().description == "Traitor_Piece" || traitorPiece.GetComponent<Properties>().description == "Player_Piece_Traitor")
            {
                if (gameobject.GetComponent<Player_Piece_Properties>().belongsTo == "Player1")
                {
                    traitorPiece.GetComponent<Player_Piece_Properties>().belongsTo = "Player1";
                }
                else
                {
                    traitorPiece.GetComponent<Player_Piece_Properties>().belongsTo = "Player2";
                }
                traitorPiece.GetComponent<Renderer>().material = gameobject.GetComponent<Properties>().colour;
                traitorPiece.GetComponent<Properties>().colour = gameobject.GetComponent<Properties>().colour;
                
                traitorPiece.GetComponent<Properties>().description = "Player_Piece_Traitor";

            }
            
        }
    }
}
