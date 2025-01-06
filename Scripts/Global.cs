using Godot;
using System;
using System.Linq;
using static Global;
using System.IO;
using System.Linq;

public partial class Global : Node
{
	public static bool SaveFile = false;
	public struct HeroSave
	{
		public int type;
		public int maxHealth;
		public int health;
		public int damage;
		public int level;
		public int charge;
	}
	public static HeroSave LoadingHero;

	public static int Gold = 20;
	public static int Stage = 0;
	public static bool FightState = false;

	public static bool stateChanger = false;

	public static int MaxUnitLevel = 15;
	public static int MaxStage = 20;

	public enum priority
	{
		frontbackH,
		backfrontH,
		frontbackE,
		backfrontE,
	}
	public enum HeroTypes
	{
		Solder,
		Archer,
		Healer,
		Chaverly,
		Executioner,
		Knight
	}

	public static int[] capacityPrices;
	public static int[] capacitiUpgrades;

	public static string[] heroNames;
	public static int[] heroLevels;
	public static int[] heroPrices;
	public static int[] heroUnlockstage;
	public static int[] heroMaxCharge;
	public static int[] heroSellvalues;

	public static int[][] heroMaxHealth;
	public static int[][][] heroDamage;
	public static int[][][] heroChargeRate;

	public enum EnemyTypes
	{
		Goblin,
		ArmourOrc,
		Rider,
		Skeleton,
		Wherewolf
	}

	public static string[] enemyNames;
	public static int[] enemyMaxCharge;

	public static int[][] enemyMaxHealth;
	public static int[][][] enemyDamage;

	public static int[][][] enemyChargeRate;


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

	public static int[] enemyStageCapacitiesFront ;
	public static int[] enemyStageCapacitiesBack ;

	public static StageEnemy[][] FrontEnemyStages;
	public static StageEnemy[][] BackEnemyStages;

	public static int[] levelUpPrices;
	public static int[] purchaseUpPrices;

	public static bool enemyFrontEmpty = true;
	public static bool enemyBackEmpty = true;

	public enum Iteams
	{
		Apple,
		Pie,
		Cookie,
		Potion,
		Pretzel,
		Bread,
		Tea,
		Shrimp
	}

	public enum IteamsAbilities
	{
		Heal,
		LevelUp,
		IncreaseHp,
		IncreaseDamage
	}

	public static System.Collections.Generic.Dictionary<Iteams, IteamsAbilities> itemMap = new System.Collections.Generic.Dictionary<Iteams, IteamsAbilities>()
	{
		{Iteams.Apple,IteamsAbilities.Heal},
		{Iteams.Pie,IteamsAbilities.Heal},
		{Iteams.Cookie,IteamsAbilities.IncreaseHp},
		{Iteams.Potion,IteamsAbilities.IncreaseHp},
		{Iteams.Pretzel,IteamsAbilities.IncreaseHp},
		{Iteams.Bread,IteamsAbilities.IncreaseDamage},
		{Iteams.Tea,IteamsAbilities.IncreaseDamage},
		{Iteams.Shrimp,IteamsAbilities.LevelUp},
	};

	public static string[] itemsNames;

	public static string[] itemsDescriptions;

	public static int[] itemValues = new int[] { 10, 15, 50, 75, 10, 25, 1, 1 };

	public static int[] maxIteamDrop = new int[] { 1, 1, 2, 4, 4, 4, 6, 6, 6, 6, 8, 8, 8, 8 };

	[Signal]
	public delegate void GoldUpdateEventHandler();

	public static Hero currentlySelectedHero = null;
	public static bool SpendGold(int gold)
	{
		if (gold <= Gold)
		{
			Gold -= gold;
			return true;
		}
		return false;
	}

	public static void GainGold(int gold)
	{
		Gold += gold;
	}

	public static void LoadDataFromFile(string filePath)
	{
		if (!Godot.FileAccess.FileExists(filePath))
		{
			GD.Print("File not found!");
			return;
		}

		var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);

		string[] lines = file.GetAsText().Split("\n");

