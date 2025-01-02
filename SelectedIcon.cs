using Godot;
using System;
using System.Drawing;

public partial class SelectedIcon : Control
{
	private TextureRect icon;
	private AnimationPlayer player;
	public override void _Ready()
	{
		this.icon = GetNode<TextureRect>("TextureRect");
		this.player = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	public void SetSize(Vector2 location, Vector2 size)
	{
		this.GlobalPosition = location;
		this.Scale = size;
	}
	public void MoveHere()
	{
		player.Play("Idle");
		this.Visible = true;
	}

	public void TurnOff()
	{
		player.Stop();
		this.Visible = false;
	}
}
