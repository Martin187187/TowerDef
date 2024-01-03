public class TestEffect : AbstractStatsEffect<Test>
{
    public override float getAdditionValue(Test effector)
    {
        return effector.value *2;
    }

    public override float getMultiplicationValue(Test effector)
    {
        return 1;
    }
}