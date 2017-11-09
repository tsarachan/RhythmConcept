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
	private const float OK_TOLERANCE = 1000.0f;
	private const float BAD_TOLERANCE = 2000.0f;
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
		currentTrack.color = GetNextColor();

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
	/// <returns>The new color.</returns>
	private Color GetNextColor(){
		if (currentColor != Rainbow.Red) currentColor++;

		if (currentColor == Rainbow.Red) return colors[(int)Rainbow.Red];
		return colors[(int)currentColor];
	}


	/// <summary>
	/// Step the current color backward through the backward rainbow (VIBGYOR). Start at red, end at violet.
	/// </summary>
	/// <returns>The new color.</returns>
	private Color GetPrevColor(){
		if (currentColor != Rainbow.Violet) currentColor--;
		if (currentColor == Rainbow.Violet) return colors[(int)Rainbow.Violet];
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
			if (scoreEvent.missAmount <= GREAT_TOLERANCE){
				currentTrack.fillAmount += GREAT_SCORE;
				Debug.Log("Great");
			}
			else if (scoreEvent.missAmount <= OK_TOLERANCE){
				currentTrack.fillAmount += OK_SCORE;
				Debug.Log("OK");
			}
			else if (scoreEvent.missAmount <= BAD_TOLERANCE){
				currentTrack.fillAmount += BAD_SCORE;
				Debug.Log("Bad");
			}

			//if the missAmount was TOO positive, it's a miss
			else {
				HandleMiss();
			}

		//the missAmount was negative, meaning the player pressed the button too late
		} else HandleMiss();

		if (currentTrack.fillAmount >= FULL_FILL) GoToNext();
	}


	/// <summary>
	/// Set the color and fill amount of the previous and current score tracks after a miss.
	/// </summary>
	private void HandleMiss(){
		//if the current score track is full to at least the MISS amount, just reduce its fill
		if (currentTrack.fillAmount >= Mathf.Abs(MISS)) currentTrack.fillAmount += MISS;

		//if the current track is at the first color, but it's only slightly full, set it to zero.
		else if (currentTrack.color == colors[(int)Rainbow.Violet]) currentTrack.fillAmount = NO_FILL;

		//if the current track is not at the first color, and is only slightly full,
		//set it and the previous track both back a color and then set the current track to be mostly full of the new color
		else GoToPrev(FULL_FILL + currentTrack.fillAmount + MISS);
	}


	/// <summary>
	/// Increment the score system by turning the previous track the current track's color, and then resetting the current track with a new color.
	/// In this way, imitate creating a new track.
	/// </summary>
	private void GoToNext(){
		previousTrack.color = colors[(int)currentColor];
		previousTrack.fillAmount = FULL_FILL;

		currentTrack.color = GetNextColor();
		currentTrack.fillAmount = NO_FILL;
	}


	/// <summary>
	/// Decrement the score system by moving the current track backwards through the backwards rainbow (VIBGYOR), stopping at violet,
	/// and pushing the previous track back a further color.
	/// </summary>
	/// <param name="currentFill">The intended fill for the current track after the system is done decrementing the colors.</param>
	private void GoToPrev(float currentFill){
		currentTrack.color = GetPrevColor();
		previousTrack.color = colors[(int)currentColor - 1];
		previousTrack.fillAmount = FULL_FILL;
		currentTrack.fillAmount = currentFill;
	}
}
