using UnityEngine;
using UnityEngine.UI;

public class SimpleEmojiManager : MonoBehaviour
{
    // The invisible "stage" where the emoji will appear
    public Transform popupLocation; 

    // The template for the emoji pop-up
    public GameObject emojiPopupPrefab; 

    // An array of all your emoji images
    public Sprite[] emojiSprites; 

    // This single function will be called by all your emoji buttons
    public void ShowEmoji(int emojiIndex)
    {
        // First, check if the prefab and location are set up
        if (popupLocation == null || emojiPopupPrefab == null)
        {
            Debug.LogError("Popup Location or Prefab is not set in the Inspector!");
            return;
        }

        // Check if the chosen emoji index is valid
        if (emojiIndex < 0 || emojiIndex >= emojiSprites.Length)
        {
            Debug.LogError("Invalid Emoji Index provided: " + emojiIndex);
            return;
        }

        // Create the emoji pop-up at the "stage" location
        GameObject newEmojiPopup = Instantiate(emojiPopupPrefab, popupLocation);

        // Set the image of the pop-up to the correct emoji
        newEmojiPopup.GetComponent<Image>().sprite = emojiSprites[emojiIndex];

        // Automatically destroy the pop-up after 2 seconds
        Destroy(newEmojiPopup, 2f);
    }
}
