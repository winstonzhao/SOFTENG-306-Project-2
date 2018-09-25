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