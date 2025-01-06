using Godot;
using System;

public partial class PlayButton : Control
{
	public AnimationPlayer player;
	public override void _Ready()
	{
		player = GetNode<AnimationPlayer>("AnimationPlayer");
		player.Play("PLAY");
	}

	[Signal]
	public delegate void StartFightEventHandler();

	public void StartPlan()
	{
		player.Play("HSOOW");
	}

	public void OnButtonToggle(bool toggle)
	{
		EmitSignal(PlayButton.SignalName.StartFight);
		player.Play("WOOSH");
	}

}
