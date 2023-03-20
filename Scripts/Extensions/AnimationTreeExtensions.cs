using Godot;

namespace WildJam2023.Scripts.Extensions;

public static class AnimationTreeExtensions
{
    public static AnimationNodeStateMachinePlayback GetStateMachinePlayback(this AnimationTree tree)
    {
        return (AnimationNodeStateMachinePlayback) tree.Get("parameters/playback");
    }
}