namespace Test
{
	using UnityEngine;

	public class BeatTestManager : MonoBehaviour {

		////////////////////////////////////////////////
		/// Functions
		////////////////////////////////////////////////


		private void Awake(){
			Services.Events = new EventManager();
		}
	}
}
