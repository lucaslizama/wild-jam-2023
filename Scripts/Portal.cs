using Godot;

public partial class Portal : Area2D
{
  [Export] private Portal destination;

  public bool canSend = true;

  private void OnBodyEntered(Node2D body)
  {
    if (body is Bomb bomb && canSend)
    {
      GD.Print("Entered: " + Name);

      destination.canSend = false;
      bomb.teleportTarget = destination.Position;
      bomb.willTeleport = true;
    }
  }

  private void OnBodyExited(Node2D body)
  {
    if (body is Bomb bomb && !canSend)
    {
      GD.Print("Exited: " + Name);
      canSend = true;
    }
  }
}
