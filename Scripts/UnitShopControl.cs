using Godot;
using System;
using System.Collections.Generic;

public partial class UnitShopControl : Control
{
	private GridContainer grid;
	public List<LevelUpUnitButton> units = new List<LevelUpUnitButton>();

	public MarginContainer openedTab;
	public NinePatchRect closedTab;
	public override void _Ready()
	{
		this.grid = GetNode<GridContainer>("OpenedTab/GridContainer");
		openedTab = GetNode<MarginContainer>("OpenedTab");
		closedTab = GetNode<NinePatchRect>("ClosedTab");


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
		var newUnit = GD.Load<PackedScene>("res://Scenes/LevelUpUnitButton.tscn").Instantiate<LevelUpUnitButton>();
		newUnit.OnPurchase += this.PurchaseUnit;
		newUnit.Init(type);
		grid.AddChild(newUnit);
		units.Add(newUnit);
	}

	public void OnButtonClose(bool toggle)
	{
		openedTab.Visible = !(openedTab.Visible);
		closedTab.Visible = !(closedTab.Visible);
	}
	public override void _Process(double delta)
	{
	}
}
