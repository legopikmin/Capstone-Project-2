//-JAM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("(Joey)/Input Test, Movement")]
/*	quick summery
simple movement test to display that multiple people can control only their respective player
works with up to 4 players, player zero (P0) is a phantom value, used to make player one (P1) actually equal 1 in the enum
*/
public class InputTest1 : MonoBehaviour{
[SerializeField] private float speed;
private enum PlayerID{
	P0,
	P1,
	P2,
	P3,
	P4
}
[SerializeField] private PlayerID player;

	void FixedUpdate(){
		float moveH = Input.GetAxis("LeftAnalogHorizontal"+player.ToString());
		float moveV = Input.GetAxis("LeftAnalogVertical"+player.ToString());
		
		transform.Translate(moveH * speed * Time.fixedDeltaTime, 0, -moveV * speed * Time.fixedDeltaTime);
	}
}
