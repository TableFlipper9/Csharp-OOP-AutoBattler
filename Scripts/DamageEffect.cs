using Godot;
using System;

public partial class DamageEffect : Node2D
{
	RandomNumberGenerator rng =  new Godot.RandomNumberGenerator();
	public Label label;
	public AnimationPlayer player;
	public string text;

	public void Init(int value, Vector2 Position)
	{
		//this.GlobalPosition = Position;
		if (value < 0)
		{
			text = "++" + (value - value *2);
		}
		else
		{
			text = "- " + value;
		}
		this.GlobalPosition -= new Vector2(rng.RandiRange(-20, 20), this.GlobalPosition.Y);
	}

	public override void _Ready(){
		label = GetNode<Label>("Label");
		player = GetNode<AnimationPlayer>("AnimationPlayer");
		rng.Randomize();
		label.Text = text;
		player.Play("Fade");
	}

	public void OnAnimationFinished(Animation anim)
	{
		QueueFree();
	}
}
