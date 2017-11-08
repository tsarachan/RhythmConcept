using UnityEngine;
using UnityEngine.SceneManagement;

public class FlipLogoSystem: MonoBehaviour {


	///////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the next scene to load
	private const string NEXT_SCENE = "Measure visual latency";


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//initialize variables
	private void Start(){
		Services.Tasks = new TaskManager();
	}


	/// <summary>
	/// The task manager runs each frame, and handles flipping the logo.
	/// </summary>
	private void Update(){
		Services.Tasks.Tick();
	}


	/// <summary>
	/// When the player presses on the first logo, flip it out of view and then flip the second logo into view.
	/// </summary>
	public void FlipLogo(){
		if (Services.Tasks.CheckForTaskOfType<FlipLogo1Task>() || Services.Tasks.CheckForTaskOfType<FlipLogo2Task>()) return; //do nothing if the logo is flipping

		FlipLogo1Task flip1Task = new FlipLogo1Task();
		FlipLogo2Task flip2Task = new FlipLogo2Task();
		flip1Task.Then(flip2Task);
		Services.Tasks.AddTask(flip1Task);
	}


	/// <summary>
	/// When the player presses the second logo, load the next scene.
	/// </summary>
	public void GoToMenu(){
		SceneManager.LoadScene(NEXT_SCENE);
	}
}
