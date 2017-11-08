/*
 * 
 * This class determines when a beat is occurring by counting the audio system's time. It doesn't do anything with beats, and it doesn't "listen"
 * to the song--it just reports when a beat has occurred.
 * 
 */
namespace Test
{
	using UnityEngine;
	using UnityEngine.UI;

	public class BeatDetector : MonoBehaviour {

		////////////////////////////////////////////////
		/// Fields
		////////////////////////////////////////////////


		//the audio source this is detecting beats from
		private AudioSource speaker;
		private const string SPEAKER_OBJ = "Speaker";


		//delay before the song starts
		private double startPlayDelay = 0.0f;


		//audio time
		private double songTime = 0.0f; //how far we are through the song
		private double lastReportedAudioTime = 0.0f; //audio system time
		private double lastBeatTime = 0.0f; //when did the last beat start?
		private double beatDuration = 0.0f; //how long is a beat, in seconds?
		private double bpm = 60.0f; //must be manually set for the track
		private const float SECONDS_IN_MINUTE = 60.0f;




		//the cube being used for tests
		private ColorChanger cube;
		private const string CUBE_OBJ = "Test cube";


		//UI
		private Image timerCircle;
		private const string CIRCLE_OBJ = "Helper fill";


		////////////////////////////////////////////////
		/// Functions
		////////////////////////////////////////////////


		//initialize variables
		private void Start(){
			speaker = GameObject.Find(SPEAKER_OBJ).GetComponent<AudioSource>();
			cube = GameObject.Find(CUBE_OBJ).GetComponent<ColorChanger>();
			timerCircle = GameObject.Find(CIRCLE_OBJ).GetComponent<Image>();

			StartSong(startPlayDelay);
		}


		/// <summary>
		/// When starting the song, note the audio system's time, the time of the first beat, and calculate how long a beat lasts.
		/// </summary>
		/// <param name="delay">Delay before starting the song, in seconds.</param>
		private void StartSong(double delay){
			lastReportedAudioTime = AudioSettings.dspTime;
			lastBeatTime = (double)Time.time + delay;
			beatDuration = SECONDS_IN_MINUTE/bpm;
			speaker.Play();
		}


		/// <summary>
		/// Each frame, determine where we are in the song (in milliseconds). Provide appropriate feedback.
		/// 
		/// If the song has reached a new beat, change the cube's color and update when the last beat occurred so that
		/// we can detect the next beat.
		/// </summary>
		private void Update(){
			songTime = GetCurrentSongTime();
			timerCircle.fillAmount = (float)((songTime - lastBeatTime)/beatDuration);

			if (songTime >= lastBeatTime + beatDuration){
				cube.ChangeColor();
				lastBeatTime = songTime;
			}
		}


		/// <summary>
		/// Determine where we are in the song (in milliseconds).
		/// </summary>
		/// <returns>How far we are through the song (in ms).</returns>
		private double GetCurrentSongTime(){
			double temp = songTime;

			//track where we are in the song, using the game timer
			temp += Time.deltaTime;


			//when there's current information from the audio system, average its reported time with
			//the time fromt the game timer in order to get as close as possible to the actual time of the
			//audio track
			if (AudioSettings.dspTime != lastReportedAudioTime){
				temp = (songTime + AudioSettings.dspTime)/2.0f;
				lastReportedAudioTime = AudioSettings.dspTime;
				Debug.Log("temp == " + temp + ", lastReportedAudioTime == " + lastReportedAudioTime);
			}


			return temp;
		}
	}
}
