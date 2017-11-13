using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SocketIO;



public class NetworkManager : MonoBehaviour {

	public GameObject TANK;
	public Transform GameField;

	public SocketIOComponent socket;
	public static NetworkManager instance;
	public class Player
	{
		public int Id;
		public string Name;
		public Vector3 position;
		public Vector3 rotation;
		public Color color;
	}

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		socket.On ("localPlayerConnect", localPlayer);
		socket.On ("otherPlayerConnect", otherPlayer);
		socket.On("player move", OnPlayerMove);
		socket.On("player turn", OnPlayerTurn);
	}
	
	// Update is called once per frame
	void Update () {
		 
	}

	public void Join (string name) {
		StartCoroutine(ConnectToServer(name));
	}
		

	IEnumerator ConnectToServer(string name)
	{
		yield return new WaitForSeconds (0.5f);

		socket.Emit ("player connect");

		yield return new WaitForSeconds (1f);

		var P = new Player ();
		P.Name = name;
		P.position = Game.gameInstanse.spawnPoints [UnityEngine.Random.Range (0, Game.gameInstanse.spawnPoints.Count)].transform.position;
		P.rotation = new Vector3 (0,0);
		var data = JsonUtility.ToJson(P);
		socket.Emit ("JoinNew", new JSONObject(data));
	}

	void localPlayer(SocketIOEvent socketIOEvent)
	{
		var player = JsonUtility.FromJson<Player>(socketIOEvent.data.ToString());

		GameObject playerTank = Instantiate(TANK, player.position, Quaternion.identity) as GameObject;
		playerTank.name = player.Name;

		var tanchik = playerTank.transform.Find ("Player").gameObject.GetComponent<Tank>();
		tanchik.id = player.Id;

		var sprite = playerTank.transform.Find ("Player").gameObject.GetComponent<SpriteRenderer>();
		sprite.color = player.color;

		playerTank.transform.Find ("Panel").gameObject.transform.Find ("Name").GetComponent<Text>().text = player.Name;

		playerTank.transform.parent = GameField;
	}

	void otherPlayer(SocketIOEvent socketIOEvent)
	{
		var player = JsonUtility.FromJson<Player>(socketIOEvent.data.ToString());

		GameObject playerTank = Instantiate(TANK, player.position, Quaternion.identity) as GameObject;
		playerTank.name = player.Name;

		var tanchik = playerTank.transform.Find ("Player").gameObject.GetComponent<Tank>();
		tanchik.id = player.Id;
		tanchik.isLocal = false;

		playerTank.transform.Find ("Panel").gameObject.transform.Find ("Name").GetComponent<Text>().text = player.Name;

		var sprite = playerTank.transform.Find ("Player").gameObject.GetComponent<SpriteRenderer>();
		sprite.color = player.color;

		playerTank.transform.parent = GameField;
	}

	public void CommandMove(Vector3 vec3)
	{
		string data = JsonUtility.ToJson(new PositionJSON(vec3));
		socket.Emit("player move", new JSONObject(data));
	}

	public void CommandTurn(Quaternion quat)
	{
		string data = JsonUtility.ToJson(new RotationJSON(quat));
		socket.Emit("player turn", new JSONObject(data));
	}


	void OnPlayerMove(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
		// if it is the current player exit
		if (userJSON.name == PlayerPrefs.GetString("Name"))
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name+"/Player") as GameObject;
		if (p != null)
		{
			p.transform.position = position;
		}
	}
	void OnPlayerTurn(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
		// if it is the current player exit
		if (userJSON.name == PlayerPrefs.GetString("Name"))
		{
			return;
		}
		GameObject p = GameObject.Find(userJSON.name+"/Player") as GameObject;
		if (p != null)
		{
			p.transform.rotation = rotation;
		}
	}


	[Serializable]
	public class PositionJSON
	{
		public float[] position;

		public PositionJSON(Vector3 _position)
		{
			position = new float[] { _position.x, _position.y, _position.z };
		}
	}

	[Serializable]
	public class RotationJSON
	{
		public float[] rotation;

		public RotationJSON(Quaternion _rotation)
		{
			rotation = new float[] { _rotation.eulerAngles.x,
				_rotation.eulerAngles.y, 
				_rotation.eulerAngles.z };
		}
	}

	[Serializable]
	public class UserJSON
	{
		public string name;
		public float[] position;
		public float[] rotation;

		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON>(data);
		}
	}

}

