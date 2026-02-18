namespace SpaceBattle.Core.Abstractions;

public interface IFireable
{
    int MissileCount { get; set; }

    void Fire();
}
