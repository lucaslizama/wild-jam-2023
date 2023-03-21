using Godot;

public partial class PlayerCamera : Camera2D
{
  [Export] private TileMap tileMap;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
	var mapRect = tileMap.GetUsedRect();
	var tileSize = tileMap.CellQuadrantSize;
	var worldSizeInPixels = mapRect.Size * tileSize;
	LimitRight = worldSizeInPixels.X;
	LimitBottom = worldSizeInPixels.Y;
  }
}
