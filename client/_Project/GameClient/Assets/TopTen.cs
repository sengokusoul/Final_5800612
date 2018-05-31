using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopTen : MonoBehaviour {
	public Text youName;
	public Text youScore;

	public Text Top1;
	public Text Top2;
	public Text Top3;
	public Text Top4;
	public Text Top5;
	public Text Top6;
	public Text Top7;
	public Text Top8;
	public Text Top9;
	public Text Top10;

	public Text s1;
	public Text s2;
	public Text s3;
	public Text s4;
	public Text s5;
	public Text s6;
	public Text s7;
	public Text s8;
	public Text s9;
	public Text s10;
	// Use this for initialization
	void Start () {
		youName.text = GamManager.getName;
		youScore.text = GamManager.getScore.ToString();

		string Url = "http://ec2-13-126-252-100.ap-south-1.compute.amazonaws.com:8081/Topusers";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		Stream stream = response.GetResponseStream();
		string responseBody = new StreamReader(stream).ReadToEnd();

		Player[] players = JsonConvert.DeserializeObject<Player[]>(responseBody);

		Top1.text = "1. "+players[0].Name; 
		Top2.text = "2. "+players[1].Name; 
		Top3.text = "3. "+players[2].Name; 
		Top4.text = "4. "+players[3].Name; 
		Top5.text = "5. "+players[4].Name; 
		Top6.text = "6. "+players[5].Name; 
		Top7.text = "7. "+players[6].Name; 
		Top8.text = "8. "+players[7].Name; 
		Top9.text = "9. "+players[8].Name; 
		Top10.text = "10. "+players[9].Name; 

		s1.text = players[0].Score.ToString();
		s2.text = players[1].Score.ToString();
		s3.text = players[2].Score.ToString();
		s4.text = players[3].Score.ToString();
		s5.text = players[4].Score.ToString();
		s6.text = players[5].Score.ToString();
		s7.text = players[6].Score.ToString();
		s8.text = players[7].Score.ToString();
		s9.text = players[8].Score.ToString();
		s10.text =players[9].Score.ToString();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void back()
	{
		SceneManager.LoadScene ("PlayerMain");
	}
}
