using Godot;
using System;

public partial class EnemyUnitField : UnitField
{
	//public bool isEmpty = false;
	public EnemyUnitField()
	{
		capacity = 1;
		level = 0;
	}
	public override void Init(int level, Vector2 center)
	{
		base.Init(level, center);
		for(int i=0; i < 30; i++){
			units.Add(null);
		}
	}

	[Signal]
	public delegate void FindNewEnemyEventHandler(Unit unit,int prio);

	public void SendSignal(Unit uni,int prio)
	{
		EmitSignal(EnemyUnitField.SignalName.FindNewEnemy,uni,prio);
	}

	public void CreateNewFrontField()
	{
		this.LevelUp(Global.enemyStageCapacitiesFront[Global.Stage]);
		this.calculateInRowLocations();

		for (int i = 0; i < Global.FrontEnemyStages[Global.Stage].Length; i++){
			var newUnit = GD.Load<PackedScene>("res://Scenes/Goblin.tscn").Instantiate<Enemy>();
			Global.StageEnemy enemy = Global.FrontEnemyStages[Global.Stage][i];
			newUnit.SearchNewOpponent += this.SendSignal;
			isEmpty = false;

			newUnit.Init(enemy.level, enemy.type, locations[enemy.index]);
			units[i] = newUnit;
			newUnit.SetIndex(enemy.index);
			newUnit.UnitDeath += this.UnitDeath;
			AddChild(newUnit);
		}
	}

	public void CreateNewBackField()
	{
		this.LevelUp(Global.enemyStageCapacitiesBack[Global.Stage]);
		this.calculateInRowLocations();

		for (int i = 0; i < Global.BackEnemyStages[Global.Stage].Length; i++)
		{
			var newUnit = GD.Load<PackedScene>("res://Scenes/Goblin.tscn").Instantiate<Goblin>();
			Global.StageEnemy enemy = Global.BackEnemyStages[Global.Stage][i];
			newUnit.SearchNewOpponent += this.SendSignal;
			isEmpty = false;

			newUnit.Init(enemy.level, enemy.type, locations[enemy.index]);
			units[i] = newUnit;
			newUnit.SetIndex(enemy.index);
			newUnit.UnitDeath += this.UnitDeath;
			AddChild(newUnit);
		}
	}

	[Signal]
	public delegate void AllEnemiesDeafetedEventHandler();
	public override void UnitDeath(int index)
	{
		base.UnitDeath(index);
		if (CheckFieldEmpty()){
			EmitSignal(EnemyUnitField.SignalName.AllEnemiesDeafeted);
		}
	}
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
