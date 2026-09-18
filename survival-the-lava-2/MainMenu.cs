using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _startButton;
	private Button _quitButton;

	public override void _Ready()
	{
		_startButton = GetNode<Button>("StartButton");
		_startButton.Pressed += OnStartButtonPressed;
		
		_quitButton = GetNode<Button>("QuitButton");
		_quitButton.Pressed += OnQuitButtonPressed;
	}

	private void OnStartButtonPressed()
	{
		string gameScenePath = "res://Test.tscn"; 

		GetTree().ChangeSceneToFile(gameScenePath);
	}
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
