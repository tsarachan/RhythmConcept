/*
 * 
 * Every button has this script attached to it.
 * 
 * All this does is send out an event when the button is pressed. It's up to other scripts to listen for that event, and to
 * respond appropriately.
 * 
 */
using UnityEngine;

public class ButtonBehavior : MonoBehaviour {


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	/// <summary>
	/// When this button is pressed, this function sends out an event with the button's number.
	/// </summary>
	/// <param name="myNumber">The number of this button, set in the inspector.</param>
	public void GetPressed(int myNumber){
		Services.Events.Fire(new ButtonPressedEvent(myNumber));
	}
}
