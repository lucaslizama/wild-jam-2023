using Godot;

namespace WildJam2023.Scripts.Extensions;

public static class NodeExtensions
{
  public static Node GetRootNode(this Node node)
  {
    Node world = node.GetParent();

    while (world.Owner != null)
    {
      world = world.GetParent();
    }

    return world;
  }
}