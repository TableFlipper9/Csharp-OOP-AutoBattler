using Godot;
using System;

public partial class Chaverly : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Chaverly.tres");
		this.range = 120;
		this.prio = (int)Global.priority.frontbackE;

	}
	public override void BasicAttack()
	{
		this.enemy.TakeDamage(this.damage.basicDamage);
		this.charge += chargeRate.basicCharge;
	}

	public override void ChargeAttack()
	{
		this.charge = 0;
		var aoe = GD.Load<PackedScene>("res://Scenes/AreaOfEffect.tscn").Instantiate<AreaAttack>();
		aoe.Init("Chaverly", new CircleShape2D(), this.damage.specialDamage, this.GlobalPosition, new Vector2(3, 3), new Vector2(3, 3));
		GetTree().Root.AddChild(aoe);

		MoveHere(enemy.GlobalPosition);
		this.enemy.TakeDamage(this.damage.basicDamage);
		this.charge += chargeRate.basicCharge;
		this.charge += chargeRate.specialCharge;
	}
	public override void UltimateAttack()
	{

	}
}
