using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
namespace Toolset
{
    [System.Serializable]
    public abstract class DebugSetting : System.ICloneable
    {
        private static DebugSetting _current;
        /// <summary>
        /// Returns current Debug Settings used for debugging
        /// </summary>
        /// <remarks>Use <b>#if UNITY_EDITOR</b> directive when using this variable</remarks>
        public static DebugSetting Current
        {
            get
            {
                if (_current == null)
                {
                    DebugSettingsWindow window = DebugSettingsWindow.GetWindow<DebugSettingsWindow>();
                    _current = window.SelectedSetting;

                }
                return _current;
            }
            set => _current = value;
        }

        public static bool IsSelected()
        {
            if (_current == null)
            {
                DebugSettingsWindow window = DebugSettingsWindow.GetWindow<DebugSettingsWindow>();
                _current = window.SelectedSetting;
            }
            return _current != null;
        }

        public bool IsImplementing<T>() where T : DebugSetting
        {
            return this is T;
        }

        public T Cast<T>() where T : DebugSetting
        {
            return this as T;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
#endif
