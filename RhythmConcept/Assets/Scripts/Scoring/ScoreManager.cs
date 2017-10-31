using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the score tracks
	private Image currentTrack;
	private Image previousTrack;
	private const string CURRENT_TRACK_OBJ = "Current track";
	private const string PREVIOUS_TRACK_OBJ = "Previous track";


	//how much of the track fills for a button press? How much of the track does a player lose for missing a press?
	//capped at 1.0f, a full track
	private const float BAD_SCORE = 0.1f;
	private const float OK_SCORE = 0.25f;
	private const float GREAT_SCORE = 0.5f;
	private const float MISS = -0.15f;


	//how close to perfect does the player have to be for each type of score?
	private const float GREAT_TOLERANCE = 100.0f;
	private const float OK_TOLERANCE = 250.0f;
	private const float BAD_TOLERANCE = 1000.0f;
	public const float MISS_INDICATOR = 9999.0f; //nonsense value RhythmManager can send to indicate a miss


	//base values for track fill
	private const float NO_FILL = 0.0f;
	private const float FULL_FILL = 1.0f;


	//colors for the tracks
	private enum Rainbow { None, Violet, Indigo, Blue, Green, Yellow, Orange, Red };
	private Rainbow currentColor = Rainbow.None;
	private List<string> rainbowColors = new List<string>() { "#00000000",
															  "#9400D3FF",
															  "#4B0082FF",
															  "#0000FFFF",
															  "#00FF00FF",
															  "#FFFF00FF",
															  "#FF7F00FF",
															  "#FF0000FF" };
	private List<Color> colors = new List<Color>();


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//initialize variables and set the score-tracking borders to initial values; register to listen for ScoreEvents from RhythmManager.
	public void Init(){
		currentTrack = GameObject.Find(CURRENT_TRACK_OBJ).GetComponent<Image>();
		previousTrack = GameObject.Find(PREVIOUS_TRACK_OBJ).GetComponent<Image>();

		currentTrack.fillAmount = NO_FILL;
		previousTrack.fillAmount = FULL_FILL;

		colors = MakeColors();
		currentTrack.color = GetCurrentColor();

		Services.Events.Register<ScoreEvent>(DetermineScoreChange);
	}


	/// <summary>
	/// Create a list of the colors of a rainbow, backward (VIBGYOR).
	/// </summary>
	/// <returns>The list of colors.</returns>
	private List<Color> MakeColors(){
		List<Color> temp = new List<Color>();

		Color newColor;

		foreach (string color in rainbowColors){
			if (ColorUtility.TryParseHtmlString(color, out newColor)) temp.Add(newColor);
			else Debug.Log("Unable to add " + color);
		}

		return temp;
	}


	/// <summary>
	/// Step the current score track's color through the colors of a rainbow, backward (VIBGYOR). Start at violet, stop at red.
	/// </summary>
	/// <returns>The current score track's color.</returns>
	private Color GetCurrentColor(){
		if (currentColor != Rainbow.Red) currentColor++;

		if (currentColor == Rainbow.Red) return colors[(int)Rainbow.Red];
		return colors[(int)currentColor];
	}


	/// <summary>
	/// Increase or decrease the player's score based on how accurately timed their button press was.
	/// </summary>
	/// <param name="e">The ScoreEvent fired by RhythmManager.</param>
	private void DetermineScoreChange(Event e){
		Debug.Assert(e.GetType() == typeof(ScoreEvent), "Non-ScoreEvent in DetermineScoreChange.");

		ScoreEvent scoreEvent = e as ScoreEvent;

		if (scoreEvent.missAmount >= 0.0f){
			if (scoreEvent.missAmount <= GREAT_TOLERANCE) currentTrack.fillAmount += GREAT_SCORE;
			else if (scoreEvent.missAmount <= OK_TOLERANCE) currentTrack.fillAmount += OK_SCORE;
			else if (scoreEvent.missAmount <= BAD_TOLERANCE) currentTrack.fillAmount += BAD_SCORE;

			//if the missAmount was TOO positive, it's a miss
			else currentTrack.fillAmount += MISS;

		//the missAmount was negative, meaning the player pressed the button too late
		} else currentTrack.fillAmount += MISS;

		if (currentTrack.fillAmount >= FULL_FILL) GoToNext();
	}


	/// <summary>
	/// Increment the score system by turning the previous track the current track's color, and then resetting the current track with a new color.
	/// In this way, imitate creating a new track.
	/// </summary>
	private void GoToNext(){
		previousTrack.color = colors[(int)currentColor];
		previousTrack.fillAmount = FULL_FILL;

		currentTrack.color = GetCurrentColor();
		currentTrack.fillAmount = NO_FILL;
	}
}
