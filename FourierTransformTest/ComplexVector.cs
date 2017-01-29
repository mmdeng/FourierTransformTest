using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourierTransformTest
{
    /// <summary>
    /// 複素数ベクトル
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class ComplexVector
    {
        /// <summary>
        /// 
        /// </summary>
        protected Complex[] values;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ComplexVector()
        {
            this.values = new Complex[0];
        }
        /// <summary>
        /// サイズ設定（全ての既存データはクリアされます）
        /// </summary>
        /// <remarks></remarks>
        /// <param name="count">サイズ</param>
        public virtual void SetSize(int count)
        {
            this.values = new Complex[count];
        }
        /// <summary>
        /// インデクサの取得、設定
        /// </summary>
        /// <remarks>本リストアイテムのインデクサを取得、設定します。</remarks>
        /// <param name="index">指定インデックス</param>
        /// <returns>指定インデックスに対応するリストアイテム</returns>
        public Complex this[int index]
        {
            get
            {
                return this.values[index];
            }
            set
            {
                this.values[index] = value;
            }
        }
        /// <summary>
        /// リストアイテムのイテレータを取得します
        /// </summary>
        /// <remarks>リストアイテムのイテレータを取得します</remarks>
        /// <returns>イテレータ</returns>
        public IEnumerator<Complex> GetEnumerator()
        {
            foreach (var m in this.values) yield return m;
        }
        /// <summary>
        /// 本クラスインスタンスのクローンを作成します。
        /// </summary>
        /// <remarks></remarks>
        /// <returns>クローン</returns>
        public ComplexVector Clone()
        {
            var clone = (ComplexVector)this.MemberwiseClone();
            clone.SetSize(this.Count);
            Enumerable.Range(0, this.Count).ToList().ForEach
                (i => clone[i] = (Complex)this[i].Clone());
            return clone;
        }
        /// <summary>
        /// リスト内のアイテム数を取得します
        /// </summary>
        /// <remarks>リスト内のアイテム数を取得します</remarks>
        public int Count
        {
            get
            {
                return this.values.Length;
            }
        }
        /// <summary>
        /// 全ての値を設定
        /// </summary>
        /// <remarks></remarks>
        /// <param name="value">設定値</param>
        public void SetAll(Complex value)
        {
            Enumerable.Range(0, this.Count).ToList().ForEach
                (i => this[i] = (Complex)value.Clone());
        }
        /// <summary>
        /// 生データを実部に設定する。既存データは削除します。虚部は全てゼロに設定されます。
        /// </summary>
        /// <remarks></remarks>
        /// <param name="data">生データ</param>
        public void SetRealData(double[] data)
        {
            SetSize(data.Length);
            Enumerable.Range(0, data.Length).ToList().ForEach
                (i => this.values[i] = new Complex(data[i], 0));
        }
    }
}
