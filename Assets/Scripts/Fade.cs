using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public SpriteRenderer[] renderers;
	public SpriteRenderer credits;

	private float time = 0f;
	private float pauseTime = 0f;
	private int idx = 0;
	private bool isLoadingCredits;
	private bool pauseCredit;
	private int[] scenrio1 = {0, 1, 2, 3 , 4};
	private int[] scenrio2 = {0, 3, 2, 1};
	private int[] scenrio3 = {3, 2, 3, 2};
	private int[] currentScenario;

	private Color hidden() {
		return new Color (1, 1, 1, 0);
	}
	private Color shown() {
		return new Color (1, 1, 1, 1);
	}

	private float transitionTime = .6f;

	void Start () {
		// TODO(corda): Based on global context.
		currentScenario = scenrio1;
		isLoadingCredits = false;
		pauseCredit = false;
	}

	void Update() {
		var sceneLength = 5f;
		var currentIdx = idx < currentScenario.Length ? currentScenario[idx] : 0;
		var currentSprite = isLoadingCredits ? credits : renderers [currentIdx];

		// Set the colors on the previous and current scenes.
		var interpolation = Mathf.Min(time/transitionTime, 1);
		
		currentSprite.color = Color.Lerp (hidden (), shown (), interpolation);

		if (isLoadingCredits && !pauseCredit) {
			Vector3 position = credits.transform.position;
			credits.transform.position = new Vector3(position.x, position.y + 0.05f, position.z);
		}

		// Switch to the next scene if needed.
		time += Time.deltaTime;
		pauseTime += Time.deltaTime;
		if ((time > sceneLength || Input.GetButtonDown("Jump")) && idx < currentScenario.Length) {
			idx++;
			time = 0;

			// Did we show the last cutscene?
			if (idx >= currentScenario.Length) {
				LoadCredits(currentIdx);
			} else {
				// set the order of the layer to show up in front of the previous one
				var newIdx = currentScenario[idx];
				renderers[newIdx].sortingOrder = currentSprite.sortingOrder + 1;
			}
		} else if (isLoadingCredits && !pauseCredit && credits.transform.position.y > 9.7f) {
			pauseTime = 0;
			pauseCredit = true;
		} else if (pauseCredit && pauseTime > 0.1f) {
			// TODO(corda): Based on global context, go to the right level.
			Application.LoadLevel(1);
		}
	}

	void LoadCredits(int lastSpriteIdx) {
		isLoadingCredits = true;

		credits.sortingOrder = renderers[lastSpriteIdx].sortingOrder + 1;
		Vector3 position = credits.transform.position;
		credits.transform.position = new Vector3(position.x, -9.4f, position.z);
	}

}
