﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public GameObject playerLaserPrefab;
	private LevelManager levelManager;
	public float moveStep;
	public int controlMode;

	public float health = 300f;

	public float laserSpeed;
	public float firingRate = 0.2f;

	private bool hasStarted = false;

	// restrict the player in the game screen
	private float xMax, xMin, yMax, yMin;
	private float padding = 1f;

	// Use this for initialization
	void Start () {
		moveStep *= Time.deltaTime;
		transform.position = new Vector3(0f, -4f, 0f);
		// camera x,y /in [0,1]
		float distance = transform.position.z - Camera.main.transform.position.z;
		// use the limit of camera to restrain the player's movement
		Vector3 lowCorner = Camera.main.ViewportToWorldPoint(new Vector3(0f,0f,distance));
		Vector3 highCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,distance));

		xMin = lowCorner.x + padding; xMax = highCorner.x - padding;
		yMin = lowCorner.y + padding; yMax = highCorner.y - padding;
	}
	
	// Update is called once per frame
	void Update () {
		if(controlMode == 0){
			MoveObjectUsingKB();
		}
		else if(controlMode == 1){
			MoveObjectUsingM();
		}
		PlayerLauchLaser();
	}

	void PlayerLauchLaser(){
		// in "*" is the name of the function requires to be repeated
		if(Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.000001f, firingRate);
		}
		if(Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
	}

	void Fire(){
		Vector3 playerLaserPos = transform.position + Vector3.up*0.5f;
		GameObject playerLaser = Instantiate(playerLaserPrefab, playerLaserPos, Quaternion.identity) as GameObject;
		playerLaser.GetComponents<Rigidbody2D>()[0].velocity = new Vector2(0f, laserSpeed);
	}

	void MoveObjectUsingKB(){
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
			transform.position += Vector3.up * moveStep;
			float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
			transform.position 
				= new Vector3(transform.position.x, newY, transform.position.z);
		}
		else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
			transform.position += Vector3.down * moveStep;
			float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
			transform.position 
				= new Vector3(transform.position.x, newY, transform.position.z);
		}
		else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * moveStep;
			float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
			transform.position 
				= new Vector3(newX, transform.position.y, transform.position.z);
		}
		else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
			transform.position += Vector3.right * moveStep;
			float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
			transform.position 
				= new Vector3(newX, transform.position.y, transform.position.z);
		}
	}

	void MoveObjectUsingM(){
		print(Input.mousePosition);
		// original = Screen.width * 16 - 8
		//          = 
		Vector3 objPos 
			= new Vector3(Mathf.Clamp(Input.mousePosition.x / Screen.width * 16 - 8f, -6.1f, 6.1f),
					  	  Mathf.Clamp(Input.mousePosition.y / Screen.height * 12 - 6f, -4.5f, 4.5f),
					  	  0f);
		gameObject.transform.position = objPos;
	}

}
