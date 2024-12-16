using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Unit : CharacterBody2D
{
	public int health;
	public int maxCharge;
	public int damage;
	public bool lockOn = false;

		public int Speed { get; set; } = 400;

		public Vector2 target = new Vector2(0,0);
		public bool isMoving = false;

		public override void _PhysicsProcess(double delta)
		{
			Velocity = GlobalPosition.DirectionTo(target) * Speed;
			if (GlobalPosition.DistanceTo(target) > 10 && isMoving == true){
				MoveAndSlide();
				sprite.Play("Walk");
			}
			else if(isMoving == true) {  isMoving = false;
			sprite.Play("Idle");
			}
	}

	public int range;
	public CollisionShape2D collision;
	public AnimatedSprite2D sprite;

	public Unit() { }
	public Unit(int health, int charge, int damage)
	{
		this.health = health;
		this.maxCharge = charge;
		this.damage = damage;
	}
	public Unit closestEnemy(List<Unit> list)
	{
		Unit min = list[0];
		foreach (Unit current in list) {
			if (this.GlobalPosition.DistanceTo(current.GlobalPosition) < this.GlobalPosition.DistanceTo(min.GlobalPosition)) {
				min = current;
			}
		}
		enemy = min;
		return min;
	}

	public void moveHere(Vector2 destonation)
	{
		isMoving = true;
		this.target = destonation;
	}
	public bool isAttacking = false;
	public Unit enemy = null;

	public Timer attackTimer;

	public void OnTimerTimeout()
	{
		if (isMoving == false){
			sprite.Play("Attack");
		}
	}

	public void OnAnimationFinished()
	{
		if(sprite.Animation == "Attack"){
			sprite.Play("Idle");
			attackTimer.Start();
		}
	}

	public void moveToAttack(Vector2 enemyGlobalPosition)
	{
		//float distance = (float)Math.Sqrt(Math.Pow((enemyGlobalPosition.X - this.GlobalPosition.X), 2) + Math.Pow((enemyGlobalPosition.Y - this.GlobalPosition.Y), 2));
		//this.target.X = this.GlobalPosition.X + range * ((enemyGlobalPosition.X - this.GlobalPosition.X) / distance);
		//this.target.Y = this.GlobalPosition.Y + range * ((enemyGlobalPosition.Y - this.GlobalPosition.Y) / distance);

		float distance = (float)Math.Sqrt(Math.Pow((this.GlobalPosition.X - enemyGlobalPosition.X), 2) + Math.Pow((this.GlobalPosition.Y - enemyGlobalPosition.Y), 2));
		//if(distance >= 20){
			this.target.X = enemyGlobalPosition.X + range * ((this.GlobalPosition.X - enemyGlobalPosition.X) / distance);
			this.target.Y = enemyGlobalPosition.Y + range * ((this.GlobalPosition.Y - enemyGlobalPosition.Y) / distance);
			isMoving = true;
		//}

		//this.target.X = this.Position.X + range * ((enemyGlobalPosition.X - this.Position.X) / (float)Math.Sqrt(Math.Pow((enemyGlobalPosition.X - this.Position.X), 2) - Math.Pow((enemyGlobalPosition.Y - this.Position.Y), 2)));
		//this.target.Y = this.Position.Y + range * ((enemyGlobalPosition.Y - this.Position.Y) / (float)Math.Sqrt(Math.Pow((enemyGlobalPosition.X - this.Position.X), 2) - Math.Pow((enemyGlobalPosition.Y - this.Position.Y), 2)));
	}
}
