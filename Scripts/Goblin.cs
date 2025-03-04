using Godot;
using System;

public partial class Goblin : Enemy
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Goblin.tres");
		this.range = 60;
		this.prio = (int)Global.priority.frontbackH;

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
		aoe.Init("Goblin", new CircleShape2D(), this.damage.specialDamage, this.enemy.GlobalPosition, new Vector2(2, 2), new Vector2(2, 2));
		GetTree().Root.AddChild(aoe);
		//this.charge += chargeRate.specialCharge;
	}
}
