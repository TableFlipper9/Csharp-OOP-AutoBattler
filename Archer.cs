using Godot;
using System;

public partial class Archer : Hero
{
	public override void _Ready()
	{
		base._Ready();

		sprite.SpriteFrames = GD.Load<SpriteFrames>("res://Animations/Hero.tres");
		this.range = 60;
	}
	public override void BasicAttack()
	{
		GD.Print("PP");
		this.enemy.takeDamage(this.damage.basicDamage);
		this.charge += chargeRate.basicCharge;
		CheckEnemyAlive();
	}

	public override void ChargeAttack()
	{
		this.charge = 0;
		var aoe = GD.Load<PackedScene>("res://AreaOfEffect.tscn").Instantiate<AreaAttack>();
		aoe.Init("Archer",new CircleShape2D(), this.damage.specialDamage, this.GlobalPosition, new Vector2(5, 5));
		this.AddChild(aoe);
		//this.charge += chargeRate.specialCharge;
		CheckEnemyAlive();
	}
	public override void UltimateAttack()
	{
		var arrow = GD.Load<PackedScene>("res://Projectile.tscn").Instantiate<Projectile>();
		arrow.Init(new RectangleShape2D(),this.damage.ultimateDamage,this.GlobalPosition,enemy.GlobalPosition, new Vector2(2, 2));
		GetTree().Root.AddChild(arrow);
		this.charge += chargeRate.ultimateCharge;
		CheckEnemyAlive();
	}
}
