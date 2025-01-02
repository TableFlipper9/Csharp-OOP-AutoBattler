using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class Main : Node2D
{ 
	public EnemyUnitField EFF;
	public HeroUnitField HFF;
	public BackGround BackGround;
	public UnitFieldControl FieldControl;
	public UnitShopControl ShopControl;

	public override async void _Ready()
	{
		this.HFF = GetNode<HeroUnitField>("HeroFrontField");
		this.EFF = GetNode<EnemyUnitField>("EnemyFrontField");
		this.BackGround = GetNode<BackGround>("BackGround");
		this.FieldControl = GetNode<UnitFieldControl>("FieldControl");
		this.ShopControl = GetNode<UnitShopControl>("UnitShopControl");

		Global.GainGold(200);
		GD.Print("KK ",ShopControl.units[1].purchasePrice);
		ShopControl.units[1].ChangeIcon();

		ShopControl.PurchaseHero += this.CreateNewHero;

		HFF.Init(1, new Vector2(300, 370));
		EFF.Init(1, new Vector2(800, 370));

		FieldControl.Init(HFF,null);

		HFF.LevelUp(12);
		//EFF.capacity = 7;

		EFF.isHero = false;

		HFF.calculateInRowLocations();
		//EFF.calculateInRowLocations();

		for (int i = 0; i < 8; i++)
		{
			if (HFF.HasAvalibleSpace())
			{
				var hr = GD.Load<PackedScene>("res://Archer.tscn").Instantiate<Archer>();
				hr.Init(Global.HeroTypes.Archer, HFF.locations[HFF.nextavalibleLocation]);
				hr.KilledOpponent += this.FindNewEnemyHFF;

				HFF.AddNewUnit(hr);
			}
		}

		EFF.CreateNewField();

		BackGround.StopMoving += HFF.Idle;
		//BackGround.StopMoving += EFF.Idle;

		//var time = GetTree().CreateTimer(3);

		//time.Timeout += () => { HFF.units[0].takeDamage(1000);};
		//var time2 = GetTree().CreateTimer(5);
		//time2.Timeout += () => { GD.Print(HFF.units[0]); /*HFF.units[1].moveHere(new Vector2(500, 300));*/ HFF.capacity = 7; HFF.updateLocations();};

		var time3 = GetTree().CreateTimer(3);
		time3.Timeout += () => { HFF.units[0].moveToAttack(HFF.units[0].closestEnemy(EFF.units).GlobalPosition); };
		///////////

		//{
		//    des = hr.closestEnemy(EFF.units).GlobalPosition; GD.Print(des); hr.moveToAttack(des);
		//    des2 = en.closestEnemy(HFF.units).GlobalPosition; GD.Print(des2); en.moveToAttack(des2);
		//};

	}

	/// need to fix front line and back line creation to match the signals, and pay attention when swapping unit between fields
	public void FindNewEnemyHFF(int index)
	{
		//GD.Print(HFF.units[0].enemy);
		//GD.Print(HFF.units[0].closestEnemy(EFF.units));
		//GD.Print(EFF.units[2]);
		HFF.units[0].moveToAttack(HFF.units[0].closestEnemy(EFF.units).GlobalPosition);
	}

	public void CreateNewHero(int type, int Price)
	{
		if (HFF.HasAvalibleSpace()){
			var hr = GD.Load<PackedScene>("res://Archer.tscn").Instantiate<Archer>();
			hr.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
			this.ShopControl.units[type].OnLevelUp += hr.LevelUp;
			HFF.AddNewUnit(hr);

			Global.SpendGold(Price);
			//Global.EmitSignal(Global.SignalName.GoldUpdate);
		}
	}
	public override void _Process(double delta)
	{
	}
}
