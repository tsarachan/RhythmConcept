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
		}


		/// <summary>
		/// Cycle through colors
		/// </summary>
		public void ChangeColor(){
			index++;

			if (index > colors.Length - 1) index = 0;

			mat.color = colors[index];
		}
	}
}
