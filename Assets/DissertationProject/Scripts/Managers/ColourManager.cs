using System;
using UnityEngine;

public class ColourManager : MonoBehaviour {


    // Colour values that are used throughout the game. Each colour value is used to indicate a different type of action that the user can take.
    public Color darkRed = new Color32(94, 10, 10, 255);
    public Color darkBlue = new Color32(16, 47, 96, 255);
    public Color turqoise = new Color32(90, 255, 160, 255);
    public Color lightGreen = new Color32(85, 225, 85, 255);
    public Color babyBlue = new Color32(63, 127, 191, 255);
    public Color red;
    public Color blue;

    // Ensures that GameObjects belonging to player 2 are blue and deactivated as player 1 always goes first.
    public void InitialiseColours(object[] player2Pieces)
    {
        foreach (GameObject piece in player2Pieces)
        {
            if (piece.GetComponent<Player_Piece_Properties>().belongsTo == "Player2")
            {
                piece.GetComponent<Renderer>().material.color = darkBlue;
                piece.GetComponent<Player_Piece_Properties>().activated = true;
            }
        }

    }

    // The CompleteAction function darkens the current GameObject in question and enables the boolean value activated to true.
    public void CompleteAction(GameObject piece)
    {
        if (piece.GetComponent<Player_Piece_Properties>().belongsTo == "Player1")
        {
            piece.GetComponent<Renderer>().material.color = darkRed;
        }
        else
        {
            piece.GetComponent<Renderer>().material.color = darkBlue;
        }
        piece.GetComponent<Player_Piece_Properties>().activated = true;
    }

    // The ManageColours function manages the different colours used within the game to signify active and inactive pieces. This may change when models are introduced at a later point.
    public void ManageColours(Boolean isPlayer1Turn)
    {
        object[] pieces = GameObject.FindGameObjectsWithTag("Player_Piece");



        foreach (GameObject piece in pieces)
        {
            piece.GetComponent<Renderer>().material = piece.GetComponent<Properties>().colour;
            piece.GetComponent<Player_Piece_Properties>().activated = false;
            piece.GetComponent<Player_Piece_Properties>().hasSpawnedUnit = false;

            if (piece.GetComponent<Player_Piece_Properties>().belongsTo == "Player2" && isPlayer1Turn == true)
            {
                piece.GetComponent<Renderer>().material.color = darkBlue;
                piece.GetComponent<Player_Piece_Properties>().activated = true;
            }
            else if (piece.GetComponent<Player_Piece_Properties>().belongsTo == "Player1" && isPlayer1Turn == false)
            {
                piece.GetComponent<Renderer>().material.color = darkRed;
                piece.GetComponent<Player_Piece_Properties>().activated = true;
            }
        }
    }
}
