using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolset
{
    namespace Persistent
    {
        public class LocationWrapper : ScriptableObject, ILocation
        {
            [SerializeField]
            ILocation[] _saveLocations;

            public bool Delete(byte profileId = 0, string subLocation = "")
            {
                return _saveLocations.Count(sl => !sl.Delete(profileId, subLocation)) == 0;
            }

            public bool FileExists(byte profileId = 0, string subLocation = "")
            {
                return _saveLocations.Count(sl => !sl.FileExists(profileId, subLocation)) == 0;
            }

            public bool Read(byte profileId = 0, string subLocation = "")
            {
                return _saveLocations.Count(sl => !sl.Read(profileId, subLocation)) == 0;
            }

            public bool Write(byte profileId = 0, string subLocation = "")
            {
                return _saveLocations.Count(sl => !sl.Write(profileId, subLocation)) == 0;
            }
        }
    }
}
