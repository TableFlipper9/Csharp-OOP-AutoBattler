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
	// Called when the node enters the scene tree for the first time.
	public void Init(HeroUnitField HFF,HeroUnitField HBF)
	{
		if (HFF != null){
			this.front.UpdateAfterSwap += HFF.UpdateAfterSwap;
			HFF.UpdateUnitIcon += this.front.update;
			HFF.IncreaseCapacity += this.front.UpdateCap;
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
		GD.Print("PP");
		openedTab.Visible = !(openedTab.Visible);
		closedTab.Visible = !(closedTab.Visible);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
