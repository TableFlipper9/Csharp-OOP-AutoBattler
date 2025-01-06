using Godot;
using System;
using System.Collections.Generic;
using static Godot.Control;

public partial class UnitFieldControl : Control
{
	public Button minimize; 
	public NinePatchRect openedTab;
	public NinePatchRect closedTab;
	public UnitGridControl front;
	public UnitGridControl back;

	public SelectedAnimation select;

	public bool globalMouseLoad;
	public UnitFieldIcon globalLoad;

	public HeroUnitField HFF;
	public HeroUnitField HBF;

	public Unit selectedUnit = null;

	public void Init(HeroUnitField HFF,HeroUnitField HBF)
	{
		this.HFF = HFF;
		this.HBF = HBF;
		if (HFF != null){
			this.front.UpdateAfterSwap += HFF.UpdateAfterSwap;
			HFF.UpdateUnitIcon += this.front.update;
			HFF.IncreaseCapacity += this.front.UpdateCap;
			this.front.SellUnit += HFF.SoldUnit;

			this.front.Selected += SelectHFF;
			this.front.IfSwap += LoadGlobalMouseFront;
			this.front.SwapBetweenFields += SwapHBF;
		}
		if (HBF != null){
			this.back.UpdateAfterSwap += HBF.UpdateAfterSwap;
			HBF.UpdateUnitIcon += this.back.update;
			HBF.IncreaseCapacity += this.back.UpdateCap;
			this.back.SellUnit += HBF.SoldUnit;

			this.back.Selected += SelectHBF;
			this.back.IfSwap += LoadGlobalMouseBack;
			this.back.SwapBetweenFields += SwapHHF;
		}
	}
	public override void _Ready()
	{
		minimize = GetNode<Button>("Close");
		openedTab = GetNode<NinePatchRect>("OpenedTab");
		closedTab = GetNode<NinePatchRect>("ClosedTab");
		front = GetNode<UnitGridControl>("OpenedTab/Back");
		back = GetNode<UnitGridControl>("OpenedTab/Front");
		select = GetNode<SelectedAnimation>("SelectedAnimation");
	}

	public void SelectHFF(int index)
	{
		select.Visible = true;
		back.UnSelect();
		if (HFF.units[index] != null){
			selectedUnit = HFF.units[index];
			select.Play(HFF.units[index].GlobalPosition, 66);
			Global.currentlySelectedHero = (Hero)HFF.units[index];
		}
		else
		{
			select.Play(HFF.locations[index], 44);
		}
	}

	public void SelectHBF(int index)
	{
		select.Visible = true;
		front.UnSelect();
		if (HBF.units[index] != null)
		{
			selectedUnit = HBF.units[index];
			select.Play(HBF.units[index].GlobalPosition, 66);
			Global.currentlySelectedHero = (Hero)HBF.units[index];
		}
		else
		{
			select.Play(HBF.locations[index], 44);
		}
	}

	public void SwapHHF(int index,int index2)
	{
		Unit temp = HFF.units[index];
		HFF.Disconnect(index);
		HFF.Connect(HBF.units[index2], index);
		HBF.Disconnect(index2);
		HBF.Connect(temp, index2);
	}

	public void SwapHBF(int index, int index2)
	{
		Unit temp = HBF.units[index];
		HBF.Disconnect(index);
		HBF.Connect(HFF.units[index2], index);
		HFF.Disconnect(index2);
		HFF.Connect(temp, index2);
	}

	public void LoadGlobalMouseFront(UnitFieldIcon icon, bool load)
	{
		globalMouseLoad = load;
		globalLoad = icon;
		back.LoadMouse(load,icon,load);
	}

	public void LoadGlobalMouseBack(UnitFieldIcon icon, bool load)
	{
		globalMouseLoad = load;
		globalLoad = icon;
		front.LoadMouse(load, icon, load);
	}

	public void OnCloseToggle(bool pressed)
	{
		openedTab.Visible = !(openedTab.Visible);
		closedTab.Visible = !(closedTab.Visible);
	}
	public override void _Process(double delta)
	{
		if (selectedUnit != null && Global.currentlySelectedHero != null)
		{
			select.GlobalPosition = new Vector2(selectedUnit.GlobalPosition.X - 33,selectedUnit.GlobalPosition.Y - 33);
		}
		else
		{
			select.Visible = false;
			selectedUnit = null;
			back.UnSelect();
			front.UnSelect();
		}
	}
}
