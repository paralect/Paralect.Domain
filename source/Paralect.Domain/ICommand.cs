namespace Paralect.Domain
{
    /// <summary>
    /// Domain Command interface
    /// </summary>
    public partial interface ICommand
    {
        ICommandMetadata Metadata { get; set; }
    }
}