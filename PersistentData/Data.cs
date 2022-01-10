using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    namespace Persistent
    {
        public abstract class Location : ScriptableObject, ILocation
        {
            [SerializeField]
            private string _fileName;
            public string FileName { get => _fileName; }
            [SerializeField]
            private string _fileExtension;
            public string FileExtension { get => _fileExtension; }
            [SerializeField]
            private string _path;
            public string Path { get => _path; }
            public string FullFilename { get => _fileName + '.' + _fileExtension; }
            public string DefaultFullPath { get => GetFullPath(0); }

            protected bool IsPathValid()
            {
                return _fileName.Length > 0 && _fileExtension.Length > 0;
            }

            public string GetPath(byte profileId, string subLocation)
            {
                return Application.persistentDataPath
                    + (subLocation.Length > 0 ? '/' + _path : "")
                    + (_path.Length > 0 ? '/' + _path : "")
                    + '/' + profileId.ToString().PadLeft(3, '0');
            }

            public string GetFullPath(byte profileId, string subLocation = "")
            {
                return GetPath(profileId, subLocation) + '/' + FullFilename;
            }

            public abstract bool Write(byte profileId = 0, string subLocation = "");
            public abstract bool Read(byte profileId = 0, string subLocation = "");
            public abstract bool Delete(byte profileId = 0, string subLocation = "");
            public abstract bool FileExists(byte profileId = 0, string subLocation = "");
        }

        public class Data<T> : Location, IData<T>
        {
            protected T data;

            public override bool Write(byte profileId = 0, string subLocation = "")
            {
                if (IsPathValid())
                {
                    string json = JsonUtility.ToJson(data);
                    System.IO.Directory.CreateDirectory(GetPath(profileId, subLocation));
                    System.IO.File.WriteAllText(GetFullPath(profileId, subLocation), json);
                    return true;
                }
                return false;
            }

            public override bool Read(byte profileId = 0, string subLocation = "")
            {
                if (FileExists(profileId))
                {
                    string json = System.IO.File.ReadAllText(GetFullPath(profileId, subLocation));
                    data = JsonUtility.FromJson<T>(json);
                    return true;
                }
                return false;
            }

            public override bool Delete(byte profileId, string subLocation = "")
            {
                if (FileExists(profileId))
                {
                    System.IO.File.Delete(GetFullPath(profileId, subLocation));
                    return true;
                }
                return false;
            }

            public int Delete()
            {
                byte count = 0;
                for (byte i = 0; i < byte.MaxValue; ++i)
                {
                    if (Delete(i))
                    {
                        ++count;
                    }
                }
                return count;
            }

            public override bool FileExists(byte profileId = 0, string subLocation = "")
            {
                return IsPathValid() && System.IO.Directory.Exists(GetPath(profileId, subLocation)) && System.IO.File.Exists(GetFullPath(profileId, subLocation));
            }

            public bool Save(T data)
            {
                this.data = data;
                return true;
            }

            public bool Load(out T data)
            {
                if (this.data != null)
                {
                    data = this.data;
                    return true;
                }
                data = default(T);
                return false;
            }
        }
    }
}
