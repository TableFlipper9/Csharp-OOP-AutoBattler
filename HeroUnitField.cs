using Godot;
using System;

public partial class HeroUnitField : UnitField
{
	public HeroUnitField()
	{
		capacity = 1;
		level = 0;
	}

	[Signal]
	public delegate void UpdateUnitIconEventHandler(int index, string type);

	[Signal]
	public delegate void IncreaseCapacityEventHandler(int capacity);

	public int nextavalibleLocation;
	public bool HasAvalibleSpace()
	{
		if (units.Count < capacity){
			nextavalibleLocation = units.Count;
			return true;
		}
		for (int i = 0; i < units.Count; i++){
			if (units[i] == null){
				nextavalibleLocation = i;
				return true;
			}
		}
		return false;
	}

	public void AddNewUnit(Hero newUnit)
	{
		if (nextavalibleLocation == units.Count)
			units.Add(newUnit);
		else
			units[nextavalibleLocation] = newUnit;

		AddChild(newUnit);
		newUnit.SetIndex(nextavalibleLocation);
		newUnit.UnitDeath += this.UnitDeath;
		EmitSignal(HeroUnitField.SignalName.UpdateUnitIcon, newUnit.indexInList, Global.heroNames[Global.heroStringMap[newUnit.type]]);
	}
	public void UpdateLocations()
	{
		for (int i = 0; i < this.locations.Count; i++)
		{
			locations.Remove(this.locations[i]);
		}

		calculateInRowLocations();

		for (int i = 0; i < this.units.Count; i++)
		{
			if (this.units[i] != null)
			{
				this.units[i].updateInRowLocation(this.locations[i]);
			}
		}
	}

	public void Move()
	{
		for (int i = 0; i < this.units.Count; i++){
			if (this.units[i] != null){
				this.units[i].playWalk();
			}
		}
	}
	public void Idle()
	{
		for (int i = 0; i < this.units.Count; i++){
			if (this.units[i] != null){
				this.units[i].playIdle();
			}
		}
	}
	public void UpdateAfterSwap(int index, int index2)
	{
		Unit temp = this.units[index];
		this.units[index] = this.units[index2];
		this.units[index2] = temp;

		if (units[index] != null){
			this.units[index].SetIndex(index);
			this.units[index].updateInRowLocation(locations[index]);
		}
		if (units[index2] != null){
			this.units[index2].SetIndex(index2);
			this.units[index2].updateInRowLocation(locations[index2]);
		}
	}

	public override void UnitDeath(int index)
	{
		base.UnitDeath(index);
		EmitSignal(HeroUnitField.SignalName.UpdateUnitIcon,index, (string)null);
	}

	public override void LevelUp(int capacity)
	{
		for (int i = this.capacity; i < capacity; i++){
			units.Add(null);
		}
		base.LevelUp(capacity);
		UpdateLocations();
		EmitSignal(HeroUnitField.SignalName.IncreaseCapacity, this.capacity);
	}
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
