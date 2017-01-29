using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FourierTransformTest
{
    /// <summary>
    /// 二次元フーリエ変換のテスト
    /// </summary>
    [TestClass]
    public class FourierTransformTest2D
    {
        /// <summary>
        /// 矩形サインのデータを作る
        /// </summary>
        /// <param name="size">データサイズ</param>
        /// <param name="period">周期</param>
        /// <param name="amplitude">振幅</param>
        /// <returns>データ</returns>
        private double[] GetRectangleSin(int size, double period, double amplitude)
        {
            var data = new double[256];
            for (int i = 0; i < data.Length; i++)
            {
                if ((1 / period) * Math.Sin(i) >= 0) data[i] = amplitude;
                else data[i] = -amplitude;
            }
            return data;
        }
        /// <summary>
        /// 全ての複素ベクトル値が指定精度内で等しいことを確認。実部虚部ともにチェック。
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="accuracy"></param>
        private static void Check(ComplexVector p1, ComplexVector p2, double accuracy)
        {
            Assert.AreEqual(p1.Count, p2.Count);
            for (int i = 0; i < p1.Count; i++)
            {
                var difference = Math.Abs(p1[i].Re - p2[i].Re);
                Assert.IsTrue(difference < accuracy);
                difference = Math.Abs(p1[i].Im - p2[i].Im);
                Assert.IsTrue(difference < accuracy);
            }
        }
        /// <summary>
        /// 離散フーリエ変換
        /// </summary>
        [TestMethod]
        public void Dft()
        {
            // 矩形サインのデータを作る
            var data = GetRectangleSin(256, 64, 4);

            // 生データを実部に設定
            var list1 = new ComplexVector();
            list1.SetRealData(data);

            // フーリエ変換
            var list2 = FourierTransform.Dft(list1, false);

            // フーリエ逆変換
            var list3 = FourierTransform.Dft(list2, true);

            // 生データと逆変換後で、かなり良い精度で一致する。
            const double ACCURACY = 0.00000000001;
            Check(list1, list3, ACCURACY);

            // フーリエ級数展開
            var series = FourierTransform.GetSeries(list2);

            // 生データとフーリエ級数展開後で、かなり良い精度で一致する。
            for (int i = 0; i < list1.Count; i++)
            {
                var difference = Math.Abs(list1[i].Re - series[i]);
                Assert.IsTrue(difference < ACCURACY);
            }
            // パワースペクトル
            var vectorPowerSpectrum = FourierTransform.GetPowerSpectrum(list2, true);
        }
        /// <summary>
        /// 高速フーリエ変換。DFDより4倍ぐらい速い。
        /// </summary>
        [TestMethod]
        public void Fft()
        {
            // 矩形サインのデータを作る
            var data = GetRectangleSin(256, 64, 4);

            // 生データを実部に設定
            var list1 = new ComplexVector();
            list1.SetRealData(data);

            // フーリエ変換
            var list2 = FourierTransform.Fft(list1, false);

            // フーリエ逆変換
            var list3 = FourierTransform.Fft(list2, true);

            // かなり良い精度で一致する。
            const double ACCURACY = 0.00000000001;
            Check(list1, list3, ACCURACY);

            // フーリエ級数展開
            var series = FourierTransform.GetSeries(list2);

            // 生データとフーリエ級数展開後で、かなり良い精度で一致する。
            for (int i = 0; i < list1.Count; i++)
            {
                var difference = Math.Abs(list1[i].Re - series[i]);
                Assert.IsTrue(difference < ACCURACY);
            }
            // パワースペクトル
            var vectorPowerSpectrum = FourierTransform.GetPowerSpectrum(list2, true);
        }
    }
}