		foreach (var line in lines)
		{
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
				continue;

			string[] parts = line.Split('=');

			if (parts.Length != 2)
				continue;

			string key = parts[0].Trim();
			string value = parts[1].Trim();

			switch (key)
			{
				case "Gold":
					Gold = int.Parse(value);
					break;

				case "Stage":
					Stage = int.Parse(value);
					break;

				case "FightState":
					FightState = bool.Parse(value);
					break;

				case "heroNames":
					heroNames = value.Split(',').ToArray();
					break;

				case "heroLevels":
					heroLevels = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "heroPrices":
					heroPrices = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "heroMaxHealth":
					heroMaxHealth = Parse2DArray(value);
					break;

				case "heroDamage":
					heroDamage = Parse3DIntArray(value);
					break;

				case "heroMaxCharge":
					heroMaxCharge = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "heroChargeRate":
					heroChargeRate = Parse3DIntArray(value);
					break;

				case "heroUnlockstage":
					heroUnlockstage = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "heroSellvalues":
					heroSellvalues = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "BackEnemyStages":
					BackEnemyStages = ParseStageEnemies(value);
					break;

				case "FrontEnemyStages":
					FrontEnemyStages = ParseStageEnemies(value);
					break;

				case "levelUpPrices":
					levelUpPrices = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "capacityPrices":
					capacityPrices = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "capacitiUpgrades":
					capacitiUpgrades = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "capacitiPrices":
					capacityPrices = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "enemyNames":
					enemyNames = value.Split(',').ToArray();
					break;

				case "enemyMaxHealth":
					enemyMaxHealth = Parse2DArray(value);
					break;

				case "enemyMaxCharge":
					enemyMaxCharge = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "enemyChargeRate":
					enemyChargeRate = Parse3DIntArray(value);
					break;

				case "enemyDamage":
					enemyDamage = Parse3DIntArray(value);
					break;

				case "enemyStageCapacitiesFront":
					enemyStageCapacitiesFront = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "enemyStageCapacitiesBack":
					enemyStageCapacitiesBack = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "itemsNames":
					GD.Print("OOOOOOOOO");
					itemsNames = value.Split(',').ToArray();
					break;

				case "itemsDescriptions":
					itemsDescriptions = value.Split(',').ToArray();
					break;

				case "itemValues":
					itemValues = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "maxIteamDrop":
					maxIteamDrop = value.Split(',').Select(int.Parse).ToArray();
					break;

				case "purchaseUpPrices":
					purchaseUpPrices = value.Split(',').Select(int.Parse).ToArray();
					break;


				default:
					GD.Print($"Unknown key: {key}");
					break;
			}
		}
	}

	private static int[][] Parse2DArray(string value)
	{
		string[] rows = value.Split(';');
		int[][] result = new int[rows.Length][];

		for (int i = 0; i < rows.Length; i++)
		{
			string[] columns = rows[i].Split(',');
			result[i] = Array.ConvertAll(columns, int.Parse);
		}

		return result;
	}

	private static int[][][] Parse3DIntArray(string value)
	{
		string[] outerArrays = value.Split('|');

		int[][][] result = new int[outerArrays.Length][][];

		for (int i = 0; i < outerArrays.Length; i++)
		{
			string[] innerArrays = outerArrays[i].Split(';');

			result[i] = new int[innerArrays.Length][];

			for (int j = 0; j < innerArrays.Length; j++)
			{
				result[i][j] = innerArrays[j].Split(',').Select(int.Parse).ToArray();
			}
		}

		return result;
}

	private static StageEnemy[][] ParseStageEnemies(string value)
	{
		string[] stages = value.Split('|');
		return stages.Select(stage =>
			stage.Split(';').Select(enemy =>
			{
				string[] parts = enemy.Split(',');
				EnemyTypes type = (EnemyTypes)Enum.Parse(typeof(EnemyTypes), parts[0]);
				int index = int.Parse(parts[1]);
				int level = int.Parse(parts[2]);
				return new StageEnemy(type, index, level);
			}).ToArray()).ToArray();
	}
}
