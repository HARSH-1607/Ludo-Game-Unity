# Simplified 2-Player Ludo Game

This project is a simplified, 2-player version of the classic Ludo board game, created in Unity as part of a time-limited assignment. It includes core Ludo mechanics, a complete UI flow from the main menu to a win screen, and several additional features for polish and user engagement.

---

## How to Run the Project

1.  **Download & Unzip:** Download the provided `.zip` file and extract it to a folder on your computer.
2.  **Open in Unity Hub:** Open Unity Hub, click the "Open" button, and navigate to the unzipped project folder to add it to your projects list.
3.  **Launch Project:** Open the project in a compatible version of the Unity Editor (e.g., 2022.3.x or newer).
4.  **Open Home Scene:** In the `Project` window, navigate to the `_Scenes` folder and double-click the `Home` scene to open it.
5.  **Run Game:** Press the **Play** button at the top-center of the Unity Editor to start the game.

---

## What’s Implemented

* **Complete Game Loop:** A full 2-player hot-seat game with a clear start, turn-based gameplay, and end-state. Each player controls 2 tokens.
* **Core Ludo Rules:**
    * Dice rolls from 1-6 with a visual "rolling" animation.
    * A roll of 6 is required to move a token from its base to the starting square.
    * Tokens move along their defined path according to the dice value.
    * A custom capture mechanic sends an opponent's token back to their base.
* **Winning Condition:** The game correctly detects when a player has moved both of their tokens to the designated home spots and displays a winner announcement screen.
* **Complete UI Flow:**
    * **Home Screen:** With options for a standard "Play" match and a "Play for ₹10" match.
    * **Dummy Payment Screen:** A simulated payment flow for the paid match option, displaying the potential winnings.
    * **Game Screen:** A clear layout with the board, dice, turn indicator text, and emoji buttons.
    * **Win Screen:** A pop-up panel announcing the winner with a "Play Again" button that returns to the Home Screen.
* **Visual Polish & Extra Features:**
    * **Animated Dice:** The dice visually "rolls" through different faces before showing the final result.
    * **Capture Effect:** A particle effect plays at the location of a capture for better visual feedback.
    * **Emoji System:** A simple UI allows players to send an emoji that pops up on the screen.
    * **Debugging Visuals:** (For testing purposes) Tokens display their internal path index number, making it easy to verify game logic.

---

## Known Limitations

* **Extra Turn on 6:** The game does not grant the player an extra turn after rolling a 6.
* **Safe Zones:** There are no "safe" squares on the board where a token is immune to capture.
* **Exact Roll to Win:** There is no requirement for a player to roll the exact number of steps to get a token home.
* **No Token Selection:** The game automatically moves the first available token; the player cannot choose which token to move if multiple moves are possible.
* **Strictly 2-Player:** The current architecture is designed for exactly two players and is not scalable to 3 or 4 players without significant code changes.
* **No Sound:** The game is currently silent and has no sound effects or music.

---

## What I’d Do Better With More Time

* **Implement Full Ludo Rules:** Add the remaining rules, such as getting an extra turn on a 6 and forming a "block" when two of the same tokens are on one square.
* **Player Token Selection:** Refactor the game manager to allow the player to click on and choose which of their active tokens they wish to move.
* **Shared Path System:** The current custom capture logic is a workaround for having separate paths for each player. With more time, I would re-architect the path system to use a single "global" path for all shared squares. This would simplify the capture code and make the project much more scalable for 4 players.
* **Refactor for Scalability:** Use Scriptable Objects to hold player data (color, prefabs, home spots, etc.). This would decouple the game logic from the scene setup and make adding new players much easier.
* **Enhanced UI/UX:** Add more fluid UI animations, interactive button feedback, and sound effects for all major game events (dice roll, token movement, capture, win/loss) to create a more polished and engaging user experience.
* **Online Multiplayer:** Integrate a networking solution like Firebase Realtime Database or Unity's Netcode for Paradigms to allow players to compete on different devices over the internet.
