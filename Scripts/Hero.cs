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
		this.maxHealth = Global.heroMaxHealth[(int)type][level];
		this.damage.basicDamage = Global.heroDamage[(int)type][level][0];
		GD.Print(this.type, this.damage.basicDamage);
		this.damage.specialDamage = Global.heroDamage[(int)type][level][1];
		this.damage.ultimateDamage = Global.heroDamage[(int)type][level][2];
		this.chargeRate.basicCharge = Global.heroChargeRate[(int)type][level][0];
		this.chargeRate.specialCharge = Global.heroChargeRate[(int)type][level][1];
		this.chargeRate.ultimateCharge = Global.heroChargeRate[(int)type][level][2];

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
				this.sprite.Scale *= new Vector2(-1, 1);
				this.healthBar.Scale *= new Vector2(-1, 1);
				break;
			case false:
				this.sprite.Scale *= new Vector2(1, 1);
				this.healthBar.Scale *= new Vector2(1, 1);
				break;
		}
	}

	public void Init(Global.HeroTypes type, Vector2 locationInField)
	{
		this.type = type;
		this.level = Global.heroLevels[(int)type];
		this.maxCharge = Global.heroMaxCharge[(int)type];
		this.LevelUp();

		isMoving = true;
		locationInrow = locationInField; //new Vector2(200, 600);
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

	public override void die()
	{
		base.die();
		if (this == Global.currentlySelectedHero){
			Global.currentlySelectedHero = null;
		}
	}

	public void UploadData(Global.HeroSave load)
	{
		for (int i = this.level; i < load.level; i++)
		{
			LevelUp();
		}
		this.damage.basicDamage = load.damage;
		this.health = load.health;
		this.maxHealth = load.maxHealth;
		this.charge = load.charge;

		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);

		healthPercentage = (maxHealth / 100) * health;
	}

	public void IncreaseHP(int hp)
	{
		GD.Print(health);
		this.health += hp;
		GD.Print(health);
		healthPercentage = (maxHealth / 100) * health;
		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);
	}

	public void IncreaseDamage(int damage)
	{
		GD.Print("FFFF");
		this.damage.basicDamage += damage;
	}

	public void IncreaseMaxHP(int maxHP)
	{
		GD.Print("FFFF");
		this.maxHealth += maxHP;
		this.health = (maxHealth * healthPercentage) / 100;
		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);
	}

	public abstract void BasicAttack();
	public abstract void ChargeAttack();
	public abstract void UltimateAttack();

	public override void SaveHero(Godot.FileAccess file)
	{
		file.StoreString("" + health + "," + maxHealth + "," + (int)this.type + "," + this.level + "," + this.damage.basicDamage + "," + this.charge);
	}
}
