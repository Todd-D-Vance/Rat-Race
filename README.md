# Rat Race: Creating a 2D Classic Arcade Style Maze Chase Game in Unity 3D

> Build a complete feature-filled and polished classic 2D arcade game from start to finish in Unity.  Unlike other books of this type, this book is \emph{detailed} and covers a wide range of game-making aspects, while focusing on making one game as good as possible.  

> Mathematics is used in game programming, and for advanced programmers, some of the math is shown.  However, the solutions are given so one need not actually know how to solve the problems.  A reader who can write programs will do well with this book.  A reader who also has mathematics knowledge through high-school level algebra and trigonometry will do even better.

##Chapter 1: Let's Create a Game!

Overview of the book

##Chapter 2: Introduction to Unity

Install Unity and mock-up a nonfunctional prototype


##Chaptger 3: C# Programming I

"Hello World" in Unity

##Chapter 4: Non-Game Prototype

Adding motion and collision to the prototype

##Chapter 5: Source Control

Protect yourself from future problems

##Chapter 6: C# Programming II

A crash-course in programming C#

##Chapter 7: Procedural Content Generation I: Automatic Maze Generation

Design and automatically create a basic playable maze

##Chapter 8: Artificial Intelligence: The A* Algorithm

How to make enemies move toward a goal

##Chapter 9: Game Mechanics Prototype

Pickups (edibles) and an enemy that chases you

##Chapter 10: Designing a Game

Writing your Game Design Document

##Chapter 11: Creating Game Art I: The Mouse

Create a mouse sprite that can be animated out of ellipses

##Chapter 12: Unity Animation Framework I

The mouse wags its tail and twitches its whiskers

##Chapter 13: Creating Game Art II: The Cat, the Cheese, and the Walls

Creating more sprites and making them look good, and making the maze look much better

##Chapter 14: Unity Animation Framework II: Improving the Animation

The cat and mouse now run more realistically

##Chapter 15: Procedural Content Generation II: Improving the Walls

Make the maze look less like midtown Manhattan and more like lower Manhattan

##Chapter 16: Game States I: Win and Lose

Adding a finite-state machine to the game to handle game states, and
introduction to Unity's User Interface framework

##Chapter 17: Game States II:  Attract Mode

Setting up title screen, high score screen, and so on before the game is played

##Chapter 18: Game States III: Improving Game Behavior

New level when all cheese is eatent

##Chapter 19: Cat Box, More Cats, and Teleportation Tunnels

Adding the central portion of the maze: a box full of cats and tunnels
that teleport you

##Chapter  20: AI II: Cat Personalities and Flee from Player

Add a finite-state machien to the AI so cats have different
personalities and different modes.  Cats can now be eaten if the mouse
eats the growth cheese.

##Chapter 21: Unity Sound Framework

Make a music-player script and compose game music for it.  Also,
create sound effects in Audacity.  Also some random bug fixes.

##Chapter 22: Options Menus and Saving Values

Unity's persistance framework and saving game preferences

##Chapter 23: Review and Refactor

An introduction to refactoring, including a better music player and a
better game state manager.  More random bug fixes.

##Chapter 24: Game State IV: Making Every State Work

Walking through every state of the game and making sure it complies
with the GDD.

##Chapter 25: The Demo State

Write code to record a demo and play it back

##Chapter 26: Game State V: Continuing Where We Left Off

We interrupted Chapter 24 to fix the demo state, which was a
full-chapter rabbit hole.  We now resume our regular programming,
including adding a scripted cutscene.

##Chapter 27: Game State VI: Post Game Mode

The cutscene was another rabbit hole.  We resume with what happens
when the game is over: save a high score, enter your name, back to
title screen.

##Chapter 28: Improving the Game

We play-test the game and write down everything we don't like.  Then we fix it.

##Chapter 29: Additional Edibles and Hazards

Another rabit hole: add pickups like the cherries in Pac-Man.

##Chapter 30: Fixing the Remaining Issues

Back to the list of issues to fix.

##Chapter 31: Extensive Play Testing

We are not done.  We still have to discover and fix issues.  We add a
cheat code system to make this easier.

##Chapter 32: Building for Deployment

Get the game running outside the Unity editor, on real machines

##Chapter 33: Touch Screen Controls

Mobile devicies typically do not have keyboards.

##Chapter 34: More Features

Anything else to do before we publish?

##Chapter 35: Publishing

Time to make money, or at least get the game out there where people can play it.

