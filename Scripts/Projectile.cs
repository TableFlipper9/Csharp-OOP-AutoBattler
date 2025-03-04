using Godot;
using System;
using System.Collections.Generic;

public partial class Projectile : Area2D
{
	public int damage;
	private CollisionShape2D collisionShape;
	private Sprite2D sprite;
	private string ownerType;

	private Unit enemy;

	private Shape2D shape;
	private Vector2 destination;
	private Vector2 scale;

	private int speed = 300;

	public override void _PhysicsProcess(double delta)
	{
		if (destination != null){
			Vector2 direction = GlobalPosition.DirectionTo(destination);
			GlobalPosition += direction * speed * (float)delta;
			LookAt(destination);

			if (GlobalPosition.DistanceTo(destination) < 10){
				enemy.TakeDamage(damage);
				QueueFree();
			}
		}
	}
	public void OnBodyEntered(Node2D body)
	{
		switch (ownerType){
			case "Archer" or "Executioner" or "Knight" or "Solder" or "Chaverly":
				if (body is Hero hero){
					hero.TakeDamage(damage);
				}
				break;
			case "Goblin" or "Healer" or "":
				if (body is Enemy enemy){
					enemy.TakeDamage(damage);
				}
				break;
			default:
				break;
		}
	}
	public void Init(string ownerType, Shape2D shape, int damage, Vector2 position, Unit enemy, Vector2 animationScale, Vector2 hitBoxScale)
	{
		this.ownerType = ownerType;
		this.enemy = enemy;
		this.shape = shape;
		this.damage = damage;
		this.destination = enemy.GlobalPosition;
		this.GlobalPosition = position;
		this.scale = hitBoxScale;
		this.Scale = animationScale;
	}
	public override void _Ready()
	{
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		collisionShape.Shape = this.shape;
		collisionShape.Scale = this.scale;
		sprite.Texture = GD.Load<Texture2D>("res://Animations/Ability/" + ownerType + ".png");
	}

}
