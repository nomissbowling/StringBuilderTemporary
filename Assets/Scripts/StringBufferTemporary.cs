﻿/**
The MIT License (MIT)

Copyright (c) 2016 Yusuke Kurokawa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
using System.Threading;
using System.Text;

namespace StringBufferTemporary
{
    /// <summary>
    /// Using this,you can optimize string concat operation easily.
    /// 
    /// - before code
    /// string str = "aaa" + 20 + "bbbb";
    /// 
    /// - after code
    /// string str = Sbt.i + "aaa" + 20 + "bbbb";
    /// 
    /// "Sbt.i" is not "ThreadSafe" , and reuse same object.
    /// 
    /// You can use "Sbt.small" / "Sbt.medium" / "Sbt.large" instead of "Sbt.i". 
    /// These are creating instance. So it is "Thread safe".
    /// </summary>
    public class Sbt
    {
        private static Sbt s_this;
        private StringBuilder sb;

        static Sbt()
        {
            s_this = new Sbt(1024);
        }
        private Sbt(int capacity)
        {
            sb = new StringBuilder(capacity);
        }

        public static Sbt Create(int capacity)
        {
            return new Sbt(capacity);
        }

        public static Sbt small
        {
            get
            {
                return Create(64);
            }
        }

        public static Sbt medium
        {
            get
            {
                return Create(256);
            }
        }
        public static Sbt large
        {
            get
            {
                return Create(1024);
            }
        }

        /// <summary>
        /// Be careful . This isn't thread safe.
        /// </summary>
        public static Sbt i
        {
            get
            {
                s_this.sb.Length = 0;
                return s_this;
            }
        }

        public int Capacity
        {
            set { this.sb.Capacity = value; }
            get { return sb.Capacity; }
        }

        public int Length
        {
            set { this.sb.Length = value; }
            get { return this.sb.Length; }
        }
        public Sbt Remove(int startIndex, int length)
        {
            sb.Remove(startIndex, length);
            return this;
        }
        public Sbt Replace(string oldValue, string newValue)
        {
            sb.Replace(oldValue, newValue);
            return this;
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public void Clear()
        {
            // StringBuilder.Clear() doesn't support .Net 3.5...
            // "Capasity = 0" doesn't work....
            sb = new StringBuilder(0);
        }

        public Sbt ToLower()
        {
            int length = sb.Length;
            for (int i = 0; i < length; ++i)
            {
                if (char.IsUpper(sb[i]))
                {
                    sb.Replace(sb[i], char.ToLower(sb[i]), i, 1);
                }
            }
            return this;
        }
        public Sbt ToUpper()
        {
            int length = sb.Length;
            for (int i = 0; i < length; ++i)
            {
                if (char.IsLower(sb[i]))
                {
                    sb.Replace(sb[i], char.ToUpper(sb[i]), i, 1);
                }
            }
            return this;
        }

        public Sbt Trim()
        {
            return TrimEnd().TrimStart();
        }

        public Sbt TrimStart()
        {
            int length = sb.Length;
            for (int i = 0; i < length; ++i)
            {
                if (!char.IsWhiteSpace(sb[i]))
                {
                    if (i > 0)
                    {
                        sb.Remove(0, i);
                    }
                    break;
                }
            }
            return this;
        }
        public Sbt TrimEnd()
        {
            int length = sb.Length;
            for (int i = length - 1; i >= 0; --i)
            {
                if (!char.IsWhiteSpace(sb[i]))
                {
                    if (i < length - 1)
                    {
                        sb.Remove(i, length - i);
                    }
                    break;
                }
            }
            return this;
        }


        public static implicit operator string(Sbt t)
        {
            return t.ToString();
        }

        #region ADD_OPERATOR
        public static Sbt operator +(Sbt t, bool v)
        {
            t.sb.Append(v);
            return t;
        }
        public static Sbt operator +(Sbt t, int v)
        {
            t.sb.Append(v);
            return t;
        }
        public static Sbt operator +(Sbt t, short v)
        {
            t.sb.Append(v);
            return t;
        }
        public static Sbt operator +(Sbt t, byte v)
        {
            t.sb.Append(v);
            return t;
        }
        public static Sbt operator +(Sbt t, float v)
        {
            t.sb.Append(v);
            return t;
        }
        public static Sbt operator +(Sbt t, char c)
        {
            t.sb.Append(c);
            return t;
        }
        public static Sbt operator +(Sbt t, char[] c)
        {
            t.sb.Append(c);
            return t;
        }
        public static Sbt operator +(Sbt t, string str)
        {
            t.sb.Append(str);
            return t;
        }
        public static Sbt operator +(Sbt t, StringBuilder sb)
        {
            t.sb.Append(sb);
            return t;
        }
        #endregion ADD_OPERATOR
    }
}