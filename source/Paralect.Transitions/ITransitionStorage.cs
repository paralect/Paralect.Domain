using System;

namespace Paralect.Transitions
{
    public interface ITransitionStorage
    {
        ITransitionStream OpenStream(String streamId, Int32 fromVersion, Int32 toVersion);
        ITransitionStream CreateStream(String streamId);
    }
}
