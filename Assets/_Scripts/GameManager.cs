using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- Game State & Player Turn ---
    public enum GameState { WaitingForRoll, MovingToken, TurnOver }
    public GameState currentState;
    public int activePlayer = 1;

    // --- Dice Logic & Animation ---
    public int diceNumber;
    public Button rollDiceButton;
    public Image diceImage;
    public Sprite[] diceSprites;

    // --- Token & Player References ---
    public Token[] player1Tokens;
    public Token[] player2Tokens;
    public Transform[] player1HomeSpots;
    public Transform[] player2HomeSpots;

    // --- UI Panels & Text ---
    public TextMeshProUGUI statusText;
    public GameObject winScreenPanel;
    public TextMeshProUGUI winnerText;
    
    // --- Capture Animation ---
    public ParticleSystem captureEffect;

    void Start()
    {
        winScreenPanel.SetActive(false);
        if(captureEffect != null) captureEffect.Stop();
        SwitchToPlayer(1);
    }

    public void OnRollDiceButtonClicked()
    {
        if (currentState == GameState.WaitingForRoll)
        {
            StartCoroutine(RollDiceCoroutine());
        }
    }

    IEnumerator RollDiceCoroutine()
    {
        currentState = GameState.MovingToken;
        rollDiceButton.interactable = false;
        statusText.text = "Player " + activePlayer + " is rolling...";

        // Dice Animation
        float rollDuration = 1.0f;
        float frameTime = 0.05f;
        for (float i = 0; i < rollDuration; i += frameTime)
        {
            int randomFace = Random.Range(0, 6);
            diceImage.sprite = diceSprites[randomFace];
            yield return new WaitForSeconds(frameTime);
        }

        // Final Result
        diceNumber = Random.Range(1 , 7);
        diceImage.sprite = diceSprites[diceNumber - 1];
        statusText.text = "Player " + activePlayer + " rolled a " + diceNumber;
        yield return new WaitForSeconds(1.0f);

        // --- Automatic Move Logic ---
        Token tokenToMoveFromBase = FindFirstTokenInBase(activePlayer);
        Token tokenToMoveOnBoard = FindFirstActiveToken(activePlayer);

        if (diceNumber == 6 && tokenToMoveFromBase != null)
        {
            tokenToMoveFromBase.MoveToStart();
            StartCoroutine(TurnSwitchCoroutine());
        }
        else if (tokenToMoveOnBoard != null)
        {
            StartCoroutine(MoveTokenCoroutine(tokenToMoveOnBoard));
        }
        else
        {
            statusText.text = "No possible moves!";
            StartCoroutine(TurnSwitchCoroutine());
        }
    }
    
    IEnumerator MoveTokenCoroutine(Token token)
    {
        yield return StartCoroutine(token.MoveCoroutine(diceNumber));

        if (token.isHome)
        {
            MoveToEmptyHomeSpot(token);
        }

        CheckForCapture(token);
        CheckForWin();

        if (!winScreenPanel.activeInHierarchy)
        {
            StartCoroutine(TurnSwitchCoroutine());
        }
    }

    void CheckForCapture(Token movedToken)
    {
        // No capture can happen if the token is home or in base.
        if (movedToken.isHome || movedToken.isInBase) return;

        Debug.Log("--- Starting Capture Check ---");
        Debug.Log("Moved Token (Player " + activePlayer + ") landed on its personal path index: " + movedToken.currentPathIndex);

        // --- NEW LOGIC ---
        // This is the number of steps between each player's starting point on the board.
        int pathOffset = 13;
        // This is the total number of steps in the main loop of the board.
        int totalPathSteps = 52; 

        // Calculate the "global" position of the token that just moved.
        int movedTokenGlobalPos = movedToken.currentPathIndex;
        if (activePlayer == 2)
        {
            // Player 2's path starts 13 steps after Player 1's.
            movedTokenGlobalPos += pathOffset;
        }

        // Determine which tokens are the opponent's
        Token[] opponentTokens = (activePlayer == 1) ? player2Tokens : player1Tokens;

        foreach (Token opponent in opponentTokens)
        {
            // We only care about opponents that are on the board
            if (!opponent.isInBase && !opponent.isHome)
            {
                // Calculate the "global" position of the opponent token.
                int opponentGlobalPos = opponent.currentPathIndex;
                if (activePlayer == 1) // If the attacker is P1, the opponent is P2
                {
                    opponentGlobalPos += pathOffset;
                }

                Debug.Log("Checking against opponent at personal index: " + opponent.currentPathIndex);
                
                // THE CORE LOGIC: Do their global positions match?
                // We use the modulo (%) operator to handle wrapping around the board.
                if (movedTokenGlobalPos % totalPathSteps == opponentGlobalPos % totalPathSteps)
                {
                    Debug.Log("SUCCESS! Global positions match (" + (movedTokenGlobalPos % totalPathSteps) + "). CAPTURE TRIGGERED!");
                    statusText.text = "Player " + activePlayer + " captured a token!";
                    
                    if(captureEffect != null)
                    {
                        captureEffect.transform.position = opponent.transform.position;
                        captureEffect.Play();
                    }
                    
                    opponent.ReturnToBase();
                    Debug.Log("--- Capture Check Finished ---");
                    return; // Stop checking, we found a capture
                }
            }
        }

        Debug.Log("No opponent tokens had a matching global position. No capture.");
        Debug.Log("--- Capture Check Finished ---");
    }

    void MoveToEmptyHomeSpot(Token token)
    {
        Transform[] homeSpots = (activePlayer == 1) ? player1HomeSpots : player2HomeSpots;
        foreach(Transform spot in homeSpots)
        {
            if(spot.childCount == 0)
            {
                token.MoveToFinalHome(spot.position);
                token.transform.SetParent(spot);
                return;
            }
        }
    }

    void CheckForWin()
    {
        if (AllTokensHome(player1Tokens)) GameOver("Player 1");
        else if (AllTokensHome(player2Tokens)) GameOver("Player 2");
    }

    bool AllTokensHome(Token[] tokens)
    {
        foreach (Token token in tokens)
        {
            if (!token.isHome) return false;
        }
        return true;
    }

    void GameOver(string winnerName)
    {
        winnerText.text = winnerName + " Wins!";
        winScreenPanel.SetActive(true);
        rollDiceButton.interactable = false;
    }

    IEnumerator TurnSwitchCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        int nextPlayer = (activePlayer == 1) ? 2 : 1;
        SwitchToPlayer(nextPlayer);
    }

    void SwitchToPlayer(int playerNumber)
    {
        activePlayer = playerNumber;
        statusText.text = "Player " + activePlayer + ": Your Turn!";
        currentState = GameState.WaitingForRoll;
        rollDiceButton.interactable = true;
    }

    Token FindFirstTokenInBase(int player)
    {
        Token[] tokens = (player == 1) ? player1Tokens : player2Tokens;
        foreach (Token token in tokens) { if (token.isInBase) return token; }
        return null;
    }

    Token FindFirstActiveToken(int player)
    {
        Token[] tokens = (player == 1) ? player1Tokens : player2Tokens;
        foreach (Token token in tokens) { if (!token.isInBase && !token.isHome) return token; }
        return null;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Home");
    }
}