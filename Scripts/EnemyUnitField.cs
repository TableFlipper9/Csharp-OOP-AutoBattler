using Godot;
using System;

public partial class EnemyUnitField : UnitField
{
	public bool isEmpty = false;
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

	private void CheckFieldEmpty()
	{
		isEmpty = true;
		for (int i = 0; i < units.Count; i++){
			if (units[i] != null){
				isEmpty = false;
			}
		}
	}
	public void CreateNewField()
	{
		this.LevelUp(Global.enemyStageCapacitiesFront[0]);
		this.calculateInRowLocations();

		for (int i = 0; i < 3/*Global.FrontEnemyStages[0].Length*/; i++){
			var newUnit = GD.Load<PackedScene>("res://Scenes/Enemy.tscn").Instantiate<Enemy>();
			Global.StageEnemy enemy = Global.FrontEnemyStages[0][i];

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
		CheckFieldEmpty();
		if (isEmpty){
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
