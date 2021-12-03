using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Persistent
{
    public interface IData<T>
    {
        public bool Save(T data);

        public bool Load(out T data);
    }

    public interface ILocation
    {
        public bool Write(byte profileId = 0, string subLocation = "");
        public bool Read(byte profileId = 0, string subLocation = "");
        public bool Delete(byte profileId = 0, string subLocation = "");
        public bool FileExists(byte profileId = 0, string subLocation = "");
    }
}
