using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : Unit
{
	public Global.EnemyTypes type;
	public struct chargeRates
	{
		public int basicCharge;
		public int specialCharge;
	}
	public chargeRates chargeRate;
	public struct damages
	{
		public int basicDamage;
		public int specialDamage;
	}
	public damages damage;
	public enum attackTypes
	{
		basic,
		special,
	}
	public attackTypes attackType;
	public void LevelUp(int level)
	{
		this.maxHealth = Global.enemyMaxHealth[Global.enemyStringMap[type]][level];
		this.damage.basicDamage = Global.enemyDamage[Global.enemyStringMap[type]][level][0];
		this.damage.specialDamage = Global.enemyDamage[Global.enemyStringMap[type]][level][1];
		this.chargeRate.basicCharge = Global.enemyChargeRate[Global.enemyStringMap[type]][level][0];
		this.chargeRate.specialCharge = Global.enemyChargeRate[Global.enemyStringMap[type]][level][1];

		this.health = (maxHealth * healthPercentage) / 100;
		if (healthBar != null)
		{
			healthBar.ChangeMax(maxHealth, maxCharge);
			healthBar.ChangeValue(health, charge);
		}
	}

	public Enemy() : base() { }
	public void Init(int level,Global.EnemyTypes type, Vector2 locationInfield)
	{
		this.level = level;
		this.maxCharge = Global.enemyMaxCharge[Global.enemyStringMap[type]];
		this.LevelUp(level);

		isMoving = true;
		locationInrow = locationInfield;
		this.type = type;
	}

	public override void _Ready()
	{
		base._Ready();

		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);

		switch (type){
			case Global.EnemyTypes.GOBLIN:
				//sprite.SpriteFrames.ResourcePath = "res://Animations/Orc.tres";
				sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Orc.tres");
				this.range = 60;
				break;
			default:
				break;
		}
		attackTimer.Start();
		moveHere(locationInrow);
	}
}
