﻿// Survey Manager 1.0
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
	private Button star1,star2,star3,star4,star5;
	private Image thanksImage;
	private Text questionText;

	private bool loading;
	private bool rated;
	private int questionIndex;
	private int rate;

	private Sprite gold_star;
	private Sprite gray_star;


    // some sounds
    private AudioSource star_wav;
	private AudioSource thanks_wav;

	// A serialized list of questions
	[SerializeField] private List<string> questions;


	void Start() {

		surveyCanvas = GameObject.Find("Survey Canvas").GetComponent<Canvas>();

		//instance db
		db = surveyCanvas.gameObject.AddComponent<DBConnection>() as DBConnection;

		gold_star = Resources.Load <Sprite>("star");
		gray_star = Resources.Load <Sprite>("gray_star");

		// my rating stars
		star1 = GameObject.Find ("1-star").GetComponent<Button> ();
		star2 = GameObject.Find ("2-star").GetComponent<Button> ();
		star3 = GameObject.Find ("3-star").GetComponent<Button> ();
		star4 = GameObject.Find ("4-star").GetComponent<Button> ();
		star5 = GameObject.Find ("5-star").GetComponent<Button> ();
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


    }

	public void OneStars(){
		star1.image.sprite = gold_star;
		rate = 1;
	}

	public void TwoStars(){
		star1.image.sprite = gold_star;
		star2.image.sprite = gold_star;
		rate = 2;
	}

	public void ThreeStars(){
		star1.image.sprite = gold_star;
		star2.image.sprite = gold_star;
		star3.image.sprite = gold_star;
		rate = 3;
	}

	public void FourStars(){
		star1.image.sprite = gold_star;
		star2.image.sprite = gold_star;
		star3.image.sprite = gold_star;
		star4.image.sprite = gold_star;
		rate = 4;
	}

	public void FiveStars(){
		star1.image.sprite = gold_star;
		star2.image.sprite = gold_star;
		star3.image.sprite = gold_star;
		star4.image.sprite = gold_star;
		star5.image.sprite = gold_star;
		rate = 5;
	}

	public void DeselectStars()
	{
		star1.image.sprite = gray_star;
		star2.image.sprite = gray_star;
		star3.image.sprite = gray_star;
		star4.image.sprite = gray_star;
		star5.image.sprite = gray_star;
	}
		

    public void SelectRate()
    {
        if (loading)
        {
            rated = true;
            star_wav.Play();
            //Ask a new question
            Invoke("AskQuestion", 1);
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
			
}
