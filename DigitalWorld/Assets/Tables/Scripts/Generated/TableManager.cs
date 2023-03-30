using Dream.Core;
using Dream.Table;

namespace DigitalWorld.Table
{
    /// <summary>
    /// 表格管理器
    /// </summary>
    public sealed partial class TableManager : Singleton<TableManager>
    {
        #region Tables
        public CharacterTable CharacterTable { get; private set; } = new CharacterTable();
        public MapTable MapTable { get; private set; } = new MapTable();
        public CampTable CampTable { get; private set; } = new CampTable();
        public SkillTable SkillTable { get; private set; } = new SkillTable();
        public BuildingTable BuildingTable { get; private set; } = new BuildingTable();
        #endregion

        #region Decode
        public void Decode()
        {
           CharacterTable = this.ApplyDecodeTable<CharacterTable>("character");
           MapTable = this.ApplyDecodeTable<MapTable>("map");
           CampTable = this.ApplyDecodeTable<CampTable>("camp");
           SkillTable = this.ApplyDecodeTable<SkillTable>("skill");
           BuildingTable = this.ApplyDecodeTable<BuildingTable>("building");
        }

        public void DecodeJson()
        {
            CharacterTable = this.ApplyDecodeTableWithJson<CharacterTable>("character");
            MapTable = this.ApplyDecodeTableWithJson<MapTable>("map");
            CampTable = this.ApplyDecodeTableWithJson<CampTable>("camp");
            SkillTable = this.ApplyDecodeTableWithJson<SkillTable>("skill");
            BuildingTable = this.ApplyDecodeTableWithJson<BuildingTable>("building");
        }

        private T ApplyDecodeTable<T>(string tableName) where T : class
        {
            return this.ProcessDecodeTable<T>(tableName);
        }

        private T ApplyDecodeTableWithJson<T>(string tableName) where T : class
        {
            return this.ProcessDecodeTableWithJson<T>(tableName);
        }
        #endregion

        #region Encode
        public void Encode()
        {
            this.ApplyEncodeTable(CharacterTable, "character");
            this.ApplyEncodeTable(MapTable, "map");
            this.ApplyEncodeTable(CampTable, "camp");
            this.ApplyEncodeTable(SkillTable, "skill");
            this.ApplyEncodeTable(BuildingTable, "building");
        }

        private void ApplyEncodeTable(object table, string tableName)
        {
            this.ProcessEncodeTable(table, tableName);
        }
        #endregion

        #region Utility
        public static ITable CreateTable(string tableName)
        {
            switch (tableName)
            {
                case "character":
                    return new CharacterTable();
                case "map":
                    return new MapTable();
                case "camp":
                    return new CampTable();
                case "skill":
                    return new SkillTable();
                case "building":
                    return new BuildingTable();
                default:
                {
                    return null;
                }
            }
        }

        public static System.Type GetTableType(string tableName)
        {
            switch (tableName)
            {
                case "character":
                    return typeof(CharacterTable);
                case "map":
                    return typeof(MapTable);
                case "camp":
                    return typeof(CampTable);
                case "skill":
                    return typeof(SkillTable);
                case "building":
                    return typeof(BuildingTable);
                default:
                {
                    return null;
                }
            }
        }
        #endregion

    }
}
