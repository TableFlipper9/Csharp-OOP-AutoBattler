using Godot;
using System;
using static Global;

public partial class Global : Node
{
	public static int Gold = 20;
	public static int Stage = 0;
	public enum HeroTypes
	{
		Solder,
		Archer,
		Healer,
		Chaverly,
		Executioner
	}
	public static System.Collections.Generic.Dictionary<HeroTypes, int> heroStringMap = new System.Collections.Generic.Dictionary<HeroTypes, int>()
	{
		{ HeroTypes.Solder, 0 },
		{ HeroTypes.Archer, 1 },
		{ HeroTypes.Healer, 2 },
		{ HeroTypes.Chaverly, 3 },
		{ HeroTypes.Executioner, 4 },
	};

	public static string[] heroNames = new string[] { "Solder", "Archer", "Healer", "Chaverly", "Executioner" };
	public static int[] heroLevels = new int[] { 0, 0, 1, 1, 1 };
	public static int[] heroPrices = new int[] { 10, 300, 500, 3000, 8000 };
	public static int[] heroUnlockstage = new int[] { 1, 1, 3, 8, 14 };
	public static int[] heroMaxCharge = new int[] { 100, 75, 50, 250, 155 };
	public static int[] heroSellvalues = new int[] { 5, 10, 20, 30, 50 };

	public static int[][] heroMaxHealth = new int[][] {
		new int[] { 100, 200, 300 },
		new int[] { 150, 250, 350 },
		new int[] { 120, 220, 320 },
		new int[] { 130, 230, 330 },
		new int[] { 140, 240, 340 }
	};
	public static int[][][] heroDamage = new int[][][] {
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 100, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 }, new int[] { 210, 320, 430 } }
	};
	public static int[][][] heroChargeRate = new int[][][] {
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] {new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 }, new int[] { 210, 320, 430 } },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}, new int[] { 210, 320, 430 } },
		new int[][] {new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 }, new int[] { 210, 320, 430 } },
		new int[][] {new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 }, new int[] { 210, 320, 430 } }
	};

	public enum EnemyTypes
	{
		GOBLIN,
		Archer,
		Healer,
		Chaverly,
		Executioner
	}
	public static System.Collections.Generic.Dictionary<EnemyTypes, int> enemyStringMap = new System.Collections.Generic.Dictionary<EnemyTypes, int>()
	{
		{ EnemyTypes.GOBLIN, 0 },
		{ EnemyTypes.Archer, 1 },
		{ EnemyTypes.Healer, 2 },
		{ EnemyTypes.Chaverly, 3 },
		{ EnemyTypes.Executioner, 4 },
	};

	public static string[] enemyNames = new string[] { "GOBLIN", "Archer", "Healer", "Chaverly", "Executioner" };
	/// <summary>
	/// public static int[] enemyLevels = new int[] { 1, 0, 0, 0, 0 };
	/// </summary>
	public static int[] enemyMaxCharge = new int[] { 100, 75, 50, 250, 155 };

	public static int[][] enemyMaxHealth = new int[][] {
		new int[] { 100, 200, 300 },
		new int[] { 150, 250, 350 },
		new int[] { 120, 220, 320 },
		new int[] { 130, 230, 330 },
		new int[] { 140, 240, 340 }
	};
	public static int[][][] enemyDamage = new int[][][] {
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 } }
	};
	public static int[][][] enemyChargeRate = new int[][][] {
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] {new int[] { 100, 200, 300 }, new int[] { 120, 230, 340} },
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340}},
		new int[][] { new int[] { 100, 200, 300 }, new int[] { 120, 230, 340 } }
	};

	public struct StageEnemy
	{
		public Global.EnemyTypes type;
		public int index;
		public int level;
		public StageEnemy(Global.EnemyTypes type, int index, int level)
		{
			this.type = type;
			this.index = index;
			this.level = level;
		}
	}

	public static int[] enemyStageCapacitiesFront = new int[] {3,3,5,5,7};
	public static int[] enemyStageCapacitiesBack = new int[] { 1, 1, 2, 3, 4 };

	/// <summary>
	/// how to make null-able
	/// </summary>
	public static StageEnemy[][] FrontEnemyStages = new StageEnemy[][] {
		new StageEnemy[]{
			new StageEnemy(Global.EnemyTypes.GOBLIN, 0, 0),
			new StageEnemy(Global.EnemyTypes.GOBLIN, 1, 0), 
			new StageEnemy(Global.EnemyTypes.GOBLIN, 2, 0) ,
		},
	};
	public static StageEnemy[][] BackEnemyStages = new StageEnemy[][] {
		new StageEnemy[]{
			new StageEnemy(Global.EnemyTypes.GOBLIN, 1, 1),
			new StageEnemy(Global.EnemyTypes.GOBLIN, 2, 1)
		},
	};

	public static int[] levelUpPrices = new int[] {10,15,20,30,50 };
	public static int[] purchaseUpPrices = new int[] {1,2,3,10,15 };

	/////////////////////////////////////////////////////////////////////////

	[Signal]
	public delegate void GoldUpdateEventHandler();
	
	public Hero currentlySelectedHero = null;
	public static void SpendGold(int gold)
	{
		//GD.Print(heroStringMap[HeroTypes.Solder]);
		if (gold <= Gold){
			Gold -= gold;
			//EmitSignal(Global.SignalName.GoldUpdate);
		}
	}

	public static void GainGold(int gold)
	{
		Gold += gold;
		//EmitSignal(Global.SignalName.GoldUpdate);
	}
}
