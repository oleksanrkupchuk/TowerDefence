public class Minotaur : Enemy
{
    private new void Start()
    {
        base.Start();
        _health = 5;
        ShiftHealthBar();
    }

    private new void Update()
    {
        base.Update();
    }
}
