using UnityEngine;
using System.Collections;

public class Start : MonoBehaviour {

	public void OnStartClicked() {
		Application.LoadLevel((int)LevelState.levelState);
	}
}
