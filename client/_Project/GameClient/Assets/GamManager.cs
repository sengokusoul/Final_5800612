using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamManager : MonoBehaviour {
	//public string Url = "http://localhost:8081/users";
	public InputField name;
	public InputField pass;
	static public string getName;
	static public string getPass;
	static public int getScore;
	public GameObject Head1;
	public GameObject Head2;
	public GameObject B1;
	public GameObject B2;
	public GameObject B3;

	// Use this for initialization
	void Start () {
		name.text = "";
		pass.text = "";
		getName = "";
		getPass = "";
		getScore = 0;
	/*	try
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream stream = response.GetResponseStream();
			string responseBody = new StreamReader(stream).ReadToEnd();

			print(responseBody);

			Player[] players = JsonConvert.DeserializeObject<Player[]>(responseBody);
			print(players[0].Name);
		}
		catch(WebException ex)
		{
		}
*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Register(){
		name.text = "";
		pass.text = "";
		Head1.SetActive(false);
		Head2.SetActive(true);
		B1.SetActive (false);
		B2.SetActive (false);
		B3.SetActive(true);
	}
	public void Login(){
		getName = name.text;
		getPass = pass.text;
		string Url = "ec2-13-126-252-100.ap-south-1.compute.amazonaws.com:8081/userpass/"+getName+"/"+getPass;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		Stream stream = response.GetResponseStream();
		string responseBody = new StreamReader(stream).ReadToEnd();

		Player[] players = JsonConvert.DeserializeObject<Player[]>(responseBody);
		getScore = players [0].Score;
		if(players[0].Check_ID == 1) 
		{
			name.text = "";
			pass.text = "";
			SceneManager.LoadScene("PlayerMain");
		}
	}
	public void SubmitRegister(){
		if (name.text != "" & pass.text != "") {
			getName = name.text;
			getPass = pass.text;
			string Url = "ec2-13-126-252-100.ap-south-1.compute.amazonaws.com:8081/user/add/user?Name=" + getName + "&Pasword=" + getPass;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (Url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			Stream stream = response.GetResponseStream ();
			string responseBody = new StreamReader (stream).ReadToEnd ();
			name.text = "";
			pass.text = "";
			Head1.SetActive (true);
			Head2.SetActive (false);
			B1.SetActive (true);
			B2.SetActive (true);
			B3.SetActive (false);
		} 
	}
}
