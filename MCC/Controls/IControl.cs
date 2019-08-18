namespace isa.MCC.Controls
{
    public interface IControl<S>
    {
        string Name { get; }
        string Description { get; }
        ControlInformation Information { get; }

        S Evaluate();
    }
}
