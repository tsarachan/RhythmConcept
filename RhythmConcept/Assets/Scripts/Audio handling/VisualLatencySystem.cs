using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualLatencySystem : MonoBehaviour {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the visual latency, as determined by this script
	//static so that it can be accessed later
	public static float VisualLatency { get; private set; }


	//the cube the player will tap in time with
	private Test.ColorChanger cube;
	private const string CUBE = "Test cube";


	//used to test visual latency
	private const int TOTAL_LATENCIES_NEEDED = 15; //how many latencies will be collected
	private const float CHANGE_PERIOD = 1.0f; //time between signals to click
	private float lastChangeTime = 0.0f; //when a signal was sent
	private float timer = 0.0f;
	private List<float> latencies = new List<float>(); //the results from individual visual latency tests
	private List<float> changeTimes = new List<float>(); //the exact times when the cube changed color


	//loading the next scene
	private const string SCENE_TO_LOAD = "Audio tests";
	private const float LOAD_DELAY = 1.0f;


	//UI
	private Text instructions;
	private const string INSTRUCTIONS_OBJ = "Instructions";
	private const string THANKS = "Thanks! Visual latency: ";
	private const string SECONDS = "s";
	private Image helperFill;
	private const string HELPER_OBJ = "Helper fill";


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//initialize variables
	private void Start(){
		cube = GameObject.Find(CUBE).GetComponent<Test.ColorChanger>();
		instructions = GameObject.Find(INSTRUCTIONS_OBJ).GetComponent<Text>();
		helperFill = GameObject.Find(HELPER_OBJ).GetComponent<Image>();
		lastChangeTime = Time.time;
		timer = Time.time;
	}


	/// <summary>
	/// Each frame, increase the timer. If it's been CHANGE_PERIOD since the last test, run another test
	/// </summary>
	private void Update(){
		if (latencies.Count < TOTAL_LATENCIES_NEEDED || changeTimes.Count < TOTAL_LATENCIES_NEEDED){
			timer += Time.deltaTime;
			helperFill.fillAmount = ((timer - lastChangeTime)/(CHANGE_PERIOD));

			if (timer >= lastChangeTime + CHANGE_PERIOD){
				cube.ChangeColor();
				lastChangeTime = Time.time;
				changeTimes.Add(Time.time);
			}

			if (Input.GetMouseButtonDown(0) && latencies.Count < TOTAL_LATENCIES_NEEDED) latencies.Add(Time.time); //record the times when the player clicks

			if (latencies.Count == TOTAL_LATENCIES_NEEDED && changeTimes.Count == TOTAL_LATENCIES_NEEDED) StartCoroutine(EndTest());
		}
	}


	/// <summary>
	/// Determines the average visual latency over the tests performed.
	/// </summary>
	/// <returns>The average visual latency.</returns>
	private float CalculateVisualLatency(){
		Debug.Assert(latencies.Count == TOTAL_LATENCIES_NEEDED, "Incorrect number of latencies measured: " + latencies.Count);

		List<float> actualLatencies = new List<float>();

		for (int i = 0; i < TOTAL_LATENCIES_NEEDED; i++) actualLatencies.Add(latencies[i] - changeTimes[i]);

		float totalLatency = 0.0f;

		foreach (float latency in actualLatencies) totalLatency += latency;

		return totalLatency/TOTAL_LATENCIES_NEEDED;
	}


	/// <summary>
	/// Handles the end of the test.
	/// </summary>
	private IEnumerator EndTest(){
		//the purpose of this scene! Get the visual latency.
		VisualLatency = CalculateVisualLatency();
		instructions.text = THANKS + VisualLatency.ToString() + SECONDS;


		float timer = 0.0f;

		while (timer < LOAD_DELAY){
			timer += Time.deltaTime;

			yield return null;
		}

		SceneManager.LoadScene(SCENE_TO_LOAD);
	}
}
