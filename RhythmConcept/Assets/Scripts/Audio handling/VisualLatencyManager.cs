using UnityEngine;

public class VisualLatencyManager : MonoBehaviour {

	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	private void Awake(){
		Services.Events = new EventManager();
	}
}
