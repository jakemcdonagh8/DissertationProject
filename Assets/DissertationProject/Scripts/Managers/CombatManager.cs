using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    // Manager variables.
    public MovementManager movementManager;
    public ColourManager colourManager;
    public UnitManager unitManager;
    public GameManager gameManager;

    // Health bar image used to publicly display the health bars of units currently on the board.
    public Image healthBar;

    // EngageCombat function deals with the combat within the game. 
    public void EngageCombat (GameObject gameObject, object[] tiles, GameObject prevPiece)
    {
        colourManager.CompleteAction(prevPiece);

        gameObject.GetComponent<Player_Piece_Properties>().HP -= prevPiece.GetComponent<Player_Piece_Properties>().attackDamage;

        healthBar.fillAmount = gameObject.GetComponent<Player_Piece_Properties>().HP / gameObject.GetComponent<Player_Piece_Properties>().InitialHP;

        if (gameObject.GetComponent<Player_Piece_Properties>().HP > 0)
        {
           
            movementManager.ClearHighlights(tiles);
            return;
        }
        else
        {
            // If the unit taken out was a Chief, end the game, otherwise destroy the object.
            if (gameObject.GetComponent<Properties>().description == "Player_Piece_Chief")
            {
                gameManager.EndGame(gameObject);
            }
            movementManager.ManageMovement(gameObject, tiles, prevPiece);
            unitManager.ChangeTraitorSide(gameObject, prevPiece);
            Destroy(gameObject);

            object[] pieces = GameObject.FindGameObjectsWithTag("Player_Piece");

            foreach (GameObject traitorPiece in pieces)
            {

                if (traitorPiece.GetComponent<Properties>().description == "Player_Piece_Traitor")
                {
                    colourManager.CompleteAction(traitorPiece);
                }
            }
                return;
        }
    }


}
