using UnityEngine;
using System.Collections;

public class LevelLoader : Singleton<LevelLoader> {

	public void loadNextLevel() {
		Application.LoadLevel(mod(Application.loadedLevel + 1, Application.levelCount));

	}

	public void loadPreviousLevel() {
		Application.LoadLevel(mod (Application.loadedLevel -1, Application.levelCount));
	}

	int mod(int x, int m) {
		return (x%m + m)%m;
	}
}
