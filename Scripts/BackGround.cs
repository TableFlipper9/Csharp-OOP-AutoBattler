using Godot;
using System;

public partial class BackGround : ParallaxBackground
{
	bool isMoving = false;
	int speed = 1;

	int destination = -440;
	public void Move()
	{	
		this.isMoving = true;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isMoving){
			this.ScrollBaseOffset -=new Vector2( this.speed,0);
			if (this.ScrollBaseOffset.X == destination)
			{
				this.isMoving = false;
				EmitSignal(BackGround.SignalName.StopMoving);
				//this.ScrollBaseOffset = new Vector2(0,0);
				destination -= 440;
			}
		}
	}

	[Signal]
	public delegate void StopMovingEventHandler();

}
