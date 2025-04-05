
/// <summary>
/// 可放置骰子的格子接口
/// </summary>
public interface IDiceContainer
{
    Dice GetAttachedDice();
    bool CanPlaceDice(Dice dice);
    void PlaceDice(Dice dice);
    Dice RemoveDice();
}