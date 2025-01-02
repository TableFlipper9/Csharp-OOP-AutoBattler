using Godot;
using System;

public partial class HealthBar : MarginContainer
{
	public TextureProgressBar healthB;
	public TextureProgressBar chargeB;

	public int health;
	public int charge;
	public override void _Ready()
	{
		healthB = GetNode<TextureProgressBar>("VBoxContainer/HBoxContainer/Health");
		chargeB = GetNode<TextureProgressBar>("VBoxContainer/HBoxContainer/Charge");
	}

	public void ChangeMax(int MH, int MC)
	{
		healthB.MaxValue = MH;
		chargeB.MaxValue = MC;
	}

	public void ChangeValue(int H, int C)
	{
		health = H;
		charge = C;
	}

	private float Lerp(float firstFloat, float secondFloat, float by)
	{
		return firstFloat * (1 - by) + secondFloat * by;
	}

	public override void _Process(double delta)
	{
		healthB.Value = Lerp((float)healthB.Value, health,(float) 0.1);
		chargeB.Value = Lerp((float)chargeB.Value, charge, (float)0.1);
	}
}