##Appendix A: Selected solutions to exercises

##Appendix B: Installing Firefox and Acrobat Reader

##Bibliography

Some additional reading

=============================================

#Game Design Document

##Marketing Statement

Remember the Golden Age of the arcade, around the 1980s or so? It was a PacMan-eat-dot industry, where a game that didn't earn a quarter every three minutes got removed and replaced. The games were built to have lights and sounds to attract the player, and then to challenge the player enough that he's likely to lose the game...fast. Still, from time to time, a player would do really well and get on the high score list.

We shall build a game of this type.

##Elevator Ptich
The player controls a mouse who must eat all the cheese in the maze without being eaten by some very aggressive cats.

##Technology
We shall use the Unity game engine, whose software development editor and integrated development environment (IDE) runs on the PC and Macintosh, and can deploy games than run on the PC and Mac as well as the web and mobile devices and Linux.

We shall use Git for source control.

##Features

 * 2D classic arcade action
 * Written with \software{Unity} 5.6 and \csharp
 * Artificial Intelligence (AI) for the cats
 * Grid-based automated level building and Procedural Content Generation (PCG)
 * Score keeping with high-score saving
 * Multiple lives
 * Teleportation tunnels
 * Sound and music and animated sprite assets
 * A game state system managing the title and high score screens
 * Cutscenes
 * Published to multiple locations for multiple platforms
 * and more...

##Mechanics

 * Player is controled by a keyboard, joystick, game pad, or (for mobile devices) a touch screen, with inputs representing up, down, left, and right.  An input will change the direction of the player sprite (the mouse) to that direction if it is possible to go that direction.  Upon reaching an obstacle, the sprite will stop until a valid directional input is given.

 * The enemy sprites (cats) will be controlled by AI and will try to catch the mouse.  If the mouse is caught, a life is lost.  Initially, the mouse has three lives but new lives are sometimes given as a bonus.  When all lives are lost, the game is over.

 * The mouse must eat all the dots (cheese) in the maze, at which time a newer, harder maze is given (levelling up).  

 * Special ``growth hormone'' cheese gives the mouse the ability to chase and eat the cats for a short period of time.

 * Tunnels allow the mouse (or cats) to teleport, thus confusing the AI of the cats.  

 * Scoring is based on cheese eaten, cats eaten, bonus pickups eaten, and levels achieved.  Top scores are saved.

##Attract Mode

 * When the game is not in progress, it cycles through a title screen, a high score screen, a details screen, and a demo screen.  The demo is a video clip of sample gameplay.

 * An options button, leading to an options screen, is available.  User-selectable options include music volume, sound effects volume, and perhaps other options.

 * Attract mode ends when a ``Play'' button is pressed, and resumes on end of game (perhaps with a high score screen interlude).

##Assets

Assets include an animated mouse, animated cats, cheese (dots), walls pieces, maze designs, sound effect clips, and level music.

##Ideas for Future Development

 * Adrenaline: start with high reserve, low usage.  If mouse is near cat, can use it to go faster, but reserves are depleted and mouse slows down.  Need a ``run key'' (or maybe double-tap direction key) to signal ``use the adrenaline now''

 * Eating cats makes mouse full when growth hormone runs out and he walks slower for a time

 * Cat seeing mouse directly ahead gets excited and runs faster, but tires out quickly and time needed to have speed burst again

 * Mouse eating growth hormone cheese makes cats faster in running away in fear for a short interval of time

 * Special animation frames: mouse sniffs nearby cat and looks afraid, cat sniffs nearby mouse and looks hungry, etc.

 * Special pickups for special powers?

 * Dangerous ``pickups'' like a marching mousetrap?

##Game States


The game will have the following states:


 * State: {Init} This is the state of the game at startup.  Things that should only be done once, such as instantiating Singleton classes, happen here.  A splash screen will be shown for a short time.  

After all startup tasks are complete and when a certain short time period has passed, it transitions to the first state of the Attract Mode, the Title state.


 * State: {Attract Mode: Title} A screen is shown with the title of the game and some graphics intended to make one excited about playing the game.  

After a certain amount of time has passed, the state transitions to the State: {Attract Mode: Description} state.

If the Button: {Play} button is pressed, the state transitions to the State: {Game Mode: Intro} state.

If the Button: {Options} button is pressed, the state transitions to the State: {Attract Mode: Options} state.


 * State: {Attract Mode: Description} A screen is shown with a description of the game and some graphics with point values and a brief how-to for the game.

