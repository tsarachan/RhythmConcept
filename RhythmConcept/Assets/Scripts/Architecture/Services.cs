/*
 * 
 * This script provides easy access to managers and other game systems.
 * 
 */
using UnityEngine;

public static class Services {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the event manager
	private static EventManager events;
	public static EventManager Events {
		get {
			Debug.Assert(events != null, "No event manager. Are services being created in the wrong order?");
			return events;
		}
		set { events = value; }
	}


	//RhythmManager figures out whether the player is on-beat
	private static RhythmManager rhythm;
	public static RhythmManager Rhythm {
		get {
			Debug.Assert(rhythm != null, "No rhythm manager. Are services being created in the wrong order?");
			return rhythm;
		}
		set { rhythm = value; }
	}


	//ScoreManager operates the image fills that keep track of the player's score
	private static ScoreManager score;
	public static ScoreManager Score {
		get {
			Debug.Assert(score != null, "No score manager. Are services being created in the wrong order?");
			return score;
		}
		set { score = value; }
	}
}
