namespace SpaceBattle.Core.Abstractions;

public interface IRotatable
{
    float OrientationDegrees { get; set; }

    float AngularVelocityDegrees { get; }
}
