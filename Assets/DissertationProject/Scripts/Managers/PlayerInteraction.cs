using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * This class focuses on managing all of the different interactions that can take place between players, their pieces, the tiles on the board as well as the main game loop.
 * 
 */

public class PlayerInteraction : MonoBehaviour {
    // Class variables which are used in order determine the current state of the game.
    public static Boolean isPlayer1Turn = true;
    public static Boolean isHeld = false;

    public MovementManager movementManager;
    public ColourManager colourManager;
    public CombatManager combatManager;
    public UnitManager unitManager;
    public UnitInfoDisplay unitInfoDisplay;

    // Important variable used to store informaiton about the previous gameobject clicked. Used in order to make comparisons between this object and the next gameobject clicked.
    public static GameObject prevPiece;
    public static GameObject unitPiece;
    public static GameObject repositionPiece;

    public GameObject unitInfoBox;

    public Button hunterButtonPlayer1;
    public Button archerButtonPlayer1;
    public Button dragonButtonPlayer1;

    public Button hunterButtonPlayer2;
    public Button archerButtonPlayer2;
    public Button dragonButtonPlayer2;

    public Button endTurnPlayer1;
    public Button endTurnPlayer2;


    // Iniitalizes the scene so that Player 1 always goes first.
    void Start() {
        object[] player2Pieces = GameObject.FindGameObjectsWithTag("Player_Piece");
        colourManager.InitialiseColours(player2Pieces);
    }

