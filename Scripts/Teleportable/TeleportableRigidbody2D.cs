using Godot;

public abstract partial class TeleportableRigidbody2D : RigidBody2D, ITeleportable
{
  protected bool willTeleport = false;
  protected Vector2 teleportTarget = Vector2.Zero;

  public abstract void Teleport(Vector2 destination);
}