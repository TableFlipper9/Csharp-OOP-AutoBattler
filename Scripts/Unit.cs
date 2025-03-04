using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public abstract partial class Unit : CharacterBody2D
{
	public int maxHealth;
	public int health;
	public float healthPercentage = 100;
	public int maxCharge;
	public int charge;
	public bool isDead = false;

	public int level = 0;

	public int speed { get; set; } = 300;

	public Vector2 target = new Vector2(0, 0);
	protected bool isMoving = false;

	public override void _PhysicsProcess(double delta)
	{
		Velocity = GlobalPosition.DirectionTo(target) * speed;
		if (GlobalPosition.DistanceTo(target) > 5 && isMoving == true) {

			MoveAndSlide();
			if (sprite.Animation != "Walk") {
				sprite.Play("Walk");
			}
		}
		else if (isMoving == true) {
			isMoving = false;
			sprite.Play("Idle");
		}
	}

	public abstract void FlipHorizontal(bool dir);

	public override void _Process(double delta)
	{
		if (isMoving == true && enemy != null && IsInstanceValid(enemy)) {
			MoveToAttack(enemy.GlobalPosition);
		}
		CheckEnemyAlive();
	}

	public CollisionShape2D collision;
	public AnimatedSprite2D sprite;
	public HealthBar healthBar;
	public override void _Ready()
	{
		this.collision = GetNode<CollisionShape2D>("CollisionShape2D");
		this.sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.attackTimer = GetNode<Timer>("Timer");
		this.healthBar = GetNode<HealthBar>("HealthBar");
	}

	public int range;

	public Unit() { }
	public Unit ClosestEnemy(List<Unit> list)
	{
		Unit min = null;
		foreach (Unit current in list) {
			if (current != null ) {
				if (min == null && current != this){
					min = current;
				}
				else if (this.GlobalPosition.DistanceTo(current.GlobalPosition) < this.GlobalPosition.DistanceTo(min.GlobalPosition) && current != this) {
					min = current;
				}
			}
		}
		enemy = min;
		return min;
	}

	public void PlayWalk()
	{
		if (isDead) return;
		FlipHorizontal(true);
		sprite.Play("Walk");
	}

	public void PlayIdle()
	{
		if(isDead) return;
		sprite.Play("Idle");
	}

	public void MoveHere(Vector2 destonation)
	{
		if (isDead) return;
		isMoving = true;
		this.target = destonation;
	}

	public bool isAttacking = false;
	public Unit enemy = null;
	public Timer attackTimer;

	public void OnAnimationFinished()
	{
		String animation = sprite.Animation;
		if (animation.Contains("Attack")) {
			Attack();
			sprite.Play("Idle");
		}
		if (animation == "Death") {
			this.QueueFree();
		}
	}

	public void MoveToAttack(Vector2 enemyGlobalPosition)
	{
		float distance = (float)Math.Sqrt(Math.Pow((this.GlobalPosition.X - enemyGlobalPosition.X), 2) + Math.Pow((this.GlobalPosition.Y - enemyGlobalPosition.Y), 2));
		if (distance > range)
		{
			this.target.X = enemyGlobalPosition.X + range * ((this.GlobalPosition.X - enemyGlobalPosition.X) / distance);
			this.target.Y = enemyGlobalPosition.Y + range * ((this.GlobalPosition.Y - enemyGlobalPosition.Y) / distance);
		}
		else
		{
			//this.target = enemyGlobalPosition;
		}
		isMoving = true;
		//FlipHorizontal(target.X < this.GlobalPosition.X);
	}

	public void TakeDamage(int damage)
	{
		if (isDead) return;
		health -= damage;

		var de = GD.Load<PackedScene>("res://Scenes/DamageEffect.tscn").Instantiate<DamageEffect>();
		de.Init(damage, this.GlobalPosition);
		AddChild(de);

		healthPercentage = (health / (float)maxHealth) * 100;
		if (health <= 0) {
			Die();
		}
		healthBar.ChangeValue(health, charge);
	}

	[Signal]
	public delegate void UnitDeathEventHandler(int indexInList);
	public int indexInList;

	public void SetIndex(int index)
	{
		this.indexInList = index;
	}

	[Signal]
	public delegate void SearchNewOpponentEventHandler(Unit self,int prio);

	public int prio = 0;
	public void CheckEnemyAlive()
	{
		if (isDead) return;
		if (enemy != null){
			if (enemy.health <= 0){
				EmitSignal(Unit.SignalName.SearchNewOpponent, this,prio);
			}
		}
	}

	public virtual void Die()
	{
		EmitSignal(Unit.SignalName.UnitDeath, indexInList);
		attackTimer.Stop();
		isAttacking = false;
		isMoving = false;
		healthBar.Visible = false;
		isDead = true;
		sprite.Play("Death");
	}

	public Vector2 locationInrow;
	public void updateInRowLocation(Vector2 location)
	{
		if (isDead) return;
		this.locationInrow = location;
		FlipHorizontal(location.X < this.GlobalPosition.X);
		MoveHere(locationInrow);
	}
	public abstract void Attack();
	public virtual void SaveHero(Godot.FileAccess file) { }
}
