using Godot;
using System;

public partial class Executioner : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Executioner.tres");
		this.range = 100;
		this.prio = (int)Global.priority.backfrontE;

	}
	public override void BasicAttack()
	{
		this.charge = 0;
		var aoe = GD.Load<PackedScene>("res://Scenes/AreaOfEffect.tscn").Instantiate<AreaAttack>();
		aoe.Init("Executioner", new CircleShape2D(), this.damage.basicDamage, this.GlobalPosition, new Vector2(5, 5), new Vector2(5, 5));
		GetTree().Root.AddChild(aoe);
		//this.charge += chargeRate.specialCharge;

		this.enemy.takeDamage(this.damage.basicDamage);
		this.charge += chargeRate.basicCharge;
	}

	public override void ChargeAttack()
	{
		this.enemy.takeDamage(this.damage.specialDamage);
		this.charge += chargeRate.basicCharge;
	}
	public override void UltimateAttack()
	{

	}
}
