using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneBehavior : MonoBehaviour {

	///////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//scenes to load
	private const string GAME_SCENE = "Game";


	///////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//the play button calls this to start the game
	public void StartGame(){
		SceneManager.LoadScene(GAME_SCENE);
	}
}
