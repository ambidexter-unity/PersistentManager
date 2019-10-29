using Common.PersistentManager;
using System;
using System.IO;
using System.Threading.Tasks;

public class PersistentManager : PersistentManagerBase
{
    public override string PersistentKey => "sample";

    protected override Func<string, Task<Stream>> getStreamAsync => throw new NotImplementedException();
}