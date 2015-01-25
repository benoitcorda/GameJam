using UnityEngine;
using System.Collections;

public class Sequencing : MonoBehaviour {

	
	public SpriteRenderer[] renderers;
	
	private float time = 0f;
	private int idx = 0;
	private int[] scenarioChatty = {0, 1, 2, 3};
	private int[] scenarioClingy = {4, 5, 6, 7};
	private int[] currentScenario;
	
	private Color hidden() {
		return new Color (1, 1, 1, 0);
	}
	private Color shown() {
		return new Color (1, 1, 1, 1);
	}
	
	private float transitionTime = .6f;
	
	void Start () {
		switch (LevelState.levelState) {
		case LevelState.State.Clingy:
			currentScenario = scenarioClingy;
			break;
		case LevelState.State.Chatty:
		default:
			currentScenario =  scenarioChatty;
			break;
		}
	}
	
	void Update() {
		var sceneLength = 5f;
		var currentIdx = idx < currentScenario.Length ? currentScenario[idx] : 0;
		var currentSprite = renderers[currentIdx];
		
		// Set the colors on the previous and current scenes.
		var interpolation = Mathf.Min(time/transitionTime, 1);
		
		currentSprite.color = Color.Lerp (hidden (), shown (), interpolation);

		// Switch to the next scene if needed.
		time += Time.deltaTime;
		if ((time > sceneLength || Input.GetButtonDown("Jump")) && idx < currentScenario.Length) {
			idx++;
			time = 0;
			
			// Did we show the last cutscene?
			if (idx >= currentScenario.Length) {
				loadNextLevel();
			} else {
				// set the order of the layer to show up in front of the previous one
				var newIdx = currentScenario[idx];
				renderers[newIdx].sortingOrder = currentSprite.sortingOrder + 1;
			}
		}
	}

	void loadNextLevel () {
		Application.LoadLevel((int)LevelState.levelState);
	}
}
