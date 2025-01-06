using Godot;
using System;

public partial class Meniu : Control
{
	public Button  loadButton;
	public void StartPressed()
	{
		Global.LoadDataFromFile("res://Files/Load.txt");
		Global.SaveFile = false;
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	public void ExitPressed()
	{
		GetTree().Quit();
	}
	public void LoadPressed()
	{
		Global.LoadDataFromFile("res://Files/Load.txt");
		Global.SaveFile = true;
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	public override void _Ready()
	{
		loadButton = GetNode<Button>("VBoxContainer/Load");
		if (!Godot.FileAccess.FileExists("user://Save.txt"))
		{
			loadButton.Visible = false;
		}

	}
}
