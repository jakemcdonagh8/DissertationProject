using UnityEngine;

public class MovementManager : MonoBehaviour {

    // Manager variavles
    public ColourManager colourManager;
    public CombatManager combatManager;
    
    // Checks to see if the current piece is in the middle of a move. If it is, checks to see if the piece is targetting a friendly/enemy piece or if it is targetting a blank tile.
    public void CheckTileColour(GameObject gameObject, object[] tiles, GameObject prevPiece)
    {

        if (gameObject.GetComponent<Properties>().CurrentTile.GetComponent<Renderer>().material.color == Color.red)
        {
            combatManager.EngageCombat(gameObject, tiles, prevPiece);
        }
        else if (gameObject.GetComponent<Properties>().CurrentTile.GetComponent<Renderer>().material.color == colourManager.lightGreen)
        {
            HighlightReposition(prevPiece.GetComponent<Properties>().CurrentTile);
            PlayerInteraction.repositionPiece= gameObject;           
        }
        else
        {
            return;
        }
    }   

    // Highlights the area where the unit can attack/interact with other units.
    public void HighlightAttackRange(TileManagement tile, int attackRange, object[] pieces, GameObject prevPiece)
    {     
        TileManagement[] adjacentTiles = tile.GetComponent<TileManagement>().AdjacentTiles;

        foreach (TileManagement adjacentTile in adjacentTiles)
        {
            if (adjacentTile == null)
            {
                break;
            }
            else if (attackRange > 1)
            {
                adjacentTile.GetComponent<Renderer>().material.color = colourManager.turqoise;

                foreach (GameObject piece in pieces)
                {
                    if (adjacentTile.name == piece.GetComponent<Properties>().CurrentTile.name)
                    {
                        if (piece.GetComponent<Player_Piece_Properties>().belongsTo == prevPiece.GetComponent<Player_Piece_Properties>().belongsTo)
                        {
                            adjacentTile.GetComponent<Renderer>().material.color = colourManager.lightGreen;
                        }
                        else if (prevPiece.GetComponent<Player_Piece_Properties>().belongsTo == "Traitor")
                        {
                            break;
                        }
                        else
                        {
                            adjacentTile.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                }              
                attackRange -= 1;
                HighlightAttackRange(adjacentTile, attackRange, pieces, prevPiece);
                attackRange += 1;
            }
            else
            {
                adjacentTile.GetComponent<Renderer>().material.color = colourManager.turqoise;
                foreach (GameObject piece in pieces)
                {
                    if (adjacentTile.name == piece.GetComponent<Properties>().CurrentTile.name)
                    {
                        if (piece.GetComponent<Player_Piece_Properties>().belongsTo == prevPiece.GetComponent<Player_Piece_Properties>().belongsTo)
                        {
                            adjacentTile.GetComponent<Renderer>().material.color = colourManager.lightGreen;
                        }
                        else if (prevPiece.GetComponent<Player_Piece_Properties>().belongsTo == "Traitor")
                        {
                            break;
                        }
                        else
                        {
                            adjacentTile.GetComponent<Renderer>().material.color = Color.red;
                        }
                    }
                }           
            }
        }
    }


    // The highlightMovement function displays the current available moves to the user when they select an object. The method is called recursively up to allow for pieces to
    // move cardinally up to their movement speed.
    public void HighlightMovement(TileManagement tile, int speed, GameObject piece)
    {

        TileManagement[] adjacentTiles = tile.GetComponent<TileManagement>().AdjacentTiles;

        foreach (TileManagement adjacentTile in adjacentTiles)
        {
            if (adjacentTile == null)
            {
                break;
            }
            else if (adjacentTile.GetComponent<TileManagement>().CurrentPiece != piece && adjacentTile.GetComponent<TileManagement>().CurrentPiece != null || adjacentTile.GetComponent<TileProperties>().isWaterTile)
            {
                if (piece.GetComponent<Properties>().description == "Player_Piece_Dragon" && adjacentTile.GetComponent<TileProperties>().isWaterTile)
                {
                    adjacentTile.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else if (adjacentTile.GetComponent<TileProperties>().isForestTile && piece.GetComponent<Properties>().description != "Player_Piece_Dragon")
            {
                if (speed > 1)
                {
                    adjacentTile.GetComponent<Renderer>().material.color = Color.green;
                }           
            }
            else if (speed > 1)
            {


                adjacentTile.GetComponent<Renderer>().material.color = Color.green;
                speed -= 1;
                HighlightMovement(adjacentTile, speed, piece);
                speed += 1;

            }
            else
            {


                adjacentTile.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

    //Highlights the adjacent tiles around the unit in question to allow for repositioning of a friendly unit.
    public void HighlightReposition(TileManagement tile)
    {
        TileManagement[] adjacentTiles = tile.GetComponent<TileManagement>().AdjacentTiles;

        foreach (TileManagement adjacentTile in adjacentTiles)
        {
            if (adjacentTile == null)
            {
                break;
            }
            else if (adjacentTile.GetComponent<TileManagement>().CurrentPiece != null || adjacentTile.GetComponent<TileProperties>().isWaterTile)
            {
                adjacentTile.GetComponent<Renderer>().material = tile.GetComponent<Properties>().colour;
            }
            else
            {
                adjacentTile.GetComponent<Renderer>().material.color = colourManager.babyBlue;
            }
        }
    }

    // Similar to HighlightReposition, this function instead highlight the adjacent tiles to allow for the player to spawn in a new unit.
    public void HighlightSpawn (TileManagement tile)
    {
        TileManagement[] adjacentTiles = tile.GetComponent<TileManagement>().AdjacentTiles;
        foreach (TileManagement adjacentTile in adjacentTiles)
        {
            if (adjacentTile == null)
            {
                break;
            }
            else if (adjacentTile.GetComponent<TileManagement>().CurrentPiece != null || adjacentTile.GetComponent<TileProperties>().isWaterTile)
            {

            }
            else
            {
                adjacentTile.GetComponent<Renderer>().material.color = Color.yellow;
            }        
        }      
    }

    // Clears any current highlights on the board and set the board back to default.
    public void ClearHighlights(object[] tiles)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().material = tile.GetComponent<Properties>().colour;
        }
    }

    // The manageMovement function uses a manhattan distance equation in order to find the delta x/y values to allow the piece to move to it's correct position.
    // The piece then darkens to signify that the piece has moved and can not be interacted with any further.
    public void ManageMovement(GameObject piece, object[] tiles, GameObject prevPiece)
    {
        float DeltaX = (piece.GetComponent<Properties>().transform.position.x) - (prevPiece.transform.position.x);
        float DeltaZ = (piece.GetComponent<Properties>().transform.position.z) - (prevPiece.transform.position.z);

        prevPiece.GetComponent<Properties>().CurrentTile.GetComponent<TileManagement>().CurrentPiece = null;
        prevPiece.GetComponent<Properties>().CurrentTile = piece.GetComponent<Properties>().CurrentTile;

        if (piece.GetComponent<Properties>().description == "Tile")
        {
           piece.GetComponent<TileManagement>().CurrentPiece = prevPiece;
        }
        else
        {
            piece.GetComponent<Properties>().CurrentTile.GetComponent<TileManagement>().CurrentPiece = prevPiece;
        }
        prevPiece.transform.Translate(DeltaX, 0, DeltaZ);
        ClearHighlights(tiles);
    }

}
