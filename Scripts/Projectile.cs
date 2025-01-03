using Godot;
using System;
using System.Collections.Generic;

public partial class Projectile : Area2D
{
	public int damage;
	private CollisionShape2D collisionShape;
	private Sprite2D sprite;
	private string ownerType;

	private Shape2D shape;
	private Vector2 destination;

	private int speed = 1000;

	public override void _PhysicsProcess(double delta)
	{
		if (destination != null){
			Vector2 direction = GlobalPosition.DirectionTo(destination);
			GlobalPosition += direction * speed * (float)delta;
			LookAt(destination);

			if (GlobalPosition.DistanceTo(destination) < 10){
				QueueFree();
			}
		}
	}
	public void OnBodyEntered(Node2D body)
	{
		switch (ownerType){
			case "Archer" or "Executioner":
				if (body is Hero hero){
					hero.takeDamage(damage);
				}
				break;
			case "GOBLIN" or "Healer":
				if (body is Enemy enemy){
					enemy.takeDamage(damage);
				}
				break;
			default:
				break;
		}
	}
	public void Init(Shape2D shape, int damage, Vector2 position, Vector2 destination, Vector2 scale)
	{
		this.shape = shape;
		this.damage = damage;
		this.destination = destination;
		this.GlobalPosition = position;
		this.Scale = scale;
	}
	public override void _Ready()
	{
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		collisionShape.Shape = this.shape;
		sprite.Texture = GD.Load<Texture2D>("res://Sprites/" + ownerType + ".tres");
	}

	public override void _Process(double delta)
	{
	}
}
