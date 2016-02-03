// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using System.Collections;

public class SurveyManager: MonoBehaviour {


  void Start() {
    SetGazedAt(false);
  }
		

	void Update(){
			if (Input.GetKeyDown ("space"))
				ToggleVRMode ();
	}

  void LateUpdate() {
    Cardboard.SDK.UpdateState();
    if (Cardboard.SDK.BackButtonPressed) {
      Application.Quit();
    }
  }

  public void SetGazedAt(bool gazedAt) {
		print ("Hello there");
  }

	// User clicks one-to-five stars
	public void OneStar(){
		
	}
	public void TwoStars()
	{
	}
	public void ThreeStars()
	{
	}
	public void FourStars()
	{

	}
	public void FiveStars()
	{
	}


  public void Reset() {

  }

  public void ToggleVRMode() {
    Cardboard.SDK.VRModeEnabled = !Cardboard.SDK.VRModeEnabled;
  }
		
}
