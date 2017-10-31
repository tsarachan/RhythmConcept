/*
 * 
 * Whenever a button is pressed, it sends out one of these ButtonPressedEvents.
 * 
 * int button is the number of the button; each button has a unique number associated with it in the inspector. Be careful
 * not to allow two buttons to have the same number! Everything will come apart!
 * 
 */
public class ButtonPressedEvent : Event {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the number of the button; this is set in the inspector
	public readonly int button;


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//constructor
	public ButtonPressedEvent(int button){
		this.button = button;
	}
}
