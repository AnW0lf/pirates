using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BigDigit
{
    public double mantissa = 1f;
    public long exponent = 0;

    public double Mantissa { get { return mantissa; } private set { mantissa = value; } }
    public long Exponent { get { return exponent; } private set { exponent = value; } }
    public static readonly BigDigit zero = new BigDigit(0d, 0);

    private readonly string[] postfix = { "K", "M", "B", "T", "q", "Q", "s", "S" };

    public BigDigit()
    {
        Mantissa = .0f;
        Exponent = 0;
    }

    public BigDigit(double mantissa)
    {
        Mantissa = mantissa;
        Exponent = 0;
        Normalize();
    }

    public BigDigit(double mantissa, long exponenta)
    {
        Mantissa = mantissa;
        Exponent = exponenta;
        Normalize();
    }

    public BigDigit(BigDigit other)
    {
        Mantissa = other.Mantissa;
        Exponent = other.Exponent;
        Normalize();
    }

    public static BigDigit operator +(BigDigit first, BigDigit second)
    {
        double lesserMantissa, largerMantissa;
        long lesserExponent, largerExponent;

        if (first.Exponent > second.Exponent)
        {
            largerMantissa = first.Mantissa;
            largerExponent = first.Exponent;
            lesserMantissa = second.Mantissa;
            lesserExponent = second.Exponent;
        }
        else if (first.Exponent < second.Exponent)
        {
            largerMantissa = second.Mantissa;
            largerExponent = second.Exponent;
            lesserMantissa = first.Mantissa;
            lesserExponent = first.Exponent;
        }
        else
        {
            return new BigDigit(first.Mantissa + second.Mantissa, first.Exponent);
        }

        while (largerExponent > lesserExponent)
        {
            lesserExponent++;
            lesserMantissa /= 10d;
        }

        return new BigDigit(largerMantissa + lesserMantissa, largerExponent);
    }

    public static BigDigit operator -(BigDigit first, BigDigit second)
    {
        double lesserMantissa, largerMantissa;
        long lesserExponent, largerExponent;

        if (first.Exponent > second.Exponent)
        {
            largerMantissa = first.Mantissa;
            largerExponent = first.Exponent;
            lesserMantissa = second.Mantissa;
            lesserExponent = second.Exponent;
        }
        else if (first.Exponent < second.Exponent)
        {
            largerMantissa = second.Mantissa;
            largerExponent = second.Exponent;
            lesserMantissa = first.Mantissa;
            lesserExponent = first.Exponent;
        }
        else
        {
            return new BigDigit(first.Mantissa - second.Mantissa, first.Exponent);
        }

        while (largerExponent > lesserExponent)
        {
            lesserExponent++;
            lesserMantissa /= 10d;
        }

        return new BigDigit(largerMantissa - lesserMantissa, largerExponent);
    }

    public static bool operator >(BigDigit first, BigDigit second)
    {
        if (first.Mantissa > 0d && second.Mantissa <= 0d
            || first.Mantissa >= 0d && second.Mantissa < 0d) return true;
        else if (first.Mantissa < 0d && second.Mantissa >= 0d
            || first.Mantissa <= 0d && second.Mantissa > 0d) return false;
        else if (first == second) return false;
        else if (first.Exponent > second.Exponent) return true;
        else if (first.Exponent == second.Exponent)
        {
            if (first.Mantissa > second.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public static bool operator >(BigDigit other, int val)
    {
        return other > new BigDigit(val);
    }

    public static bool operator >(BigDigit other, float val)
    {
        return other > new BigDigit(val);
    }

    public static bool operator >(BigDigit other, long val)
    {
        return other > new BigDigit(val);
    }

    public static bool operator >(BigDigit other, double val)
    {
        return other > new BigDigit(val);
    }

    public static bool operator >(int val, BigDigit other)
    {
        return new BigDigit(val) > other;
    }

    public static bool operator >(float val, BigDigit other)
    {
        return new BigDigit(val) > other;
    }

    public static bool operator >(long val, BigDigit other)
    {
        return new BigDigit(val) > other;
    }

    public static bool operator >(double val, BigDigit other)
    {
        return new BigDigit(val) > other;
    }

    public static bool operator <(BigDigit first, BigDigit second)
    {
        if (first.Mantissa > 0d && second.Mantissa <= 0d
            || first.Mantissa >= 0d && second.Mantissa < 0d) return false;
        else if (first.Mantissa < 0d && second.Mantissa >= 0d
            || first.Mantissa <= 0d && second.Mantissa > 0d) return true;
        else if (first == second) return false;
        else if (first.Exponent < second.Exponent) return true;
        else if (first.Exponent == second.Exponent)
        {
            if (first.Mantissa < second.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public static bool operator <(BigDigit other, int val)
    {
        return other < new BigDigit(val);
    }

    public static bool operator <(BigDigit other, float val)
    {
        return other < new BigDigit(val);
    }

    public static bool operator <(BigDigit other, long val)
    {
        return other < new BigDigit(val);
    }

    public static bool operator <(BigDigit other, double val)
    {
        return other < new BigDigit(val);
    }

    public static bool operator <(int val, BigDigit other)
    {
        return new BigDigit(val) < other;
    }

    public static bool operator <(float val, BigDigit other)
    {
        return new BigDigit(val) < other;
    }

    public static bool operator <(long val, BigDigit other)
    {
        return new BigDigit(val) < other;
    }

    public static bool operator <(double val, BigDigit other)
    {
        return new BigDigit(val) < other;
    }

    public static bool operator >=(BigDigit first, BigDigit second)
    {
        if (first.Mantissa >= 0d && second.Mantissa <= 0d
            || first.Mantissa >= 0d && second.Mantissa <= 0d) return true;
        else if (first.Mantissa <= 0d && second.Mantissa >= 0d
            || first.Mantissa <= 0d && second.Mantissa >= 0d) return false;
        else if (first.Exponent > second.Exponent) return true;
        else if (first.Exponent == second.Exponent)
        {
            if (first.Mantissa >= second.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public static bool operator >=(BigDigit other, int val)
    {
        return other >= new BigDigit(val);
    }

    public static bool operator >=(BigDigit other, float val)
    {
        return other >= new BigDigit(val);
    }

    public static bool operator >=(BigDigit other, long val)
    {
        return other >= new BigDigit(val);
    }

    public static bool operator >=(BigDigit other, double val)
    {
        return other >= new BigDigit(val);
    }

    public static bool operator >=(int val, BigDigit other)
    {
        return new BigDigit(val) >= other;
    }

    public static bool operator >=(float val, BigDigit other)
    {
        return new BigDigit(val) >= other;
    }

    public static bool operator >=(long val, BigDigit other)
    {
        return new BigDigit(val) >= other;
    }

    public static bool operator >=(double val, BigDigit other)
    {
        return new BigDigit(val) >= other;
    }

    public static bool operator <=(BigDigit first, BigDigit second)
    {
        if (first.Mantissa > 0d && second.Mantissa <= 0d
            || first.Mantissa >= 0d && second.Mantissa < 0d) return false;
        else if (first.Mantissa < 0d && second.Mantissa >= 0d
            || first.Mantissa <= 0d && second.Mantissa > 0d) return true;
        else if (first.Exponent < second.Exponent) return true;
        else if (first.Exponent == second.Exponent)
        {
            if (first.Mantissa <= second.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public static bool operator <=(BigDigit other, int val)
    {
        return other <= new BigDigit(val);
    }

    public static bool operator <=(BigDigit other, float val)
    {
        return other <= new BigDigit(val);
    }

    public static bool operator <=(BigDigit other, long val)
    {
        return other <= new BigDigit(val);
    }

    public static bool operator <=(BigDigit other, double val)
    {
        return other <= new BigDigit(val);
    }

    public static bool operator <=(int val, BigDigit other)
    {
        return new BigDigit(val) <= other;
    }

    public static bool operator <=(float val, BigDigit other)
    {
        return new BigDigit(val) <= other;
    }

    public static bool operator <=(long val, BigDigit other)
    {
        return new BigDigit(val) <= other;
    }

    public static bool operator <=(double val, BigDigit other)
    {
        return new BigDigit(val) <= other;
    }

    public static bool operator ==(BigDigit first, BigDigit second)
    {
        return first.Equals(second);
    }

    public static bool operator ==(BigDigit other, int val)
    {
        return other == new BigDigit(val);
    }

    public static bool operator ==(BigDigit other, float val)
    {
        return other == new BigDigit(val);
    }

    public static bool operator ==(BigDigit other, long val)
    {
        return other == new BigDigit(val);
    }

    public static bool operator ==(BigDigit other, double val)
    {
        return other == new BigDigit(val);
    }

    public static bool operator ==(int val, BigDigit other)
    {
        return new BigDigit(val) == other;
    }

    public static bool operator ==(float val, BigDigit other)
    {
        return new BigDigit(val) == other;
    }

    public static bool operator ==(long val, BigDigit other)
    {
        return new BigDigit(val) == other;
    }

    public static bool operator ==(double val, BigDigit other)
    {
        return new BigDigit(val) == other;
    }

    public static bool operator !=(BigDigit first, BigDigit second)
    {
        return !first.Equals(second);
    }

    public static bool operator !=(BigDigit other, int val)
    {
        return other != new BigDigit(val);
    }

    public static bool operator !=(BigDigit other, float val)
    {
        return other != new BigDigit(val);
    }

    public static bool operator !=(BigDigit other, long val)
    {
        return other != new BigDigit(val);
    }

    public static bool operator !=(BigDigit other, double val)
    {
        return other != new BigDigit(val);
    }

    public static bool operator !=(int val, BigDigit other)
    {
        return new BigDigit(val) != other;
    }

    public static bool operator !=(float val, BigDigit other)
    {
        return new BigDigit(val) != other;
    }

    public static bool operator !=(long val, BigDigit other)
    {
        return new BigDigit(val) != other;
    }

    public static bool operator !=(double val, BigDigit other)
    {
        return new BigDigit(val) != other;
    }

    public static BigDigit operator *(BigDigit first, BigDigit second)
    {
        return new BigDigit(first.Mantissa * second.Mantissa, first.Exponent + second.Exponent);
    }

    public static BigDigit operator *(BigDigit other, int val)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(BigDigit other, float val)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(BigDigit other, long val)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(BigDigit other, double val)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(int val, BigDigit other)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(float val, BigDigit other)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(long val, BigDigit other)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator *(double val, BigDigit other)
    {
        return new BigDigit(other.Mantissa * val, other.Exponent);
    }

    public static BigDigit operator /(BigDigit first, BigDigit second)
    {
        if (first.EqualsZero) return zero;
        return new BigDigit(first.Mantissa / second.Mantissa, first.Exponent - second.Exponent);
    }

    public static BigDigit operator /(BigDigit other, int val)
    {
        return other / new BigDigit(val);
    }

    public static BigDigit operator /(BigDigit other, float val)
    {
        return other / new BigDigit(val);
    }

    public static BigDigit operator /(BigDigit other, long val)
    {
        return other / new BigDigit(val);
    }

    public static BigDigit operator /(BigDigit other, double val)
    {
        return other / new BigDigit(val);
    }

    public static BigDigit operator /(int val, BigDigit other)
    {
        return new BigDigit(val) / other;
    }

    public static BigDigit operator /(float val, BigDigit other)
    {
        return new BigDigit(val) / other;
    }

    public static BigDigit operator /(long val, BigDigit other)
    {
        return new BigDigit(val) / other;
    }

    public static BigDigit operator /(double val, BigDigit other)
    {
        return new BigDigit(val) / other;
    }

    public static BigDigit operator -(BigDigit other)
    {
        return new BigDigit(-other.Mantissa, other.Exponent);
    }

    public void Sum(BigDigit other)
    {
        double lesserMantissa, largerMantissa;
        long lesserExponent, largerExponent;
        if (Exponent > other.Exponent)
        {
            largerMantissa = Mantissa;
            largerExponent = Exponent;
            lesserMantissa = other.Mantissa;
            lesserExponent = other.Exponent;
        }
        else if (Exponent < other.Exponent)
        {
            largerMantissa = other.Mantissa;
            largerExponent = other.Exponent;
            lesserMantissa = Mantissa;
            lesserExponent = Exponent;
        }
        else
        {
            Mantissa += other.Mantissa;
            Normalize();
            return;
        }

        while (largerExponent > lesserExponent)
        {
            lesserExponent++;
            lesserMantissa /= 10d;
        }

        Mantissa = largerMantissa + lesserMantissa;
        Exponent = largerExponent;
        Normalize();
    }

    public static BigDigit Sum(BigDigit first, BigDigit second)
    {
        double lesserMantissa, largerMantissa;
        long lesserExponent, largerExponent;
        if (first.Exponent > second.Exponent)
        {
            largerMantissa = first.Mantissa;
            largerExponent = first.Exponent;
            lesserMantissa = second.Mantissa;
            lesserExponent = second.Exponent;
        }
        else if (first.Exponent < second.Exponent)
        {
            largerMantissa = second.Mantissa;
            largerExponent = second.Exponent;
            lesserMantissa = first.Mantissa;
            lesserExponent = first.Exponent;
        }
        else
        {
            return new BigDigit(first.Mantissa + second.Mantissa, first.Exponent);
        }

        while (largerExponent > lesserExponent)
        {
            lesserExponent++;
            lesserMantissa /= 10d;
        }

        return new BigDigit(largerMantissa + lesserMantissa, largerExponent);
    }

    public void Multiply(double v)
    {
        Mantissa *= v;
        Normalize();
    }

    public static BigDigit Multiply(BigDigit other, double v)
    {
        return new BigDigit(other.Mantissa * v, other.Exponent);
    }

    public void Reverse()
    {
        Mantissa = -Mantissa;
        Normalize();
    }

    public static BigDigit Reverse(BigDigit other)
    {
        return new BigDigit(-other.Mantissa, other.Exponent);
    }

    public bool EqualsZero { get { return Mantissa.Equals(0d) && Exponent.Equals(0); } }

    public bool LessThen(BigDigit other)
    {
        if (Exponent < other.Exponent) return true;
        else if (Exponent == other.Exponent)
        {
            if (Mantissa < other.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public bool LargeThen(BigDigit other)
    {
        if (Exponent > other.Exponent) return true;
        else if (Exponent == other.Exponent)
        {
            if (Mantissa > other.Mantissa)
                return true;
            else return false;
        }
        return false;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType().Equals(this.GetType()))
        {
            BigDigit other = (BigDigit)obj;
            if (other.Mantissa.Equals(Mantissa) && other.Exponent.Equals(Exponent))
                return true;
            else return false;
        }
        else return false;
    }

    public override string ToString()
    {
        if (Exponent <= 3)
        {
            int v = (int)(Mantissa * Math.Pow(10d, Exponent));
            return v.ToString();
        }
        else
        {
            string str = (Mantissa * Math.Pow(10d, Exponent % 3)).ToString();
            str = str.Length >= 5 ? str.Substring(0, 5) : str;
            str = str.Replace(',', '.');
            for (int i = str.Length - 1; i < 4; i++)
            {
                if (i == Exponent % 3)
                    str += ".";
                else str += "0";
            }
            if (postfix.Length > (Exponent / 3) - 1)
                return str + postfix[(Exponent / 3) - 1];
            else return str + "?";
        }
    }

    private void Normalize()
    {
        if (Mantissa.Equals(0d))
        {
            Exponent = 0;
        }
        else if (Math.Abs(Mantissa) >= 10d)
        {
            while (Math.Abs(Mantissa) >= 10d)
            {
                Mantissa /= 10d;
                Exponent++;
            }
            return;
        }
        else if (Math.Abs(Mantissa) < 1d)
        {
            while (Math.Abs(Mantissa) < 10d)
            {
                Mantissa *= 10d;
                Exponent--;
            }
            return;
        }
    }

    public static BigDigit Power(BigDigit other, float power)
    {
        return new BigDigit(Mathf.Pow((float)other.Mantissa, power), (long)(other.Exponent * power));
    }

    public static BigDigit Power(BigDigit other, int power)
    {
        return new BigDigit(Mathf.Pow((float)other.Mantissa, power), (long)(other.Exponent * power));
    }

    public static BigDigit Power(BigDigit other, double power)
    {
        return new BigDigit(Mathf.Pow((float)other.Mantissa, (float)power), (long)(other.Exponent * power));
    }

    public static BigDigit Power(BigDigit other, long power)
    {
        return new BigDigit(Mathf.Pow((float)other.Mantissa, power), (long)(other.Exponent * power));
    }

    public override int GetHashCode()
    {
        var hashCode = -1472829619;
        hashCode = hashCode * -1521134295 + Mantissa.GetHashCode();
        hashCode = hashCode * -1521134295 + Exponent.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(postfix);
        return hashCode;
    }

    public float ToFloat()
    {
        return (float)(Mantissa * Math.Pow(10d, Exponent));
    }

    public double ToDouble()
    {
        return Mantissa * Math.Pow(10d, Exponent);
    }
}
