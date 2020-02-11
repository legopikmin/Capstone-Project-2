//-JAM, Bryan, Jordan
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
[SerializeField] private float health;		//current health
[SerializeField] private float maxHealth;	//maximum health
[SerializeField] private Slider healthBar;	//the GUI canvas health 'bar'
[SerializeField] private float energy;		//current energy
[SerializeField] private float maxEnergy;	//maximum energy
[SerializeField] private Slider energyBar;	//the GUI canvas energy 'bar'
[SerializeField] private float shield;		//current shield
[SerializeField] private float maxShield;	//maximum shield
[SerializeField] private Slider shieldBar;	//the GUI canvas energy 'shield'
[SerializeField] private TMP_Text healthText;	//GUI numerical value of health
[SerializeField] private TMP_Text energyText;	//GUI numerical value of energy

[SerializeField] private GameManager gameManager;	//the GameManager.cs

	void Start(){
		health = maxHealth;	//sets health to max upon spawn
		healthBar.maxValue = maxHealth;	//sets the max health on the GUI slider
		healthBar.value = health;	//sets the GUI value
		healthText.text = health.ToString();	//sets the GUI value
		energy = 0;	//sets energy to zero upon spawn
		energyBar.maxValue = maxEnergy;
		energyBar.value = energy;
		energyBar.text = energy.ToString();
	}
	/*	ApplyDamage() summary
	* public method to apply damage to the player, inputs a float 
	* subtracts from the current health by the amount given when the method is called 
	* sets the GUI canvas slider value to match the health stat
	* sets the GUI text mesh pro to match the health stat
	* checks if the player health is below, or equal to, zero
		* if true, call the Death() method
	* this method is called upon by other scripts
	*/
	public void ApplyDamage(float damage)
	{
		health -= damage;
		healthBar.value = health;
		healthText.text = health.ToString();
		if(health <= 0)
		{
			Death();
		}
	}
	
	/*	RestoreHealth() summary
	* public method to apply healing to the player health, inputs a float
	* adds to the current health by the amount given when the method is called
	* checks if the current health exceeds the maximum health
		* if true, it will reset the health back down to the maximum value
	* sets the GUI canvas slider value to match the health stat
	* sets the GUI text mesh pro to match the health stat
	* this method is called upon by other scripts
	*/
	public void RestoreHealth(float heals)
	{
		health += heals;
		if(health > maxHealth)
		{
			health = maxHealth;
		}
		healthBar.value = health;
		healthText.text = health.ToString();
	}
	
	/*	GainEnergy() summary
	* public method to restore used energy to the player, inputs a float
	* adds to the current energy by the amount given when the method is called
	* checks if the current energy exceeds the maximum energy
		* if true, it will reset the energy back to the maximum value
	* sets the GUI canvas slider value to match the energy stat
	* sets the GUI text mesh pro to match the energy stat
	* this method is called upon by other scripts
	*/
	public void GainEnergy(float power)
	{
		energy += power;
		if(energy > maxEnergy)
		{
			energy = maxEnergy;
		}
		energyBar.value = energy;
		energyBar.text = energy.ToString();
	}
	
	/*	ReduceEnergy() summary
	* public method to reduce energy when using an item or ability, inputs a float, returns a boolean
	* checks if energy is greater than, or equal to, the amount input when the method is called (power)
		* if true, subtract from the current energy, and updates the GUI, and returns true
		* if energy is less than the input power, return false
	*/
	public bool ReduceEnergy(float power)
	{
		if(energy >= power){
			energy -= power;
			energyBar.value = energy;
			energyBar.text = energy.ToString();
			return true;
		}
		else if(energy < power){
			return false;
		}
	}
	
	/*	Death() summary
	* a method for death (game over)
	* this method gets called upon by ApplyDamage() when the health drops below zero
	* not yet finished, we can easily apply more to this when we'ce designed death sequence
	*/
	void Death()
	{
		//currentState = PlayerState.Dead;		move to here?
        //TODO UI Changes
        //TODO Death affects
        GameManager.gameManager.RemovePlayer(gameObject);
        gameObject.SetActive(false);
	}
	
	public void BuffHealth(float buff){
		maxHealth += buff;
	}
}




