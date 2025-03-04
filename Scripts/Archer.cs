using Godot;
using System;

public partial class Archer : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Archer.tres");
		this.range = 410;
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
		var arrow = GD.Load<PackedScene>("res://Scenes/Projectile.tscn").Instantiate<Projectile>();
		arrow.Init("Archer",new CircleShape2D(),this.damage.specialDamage, this.GlobalPosition, enemy, new Vector2(3, 3), new Vector2(3, 3));
		GetTree().Root.AddChild(arrow);
		//this.charge += chargeRate.specialCharge;
	}
	public override void UltimateAttack()
	{
	}
}
