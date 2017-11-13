//using UnityEngine;
//using System.Collections;
//using SocketIO;
//
//public class Controls : MonoBehaviour 
//{
//	public static Controls instanse;
//	public SocketIOComponent sIO;
//
//	public class toServer
//	{
//		public int id;
//		public float X;
//		public float Y;
//		public float R;
//	}
//
//	[Header("Player 1 Controls")]
//	public KeyCode p1MoveForward;
//	public KeyCode p1MoveBackwards;
//	public KeyCode p1TurnLeft;
//	public KeyCode p1TurnRight;
//	public KeyCode p1Shoot;
//
//	[Header("Player 2 Controls")]
//	public KeyCode p2MoveForward;
//	public KeyCode p2MoveBackwards;
//	public KeyCode p2TurnLeft;
//	public KeyCode p2TurnRight;
//	public KeyCode p2Shoot;
//
//	[Header("Components")]
//	public Game game;
//
//	void Awake ()
//	{
//		instanse = this;
//	}
//
//	void Start ()
//	{
//		sIO.On("MOVE", onUserMove);
//		sIO.On("SHOOT", onUserShoot);
//	}
//
//	void Update ()
//	{
//		//Quit Game
//		if(Input.GetKeyDown(KeyCode.Escape)){
//			game.ui.GoToMenu();
//		}
////		if (game.player1Tank.transform.hasChanged)
////		{
////			sendData (1);
////			game.player1Tank.transform.hasChanged = false;
////		}
////
////		if (game.player2Tank.transform.hasChanged)
////		{
////			sendData (2);
////			game.player2Tank.transform.hasChanged = false;
////		}
////
//
//		//Player 1
//		if(game.player1Tank.vZ == true){
//			game.player1Tank.rig.velocity = Vector2.zero;
//		}
//		Debug.Log (game.player1Tank.rig.velocity);
//			
//		if(game.player1Tank.canMove){
//			if(Input.GetKey(p1MoveForward)){
//				game.player1Tank.Move(1);
//			}
//			if(Input.GetKey(p1MoveBackwards)){
//				game.player1Tank.Move(-1);
//			}
//			if(Input.GetKey(p1TurnLeft)){
//				game.player1Tank.Turn(-1);
//
//			}
//			if(Input.GetKey(p1TurnRight)){
//				game.player1Tank.Turn(1);
//			}
//		}
//			
//		if(game.player1Tank.canShoot){
//			if(Input.GetKeyDown(p1Shoot)){
//				game.player1Tank.Shoot();
////				sendData (1);
////				sIO.Emit ("USER_SHOOT", new JSONObject("{id: 0}") );
//			}
//		}
//
//		//Player 2
//		game.player2Tank.rig.velocity = Vector2.zero;
//
//		if(game.player2Tank.canMove){
//			if(Input.GetKey(p2MoveForward)){
//				game.player2Tank.Move(1);
//			}
//			if(Input.GetKey(p2MoveBackwards)){
//				game.player2Tank.Move(-1);
//			}
//			if(Input.GetKey(p2TurnLeft)){
//				game.player2Tank.Turn(-1);
//			}
//			if(Input.GetKey(p2TurnRight)){
//				game.player2Tank.Turn(1);
//			}
//		}
//		if(game.player2Tank.canShoot){
//			if(Input.GetKeyDown(p2Shoot)){
//				game.player2Tank.Shoot();
////				sIO.Emit ("USER_SHOOT", new JSONObject("{id: 1}") );
//			}
//		}
//	}
//
//	public void sendData (int id)
//	{
//		var tank = game.player1Tank;
//		if (id == 1) {
//			tank = game.player2Tank;
//		}
//		toServer dataToServer = new toServer();
//		dataToServer.id = tank.id;
//		dataToServer.X = tank.transform.position.x;
//		dataToServer.Y = tank.transform.position.y;
//		dataToServer.R = tank.transform.rotation.eulerAngles.z;
//		string json = JsonUtility.ToJson (dataToServer);
//		sIO.Emit ("USER_MOVE", new JSONObject(json) );
//	}
//
//	void onUserMove (SocketIOEvent obj)
//	{
//		var data = JsonUtility.FromJson<toServer>(obj.data.ToString());
//		game.player2Tank.transform.hasChanged = false;
//
//		if (data.id == 0)
//		{
//			var tank = game.player1Tank.transform;
//			tank.position = new Vector3 (data.X, data.Y, 0);
//			tank.rotation = Quaternion.Euler(0,0,data.R); 
//		}
//
//		if (data.id == 1)
//		{
//			var tank = game.player2Tank.transform;
//			tank.position = new Vector3 (data.X, data.Y, 0);
//			tank.rotation = Quaternion.Euler(0,0,data.R);
//		}
//
//	}
//
//	void onUserShoot (SocketIOEvent obj)
//	{
//		
//		var id = (obj.data [0]).ToString ();
//		Debug.Log (id);
//
//		if (id == "0") {
//			game.player1Tank.Shoot();
//		}
//		if (id == "1") {
//			game.player2Tank.Shoot();
//		}
//	}
//}
