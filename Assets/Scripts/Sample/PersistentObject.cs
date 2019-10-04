using System;
using System.Collections;
using System.Collections.Generic;
using Common.PersistentManager;
using UnityEngine;

namespace Sample
{
    [Serializable]
    public class PersistentObject: IPersistent<PersistentObject>

    {
        [SerializeField] public string Data;

        public string PersistentId => "id";

        public void Restore<T1>(T1 data) where T1 : IPersistent<PersistentObject>
        {
            PersistentObject ob  = data as PersistentObject;

            Data = ob.Data;
        }
    }
}