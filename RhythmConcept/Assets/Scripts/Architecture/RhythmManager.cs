/*
 * 
 * The RhythmManager keeps track of buttons, and determines whether they're being pushed on-beat.
 * 
 * To create a new button, just copy one of the existing buttons in the inspector. Give it a new name that ends
 * in an int 0-9, and then go to its "Button" child object and change the number in "OnClick ()" (under the Button (Script) component)
 * to the same number. Don't repeat a number; if there needs to be more than 10 buttons, this script will have to change.
 * 
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RhythmManager {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//a dictionary of all buttons, plus everything needed to find them and populate the dictionary
	private Dictionary<int, RhythmButton> buttons = new Dictionary<int, RhythmButton>();
	private const string BUTTON_CANVAS = "Button canvas";
	private const string TIMER_IMAGE_OBJ = "Timer image";
	private const string BUTTON_OBJ = "Button";


	#region testing

	private int everyXthBeat = 2; //the game will trigger a button every Xth beat

	#endregion


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//initialize variables and start listening for events
	public void Init(){
		Services.Events.Register<ButtonPressedEvent>(HandleButtonPress);
		Services.Events.Register<BeatEvent>(HandleBeat);


		//create buttons
		foreach (Transform button in GameObject.Find(BUTTON_CANVAS).transform){
			int buttonNum = int.Parse(button.name.Substring(button.name.Length - 1));

			buttons.Add(buttonNum, new RhythmButton(buttonNum,
													button.Find(TIMER_IMAGE_OBJ).GetComponent<Image>()));
		}
	}


	/// <summary>
	/// Each frame, check all the buttons. If a button is ticking down in size, tick it again.
	/// </summary>
	public void Tick(){
		foreach (RhythmButton button in buttons.Values) if (button.timer >= 0.0f) button.timerImage.rectTransform.sizeDelta = ShrinkButton(button);
	}


	/// <summary>
	/// Start a button's image shrinking.
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="time">Time.</param>
	private void SetButton(RhythmButton button, int beats){
		button.shrinkBeats = beats;
		button.timer = (float)(BeatCounter.SECONDS_IN_MINUTE/Services.Beats.Bpm) * beats;
		button.timerImage.rectTransform.sizeDelta = new Vector2(RhythmButton.FULL_SIZE, RhythmButton.FULL_SIZE);
	}


	/// <summary>
	/// Executes SetButton for a random button.
	/// </summary>
	private void SetRandomButton(){
		List<int> buttonKeys = new List<int>();

		foreach (int key in buttons.Keys) buttonKeys.Add(key);

		SetButton(buttons[buttonKeys[Random.Range(0, buttonKeys.Count)]], 1);
	}


	/// <summary>
	/// Determine the size of a button's timer image this frame.
	/// </summary>
	/// <returns>The new size, as a Vector where x == y.</returns>
	/// <param name="button">The button whose timer image is shrinking.</param>
	private Vector2 ShrinkButton(RhythmButton button){
		button.timer -= Time.deltaTime;

		if (button.timer >= 0.0f){
			float newSize = Mathf.Lerp(RhythmButton.BUTTON_SIZE,
									   RhythmButton.FULL_SIZE,
									   button.timer/(float)((BeatCounter.SECONDS_IN_MINUTE/Services.Beats.Bpm) * button.shrinkBeats));

			return new Vector2(newSize, newSize);
		} else {
			Services.Events.Fire(new ScoreEvent(-1.0f)); //if the button is shrinking to zero, the player has missed the press
			return new Vector2(RhythmButton.ZERO, RhythmButton.ZERO);
		}
	}


	/// <summary>
	/// Everything that needs to happen when a button is pressed occurs here.
	/// </summary>
	/// <param name="e">The button pressed.</param>
	public void HandleButtonPress(Event e){
		Debug.Assert(e.GetType() == typeof(ButtonPressedEvent), "Non-ButtonPressedEvent in HandleButtonPress.");

		ButtonPressedEvent pressEvent = e as ButtonPressedEvent;


		//send out an event the ScoreManager will use to figure out how the player's score should change
		Services.Events.Fire(new ScoreEvent(buttons[pressEvent.button].timerImage.rectTransform.sizeDelta.x - RhythmButton.BUTTON_SIZE));

	
		//reset the button
		buttons[pressEvent.button].Reset();
	}


	/// <summary>
	/// Everything that happens on a beat is dealt with here.
	/// </summary>
	/// <param name="e">A BeatEvent sent out by BeatCounter.</param>
	private void HandleBeat(Event e){
		Debug.Assert(e.GetType() == typeof(BeatEvent), "Non-BeatEvent in HandleBeat.");

		if (Services.Beats.BeatCount%everyXthBeat == 0) SetRandomButton();
	}


	/// <summary>
	/// Class for the buttons players press. Puts information relating to these buttons in one place to keep
	/// button-related information organized.
	/// </summary>
	private class RhythmButton {
		public readonly int number;
		public readonly Image timerImage;
		public int shrinkBeats; //how long, in beats, it will take the timer image's width and height to go from FULL_SIZE to BUTTON_SIZE.
		public float timer;
		public const float FULL_SIZE = 5000.0f;
		public const float BUTTON_SIZE = 375.0f;
		public const float ZERO = 0.0f;


		//constructor
		public RhythmButton(int number, Image timerImage){
			this.number = number;
			this.timerImage = timerImage;


			//default initializations
			shrinkBeats = 1;
			timer = 0.0f;
		}


		/// <summary>
		/// Reset this button after, e.g., the player presses it.
		/// </summary>
		public void Reset(){
			timer = 0.0f;
			timerImage.rectTransform.sizeDelta = new Vector2(ZERO, ZERO);
		}
	}
}
