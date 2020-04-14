//-JAM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("(Joey)/Input Test")]
/*	Quick Summery
simple script to just call & test all of the inputs, setup inside Unity project settings
I have a few floats to help track/reprisent axis, and comments to clearly tell which button is being tested
programmers can use this as a base refrence on how to track axis inputs, and translate it into working like a button
*/
public class InputTest0 : MonoBehaviour{
	//floats used for joystick axis testing
private float speed;
private float stopSpeed;
private float xCycle;
private float yCycle;
private float steering;
private float vert;
private float camH;
private float camV;

private enum PlayerID{
	P0,
	P1,
	P2,
	P3,
	P4
}
[SerializeField] private PlayerID player;

	void Update(){
			//tests the acceleration button, "right trigger"
			//uses an axis, but only ever goes one direction
		speed = Input.GetAxis("RightTrigger"+player.ToString());
		if(speed != 0){
			Debug.Log("Accelerate! (RT) "+player.ToString());
		}
		
		
			//tests the reverse button, "left trigger"
			//uses an axis, but only ever goes one direction
		stopSpeed = Input.GetAxis("LeftTrigger"+player.ToString());
		if(stopSpeed != 0){
			Debug.Log("Backing up! (LT) "+player.ToString());
		}
		
		
			//tests the hand-break button, "left bumper"
		if(Input.GetButtonDown("LeftBumper"+player.ToString())){
			Debug.Log("Brake! (LB) "+player.ToString());
		}
		
		
			//tests the fire button, "right bumper"
		if(Input.GetButtonDown("RightBumper"+player.ToString())){
			Debug.Log("Fire! (RB) "+player.ToString());
		}
		
		
			//tests the play-a-card button, "A button"
		if(Input.GetButtonDown("ButtonA"+player.ToString())){
			Debug.Log("Play Card! (A) "+player.ToString());
		}
		
		
			//tests the card-slot-1 button, "X button"
		if(Input.GetButtonDown("ButtonX"+player.ToString())){
			Debug.Log("Play Card 1! (X) "+player.ToString());
		}
		
		
			//tests the card-slot-2 button, "Y button"
		if(Input.GetButtonDown("ButtonY"+player.ToString())){
			Debug.Log("Play Card 2! (Y) "+player.ToString());
		}
		
		
			//tests the card-slot-3 button, "B button"
		if(Input.GetButtonDown("ButtonB"+player.ToString())){
			Debug.Log("Play Card 3! (B) "+player.ToString());
		}
		
		
			//tests the card-cycle button, "left/right direction (D) button"
			//uses an axis, right is positive one, left is negative one, 
		xCycle = Input.GetAxis("DPadHorizontal"+player.ToString());
		if(xCycle > 0){
			Debug.Log("Cycle Right! (DR +1) "+player.ToString());
		}
		else if(xCycle < 0){
			Debug.Log("Cycle Left! (DL -1) "+player.ToString());
		}
		
		
			//tests the view-discard button, "up/down direction (D) button"
			//uses an axis, up is positive one, down is negative one, 
		yCycle = Input.GetAxis("DPadVertical"+player.ToString());
		if(yCycle > 0){
			Debug.Log("View/Hide Cards! (DU +1) "+player.ToString());
		}
		else if(yCycle < 0){
			Debug.Log("Discard Cards! (DD -1) "+player.ToString());
		}
		
		
			//tests the steering joystick, "left/right joystick, on the left hand side (LS)"
			//uses an axis, right is positive one, left is negative one
			//also tests the "Vertical" input, which uses both default keybinds as well as the left joystick
			//joystick uses an axis, up is positive one, down is negative one
		steering = Input.GetAxis("LeftAnalogHorizontal"+player.ToString());
		vert = Input.GetAxis("Vertical");
		if(steering > 0){
			Debug.Log("Steer Right! (LSR +1) "+player.ToString());
		}
		else if(steering < 0){
			Debug.Log("Steer Left! (LSL -1) "+player.ToString());
		}
		if(vert > 0){
			Debug.Log("Joystick up! (LSU +1) "+player.ToString());
		}
		else if(vert < 0){
			Debug.Log("Joystick down! (LSD -1) "+player.ToString());
		}
		
		
			//tests the camera-swivel joystick, "left/right/up/down joystick, on the right hand side (RS)"
			//uses an axis, right is positive one, left is negative one, up is negative one, down is positive one
		camH = Input.GetAxis("RightAnalogHorizontal"+player.ToString());
		if(camH > 0){
			Debug.Log("Camera Look Right! (RSR +1) "+player.ToString());
		}
		else if(camH < 0){
			Debug.Log("Camera Look Left! (RSL -1) "+player.ToString());
		}
		camV = Input.GetAxis("RightAnalogVertical"+player.ToString());
		if(camV < 0){
			Debug.Log("Camera Look Up! (RSU -1) "+player.ToString());
		}
		else if(camV > 0){
			Debug.Log("Camera Look Down! (RSD +1) "+player.ToString());
		}
		
		
			//tests the air-steer button, "LS click"
		if(Input.GetButtonDown("LeftAnalogClick"+player.ToString())){
			Debug.Log("Air Steer?... (LS click) "+player.ToString());
		}
		
		
			//tests the rear-view button, "RS click"
		if(Input.GetButtonDown("RightAnalogClick"+player.ToString())){
			Debug.Log("Rear View! (RS click) "+player.ToString());
		}
		
			//tests the pause button, "Start"
		if(Input.GetButtonDown("Start"+player.ToString())){
			Debug.Log("Game Paused! (Start) "+player.ToString());
		}
	}
}
