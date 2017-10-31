/*
 * 
 * When the player presses a button, RhythmManager sends out a ScoreEvent the ScoreManager can use to figure out what the score should be
 * for that press.
 * 
 */
public class ScoreEvent : Event {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the number of pixels' difference between the timer image's size at the moment of the player's press and the button's size
	public readonly float missAmount;



	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//constructor
	public ScoreEvent(float missAmount){
		this.missAmount = missAmount;
	}
}
