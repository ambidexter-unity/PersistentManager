using Common.PersistentManager;
using System;
using System.IO;
using System.Threading.Tasks;

public class PersistentManager : PersistentManagerBase
{
    public override string PersistentKey => "sample";

    public override Task<bool> Download<T>(T data)
    {
        throw new NotImplementedException();
    }
}