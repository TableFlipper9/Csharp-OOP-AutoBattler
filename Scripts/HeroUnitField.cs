using Godot;
using System;

public partial class HeroUnitField : UnitField
{
	public HeroUnitField()
	{}

	[Signal]
	public delegate void UpdateUnitIconEventHandler(int index, string type);

	[Signal]
	public delegate void IncreaseCapacityEventHandler(int capacity);

	public int nextavalibleLocation;
	public bool HasAvalibleSpace()
	{
		for (int i = 0; i < units.Count; i++) {
			if (units[i] == null) {
				nextavalibleLocation = i;
				return true;
			}
		}
		return false;
	}

	public void AddNewUnit(Hero newUnit)
	{
		isEmpty = false;
		units[nextavalibleLocation] = newUnit;

		AddChild(newUnit);
		newUnit.SetIndex(nextavalibleLocation);
		newUnit.UnitDeath += this.UnitDeath;
		EmitSignal(HeroUnitField.SignalName.UpdateUnitIcon, newUnit.indexInList, Global.heroNames[(int)newUnit.type]);
	}
	public void UpdateLocations()
	{
		locations.Clear();

		CalculateInRowLocations();

		for (int i = 0; i < units.Count; i++)
		{
			if (this.units[i] != null)
			{
				this.units[i].UpdateInRowLocation(this.locations[units[i].indexInList]);
			}
		}
	}

	[Signal]
	public delegate void AllHerosDeafetedEventHandler();

	public void Disconnect(int index)
	{
		if (units[index] != null)
			units[index].UnitDeath -= this.UnitDeath; 
	}

	public void Connect(Unit unit, int index)
	{
		isEmpty = false;
		units[index] = unit;
		if (units[index] != null){
			this.units[index].SetIndex(index);
			units[index].UpdateInRowLocation(this.locations[index]);
			units[index].UnitDeath += this.UnitDeath;
		}
	}

	public void Move()
	{
		for (int i = 0; i < this.units.Count; i++) {
			if (this.units[i] != null) {
				this.units[i].PlayWalk();
			}
		}
	}
	public void Idle()
	{
		for (int i = 0; i < this.units.Count; i++) {
			if (this.units[i] != null) {
				this.units[i].PlayIdle();
			}
		}
	}
	public void UpdateAfterSwap(int index, int index2)
	{
		Unit temp = this.units[index];
		this.units[index] = this.units[index2];
		this.units[index2] = temp;

		if (units[index] != null) {
			this.units[index].SetIndex(index);
			this.units[index].UpdateInRowLocation(locations[index]);
		}
		if (units[index2] != null) {
			this.units[index2].SetIndex(index2);
			this.units[index2].UpdateInRowLocation(locations[index2]);
		}
	}

	public override void UnitDeath(int index)
	{
		base.UnitDeath(index);
		EmitSignal(HeroUnitField.SignalName.UpdateUnitIcon, index, "Empty");

		if (CheckFieldEmpty())
		{
			EmitSignal(HeroUnitField.SignalName.AllHerosDeafeted);
		}
	}

	public void Upgrade()
	{
		if (Global.SaveFile == false)
		{
			if (Global.SpendGold(Global.capacityPrices[this.level]))
			{
				this.level++;
				int cap = Global.capacitiUpgrades[this.level];
				LevelUp(cap);
			}
			else GD.Print("NoMoneyForUpgarde");
		}
		else
		{
			this.level++;
			int cap = Global.capacitiUpgrades[this.level];
			LevelUp(cap);
		}
	}

	public override void LevelUp(int capacity)
	{
		for (int i = this.capacity; i < capacity; i++) {
			units.Add(null);
		}
		base.LevelUp(capacity);
		UpdateLocations();
		EmitSignal(HeroUnitField.SignalName.IncreaseCapacity, this.capacity);
	}

	public void SoldUnit(int index)
	{
		if(units[index] != null)
			units[index].Die();
		if (units[index] is Hero){
			Global.GainGold(10);
		}
	}
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
