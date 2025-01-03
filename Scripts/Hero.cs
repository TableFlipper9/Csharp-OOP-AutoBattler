using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

public abstract partial class Hero : Unit
{
	public Global.HeroTypes type;

	public struct chargeRates
	{
		public int basicCharge;
		public int specialCharge;
		public int ultimateCharge;
	}
	public chargeRates chargeRate;
	public struct damages
	{
		public int basicDamage;
		public int specialDamage;
		public int ultimateDamage;
	}
	public damages damage;
	public enum attackTypes
	{
		basic,
		special,
		ultimate
	}
	public attackTypes attackType;

	public Hero() : base() { }

	public void LevelUp()
	{
		this.level = Global.heroLevels[(int)type];
		this.maxHealth = Global.heroMaxHealth[Global.heroStringMap[type]][level];
		this.damage.basicDamage = Global.heroDamage[Global.heroStringMap[type]][level][0];
		this.damage.specialDamage = Global.heroDamage[Global.heroStringMap[type]][level][1];
		this.damage.ultimateDamage = Global.heroDamage[Global.heroStringMap[type]][level][2];
		this.chargeRate.basicCharge = Global.heroChargeRate[Global.heroStringMap[type]][level][0];
		this.chargeRate.specialCharge = Global.heroChargeRate[Global.heroStringMap[type]][level][1];
		this.chargeRate.ultimateCharge = Global.heroChargeRate[Global.heroStringMap[type]][level][2];

		this.health = (maxHealth * healthPercentage) / 100;
		if (healthBar != null){
			healthBar.ChangeMax(maxHealth, maxCharge);
			healthBar.ChangeValue(health, charge);
		}
	}

	public override void FlipHorizontal(bool dir)
	{
		switch (dir)
		{
			case true:
				this.Scale = new Vector2(-1, this.Scale.Y);
				break;
			case false:
				this.Scale = new Vector2(1, this.Scale.Y);
				break;
		}
	}

	public void Init(Global.HeroTypes type, Vector2 locationInField)
	{
		this.level = Global.heroLevels[Global.heroStringMap[type]];
		this.maxCharge = Global.heroMaxCharge[Global.heroStringMap[type]];
		this.LevelUp();

		isMoving = true;
		locationInrow = locationInField; //new Vector2(200, 600);
		this.type = type;
	}

	public override void _Ready()
	{
		base._Ready();
		/// add rest of get node
		/// //this.sprite.Scale.X = 4;

		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);

		attackTimer.Start();
		moveHere(locationInrow);
	}

	public override void Attack()
	{
		//GD.Print(enemy);
		if (enemy != null ){
			switch (attackType){
				case (attackTypes.basic):
					BasicAttack();
					break;
				case (attackTypes.special):
					ChargeAttack();
					break;
				case (attackTypes.ultimate):
					UltimateAttack();
					break;
				default:
					break;
			}
			if (charge >= maxCharge)
			{
				attackType = attackTypes.special;
			}
			if(charge < maxCharge)
			{
				attackType = attackTypes.basic;
			}
		}
		healthBar.ChangeValue(health, charge);
	}
	public void OnTimerTimeout()
	{
		if (isMoving == false && enemy != null)
		{
			switch (attackType)
			{
				case attackTypes.basic:
					sprite.Play("BasicAttack");
					break;
				case attackTypes.special:
					sprite.Play("SpecialAttack");
					break;
				case attackTypes.ultimate:
					sprite.Play("UltimateAttack");
					break;
				default: break;
			}
		}
	}

	public abstract void BasicAttack();
	public abstract void ChargeAttack();
	public abstract void UltimateAttack();
}
