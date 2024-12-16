using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Main : Node2D
{ 
	public UnitField EFF;
	public UnitField HFF;

	//Hero Hero { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		//Hero = new Hero(Hero.TYPE.KNIGHT);

		var hr = GD.Load<PackedScene>("res://Hero.tscn").Instantiate<Hero>();
		var en = GD.Load<PackedScene>("res://Enemy.tscn").Instantiate<Enemy>();

		hr.Init(Hero.TYPE.KNIGHT);
		en.Init(Enemy.TYPE.GOBLIN);

		this.HFF = GetNode<UnitField>("HeroFrontField");
		this.EFF = GetNode<UnitField>("EnemyFrontField");

		HFF.AddNewUnit(hr);
		EFF.AddNewUnit(en);

		var time = GetTree().CreateTimer(2);

		Vector2 des = new Vector2(0,0);
		Vector2 des2 = new Vector2(0,0);
		time.Timeout += () =>  {des = hr.closestEnemy(EFF.units).GlobalPosition; GD.Print(des); hr.moveToAttack(des);
								des2 = en.closestEnemy(HFF.units).GlobalPosition; GD.Print(des2); en.moveToAttack(des2);};
		//time.Timeout += ()=> hr.moveHere(new Vector2(800, 200));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
