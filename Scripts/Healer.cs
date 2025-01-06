using Godot;
using System;

public partial class Healer : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Healer.tres");
		this.range = 180;
		this.prio = (int)Global.priority.frontbackH;

	}
	public override void BasicAttack()
	{
		this.enemy.takeDamage(this.damage.basicDamage);
		this.charge += chargeRate.basicCharge;
	}

	public override void ChargeAttack()
	{
		this.charge = 0;
		var aoe = GD.Load<PackedScene>("res://Scenes/AreaOfEffect.tscn").Instantiate<AreaAttack>();
		aoe.Init("Solder", new CircleShape2D(), this.damage.specialDamage, this.GlobalPosition, new Vector2(5, 5), new Vector2(5, 5));
		GetTree().Root.AddChild(aoe);
		//this.charge += chargeRate.specialCharge;
	}
	public override void UltimateAttack()
	{
	}
	public bool walk = false;
	public override void _Process(double delta)
	{
		GD.Print(Global.stateChanger);
		base._Process(delta);
		if (Global.stateChanger == false && walk == false)
		{
			enemy = null;
			walk = true;
			moveHere(locationInrow);
		}
		else
		{
			walk = false;
		}
	}
}
