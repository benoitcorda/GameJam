using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public SpriteRenderer[] renderers;

	private float time = 0f;
	private int idx = 0;
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
	}

	void Update() {
		var sceneLength = 5f;
		var currentIdx = currentScenario[idx];

		// Set the colors on the previous and current scenes.
		var interpolation = Mathf.Min(time/transitionTime, 1);
		
		renderers[currentIdx].color = Color.Lerp (hidden (), shown (), interpolation);

		// Switch to the next scene if needed.
		time += Time.deltaTime;
		if ((time > sceneLength || Input.GetButtonDown("Jump")) && idx < currentScenario.Length) {
			idx++;
			time = 0;

			// Did we show the last cutscene?
			if (idx >= currentScenario.Length) {
				// TODO(corda): Based on global context, go to the right level.
				Application.LoadLevel(1);
			} else {
				// set the order of the layer to show up in front of the previous one
				var newIdx = currentScenario[idx];
				renderers[newIdx].sortingOrder = renderers[currentIdx].sortingOrder + 1;
			}
		}
		

	}

}
