using UnityEngine;
using System.Collections;
using Facebook.Unity;
using GameAnalyticsSDK;
using System.Collections.Generic;

public class Analytics : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GameEventManager.OnMessage += OnMessageHandler;

		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback,OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
		GameAnalytics.Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

    private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
			Debug.Log("The Facebook SDK Initialized");
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	void OnApplicationPause (bool pauseStatus)
	{
		// Check the pauseStatus to see if we are in the foreground
		// or background
		if (!pauseStatus) {
			//app resume
			if (FB.IsInitialized) {
				FB.ActivateApp();
			} else {
				//Handle FB.Init
				FB.Init(InitCallback);
			}
		}
	}

    void OnMessageHandler(string msg, object obj) {
        if(msg == "level_completed"){

            int level  = (int)obj;
            var parameters = new Dictionary<string, object>();
            parameters["level"] = level;
            FB.LogAppEvent(
                "level_completed",
                parameters:parameters
            );

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,level.ToString(),level);

        }
        else if (msg == "level_loaded")
        {
			int level = (int)obj;
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, level.ToString());
		}
    }

}
