public class ComplexityIncreaser
{
    private float _complexityCoefficient;

    public ComplexityIncreaser(float complexityCoefficient)
    {
        _complexityCoefficient = complexityCoefficient;
    }

    public float UpdateComplexity()
    {
        return 1 * _complexityCoefficient;
    }
}