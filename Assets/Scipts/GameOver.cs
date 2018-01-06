using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    PlacementChecker checker;

    public GameObject game_over_text;

    private void Start()
    {
        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();
    }

    void Update ()
    {
		if (checker.IsLevelOver())
        {
            game_over_text.SetActive(true);
        }
	}
}
