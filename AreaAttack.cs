using Godot;
using System;
using System.Collections.Generic;

public partial class AreaAttack : Area2D
{
	public int damage;
	private CollisionShape2D collisionShape;
	private AnimatedSprite2D animatedSprite;
	private List<Unit> unitList = new List<Unit>();
	private string ownerType;

	private Shape2D shape;
	public void OnBodyEntered(Node2D body)
	{
		if (body is Unit unit){
			unitList.Add(unit);
		}
	}
	public void OnBodyExited(Node2D body)
	{
		if (body is Unit unit){
			unitList.Remove(unit);
		}
	}
	public void OnAnimationFinished()
	{
		foreach (var unit in unitList){
			switch (ownerType){
				case "Archer" or "Executioner":
					if(unit is Hero hero){
						hero.takeDamage(damage);
					}
					break;
				case "GOBLIN" or "Healer":
					if (unit is Enemy enemy){
						enemy.takeDamage(damage);
					}
					break;
				default:
					break;
			}
		}
		QueueFree();
	}

	public void Init(string type, Shape2D shape, int damage, Vector2 position, Vector2 scale)
	{
		ownerType = type;
		this.shape = shape;
		this.damage = damage;
		this.GlobalPosition = position;
		this.Scale = scale;
	}
	public override void _Ready()
	{
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collisionShape.Shape = this.shape;
		animatedSprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Effects/"+ ownerType +".tres");
		animatedSprite.Play("Effect");
	}

	public override void _Process(double delta)
	{
	}
}
