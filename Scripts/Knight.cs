using Godot;
using System;

public partial class Knight : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Knight.tres");
		this.range = 80;
		this.prio = (int)Global.priority.frontbackE;

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
		aoe.Init("Knight", new CircleShape2D(), this.damage.specialDamage, this.GlobalPosition, new Vector2(5, 5), new Vector2(5, 5));
		GetTree().Root.AddChild(aoe);
		//this.charge += chargeRate.specialCharge;
	}
	public override void UltimateAttack()
	{

	}
}
