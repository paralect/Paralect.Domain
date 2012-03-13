namespace Paralect.Transitions
{
    public interface ISnapshotRepository
    {
        void Save(Snapshot ar, int minTransitionsForSnapshot = 30);

        Snapshot Load<T>(string id);
    }
}
