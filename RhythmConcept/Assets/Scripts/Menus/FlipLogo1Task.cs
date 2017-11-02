using UnityEngine;


public class FlipLogo1Task : Task {


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//the logos
	private readonly RectTransform logo1;
	private const string LOGO_1_OBJ = "Logo 1";


	//rotation
	private Vector3 rotSpeed = new Vector3(0.0f, 135.0f, 0.0f); //in degrees/second
	private const float LOGO_1_COMPLETE = 90.0f;


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//constructor
	public FlipLogo1Task(){
		logo1 = GameObject.Find(LOGO_1_OBJ).GetComponent<RectTransform>();
	}


	/// <summary>
	/// Each frame, rotate the logo until it reaches its final position
	/// </summary>
	public override void Tick (){
		if (logo1.rotation.eulerAngles.y + rotSpeed.y * Time.deltaTime >=  LOGO_1_COMPLETE){ //don't overshoot
			logo1.rotation = Quaternion.Euler(new Vector3(0.0f, LOGO_1_COMPLETE, 0.0f));
			SetStatus(TaskStatus.Success);
		} else logo1.Rotate(rotSpeed * Time.deltaTime);
	}
}
