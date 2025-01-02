using Godot;
using System;
using System.Collections.Generic;

public partial class UnitShopControl : Control
{
	private GridContainer grid;
	public List<LevelUpUnitButton> units = new List<LevelUpUnitButton>();
	public override void _Ready()
	{
		this.grid = GetNode<GridContainer>("MarginContainer/GridContainer");
		AddUnit(Global.HeroTypes.Solder);
		AddUnit(Global.HeroTypes.Archer);
		AddUnit(Global.HeroTypes.Healer);
		AddUnit(Global.HeroTypes.Chaverly);
		AddUnit(Global.HeroTypes.Executioner);
	}
	[Signal]
	public delegate void PurchaseHeroEventHandler(int type, int price);

	private void PurchaseUnit(int type, int price)
	{
		EmitSignal(UnitShopControl.SignalName.PurchaseHero, type, price);
	}
	public void AddUnit(Global.HeroTypes type)
	{
		var newUnit = GD.Load<PackedScene>("res://LevelUpUnitButton.tscn").Instantiate<LevelUpUnitButton>();
		newUnit.OnPurchase += this.PurchaseUnit;
		newUnit.Init(type);
		grid.AddChild(newUnit);
		units.Add(newUnit);
	}

	public override void _Process(double delta)
	{
	}
}
