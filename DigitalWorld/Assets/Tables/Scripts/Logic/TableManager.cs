using Dream.Proto;

namespace DigitalWorld.Table
{
    public partial class TableManager
    {
        public TableManager()
        {
            this.OnDecodeTable = OnProcessDecodeTable;
        }

        /// <summary>
        /// 处理解码表格
        /// 先I/O出数据流并解析到对象
        /// </summary>
        /// <param name="table">表的数据流</param>
        /// <param name="tableName">表名</param>
        private void OnProcessDecodeTable(ByteBuffer table, string tableName)
        {
            if (null != table)
            {

            }
        }
    }
}
