using Godot;
using WildJam2023.Scripts.Interfaces;

namespace WildJam2023.Scripts.Teleportable
{
  public abstract partial class TeleportableRigidbody2D : RigidBody2D, ITeleportable
  {
    protected bool WillTeleport { get; set; }
    protected Vector2 TeleportTarget { get; set; } = Vector2.Zero;

    public abstract void Teleport(Vector2 destination);
  }
}