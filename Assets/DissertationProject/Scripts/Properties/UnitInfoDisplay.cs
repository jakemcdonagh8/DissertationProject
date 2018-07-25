using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour {

    public GameObject unit;

    // Variables used in order to generate the needed text on the InfoDisplay panels.
    public Text nameText;
    public Text speedText;
    public Text attackText;
    public Text rangeText;
    public Text currentHealthText;
    public Text maxHealthText;


    // Automatically updates any change in values on every unit that has a InfoDisplay
    void Update () {
        nameText.text = unit.GetComponent<Properties>().objectName;
        speedText.text = unit.GetComponent<Player_Piece_Properties>().movementSpeed.ToString();
        attackText.text = unit.GetComponent<Player_Piece_Properties>().attackDamage.ToString();
        rangeText.text = unit.GetComponent<Player_Piece_Properties>().attackRange.ToString();
        currentHealthText.text = unit.GetComponent<Player_Piece_Properties>().HP.ToString();
        maxHealthText.text = unit.GetComponent<Player_Piece_Properties>().InitialHP.ToString();

    }
}
