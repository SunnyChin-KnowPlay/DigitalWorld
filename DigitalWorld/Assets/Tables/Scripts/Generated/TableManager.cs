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
        /// 处理表格
        /// </summary>
        /// <param name="table">表格的对象</param>
        /// <param name="tableName">表名</param>
        private delegate void OnTableHandle(ByteBuffer table, string tableName);
        /// <summary>
        /// 解码表格
        /// 请执行I/O读取出数据并调用table.Decode()以实现解码并注入表格功能
        /// </summary>
        private OnTableHandle OnDecodeTable;
        /// <summary>
        /// 从xml来解码表格
        /// </summary>
        private OnTableHandle OnDecodeTableWithXml;
        /// <summary>
        /// encode table
        /// </summary>
        private OnTableHandle OnEncodeTable;
        #endregion

        #region Tables
        public CharacterTable CharacterTable { get; private set; } = new CharacterTable();
        public MapTable MapTable { get; private set; } = new MapTable();
        public CampTable CampTable { get; private set; } = new CampTable();
        #endregion

        #region Decode
        public void Decode()
        {
            this.ApplyDecodeTable(CharacterTable, "character");
            this.ApplyDecodeTable(MapTable, "map");
            this.ApplyDecodeTable(CampTable, "camp");
        }

        public void DecodeXml()
        {
            this.ApplyDecodeTableWithXml(CharacterTable, "character");
            this.ApplyDecodeTableWithXml(MapTable, "map");
            this.ApplyDecodeTableWithXml(CampTable, "camp");
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
        #endregion

        #region Encode
        public void Encode()
        {
            this.ApplyEncodeTable(CharacterTable, "character");
            this.ApplyEncodeTable(MapTable, "map");
            this.ApplyEncodeTable(CampTable, "camp");
        }

        private void ApplyEncodeTable(ByteBuffer table, string tableName)
        {
            if (null != OnEncodeTable)
            {
                OnEncodeTable.Invoke(table, tableName);
            }
        }
        #endregion

    }
}
