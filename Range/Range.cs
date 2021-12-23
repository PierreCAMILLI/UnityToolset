using UnityEngine;
using System.Collections;

// Author: Pierre CAMILLI

public interface IRange<T> where T : System.IComparable<T>
{
    public T Min { get; }
    public T Max { get; }
    public bool Set(T min, T max);
    public T Length { get; }

    public T Clamp(T value);
    public T InverseLerp(T value);
    public T Lerp(T value);
    public T LerpAngle(T value);
    public T LerpUnclamped(T value);
    public void Translate(T value);

    public string ToString();
}

[System.Serializable]
public struct Range : IRange<float>
{
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
        if (min.CompareTo(max) >= 0)
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
    public Range(float min, float max)
    {
        if (min.CompareTo(max) >= 0)
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

    public override string ToString()
    {
        return string.Concat("(", m_min, ", ", m_max, ")");
    }
    #endregion
}

[System.Serializable]
public struct RangeInt : IRange<int>
{

    #region Getter Setter
    [SerializeField]
    private int m_min;
    /// <summary>
    /// Minimal value
    /// </summary>
    public int Min { get { return m_min; } }

    [SerializeField]
    private int m_max;
    /// <summary>
    /// Maximal value
    /// </summary>
    public int Max { get { return m_max; } }

    /// <summary>
    /// Set min and max values
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>True if max parameter is greater or equal than min parameter</returns>
    public bool Set(int min, int max)
    {
        if (min.CompareTo(max) >= 0)
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
    public int Length { get { return m_max - m_min; } }
    #endregion

    /// <summary>
    /// One dimensional bounds
    /// </summary>
    /// <param name="min">Minimal value of the bounds</param>
    /// <param name="max">Maximal value of the bounds</param>
    public RangeInt(int min, int max)
    {
        if (min.CompareTo(max) >= 0)
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

    public static RangeInt operator -(RangeInt bounds)
    {
        return new RangeInt(-bounds.m_max, -bounds.m_min);
    }

    public static RangeInt operator +(RangeInt bounds, int value)
    {
        return new RangeInt(bounds.m_min + value, bounds.m_max + value);
    }

    public static RangeInt operator -(RangeInt bounds, int value)
    {
        return bounds + (-value);
    }

    public static RangeInt operator +(RangeInt bounds1, RangeInt bounds2)
    {
        return new RangeInt(bounds1.m_min + bounds2.m_min, bounds1.m_max + bounds2.m_max);
    }

    public static RangeInt operator -(RangeInt bounds1, RangeInt bounds2)
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
    public int Clamp(int value)
    {
        return Mathf.Clamp(value, m_min, m_max);
    }

    /// <summary>
    /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int InverseLerp(int value)
    {
        return (int)Mathf.InverseLerp(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int Lerp(int value)
    {
        return (int)Mathf.Lerp(m_min, m_max, value);
    }

    /// <summary>
    /// Same as Lerp but makes sure the values interpolate correctly when they wrap around
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int LerpAngle(int value)
    {
        return (int)Mathf.LerpAngle(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int LerpUnclamped(int value)
    {
        return (int)Mathf.LerpUnclamped(m_min, m_max, value);
    }

    /// <summary>
    /// Increase the value of both bounds by the specificated value
    /// </summary>
    /// <param name="value"></param>
    public void Translate(int value)
    {
        m_min += value;
        m_max += value;
    }

    public override string ToString()
    {
        return string.Concat("(", m_min, ", ", m_max, ")");
    }
    #endregion
}

[System.Serializable]
public struct RangeByte : IRange<byte>
{

    #region Getter Setter
    [SerializeField]
    private byte m_min;
    /// <summary>
    /// Minimal value
    /// </summary>
    public byte Min { get { return m_min; } }

    [SerializeField]
    private byte m_max;
    /// <summary>
    /// Maximal value
    /// </summary>
    public byte Max { get { return m_max; } }

    /// <summary>
    /// Set min and max values
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>True if max parameter is greater or equal than min parameter</returns>
    public bool Set(byte min, byte max)
    {
        if (min.CompareTo(max) >= 0)
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
    public byte Length => (byte)(m_max - m_min);
    #endregion

    /// <summary>
    /// One dimensional bounds
    /// </summary>
    /// <param name="min">Minimal value of the bounds</param>
    /// <param name="max">Maximal value of the bounds</param>
    public RangeByte(byte min, byte max)
    {
        if (min.CompareTo(max) >= 0)
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
    public static RangeByte operator +(RangeByte bounds, byte value)
    {
        return new RangeByte((byte)(bounds.m_min + value), (byte)(bounds.m_max + value));
    }

    public static RangeByte operator -(RangeByte bounds, byte value)
    {
        return new RangeByte((byte)(bounds.m_min - value), (byte)(bounds.m_max - value));
    }

    public static RangeByte operator +(RangeByte bounds1, RangeByte bounds2)
    {
        return new RangeByte((byte)(bounds1.m_min + bounds2.m_min), (byte)(bounds1.m_max + bounds2.m_max));
    }

    public static RangeByte operator -(RangeByte bounds1, RangeByte bounds2)
    {
        return new RangeByte((byte)(bounds1.m_min - bounds2.m_min), (byte)(bounds1.m_max - bounds2.m_max));
    }

    #endregion

    #region Methods
    /// <summary>
    /// Clamp a value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte Clamp(byte value)
    {
        return (byte)Mathf.Clamp(value, m_min, m_max);
    }

    /// <summary>
    /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte InverseLerp(byte value)
    {
        return (byte)Mathf.InverseLerp(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte Lerp(byte value)
    {
        return (byte)Mathf.Lerp(m_min, m_max, value);
    }

    /// <summary>
    /// Same as Lerp but makes sure the values interpolate correctly when they wrap around
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte LerpAngle(byte value)
    {
        return (byte)Mathf.LerpAngle(m_min, m_max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public byte LerpUnclamped(byte value)
    {
        return (byte)Mathf.LerpUnclamped(m_min, m_max, value);
    }

    /// <summary>
    /// Increase the value of both bounds by the specificated value
    /// </summary>
    /// <param name="value"></param>
    public void Translate(byte value)
    {
        m_min += value;
        m_max += value;
    }

    public override string ToString()
    {
        return string.Concat("(", m_min, ", ", m_max, ")");
    }
    #endregion
}