After a certain amount of time has passed, the state transitions to the State: {Attract Mode: High Scores} state.

If the Button: {Play} button is pressed, the state transitions to the State: {Game Mode: Intro} state.

If the Button: {Options} button is pressed, the state transitions to the State: {Attract Mode: Options} state.


 * State: {Attract Mode: High Scores} A screen is shown with a list of the top ten saved scores.

After a certain amount of time has passed, the state transitions to the State: {Attract Mode: Demo} state.

If the Button: {Play} button is pressed, the state transitions to the State: {Game Mode: Intro} state.

If the Button: {Options} button is pressed, the state transitions to the State: {Attract Mode: Options} state.


 * State: {Attract Mode: Demo} A screen shows video playback of a sample game.

After a certain amount of time has passed, the state transitions to the State: {Attract Mode: Title} state.

If the Button: {Play} button is pressed, the state transitions to the State: {Game Mode: Intro} state.

If the Button: {Options} button is pressed, the state transitions to the State: {Attract Mode: Options} state.


 * State: {Attract Mode: Options}  An options screen is shown, allowing the setting of options, that are automatically saved.

If the Button: {Return} button is pressed, the state transitions to the State: {Attract Mode: Title} state.


If the Button: {Quit} button is pressed, the game exits if on a platform where this is possible.  Otherwise, it transitions to the State: {Attract Mode: Title} state.


 * State: {Game Mode: Intro} The level 1 board is displayed, the intro music is played, and the cats and mouse appear.  This state is analogous to the State: {Init} state in that it contains things only done once during a game being played, such as the introductory music.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After the sequence finishes, this state transitions to the State: {Game Mode: Play} state.


 * State: {Game Mode: Play} Input is connected to the player and AI to the cats.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

If all dots are eaten, the state transitions to the  State: {Game Mode: End Level} state.


If a cat catches the mouse, the state transitions to the  State: {Game Mode: Death} state.


 * State: {Game Mode: Start Level} The appropriate level board is displayed.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After a short time, the state transitions to the State: {Game Mode: Play} state.


 * State: {Game Mode: Death}  A death animation and sound is played.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

When the animation finishes, a life is lost.  If there are no more lives, the state transitions to the State: {Game Mode: Game Over} state.  Otherwise, the state transitions to the State: {Game Mode: Reset Player} state. 

 * State: {Game Mode: Game Over} A game over message is shown and the player is removed from play.  The enemies may continue to gloat.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After a short time, if the score is higher than the lowest score on the top ten list, the state transitions to the  State: {Post Game Mode: New High Score} state.  Otherwise, it transitions to the State: {Attract Mode: Title} state.


 * State: {Game Mode: Reset Player} The player and cats are reset in the existing level.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After a short pause, the state transitions to the State: {Game Mode: Play} state.


 * State: {Game Mode: End Level} An end of level animation/sound is played.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After the animation is done, the state transitions to the State: {Game Mode: Level Up} state.


 * State: {Game Mode: Level Up} The level number is incremented.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

For certain levels, the game transitions to the State: {Game Mode: Cutscene}  state.  For the remaining, it transitions to the State: {Game Mode: Start Level} state.


 * State: {Game Mode: Cutscene} A video playback screen between certain levels that advances the story.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.

After the video is complete, this state transitions to the State: {Game Mode: Start Level} state.


 * State: {Game Mode: Options} An options screen is shown, allowing the setting of options, that are automatically saved.

If the Button: {Return} button is pressed, the state transitions to the State: {Game Mode} state it came from.

If the Button: {Quit} button is pressed, the state transitions to the State: {Game Mode: Game Over} state.


 * State: {Post Game Mode: New High Score} An animation/sound shows a new high score has been achieved.

If the Button: {Options} button is pressed, the state transitions to the State: {Post Game Mode: Options} state.

After a short time, this state transitions to the State: {Post Game Mode: High Scores} state.


 * State: {Post Game Mode: High Scores} The list of high scores is shown, with the newly-added score highlighted.

If the Button: {Options} button is pressed, the state transitions to the State: {Game Mode: Options} state.


After a short time, this state transitions to the State: {Attract Mode: Title} state.


 * State: {Post Game Mode: Options}  An options screen is shown, allowing the setting of options, that are automatically saved.

If the Button: {Return} button is pressed, the state transitions to the State: {Post Game Mode} state it came from.

If the Button: {Quit} button is pressed, the state transitions to the State: {Attract Mode: Title} state.
