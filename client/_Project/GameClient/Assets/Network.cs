using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.IO;
using UnityEngine.UI;
using System.Net;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using UnityEngine.SceneManagement;


public class Network : MonoBehaviour {
	bool gameStart;
	bool isStart;
	bool onmax;
	bool count;

	static SocketIOComponent socket;
	int cPlayer;
	float ctimes;
	int P1sc = 0;
	int P2sc = 0;
	public Text Name1;
	bool en ;
	bool full;
	int en1 ;
	public Text HiScore1;
	public Text Score1;
	public Text Name2;
	public Text pTimes;
	public Text HiScore2;
	public Text Score2;
	public GameObject player1;
	public GameObject player2;
	public GameObject B1;
	public GameObject B2;
	public GameObject B3;
	bool isP1 ;
	String Nameo1;
	String His1;
	bool onB;
	//public GameObject spPoint;

	// Use this for initialization
	void Start () {
		isP1 = true;
		count = false;
		full = false;
		ctimes = 61;
		pTimes.text = "";
		gameStart = false;
		isStart = true;
		onmax = false;
		P1sc = 0;
		P2sc = 0;
		B1.SetActive (false);
		B2.SetActive (false);
		B3.SetActive (false);
		onB = false;
		en = false;
		socket = GetComponent<SocketIOComponent> ();
		cPlayer = 0;
		player1.SetActive (false);
		player2.SetActive (false);
		socket.On ("open", OnConnected);
		socket.On ("spawn", OnSpawn);
		socket.On ("Enemy", OnEnemy);
		socket.On ("P1", P1score);
		socket.On ("P2", P2score);
		socket.On ("gamestart", ctime);
	}
	void OnConnected(SocketIOEvent e)
	{
		print ("Server Connected");
		/*JSONObject j = new JSONObject (JSONObject.Type.OBJECT);

		j.AddField ("Score", GamManager.getScore);
		j.AddField("Name",GamManager.getName);

		socket.Emit ("checkin", j);*/
	}
	private void OnSpawn(SocketIOEvent e)
	{
		if (full == false) {
			print ("Spawn Activated");

			JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
			j.AddField ("Score", GamManager.getScore);
			j.AddField ("Name", GamManager.getName);
			//j.AddField("Cs",en);
			if (en == false) {
				socket.Emit ("checkin", j);
				en = true;
			}
		}
		//en++;

	
	}
private void OnEnemy(SocketIOEvent e)
	{
		if (full == false) {
			en1 = Int32.Parse (e.data.GetField ("Cs").ToString ());
			if (en1 == 1) {
				player1.SetActive (true);
				if (onB == false) {
					B1.SetActive (true);
					count = true;
					onB = true;
					isP1 = true;
				}
				Name1.text = e.data.GetField ("Name").ToString ();
				HiScore1.text = e.data.GetField ("Score").ToString ();
				print (en1.ToString ());
			}
			//cPlayer++;

			if (en1 == 2) {
				player2.SetActive (true);
				player1.SetActive (true);
				if (onB == false) {
					B2.SetActive (true);
					onB = true;
					isP1 = false;
				}
				Name1.text = e.data.GetField ("Name1").ToString ();
				HiScore1.text = e.data.GetField ("Score1").ToString ();
				Name2.text = e.data.GetField ("Name").ToString ();
				HiScore2.text = e.data.GetField ("Score").ToString ();
				onmax = true;
				print (en1.ToString ());
				//pTimes.text = "Waiting";
				full = true;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (onmax == true) {
			if (isStart == true) {
				pTimes.text = "Waiting";
				if (count == true){
					socket.Emit ("StartTime");
			}
				isStart = false;

			}
		}
		if (ctimes == 0) {
			gameStart = false;
			if (P1sc > P2sc) {
				pTimes.text = Name1.text+" Win";
			}else
			if (P1sc < P2sc) {
					pTimes.text = Name2.text+" Win";
			}else
			if (P1sc == P2sc) {
					pTimes.text = "Draw";
			}
			B3.SetActive (true);
		}
	}
	public void Lclick()
	{
		if (gameStart == true) {
			socket.Emit ("AddP1sc");
		}
	}
	public void Rclick()
	{
		if (gameStart == true) {
			socket.Emit ("AddP2sc");
		}
	}

	void P1score(SocketIOEvent e) {
		string P1scg = e.data.GetField ("P1score").ToString();
		P1sc += Int32.Parse(P1scg);
		Score1.text = P1sc.ToString();

	}
	void P2score(SocketIOEvent e) {
		string P2scg = e.data.GetField ("P2score").ToString();
		P2sc += Int32.Parse(P2scg);
		Score2.text = P2sc.ToString();
	}
	void Standby()
	{
		socket.Emit ("StartTime");
		CancelInvoke ("Standby");
	}
	void ctime(SocketIOEvent e)
	{
		gameStart = true;
		if (ctimes >= 1) {
			string timess = e.data.GetField ("ct").ToString ();
			ctimes -= Int32.Parse (timess);
			pTimes.text = ctimes.ToString ();
			Invoke ("Standby", 1);
			//CancelInvoke ("Standby");
		} else {
			CancelInvoke ();
		}
	}

	public void SaveScore ()
	{
		if (isP1 == true) {
			if (GamManager.getScore <= P1sc) {
				string Url = "ec2-13-126-252-100.ap-south-1.compute.amazonaws.com:8081/userscore/"+GamManager.getName+"/"+P1sc;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream stream = response.GetResponseStream();
				string responseBody = new StreamReader(stream).ReadToEnd();
				GamManager.getScore = P1sc;
				SceneManager.LoadScene ("PlayerMain");
			}
		} else {
			if (GamManager.getScore <= P2sc) {
				string Url = "ec2-13-126-252-100.ap-south-1.compute.amazonaws.com:8081/userscore/"+GamManager.getName+"/"+P2sc;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream stream = response.GetResponseStream();
				string responseBody = new StreamReader(stream).ReadToEnd();
				GamManager.getScore = P2sc;
				SceneManager.LoadScene ("PlayerMain");
			}
		}
	}

}
