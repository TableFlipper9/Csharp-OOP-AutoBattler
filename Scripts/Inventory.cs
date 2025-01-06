using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control
{
	private GridContainer grid;
	public List<Item> items = new List<Item>();

	public MarginContainer openedTab;
	public NinePatchRect closedTab;

	public override void _Ready()
	{
		this.grid = GetNode<GridContainer>("OpenedTab/MarginContainer/ScrollContainer/GridContainer");
		openedTab = GetNode<MarginContainer>("OpenedTab");
		closedTab = GetNode<NinePatchRect>("ClosedTab");
	}

	public void AddUnit(Global.Iteams type)
	{
		var newUnit = GD.Load<PackedScene>("res://Scenes/Item.tscn").Instantiate<Item>();
		newUnit.ButtonPressed += this.ActivateIteam;
		newUnit.Init(type,"Bread",items.Count);
		grid.AddChild(newUnit);
		items.Add(newUnit);
	}

	public void ActivateIteam(int type,int value, int index)
	{
		if (Global.currentlySelectedHero == null)
		{
			var ea = GD.Load<PackedScene>("res://Scenes/ErrorAnimation.tscn").Instantiate<ErrorAnimation>();
			ea.Init("No selected Unit", items[index].GlobalPosition);
			GetTree().Root.AddChild(ea);
		}
		else
		{
			GD.Print(type);
			switch (Global.itemMap[(Global.Iteams)type])
			{
				case Global.IteamsAbilities.LevelUp:
					Global.currentlySelectedHero.LevelUp();
					break;
				case Global.IteamsAbilities.Heal:
					Global.currentlySelectedHero.IncreaseHP(value);
					break;
				case Global.IteamsAbilities.IncreaseDamage:
					Global.currentlySelectedHero.IncreaseDamage(value);
					break;
				case Global.IteamsAbilities.IncreaseHp:
					Global.currentlySelectedHero.IncreaseMaxHP(value);
					break;
			}

			Item temp = items[index];
			temp.QueueFree();
			items[index] = null;
			items.Remove(items[index]);
		}
	}

	public void OnButtonToggle(bool toggle)
	{
		openedTab.Visible = !(openedTab.Visible);
		closedTab.Visible = !(closedTab.Visible);
	}

	public override void _Process(double delta)
	{
	}
}
