<p align="center"> 
<img src="https://github.com/winstonzhao/SOFTENG-306-Project-2/blob/master/logo-full-text.png" width="500">
</p>

# ONE DAY or DAY ONE !

## Overview
This will be a great game, where hopefully you might even be able to play it!

## Team Members
| Name             | GitHub | UoA UPI
| ---------------- | ------------ | --------- |
| Winston Zhao | @winstonzhao | wzha539 |
| Simon Su | @sloushsu | zsu801 |
| Tina Chen | @twchen97 | tche278 |
| Rebekah Berriman | @rmberriman | rber798 |
| Carl Tang | @Carl-Tang | ytan415 |
| Reuben Rakete | @rocketbang | rrak789 |
| Franklin Wang | @Gnawf | fwan182 |
| Sean Oldfield | @SheepySean | sold940 |

## Execution Instructions

## Setting Up

### Configuring Git with Unity
To use the built in Unity merge tool to merge unity `.asset`, `.prefab`, and `.unity` files,
some changes must be made to your git config file.  
Add the following to your `~/.gitconfig`:
```
[merge]
	tool = unityyamlmerge
 [mergetool "unityyamlmerge"]
	trustExitCode = false
	keepTemporaries = true
	keepBackup = false
	path = 'C:\\Program Files\\Unity\\Editor\\Data\\Tools\\UnityYAMLMerge.exe'
	cmd = 'C:\\Program Files\\Unity\\Editor\\Data\\Tools\\UnityYAMLMerge.exe' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
 ```
_Edit the above paths to match your own enviroment   
(mac = "/Applications/Unity/Unity.app/Contents/Tools/UnityYAMLMerge")_  
 Find more info [here](https://gist.github.com/Ikalou/197c414d62f45a1193fd)
 
 ## Playing
1. Checkout the “FINAL” branch from github
1. Open the game by and start running the game through Unity navigating to the ‘Menu’ scene (`Assets\Scenes\Menu.scene`)
1. Press the play button located in the the Unity interface.
1. Before pressing "Play" in the menu, make sure your screen aspect ratio is set to one of the supported ones (16:9, 16:10, or 4:3)
1. Once your aspect ratio is correct, press "Play" to begin!

## Gameplay
### Lobby
1. Now you’ve reached the lobby, you can wondering and around talk the NPCs by pressing spacebar when within sufficient distance and then pressing the spacebar again to progress through the dialogue.

### Minigames
1. Walk to the elevator and progress to the Leech level, you’ll know when you’ve reached this level when you’re in a level with many stalls listing different engineering specializations on them.
1. Talk to any of the instructors standing nearby to the stalls and choose the ‘Play [Specialization] Game’ button when presented the option.
Follow the instructions to progress through the selected minigame.

### Multiplayer
1. Open 2 instances of the game either on different systems or on the same system.
1. Making sure you’re on the same level, you should see the other player’s character moving throughout the scene.
1. You can make sure the multiplayer is working by using the chat feature, this is done by pressing enter to allow text input into the textbox and then pressing enter again to send the message.
1. You should observe a dialogue bubble instantiate above the corresponding players’ head.

