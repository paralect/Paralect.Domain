using System;

namespace Paralect.Transitions
{
    public interface IDataTypeRegistry
    {
        Type GetType(String typeId);
        String GetTypeId(Type type);
    }
}