namespace Domain.Core.CqsModule.Command
{
    public interface ISecuredCommand
    {
        string PermisoRequerido { get; }
    }
}