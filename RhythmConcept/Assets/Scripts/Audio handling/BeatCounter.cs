using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter {


	////////////////////////////////////////////////
	/// Fields
	////////////////////////////////////////////////


	//the audio source playing the music this script will count beats for
	//this script must be able to control when it starts and stops to stay in sync
	private AudioSource speaker;
	private const string SPEAKER_OBJ = "Speaker";


	//delay before the song starts
	private double startPlayDelay = 2.0f;
	private float delayTimer = 0.0f;


	//audio time
	private double songTime = 0.0f; //how far we are through the song, in seconds
	private double startTime = 0.0f; //the audio system time when the song starts
	private double lastReportedAudioTime = 0.0f; //audio system time
	private double lastBeatTime = 0.0f; //when did the last beat start?
	private double beatDuration = 0.0f; //how long is a beat, in seconds?
	public double Bpm { get; set; } //must be manually set for the track
	private const float SECONDS_IN_MINUTE = 60.0f;


	//what beat is the song on?
	int beatCounter = 0;


	////////////////////////////////////////////////
	/// Functions
	////////////////////////////////////////////////


	//initialize variables
	public void Init(){
		speaker = GameObject.Find(SPEAKER_OBJ).GetComponent<AudioSource>();
		Bpm = 60.0f; //for testing purposes

		StartSong(startPlayDelay);
	}


	/// <summary>
	/// When starting the song, note the audio system's time, the time of the first beat, and calculate how long a beat lasts.
	/// </summary>
	/// <param name="delay">Delay before starting the song, in seconds.</param>
	private void StartSong(double delay){
		startTime = AudioSettings.dspTime;
		songTime = AudioSettings.dspTime + delay;
		lastReportedAudioTime = AudioSettings.dspTime;
		lastBeatTime = 0.0f;
		beatDuration = SECONDS_IN_MINUTE/Bpm;
		speaker.PlayScheduled(AudioSettings.dspTime + delay);
	}


	/// <summary>
	/// Each frame, determine where we are in the song (in seconds).
	/// 
	/// If the song has reached a new beat, change the cube's color and update when the last beat occurred so that
	/// we can detect the next beat.
	/// </summary>
	public void Tick(){
		//don't do anything until the song starts playing
		if (delayTimer < startPlayDelay){
			delayTimer += Time.deltaTime;
			return;
		}

		songTime = GetCurrentSongTime();

		if (songTime - startTime >= lastBeatTime + beatDuration){
			Services.Events.Fire(new BeatEvent(beatCounter));
			lastBeatTime = (beatCounter * 1.0f);
			beatCounter++;
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
		//the time from the game timer in order to get as close as possible to the actual time of the
		//audio track
		if (AudioSettings.dspTime != lastReportedAudioTime){
			temp = (songTime + AudioSettings.dspTime)/2.0f;
			lastReportedAudioTime = AudioSettings.dspTime;
		}


		return temp;
	}
}
