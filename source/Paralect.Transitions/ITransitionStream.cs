namespace Paralect.Transitions
{
    public interface ITransitionStream
    {
        void AddEvent(TransitionEvent evnt);
        void Commit();
        void Rollback();
    }
}