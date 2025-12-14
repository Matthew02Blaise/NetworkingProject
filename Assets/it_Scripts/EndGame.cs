using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    //Variables
    public TextMeshProUGUI EndText;

    //Sets end text when a specific score threshold is met (10 in this case)
    //This text object is blank but active until the win quota is met
    public void SetEndText(bool gameOver, bool isWinner)
    {
        if (EndText == null) return;

        if (!gameOver)
        {
            EndText.text = ""; // nothing until someone wins
            return;
        }

        EndText.text = isWinner ? "You win" : "You lose";
    }
}
