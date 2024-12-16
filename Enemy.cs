using Godot;
using System;

public partial class Enemy : Unit
{
	public enum TYPE
	{
		GOBLIN,
		SKELETON,
		RIDER
	}

	public TYPE type;
	public Vector2 locationInrow;
	public Enemy() : base(0, 0, 0) { }
	public void Init(TYPE type)
	{
		isMoving = true;
		locationInrow = new Vector2(800, 400);
		this.type = type;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.collision = GetNode<CollisionShape2D>("CollisionShape2D");
		this.sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.attackTimer = GetNode<Timer>("Timer");
		/// add rest of get node
		/// //this.sprite.Scale.X = 4;
		switch (type)
		{
			case TYPE.GOBLIN:
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isMoving == true && enemy != null){
			moveToAttack(enemy.GlobalPosition);
		}
	}
}
