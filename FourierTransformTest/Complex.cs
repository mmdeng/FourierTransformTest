using System;
using System.Collections.Generic;
using System.Text;

namespace FourierTransformTest
{
    /// <summary>
    /// 複素数
    /// </summary>
    /// <remarks></remarks>
    public class Complex : ICloneable
    {
        /// <summary>
        /// 実部
        /// </summary>
        public double Re;
        /// <summary>
        /// 虚部
        /// </summary>
        public double Im;
        /// <summary>
        /// Constructor
        /// </summary>
        public Complex()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="re">実部</param>
        /// <param name="im">虚部</param>
        public Complex(double re, double im)
        {
            this.Re = re;
            this.Im = im;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="re">実部</param>
        public Complex(double re)
            : this(re, 0)
        {
        }
        /// <summary>
        /// クローン作成
        /// </summary>
        /// <returns>クローン</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 共役の取得(虚部のみ反転)
        /// </summary>
        /// <returns>共役</returns>
        public Complex GetConjugate()
        {
            return new Complex(this.Re, -this.Im);
        }
        /// <summary>
        /// 共役(虚部のみ反転)
        /// </summary>
        /// <returns>共役</returns>
        public void Conjugate()
        {
            this.Im *= -1;
        }
        /// <summary>
        /// 本ベクトルの距離を取得
        /// </summary>
        /// <returns>距離</returns>
        public double GetDistance()
        {
            double distance = Math.Pow(this.Re, 2) + Math.Pow(this.Im, 2);
            distance = Math.Sqrt(distance);
            return distance;
        }
        /// <summary>
        /// 複素数での文字列表現を取得
        /// </summary>
        /// <remarks></remarks>
        /// <returns>複素数での文字列表現</returns>
        public override string ToString()
        {
            if (this.Im >= 0)
            {
                return this.Re.ToString("F") + ", i" + this.Im.ToString("F");
            }
            else
            {
                return this.Re.ToString("F") + ", -i" + (-this.Im).ToString("F");
            }
        }
        /// <summary>
        /// 複素数同士の乗算。演算子「*」とは違う
        /// </summary>
        /// <remarks></remarks>
        /// <param name="complex">乗算する複素数</param>
        /// <returns>結果複素数</returns>
        public Complex MultiplyComplex(Complex complex)
        {
            var result = new Complex();
            if (complex == null) return result;

            result.Re = this.Re * complex.Re - this.Im * complex.Im;
            result.Im = this.Re * complex.Im + this.Im * complex.Re;
            return result;
        }
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator +(Complex item1, Complex item2)
        {
            var item = (Complex)item1.Clone();
            item.Re = item1.Re + item2.Re;
            item.Im = item1.Im + item2.Im;
            return item;
        }
        /// <summary>
        /// 和
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator +(Complex item1, double item2)
        {
            return item1 + new Complex(item2, item2);
        }
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator -(Complex item1, Complex item2)
        {
            var item = (Complex)item1.Clone();
            item.Re = item1.Re - item2.Re;
            item.Im = item1.Im - item2.Im;
            return item;
        }
        /// <summary>
        /// 差
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator -(Complex item1, double item2)
        {
            return item1 - new Complex(item2, item2);
        }
        /// <summary>
        /// 積
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator *(Complex item1, Complex item2)
        {
            var item = (Complex)item1.Clone();
            item.Re = item1.Re * item2.Re;
            item.Im = item1.Im * item2.Im;
            return item;
        }
        /// <summary>
        /// 積
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator *(Complex item1, double item2)
        {
            return item1 * new Complex(item2, item2);
        }
        /// <summary>
        /// 商
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator /(Complex item1, Complex item2)
        {
            if (item2.Re == 0) return null;
            if (item2.Im == 0) return null;

            var item = (Complex)item1.Clone();
            item.Re = item1.Re / item2.Re;
            item.Im = item1.Im / item2.Im;
            return item;
        }
        /// <summary>
        /// 商
        /// </summary>
        /// <param name="item1">左辺</param>
        /// <param name="item2">右辺</param>
        /// <returns>結果</returns>
        public static Complex operator /(Complex item1, double item2)
        {
            return item1 / new Complex(item2, item2);
        }
    }
}
