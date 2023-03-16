using Godot;

public partial class Portal : Area2D
{
  [Export] private Portal destination;

  public bool canSend = true;

  private void OnBodyEntered(Node2D body)
  {
    if (body is ITeleportable teleportable && canSend)
    {
      teleportable.Teleport(destination.Position);
      destination.canSend = false;
    }
  }

  private void OnBodyExited(Node2D body)
  {
    if (body is ITeleportable && !canSend)
    {
      GD.Print("Exited: " + Name);
      canSend = true;
    }
  }
}