    // This function is called whenever the user clicks on the scene. The function uses Case-switch statements to determine what type of object was clicked.
    private void OnMouseUp()
    {
        object[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        
        // The Case-switch statement used in order to determine what type of gameobject was clicked. Uses the description property in order to determine the type of gameobject.
        switch (gameObject.GetComponent<Properties>().description)
        {
            case "Player_Piece_Chief":
            case "Player_Piece_Hunter":
            case "Player_Piece_Archer":
            case "Player_Piece_Dragon":
            case "Player_Piece_Traitor":
                movementManager.CheckTileColour(gameObject, tiles, prevPiece);
                // Checks to see if the piece has already been activated. If so, nothing happens, otherwise the game will highlight the available moves with said piece.
                if (gameObject.GetComponent<Player_Piece_Properties>().activated == true)
                {
                    return;
                }

                if (gameObject == prevPiece)
                {
                    foreach (GameObject tile in tiles)
                    {
                        if (tile.GetComponent<Renderer>().material.color == colourManager.babyBlue || tile.GetComponent<Renderer>().material.color == colourManager.turqoise || tile.GetComponent<Renderer>().material.color == Color.yellow)
                        {
                            colourManager.CompleteAction(gameObject);
                            movementManager.ClearHighlights(tiles);
                            return;
                        }
                    }
                    if (gameObject.GetComponent<Properties>().description == "Player_Piece_Traitor")
                    {
                        colourManager.CompleteAction(gameObject);
                        movementManager.ClearHighlights(tiles);
                    }
                    movementManager.ClearHighlights(tiles);
                    prevPiece = null;
                }
                else
                {
                    foreach (GameObject tile in tiles)
                    {
                        if (tile.GetComponent<Renderer>().material.color == colourManager.babyBlue || tile.GetComponent<Renderer>().material.color == Color.green)
                        {
                            return;
                        }            
                    }
                    movementManager.HighlightMovement(gameObject.GetComponent<Properties>().CurrentTile, gameObject.GetComponent<Player_Piece_Properties>().movementSpeed, gameObject);
                    prevPiece = gameObject;
                }
                break;
            case "Traitor": // While the traitor piece is inactive, do not have any interactions with it.
                break;

            // If a tile is clicked and the current available moves are highlighted, then the piece GameObject will do a certain action depending on the colour of the highlight.
            case "Tile":
                object[] pieces = GameObject.FindGameObjectsWithTag("Player_Piece");

                if (gameObject.GetComponent<Renderer>().material.color == Color.yellow)
                {
                    unitManager.CreateUnit(gameObject, unitPiece);
                    movementManager.ClearHighlights(tiles);
                    unitPiece = null;
                }
                else if (gameObject.GetComponent<Renderer>().material.color == colourManager.turqoise)
                {
                    movementManager.ClearHighlights(tiles);
                    colourManager.CompleteAction(prevPiece);
                }
                else if (gameObject.GetComponent<Renderer>().material.color == Color.green)
                {
                    movementManager.ManageMovement(gameObject, tiles, prevPiece);
                    movementManager.HighlightAttackRange(gameObject.GetComponent<Properties>().CurrentTile, prevPiece.GetComponent<Player_Piece_Properties>().attackRange, pieces, prevPiece);
                }
                else if (gameObject.GetComponent<Renderer>().material.color == colourManager.babyBlue)
                {
                    movementManager.ManageMovement(gameObject, tiles, repositionPiece);
                    colourManager.CompleteAction(prevPiece);
                    repositionPiece = null;
                }
                break;
            default:
                Debug.Log("ERROR");
                break;
        }

    }

    // OnMouseOver is called whenever the user is hovering over the scene. If the mouse button is held down, display the more information box to the user, otherwise do not show the panel.
    private void OnMouseOver()
    {
        object[] infoCards = GameObject.FindGameObjectsWithTag("UnitInfoCard");

        if (Input.GetMouseButtonDown(1))
        {      
            foreach (GameObject card in infoCards)
            {
                if (card.GetComponent<UnitInfoDisplay>().unit == gameObject)
                {
                    card.transform.Translate(0, 0, 10);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            foreach (GameObject card in infoCards)
            {
                if (card.GetComponent<UnitInfoDisplay>().unit == gameObject)
                {
                    Debug.Log("Showing card");
                    card.transform.Translate(0, 0, -10);
                }
            }
        }    
    }

    // This function ends the current player turn and starts the opposing player turn.
    public void EndTurn()
    {
        object[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        movementManager.ClearHighlights(tiles);
        prevPiece = null;

        if (isPlayer1Turn == true)
        {
            isPlayer1Turn = false;
            hunterButtonPlayer1.gameObject.SetActive(false);
            archerButtonPlayer1.gameObject.SetActive(false);
            dragonButtonPlayer1.gameObject.SetActive(false);
            endTurnPlayer1.gameObject.SetActive(false);

            hunterButtonPlayer2.gameObject.SetActive(true);
            archerButtonPlayer2.gameObject.SetActive(true);
            dragonButtonPlayer2.gameObject.SetActive(true);
            endTurnPlayer2.gameObject.SetActive(true);
        }
        else
        {
            isPlayer1Turn = true;
            hunterButtonPlayer1.gameObject.SetActive(true);
            archerButtonPlayer1.gameObject.SetActive(true);
            dragonButtonPlayer1.gameObject.SetActive(true);
            endTurnPlayer1.gameObject.SetActive(true);

            hunterButtonPlayer2.gameObject.SetActive(false);
            archerButtonPlayer2.gameObject.SetActive(false);
            dragonButtonPlayer2.gameObject.SetActive(false);
            endTurnPlayer2.gameObject.SetActive(false);
        }
        colourManager.ManageColours(isPlayer1Turn);
    }

    // DisplayLocations is called whenever the user presses a corresponding button to spawn a new unit for the turn.
    public void DisplayLocations(string currentPlayer) {
        object[] playerPieces = GameObject.FindGameObjectsWithTag("Player_Piece");

        foreach (GameObject piece in playerPieces)
        {
            if (piece.GetComponent<Properties>().description == "Player_Piece_Chief" && piece.GetComponent<Player_Piece_Properties>().belongsTo == currentPlayer && piece.GetComponent<Player_Piece_Properties>().hasSpawnedUnit == false)
            {
                movementManager.HighlightSpawn(piece.GetComponent<Properties>().CurrentTile);
                piece.GetComponent<Player_Piece_Properties>().hasSpawnedUnit = true;
                colourManager.CompleteAction(piece);
                break;
            }
        }

        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Button_Create_Hunter_Player1":
                if (hunterButtonPlayer1.GetComponent<UnitCountManager>().unitCount > 0)
                {
                    unitPiece = unitManager.vikingHunter;

                    hunterButtonPlayer1.GetComponent<UnitCountManager>().unitCount -= 1;
                    hunterButtonPlayer1.gameObject.GetComponentInChildren<Text>().text = "Create Hunter [" + hunterButtonPlayer1.GetComponent<UnitCountManager>().unitCount + "]";
                } else
                {

                }         
                break;

            case "Button_Create_Archer_Player1":
                if (archerButtonPlayer1.GetComponent<UnitCountManager>().unitCount > 0)
                {
                    unitPiece = unitManager.vikingArcher;

                    archerButtonPlayer1.GetComponent<UnitCountManager>().unitCount -= 1;
                    archerButtonPlayer1.gameObject.GetComponentInChildren<Text>().text = "Create Archer [" + archerButtonPlayer1.GetComponent<UnitCountManager>().unitCount + "]";
                } else
                {

                }
                break;

            case "Button_Create_Dragon_Player1":
                if (dragonButtonPlayer1.GetComponent<UnitCountManager>().unitCount > 0)
                { 
                    unitPiece = unitManager.vikingDragon;

                    dragonButtonPlayer1.GetComponent<UnitCountManager>().unitCount -= 1;
                    dragonButtonPlayer1.gameObject.GetComponentInChildren<Text>().text = "Create Dragon [" + dragonButtonPlayer1.GetComponent<UnitCountManager>().unitCount + "]";
                }
                else
                {

                }
                break;

            case "Button_Create_Hunter_Player2":
                if (hunterButtonPlayer2.GetComponent<UnitCountManager>().unitCount > 0)
                {
                    unitPiece = unitManager.marauderHunter;

                    hunterButtonPlayer2.GetComponent<UnitCountManager>().unitCount -= 1;
                    hunterButtonPlayer2.gameObject.GetComponentInChildren<Text>().text = "Create Hunter [" + hunterButtonPlayer2.GetComponent<UnitCountManager>().unitCount + "]";
                }
                else
                {

                }
                break;

            case "Button_Create_Archer_Player2":
                if (archerButtonPlayer2.GetComponent<UnitCountManager>().unitCount > 0)
                {
                    unitPiece = unitManager.marauderArcher;

                    archerButtonPlayer2.GetComponent<UnitCountManager>().unitCount -= 1;
                    archerButtonPlayer2.gameObject.GetComponentInChildren<Text>().text = "Create Archer [" + archerButtonPlayer2.GetComponent<UnitCountManager>().unitCount + "]";
                }
                else
                {

                }
                break;

            case "Button_Create_Dragon_Player2":
                if (dragonButtonPlayer2.GetComponent<UnitCountManager>().unitCount > 0)
                {
                    unitPiece = unitManager.marauderDragon;

                    dragonButtonPlayer2.GetComponent<UnitCountManager>().unitCount -= 1;
                    dragonButtonPlayer2.gameObject.GetComponentInChildren<Text>().text = "Create Archer [" + dragonButtonPlayer2.GetComponent<UnitCountManager>().unitCount + "]";
                }
                else
                {

                }
                break;
            default:
                break;

        }
    }
}
