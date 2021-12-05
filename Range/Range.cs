using UnityEngine;
using System.Collections;

// Author: Pierre CAMILLI

[System.Serializable]
public struct Range {

    #region Getter Setter
    [SerializeField]
    private float m_min;
    /// <summary>
    /// Minimal value
    /// </summary>
    public float Min { get { return m_min; } }

    [SerializeField]
    private float m_max;
    /// <summary>
    /// Maximal value
    /// </summary>
    public float Max { get { return m_max; } }
    
    /// <summary>
    /// Set min and max values
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>True if max parameter is greater or equal than min parameter</returns>
    public bool Set(float min, float max)
    {
        if (min <= max)
        {
            m_min = min;
            m_max = max;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Difference between max and min value
    /// </summary>
    public float Length { get { return m_max - m_min; } }
    #endregion

    /// <summary>
    /// One dimensional bounds
    /// </summary>
    /// <param name="min">Minimal value of the bounds</param>
    /// <param name="max">Maximal value of the bounds</param>
    Range(float min = 0.0f, float max = 0.0f)
    {
        if (min <= max)
        {
            m_min = min;
            m_max = max;
        }
        else
        {
            m_max = min;
            m_min = min;
        }
    }

    #region Operators

    public static Range operator -(Range bounds)
    {
        return new Range(-bounds.m_max, -bounds.m_min);
    }

    public static Range operator +(Range bounds, float value)
    {
        return new Range(bounds.m_min + value, bounds.m_max + value);
    }

    public static Range operator -(Range bounds, float value)
    {
        return bounds + (-value);
    }

    public static Range operator +(Range bounds1, Range bounds2)
    {
        return new Range(bounds1.m_min + bounds2.m_min, bounds1.m_max + bounds2.m_max);
    }

    public static Range operator -(Range bounds1, Range bounds2)
    {
        return bounds1 + (-bounds2);
    }

    #endregion

    #region Methods
    /// <summary>
    /// Clamp a value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Clamp(float value)
    {
        return Mathf.Clamp(value, m_min, m_max);
    }

    /// <summary>
    /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float InverseLerp(float value)
    {
        return Mathf.InverseLerp(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Lerp(float value)
    {
        return Mathf.Lerp(m_min, m_max, value);
    }

    /// <summary>
    /// Same as Lerp but makes sure the values interpolate correctly when they wrap around
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float LerpAngle(float value)
    {
        return Mathf.LerpAngle(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float LerpUnclamped(float value)
    {
        return Mathf.LerpUnclamped(m_min, m_max, value);
    }

    /// <summary>
    /// Increase the value of both bounds by the specificated value
    /// </summary>
    /// <param name="value"></param>
    public void Translate(float value)
    {
        m_min += value;
        m_max += value;
    }
    #endregion
}
