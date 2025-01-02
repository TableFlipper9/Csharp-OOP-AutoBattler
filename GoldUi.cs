using Godot;
using System;

public partial class GoldUi : Control
{
	private TextureRect coinTextureRect;
	private AnimationPlayer animationPlayer;
	private Label coinLabel;

	public override void _Ready()
	{
		coinTextureRect = GetNode<TextureRect>("MarginContainer/HBoxContainer/TextureRect");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		coinLabel = GetNode<Label>("MarginContainer/HBoxContainer/Label");

		animationPlayer.Play("Spin");
	}
	public override void _Process(double delta)
	{
		coinLabel.Text = "" + Global.Gold;
	}
}
