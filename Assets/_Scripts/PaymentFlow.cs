using UnityEngine;
using UnityEngine.SceneManagement;

public class PaymentFlow : MonoBehaviour
{
    // References to the two panels we created
    public GameObject paymentPanel;
    public GameObject successPanel;

    // This method will be called by the "Confirm" button
    public void ConfirmPayment()
    {
        // Hide the first panel and show the second one
        paymentPanel.SetActive(false);
        successPanel.SetActive(true);
    }

    // This method will be called by the "Start Game" button
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}