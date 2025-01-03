using Godot;
using System;
using System.Collections.Generic;

public partial class UnitFieldControl : Control
{
	public Button minimize; 
	public NinePatchRect openedTab;
	public NinePatchRect closedTab;
	public UnitGridControl front;
	public UnitGridControl back;

	public void Init(HeroUnitField HFF,HeroUnitField HBF)
	{
		if (HFF != null){
			this.front.UpdateAfterSwap += HFF.UpdateAfterSwap;
			HFF.UpdateUnitIcon += this.front.update;
			HFF.IncreaseCapacity += this.front.UpdateCap;
			this.front.SellUnit += HFF.SoldUnit;
		}
		if (HBF != null){
			this.back.UpdateAfterSwap += HBF.UpdateAfterSwap;
		}
	}
	public override void _Ready()
	{
		minimize = GetNode<Button>("Close");
		openedTab = GetNode<NinePatchRect>("OpenedTab");
		closedTab = GetNode<NinePatchRect>("ClosedTab");
		front = GetNode<UnitGridControl>("OpenedTab/Back");
		back = GetNode<UnitGridControl>("OpenedTab/Front");
	}
	
	public void OnCloseToggle(bool pressed)
	{
		openedTab.Visible = !(openedTab.Visible);
		closedTab.Visible = !(closedTab.Visible);
	}
	public override void _Process(double delta)
	{
	}
}
