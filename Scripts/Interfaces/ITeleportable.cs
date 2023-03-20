using Godot;

namespace WildJam2023.Scripts.Interfaces;

public interface ITeleportable
{
  void Teleport(Vector2 destination);
}