using UnityEngine;


public class FlipLogo2Task : Task {


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//the logos
	private readonly RectTransform logo2;
	private const string LOGO_2_OBJ = "Logo 2";


	//rotation
	private Vector3 rotSpeed = new Vector3(0.0f, 135.0f, 0.0f); //in degrees/second
	private const float LOGO_2_COMPLETE = 360.0f;


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//constructor
	public FlipLogo2Task(){
		logo2 = GameObject.Find(LOGO_2_OBJ).GetComponent<RectTransform>();
	}


	/// <summary>
	/// Each frame, rotate the logo until it reaches its final position
	/// </summary>
	public override void Tick (){
		if (logo2.rotation.eulerAngles.y + rotSpeed.y * Time.deltaTime >=  LOGO_2_COMPLETE){ //don't overshoot
			logo2.rotation = Quaternion.Euler(new Vector3(0.0f, LOGO_2_COMPLETE, 0.0f));
			SetStatus(TaskStatus.Success);
		} else logo2.Rotate(rotSpeed * Time.deltaTime);
	}
}
