using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject hiddenPanel; // The more unit information box that is above the units in the editor.
    public Text winner; 


    // EndGame function is called when one of the player Chief pieces is taken out.
    public void EndGame(GameObject gameObject) 
    {
        if (gameObject.GetComponent<Player_Piece_Properties>().belongsTo == "Player1")
        {
            winner.text = "Player 2";
            winner.GetComponent<Text>().color = Color.blue;
        }
        else
        {
            winner.text = "Player 1";
            winner.GetComponent<Text>().color = Color.red;          
        }

        hiddenPanel.gameObject.SetActive(value: true);
    }

    // Called after the EndGame animation has finished playing.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

