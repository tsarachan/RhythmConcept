/*
 * 
 * To control timing, allow for pausing, etc., this script has the only Awake and Update functions in the scene.
 * 
 */
using UnityEngine;

public class GameManager : MonoBehaviour {


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	/// <summary>
	/// Initialize variables.
	/// </summary>
	private void Awake(){
		Services.Events = new EventManager();
		Services.Score = new ScoreManager();
		Services.Score.Init();
		Services.Rhythm = new RhythmManager();
		Services.Rhythm.Init();
	}


	/// <summary>
	/// Central control for the game loop. Everything that needs to update each frame gets updated from here; nothing else
	/// updates itself.
	/// </summary>
	private void Update(){
		Services.Rhythm.Tick();
	}
}
