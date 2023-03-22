using Godot;

namespace WildJam2023.Scripts;

public partial class MainMenu : CanvasLayer
{
	[Export] private PackedScene level;
	
	private void OnStartGamePressed()
	{
		GD.Print("Button Pressed!");
		GetTree().ChangeSceneToPacked(level);
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}