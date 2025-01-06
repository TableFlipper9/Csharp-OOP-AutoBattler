using Godot;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static Godot.WebSocketPeer;

public partial class CapacityUpgrade : Control
{
	public string description;
	public Button button;
	public int level = 0;
	public override void _Ready()
	{
		this.button = GetNode<Button>("Button");
	}

	public void Init(string title,string des)
	{
		button.Text = title;
		description = des;
	}

	[Signal]
	public delegate void UpgradeCapEventHandler();

	public ToolTip info = null;
	public void MouseEntered()
	{
		var tt = GD.Load<PackedScene>("res://Scenes/ToolTip.tscn").Instantiate<ToolTip>();
		tt.Init(description + " [color=#0FFFF0]"+ (Global.capacitiUpgrades[this.level+1] - Global.capacitiUpgrades[this.level]) + "[/color] unit/s", "More slots");
		GetTree().Root.AddChild(tt);
		info = tt;
	}

	public void MouseExited()
	{
		info.QueueFree();
	}

	public void Load(int level)
	{
		for (int i = this.level; i < level; i++)
		{
			this.level++;
			EmitSignal(CapacityUpgrade.SignalName.UpgradeCap);
		}
	}

	public void OnButtonPressed()
	{
		if (level < 8 && Global.stateChanger == false)
		{
			if (Global.SpendGold(Global.capacityPrices[this.level]))
			{
				this.level++;
				EmitSignal(CapacityUpgrade.SignalName.UpgradeCap);
			}
			else
			{
				var ea = GD.Load<PackedScene>("res://Scenes/ErrorAnimation.tscn").Instantiate<ErrorAnimation>();
				ea.Init("Not enough money", this.GlobalPosition);
				GetTree().Root.AddChild(ea);
			}
		}
	}

	public override void _Process(double delta)
	{
	}
}
