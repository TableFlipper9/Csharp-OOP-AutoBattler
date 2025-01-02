using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Unit : CharacterBody2D
{
	public int maxHealth;
	public int health;
	public int healthPercentage = 100;
	public int maxCharge;
	public int charge;
	public bool lockOn = false;

	public int level = 0;

	public int Speed { get; set; } = 300;

	public Vector2 target = new Vector2(0, 0);
	protected bool isMoving = false;

	public override void _PhysicsProcess(double delta)
	{
		Velocity = GlobalPosition.DirectionTo(target) * Speed;
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

	public override void _Process(double delta)
	{
		if (isMoving == true && enemy != null) {
			moveToAttack(enemy.GlobalPosition);
		}
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
	public Unit closestEnemy(List<Unit> list)
	{
		Unit min = null;
		foreach (Unit current in list) {
			if (current != null && current != this.enemy) {
				if (min == null){
					min = current;
				}
				if (this.GlobalPosition.DistanceTo(current.GlobalPosition) < this.GlobalPosition.DistanceTo(min.GlobalPosition)) {
					min = current;
				}
			}
		}
		enemy = min;
		return min;
	}

	public void playWalk()
	{
		sprite.Play("Walk");
	}

	public void playIdle()
	{
		sprite.Play("Idle");
	}

	public void moveHere(Vector2 destonation)
	{
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
			attackTimer.Start();
		}
		if (animation == "Death") {
			EmitSignal(Unit.SignalName.UnitDeath, indexInList);
			this.QueueFree();
		}
	}

	public void moveToAttack(Vector2 enemyGlobalPosition)
	{
		float distance = (float)Math.Sqrt(Math.Pow((this.GlobalPosition.X - enemyGlobalPosition.X), 2) + Math.Pow((this.GlobalPosition.Y - enemyGlobalPosition.Y), 2));
		//if(distance >= 20){
		this.target.X = enemyGlobalPosition.X + range * ((this.GlobalPosition.X - enemyGlobalPosition.X) / distance);
		this.target.Y = enemyGlobalPosition.Y + range * ((this.GlobalPosition.Y - enemyGlobalPosition.Y) / distance);
		isMoving = true;
	}

	public void takeDamage(int damage)
	{
		health -= damage;

		var de = GD.Load<PackedScene>("res://DamageEffect.tscn").Instantiate<DamageEffect>();
		de.Init(damage, this.GlobalPosition);
		AddChild(de);

		healthPercentage = (maxHealth / 100) * health;
		if (health <= 0) {
			die();
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
	public delegate void KilledOpponentEventHandler(int index);
	public void CheckEnemyAlive()
	{
		if(enemy.health <= 0){
			EmitSignal(Unit.SignalName.KilledOpponent, indexInList);
			//enemy = null;
		}
	}

	public void die()
	{
		attackTimer.Stop();
		isAttacking = false;
		isMoving = false;
		healthBar.Visible = false;
		sprite.Play("Death");
	}

	public Vector2 locationInrow;
	public void updateInRowLocation(Vector2 location)
	{
		this.locationInrow = location;
		moveHere(locationInrow);
	}
	public virtual void Attack()
	{
	}
}
