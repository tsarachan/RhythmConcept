namespace Test
{
	using UnityEngine;


	[RequireComponent(typeof(Renderer))]
	public class ColorChanger : MonoBehaviour {


		////////////////////////////////////////////////
		/// Fields
		////////////////////////////////////////////////


		//the material whose color will change
		private Material mat;


		//colors to cycle through
		private Color[] colors = new Color[6] { Color.white, Color.magenta, Color.blue, Color.red, Color.green, Color.yellow };
		private int index = 0;


		////////////////////////////////////////////////
		/// Functions
		////////////////////////////////////////////////


		//initialize the material and set its color
		private void Start(){
			mat = GetComponent<Renderer>().material;
			mat.color = colors[0];
			Services.Events.Register<BeatEvent>(ChangeColor);
		}


		/// <summary>
		/// Cycle through colors
		/// </summary>
		private void ChangeColor(global::Event e){
			Debug.Assert(e.GetType() == typeof(BeatEvent), "Non-BeatEvent in ChangeColor()");

			index++;

			if (index > colors.Length - 1) index = 0;

			mat.color = colors[index];
		}
	}
}
