using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static Godot.HttpRequest;

public partial class UnitField : Node
{
	public int capacity;
	public int level;
	public List<Unit> units = new List<Unit>();
	public List<Vector2> locations = new List<Vector2>();

	public bool isHero;
	public bool isEmpty =  true;

	public struct parcel
	{
		public int xL;
		public int yL;
		public int xR;
		public int yR;
	}
	public parcel fieldSize;

	public UnitField()
	{}

	public bool CheckFieldEmpty()
	{
		for (int i = 0; i < units.Count; i++)
		{
			if (units[i] != null)
			{
				isEmpty = false;
				return false;
			}
		}
		isEmpty = true;
		return true;
	}

	public virtual void Init(int level,Vector2 center)
	{
		this.fieldSize.xL = (int)center.X + 100;
		this.fieldSize.yL = (int)center.Y + 200;
		this.fieldSize.xR = (int)center.X - 100;
		this.fieldSize.yR = (int)center.Y - 200;
		this.level = level;
		this.capacity = level;
		this.isHero = true;
	}

	public virtual void LevelUp(int capacity)
	{
		this.capacity = capacity;
	}

	public void Attack()
	{
		foreach (var unit in units){
			if(unit != null) {
				unit.EmitSignal(Unit.SignalName.SearchNewOpponent, unit, unit.prio);
			}
		}
	}

	public void CalculateInRowLocations()
	{
		int columnIndex = 0;
		int pointsInColumn = 5;

		int columnsNeeded = (int)Math.Ceiling((float)capacity / 5);
		float columnSpacingX = (this.fieldSize.xR - this.fieldSize.xL) / (columnsNeeded + 1);

		int remainingPoints = capacity;

		for (int column = 0; column < columnsNeeded; column++){
			if (remainingPoints > 0){
				if (columnIndex % 2 == 0 && remainingPoints >= 5){
					pointsInColumn = 5;
					remainingPoints -= 5;
				}
				else if (remainingPoints < 5){
					pointsInColumn = remainingPoints;
					remainingPoints = 0;
				}
				else{
					pointsInColumn = 6;
					remainingPoints -= 6;
				}
			}

			float columnSpacing = (this.fieldSize.yR - this.fieldSize.yL) / (pointsInColumn + 1);
			float columnX;
			if (this.isHero == true){
				columnX = this.fieldSize.xL + columnSpacingX * (column + 1);
			}
			else{
				columnX = this.fieldSize.xR - columnSpacingX * (column + 1);
			}

			for (int i = 0; i < pointsInColumn; i++){
				float yPos = this.fieldSize.yR - columnSpacing * (i + 1);
				locations.Add(new Vector2(columnX, yPos));
			}

			columnIndex++;
			}
	}

	public virtual void UnitDeath(int index)
	{
		units[index] = null;
	}
}
