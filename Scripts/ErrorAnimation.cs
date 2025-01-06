using Godot;
using System;

public partial class ErrorAnimation : Control
{
	RandomNumberGenerator rng = new Godot.RandomNumberGenerator();
	public Label label;
	public AnimationPlayer player;
	public string text;

	public void Init(String text, Vector2 Position)
	{
		this.GlobalPosition = Position;
		this.text = text;
		this.GlobalPosition -= new Vector2(rng.RandiRange(-20, 20), 0);
	}
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		player = GetNode<AnimationPlayer>("AnimationPlayer");
		rng.Randomize();
		label.Text = text;
		player.Play("ERROR");
	}

	public void OnAnimationFinished(Animation anim)
	{
		QueueFree();
	}
}
