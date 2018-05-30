using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreBord : MonoBehaviour {
	public Text Name;
	// Use this for initialization
	void Start () {
		Name.text = GamManager.getName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Score(){
		SceneManager.LoadScene("Show");
	}
	public void back(){
		SceneManager.LoadScene("GameCli");
	}
	public void Startgame(){
		SceneManager.LoadScene("GameScene");
	}
}
