using Godot;
using WildJam2023.Scripts.Interfaces;

namespace WildJam2023.Scripts.Teleportable;

public abstract partial class TeleportableCharacterBody2D : CharacterBody2D, ITeleportable
{
  protected bool willTeleport = false;
  public abstract void Teleport(Vector2 destination);
}