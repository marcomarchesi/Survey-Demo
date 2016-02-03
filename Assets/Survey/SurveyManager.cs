// Survey Manager 1.0
// by Marco Marchesi, 2/3/2016

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SurveyManager: MonoBehaviour 
{

	// MySQL db handler
	public DBConnection db;

	// UI
	private Canvas surveyCanvas;
	private Image star1,star2,star3,star4,star5;
	private Image thanksImage;
	private Text questionText;


	private bool loading;
	private bool rated;
	private int questionIndex;
	private int rate;


	// some sounds
	private AudioSource star_wav;
	private AudioSource thanks_wav;

	// A serialized list of questions
	[SerializeField] private List<string> questions;







	void Start() {

		surveyCanvas = GameObject.Find("Survey Canvas").GetComponent<Canvas>();

		//instance db
		db = surveyCanvas.gameObject.AddComponent<DBConnection>() as DBConnection;

		// my rating stars
		star1 = GameObject.Find ("1-star").GetComponent<Image> ();
		star2 = GameObject.Find ("2-star").GetComponent<Image> ();
		star3 = GameObject.Find ("3-star").GetComponent<Image> ();
		star4 = GameObject.Find ("4-star").GetComponent<Image> ();
		star5 = GameObject.Find ("5-star").GetComponent<Image> ();
		questionText = GameObject.Find("Question Text").GetComponent<Text> ();

		// some sound after each question
		star_wav = surveyCanvas.GetComponent<AudioSource> ();

		// thanks message
		thanksImage = GameObject.Find ("Thanks").GetComponent<Image> ();
		thanks_wav = thanksImage.GetComponent<AudioSource> ();

		// default for stars
		surveyCanvas.enabled = false;

		//start values
		rated = false;
		questionIndex = 0;
		questionText.text = questions[questionIndex];

		loading = true; // survey started

		// just an event to start the survey (wait 1 sec)
		Invoke ("Run", 1);

	}
			

	public void Update(){
				// hide/show VR
				if (Input.GetKeyDown ("space"))
					ToggleVRMode ();

			if (rated == false) {
				star1.enabled = false;
				star2.enabled = false;
				star3.enabled = false;
				star4.enabled = false;
				star5.enabled = false;
			}
	}

			
		// User clicks one-to-five stars
	public void ShowStars()
	{

		if (loading) {

			// get pointer direction
			Vector3 pointer_dir = Cardboard.Controller.Head.Gaze.direction;

			// just a sketch, needs improvement with a hit test
			if (pointer_dir.x > -0.60 && pointer_dir.x < -0.35)
			{
				rate = 1;
			}
			else if(pointer_dir.x > -0.35 && pointer_dir.x < -0.17)
			{
				rate = 2;
			}
			else if(pointer_dir.x > -0.17 && pointer_dir.x < 0.17)
			{
				rate = 3;
			}
			else if(pointer_dir.x > 0.17 && pointer_dir.x < 0.35)
			{
				rate = 4;
			}
			else if(pointer_dir.x > 0.35 && pointer_dir.x < 0.60)
			{
				rate = 5;
			}
			else
				rate = 0;


			// turn on/off the 1-5 stars
			switch (rate) 
			{
			case 1:
				star1.enabled = true;
				star2.enabled = false;
				star3.enabled = false;
				star4.enabled = false;
				star5.enabled = false;
				rated = true;
				break;
			case 2:
				star1.enabled = true;
				star2.enabled = true;
				star3.enabled = false;
				star4.enabled = false;
				star5.enabled = false;
				rated = true;
				break;
			case 3:
				star1.enabled = true;
				star2.enabled = true;
				star3.enabled = true;
				star4.enabled = false;
				star5.enabled = false;
				rated = true;
				break;
			case 4:
				star1.enabled = true;
				star2.enabled = true;
				star3.enabled = true;
				star4.enabled = true;
				star5.enabled = false;
				rated = true;
				break;
			case 5:
				star1.enabled = true;
				star2.enabled = true;
				star3.enabled = true;
				star4.enabled = true;
				star5.enabled = true;
				rated = true;
				break;
			default:
				star1.enabled = false;
				star2.enabled = false;
				star3.enabled = false;
				star4.enabled = false;
				star5.enabled = false;
				rated = false;
				break;
			}

			if (rated == true) {
				star_wav.Play ();
				//Ask a new question
				Invoke ("AskQuestion", 1);
			}
		}

	}	

	// start the survey
	public void Run() {
			print ("Start the survey");
			surveyCanvas.enabled = true;
	}

	// ask questions and send results to MySQL db
	private void AskQuestion()
	{
		if (rated) {
			//send to DB
			//TODO user will change
			db.Insert ("Marco", questions [questionIndex], rate);
			//reset rate
			rate = 0;
			rated = false;
		}

		//update question
		questionIndex++;

		if (questionIndex < questions.Capacity) {
			questionText.text = questions [questionIndex];
		}
		else{
			ShowMessage ();
		}
	}

	private void ShowMessage()
	{
		thanks_wav.Play ();
		thanksImage.enabled = true;
		loading = false;
		print ("The survey is finished");
	}

	public void ToggleVRMode() {
	    Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
	}

	void LateUpdate() {
		Cardboard.SDK.UpdateState();
		if (Cardboard.SDK.BackButtonPressed) {
			Application.Quit();
		}
	}
			
}
