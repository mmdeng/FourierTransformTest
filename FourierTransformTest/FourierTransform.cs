using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourierTransformTest
{
    /// <summary>
    /// フーリエ変換ロジック
    /// </summary>
    public static class FourierTransform
    {
        /// <summary>
        /// ２のべき乗かどうかを調べる
        /// </summary>
        /// <remarks></remarks>
        /// <param name="n">指定値</param>
        /// <returns><para>True: ２のべき乗</para><para>False: ２のべき乗でない</para></returns>
        public static bool IsPowerOf2(int n)
        {
            if (n == 0) return true;
            return (n & (n - 1)) == 0;
        }
        /// <summary>
        /// ビットを左右反転した配列を返す
        /// </summary>
        /// <remarks></remarks>
        /// <param name="dataCount">データ数</param>
        /// <returns>ビット反転後の配列</returns>
        public static int[] GetBitExchangedArray(int dataCount)
        {
            // ２のべき乗のみ許容。
            if (IsPowerOf2(dataCount) == false) return null;

            var array = new int[dataCount];
            int arraySizeHalf = dataCount >> 1;

            array[0] = 0;
            for (int i = 1; i < dataCount; i <<= 1)
            {
                for (int j = 0; j < i; j++)
                {
                    array[j + i] = array[j] + arraySizeHalf;
                }
                arraySizeHalf >>= 1;
            }
            return array;
        }
        /// <summary>
        /// フーリエ変換係数取得 Wx = x e^iθ取得
        /// </summary>
        /// <remarks></remarks>
        /// <param name="complex">複素数</param>
        /// <param name="theta">θ</param>
        /// <param name="isPlus">偶関数成分の符号。Trueなら偶関数がプラス、奇関数がマイナス</param>
        /// <returns>フーリエ変換係数</returns>
        public static Complex GetFactor(Complex complex, double theta, bool isPlus)
        {
            var result = new Complex();
            int flag = 1;
            if (isPlus == false)
            {
                flag = -1;
            }
            result.Re = complex.Re * Math.Cos(theta) + complex.Im * Math.Sin(theta) * flag;
            result.Im = complex.Im * Math.Cos(theta) - complex.Re * Math.Sin(theta) * flag;
            return result;
        }
        /// <summary>
        /// 離散フーリエ変換(非破壊)
        /// </summary>
        /// <remarks>http://hooktail.org/computer/index.php?%B9%E2%C2%AE%A5%D5%A1%BC%A5%EA%A5%A8%CA%D1%B4%B9</remarks>
        /// <param name="vector">複素ベクトル</param>
        /// <returns>離散フーリエ変換結果</returns>
        public static ComplexVector Dft(ComplexVector vector)
        {
            return Dft(vector, false);
        }
        /// <summary>
        /// 離散フーリエ変換(非破壊)
        /// </summary>
        /// <remarks>http://hooktail.org/computer/index.php?%B9%E2%C2%AE%A5%D5%A1%BC%A5%EA%A5%A8%CA%D1%B4%B9</remarks>
        /// <param name="vector">複素ベクトル</param>
        /// <param name="isInverse"><para>True: 逆変換</para><para>False: 順変換</para></param>
        /// <returns>離散フーリエ変換結果</returns>
        public static ComplexVector Dft(ComplexVector vector, bool isInverse)
        {
            var data = vector.Clone();
            var work = new ComplexVector();
            work.SetSize(data.Count);
            work.SetAll(new Complex());

            double coefficient = 1;
            if (isInverse == false)
            {
                coefficient = data.Count;
            }
            for (var i = 0; i < data.Count; i++)
            {
                for (var j = 0; j < data.Count; j++)
                {
                    double theta = 2 * Math.PI * i * j / data.Count;
                    work[i] += GetFactor(data[j], theta, !isInverse);
                }
                work[i] /= coefficient;
            }
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = work[i];
            }
            return data;
        }
        /// <summary>
        /// 高速離散フーリエ変換(非破壊)
        /// </summary>
        /// <remarks>http://hooktail.org/computer/index.php?%B9%E2%C2%AE%A5%D5%A1%BC%A5%EA%A5%A8%CA%D1%B4%B9</remarks>
        /// <param name="vector">複素ベクトル</param>
        /// <returns>高速離散フーリエ変換結果</returns>
        public static ComplexVector Fft(ComplexVector vector)
        {
            return Fft(vector, false);
        }
        /// <summary>
        /// 高速離散フーリエ変換(非破壊)
        /// </summary>
        /// <remarks>http://hooktail.org/computer/index.php?%B9%E2%C2%AE%A5%D5%A1%BC%A5%EA%A5%A8%CA%D1%B4%B9</remarks>
        /// <param name="vector">複素ベクトル</param>
        /// <param name="isInverse"><para>True: 逆変換</para><para>False: 順変換</para></param>
        /// <returns>高速離散フーリエ変換結果</returns>
        public static ComplexVector Fft(ComplexVector vector, bool isInverse)
        {
            // ２のべき乗のみ許容。
            if (IsPowerOf2(vector.Count) == false)
            {
                throw new Exception(string.Format("Data count ({0}) is not power of 2", vector.Count));
            }
            var data = new ComplexVector();
            data.SetSize(vector.Count);
            var work = new ComplexVector();
            work.SetSize(vector.Count);
            work.SetAll(new Complex());

            int[] reverseBitArray = GetBitExchangedArray(vector.Count);
            if (reverseBitArray == null)
            {
                throw new Exception("Failed to GetBitExchangedArray");
            }

            // バタフライ演算のための置き換え
            for (int i = 0; i < vector.Count; i++)
            {
                data[i] = vector[reverseBitArray[i]];
            }

            // バタフライ演算
            int wingSize = 2;

            // ステージ数はデータ数の二乗根
            int stageCount = (int)Math.Log(vector.Count, 2);
            for (int stage = 0; stage < stageCount; stage++)
            {
                // ステージごとの蝶の数
                int butterflyCount = data.Count / wingSize;
                for (int butterflyIndex = 0; butterflyIndex < butterflyCount; butterflyIndex++)
                {
                    // 蝶一匹あたりの処理
                    for (int wingIndex = 0; wingIndex < wingSize; wingIndex++)
                    {
                        // 羽一枚辺りの処理
                        int to = butterflyIndex * wingSize + wingIndex;

                        int from1, from2;
                        if (wingIndex < wingSize / 2)
                        {
                            // 前半
                            from1 = to;
                            from2 = from1 + wingSize / 2;
                        }
                        else
                        {
                            // 後半
                            from2 = to;
                            from1 = to - wingSize / 2;
                        }
                        // フーリエ変換係数計算
                        double theta = 2 * Math.PI * wingIndex * butterflyCount / data.Count;
                        work[to] = data[from1] + GetFactor(data[from2], theta, !isInverse);
                    }
                }
                for (int j = 0; j < data.Count; j++)
                {
                    data[j] = work[j];
                }
                // 次のステージは羽のサイズが2倍になる。
                wingSize *= 2;
            }
            if (isInverse == false)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    data[i] = data[i] / data.Count;
                }
            }
            return data;
        }
        /// <summary>
        /// フーリエ変換(非破壊)。２のべき乗ならFFT、それ以外ならDFT
        /// </summary>
        /// <param name="vector">複素ベクトル</param>
        /// <returns>フーリエ変換結果</returns>
        public static ComplexVector Execute(ComplexVector vector)
        {
            return Execute(vector, false);
        }
        /// <summary>
        /// フーリエ変換(非破壊)。２のべき乗ならFFT、それ以外ならDFT
        /// </summary>
        /// <param name="vector">複素ベクトル</param>
        /// <param name="isInverse"><para>True: 逆変換</para><para>False: 順変換</para></param>
        /// <returns>フーリエ変換結果</returns>
        public static ComplexVector Execute(ComplexVector vector, bool isInverse)
        {
            if (IsPowerOf2(vector.Count) == true)
            {
                return Fft(vector, isInverse);
            }
            else
            {
                return Dft(vector, isInverse);
            }
        }
        /// <summary>
        /// フーリエ係数からフーリエ級数展開した値を取得する。
        /// </summary>
        /// <param name="vector">複素ベクトル</param>
        /// <param name="x">X</param>
        /// <returns>フーリエ級数</returns>
        public static double GetSeries(ComplexVector vector, double x)
        {
            if (vector.Count <= 0) throw new Exception("Count is zero");

            var result = vector[0].Re;
            for (int i = 1; i < vector.Count; i++)
            {
                double theta = 2 * Math.PI * i * x / (double)vector.Count;
                var complex = GetFactor(vector[i], theta, false);
                result += complex.Re;
            }
            return result;
        }
        /// <summary>
        /// フーリエ係数からフーリエ級数展開した値を取得する。
        /// </summary>
        /// <param name="vector">複素ベクトル</param>
        /// <returns>フーリエ級数</returns>
        public static double[] GetSeries(ComplexVector vector)
        {
            var list = new double[vector.Count];
            for (int x = 0; x < vector.Count; x++)
            {
                list[x] = GetSeries(vector, x);
            }
            return list;
        }
        /// <summary>
        /// パワースペクトル取得（Logをとる）
        /// </summary>
        /// <param name="vector">複素ベクトル</param>
        /// <param name="enableLog"><para>True: Logをとる</para><para>False: Logをとらない</para></param>
        /// <returns>結果</returns>
        public static double[] GetPowerSpectrum(ComplexVector vector, bool enableLog)
        {
            var result = new double[vector.Count];
            for (var x = 0; x < vector.Count; x++)
            {
                double distance = vector[x].GetDistance();
                if (double.IsNaN(distance)) throw new Exception("Failed to GetDistance");

                if (enableLog == true)
                {
                    result[x] = Math.Log(distance);
                }
                else
                {
                    result[x] = distance;
                }
            }
            return result;
        }
    }
}
