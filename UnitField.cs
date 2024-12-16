using Godot;
using System;
using System.Collections.Generic;

public partial class UnitField : Node
{
	public int capacity;
	public int level;
	public List<Unit> units = new List<Unit>();
	public List<Vector2> locations = new List<Vector2>();

	public UnitField()
	{
		capacity = 1;
		level = 0;
	}

	public void Init(int level)
	{
		this.level = level;
		//this.capacity = World.fieldCapacity[level];
		this.capacity = level + 1;
	}
	public void AddNewUnit(Unit newUnit)
	{
		for (int i = 0; i < units.Count; i++) {
			if (units[i] == null) {
				units[i] = newUnit;
				return;
			}
		}
		units.Add(newUnit);
		AddChild(newUnit);
	}
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
