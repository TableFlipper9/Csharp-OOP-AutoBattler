using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Enemy : Unit
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
		this.maxHealth = Global.enemyMaxHealth[(int)type][level];
		this.damage.basicDamage = Global.enemyDamage[(int)type][level][0];
		this.damage.specialDamage = Global.enemyDamage[(int)type][level][1];
		this.chargeRate.basicCharge = Global.enemyChargeRate[(int)type][level][0];
		this.chargeRate.specialCharge = Global.enemyChargeRate[(int)type][level][1];

		this.health = (int)((maxHealth * healthPercentage) / 100);
		if (healthBar != null)
		{
			healthBar.ChangeMax(maxHealth, maxCharge);
			healthBar.ChangeValue(health, charge);
		}
	}

	public override void FlipHorizontal(bool dir)
	{
		switch (dir)
		{
			case false:
				this.sprite.Scale *= new Vector2(-1, 1);
				this.healthBar.Scale *= new Vector2(-1, 1);
				break;
			case true:
				this.sprite.Scale *= new Vector2(1, 1);
				this.healthBar.Scale *= new Vector2(1, 1);
				break;
		}
	}

	//public Enemy() : base() { }
	public void Init(int level,Global.EnemyTypes type, Vector2 locationInfield)
	{
		this.type = type;
		this.level = level;
		this.maxCharge = Global.enemyMaxCharge[(int)type];
		this.LevelUp(level);

		isMoving = true;
		locationInrow = locationInfield;
	}

	public override void _Ready()
	{
		base._Ready();
		/// add rest of get node
		/// //this.sprite.Scale.X = 4;

		healthBar.ChangeMax(maxHealth, maxCharge);
		healthBar.ChangeValue(health, charge);

		attackTimer.Start();
		MoveHere(locationInrow);
	}

	public override void Attack()
	{
		//GD.Print(enemy);
		if (enemy != null)
		{
			switch (attackType)
			{
				case (attackTypes.basic):
					BasicAttack();
					break;
				case (attackTypes.special):
					ChargeAttack();
					break;
				default:
					break;
			}
			if (charge >= maxCharge)
			{
				attackType = attackTypes.special;
			}
			if (charge < maxCharge)
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
				default: break;
			}
		}
	}

	public abstract void BasicAttack();
	public abstract void ChargeAttack();
}
