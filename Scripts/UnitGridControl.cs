using Godot;
using System;
using System.Collections.Generic;

public partial class UnitGridControl : MarginContainer
{
	public int capacity;
	public List<UnitFieldIcon> units = new List<UnitFieldIcon>();
	public GridContainer grid1;
	public GridContainer grid2;
	public GridContainer grid3;
	public SelectedIcon selectedIcon;

	public void UpdateCap(int capacity)
	{
		while(this.capacity<capacity)
		{
			var newUnit = GD.Load<PackedScene>("res://Scenes/UnitFieldIcon.tscn").Instantiate<UnitFieldIcon>();
			LinkSignal(newUnit);

			newUnit.Init(null, this.capacity);

			units.Add(newUnit);

			if (this.capacity < 5){
				grid3.AddChild(newUnit);
			}
			else if(this.capacity < 11){
				grid2.AddChild(newUnit);
			}
			else{
				grid1.AddChild(newUnit);
			}
			this.capacity ++;
		}
	}

	public bool mouseIsInRange = false;
	public bool mouseIsLoaded = false;
	public UnitFieldIcon indexOfMouse ;
	public UnitFieldIcon indexOfUnit ;

	public void OnMousePressed(UnitFieldIcon self)
	{
		GD.Print("OO" ,self.indexInField);
		this.indexOfUnit = self; 
		mouseIsLoaded = true;
	}
	new public void MouseEntered(UnitFieldIcon self)
	{
		this.indexOfMouse = self;
		mouseIsInRange = true;
	}

	new public void MouseExited()
	{
		mouseIsInRange = false;
	}

	[Signal]
	public delegate void UpdateAfterSwapEventHandler(int index,int index2);

	public void OnMouseRelease()
	{
		GD.Print("PP", this.indexOfMouse.indexInField);
		GD.Print((units[this.indexOfMouse.indexInField].type));
		if (mouseIsLoaded && mouseIsInRange){
			if (this.indexOfMouse == this.indexOfUnit){
				selectedIcon.SetSize(indexOfMouse.GlobalPosition, new Vector2((float)0.48,(float)0.52));
				selectedIcon.MoveHere();
			}
			else{
				mouseIsInRange = false;
				mouseIsLoaded = false;
				Swap(indexOfMouse, indexOfUnit);
				EmitSignal(UnitGridControl.SignalName.UpdateAfterSwap, this.indexOfMouse.indexInField, this.indexOfUnit.indexInField);
			}
		}
	}

	public void Swap(UnitFieldIcon a,UnitFieldIcon  b)
	{
		string temp = a.type;
		a.update(b.type);
		b.update(temp);

		//int tempIndex = units[a].indexInField;
		//string tempType = units[a].type;
		//units[a].update(units[b].type, units[b].indexInField);
		//units[b].update(tempType, tempIndex);
	}

	[Signal]
	public delegate void SellUnitEventHandler(int index);

	public void SellThisUnit(int index)
	{
		EmitSignal(UnitGridControl.SignalName.SellUnit, index);
	}

	public void LinkSignal(UnitFieldIcon unit)
	{
		unit.OnMouseEntered += this.MouseEntered;
		unit.OnMouseExited += this.MouseExited;
		unit.MousePressed += this.OnMousePressed;
		unit.MouseReleased += this.OnMouseRelease;
		unit.SellUnit += this.SellThisUnit;
	}

	public void update(int index, string type)
	{
		units[index].type = type;
		units[index].updateIcon();
	}
	public override void _Ready()
	{
		this.grid1 = GetNode<GridContainer>("HBoxContainer/GridContainer");
		this.grid2 = GetNode<GridContainer>("HBoxContainer/GridContainer2");
		this.grid3 = GetNode<GridContainer>("HBoxContainer/GridContainer3");
		this.selectedIcon = GetNode<SelectedIcon>("SelectedIcon");
		//UpdateCap(3);
	}

	public override void _Process(double delta)
	{
	}
}
