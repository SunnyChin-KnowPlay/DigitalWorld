using Dream.Core;
using Dream.Proto;

namespace DigitalWorld.Table
{
    /// <summary>
    /// 表格管理器
    /// </summary>
    public sealed partial class TableManager : Singleton<TableManager>
    {
        #region Event
        /// <summary>
        /// 解码表格
        /// 请执行I/O读取出数据并调用table.Decode()以实现解码并注入表格功能
        /// </summary>
        /// <param name="table">表格的对象</param>
        /// <param name="tableName">表名</param>
        public delegate void OnDecodeTableHandle(ByteBuffer table, string tableName);
        public OnDecodeTableHandle OnDecodeTable;
        public OnDecodeTableHandle OnDecodeTableWithXml;
        #endregion

        #region Tables
        public CharacterTable CharacterTable { get; private set; }
        public TilebaseTable TilebaseTable { get; private set; }
        public TileTable TileTable { get; private set; }
        #endregion

        public void Decode()
        {
            CharacterTable = new CharacterTable();
            this.ApplyDecodeTable(CharacterTable, "character");
            TilebaseTable = new TilebaseTable();
            this.ApplyDecodeTable(TilebaseTable, "tilebase");
            TileTable = new TileTable();
            this.ApplyDecodeTable(TileTable, "tile");
        }

        private void ApplyDecodeTable(ByteBuffer table, string tableName)
        {
            if (null != OnDecodeTable)
            {
                OnDecodeTable.Invoke(table, tableName);
            }
        }

        private void ApplyDecodeTableWithXml(ByteBuffer table, string tableName)
        {
            if (null != OnDecodeTableWithXml)
            {
                OnDecodeTableWithXml.Invoke(table, tableName);
            }
        }

    }
}
