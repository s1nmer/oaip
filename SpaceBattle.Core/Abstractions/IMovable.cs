using System.Numerics;

namespace SpaceBattle.Core.Abstractions;

public interface IMovable
{
    Vector2 Position { get; set; }

    Vector2 Velocity { get; }
}
