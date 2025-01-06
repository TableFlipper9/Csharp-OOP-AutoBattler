using Godot;
using System;

public partial class SelectedAnimation : Node2D
{
	public AnimationPlayer player;
	public NinePatchRect textureRect;
	public override void _Ready()
	{
		player = GetNode<AnimationPlayer>("AnimationPlayer");
		textureRect = GetNode<NinePatchRect>("TextureRect");
	}

	public void Play(Vector2 position, int size)
	{
		this.Visible = true;
		this.GlobalPosition = new Vector2(position.X - size/2,position.Y - size/2);
		textureRect.Size = new Vector2(size, size);
		player.Play("Focus");
	}

	public void Stop()
	{
		this.Visible = false;
	}
}
