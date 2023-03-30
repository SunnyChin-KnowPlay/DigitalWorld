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

        public void DecodeJSON()
        {
            CharacterTable = this.ApplyDecodeTableWithJSON<CharacterTable>("character");
            MapTable = this.ApplyDecodeTableWithJSON<MapTable>("map");
            CampTable = this.ApplyDecodeTableWithJSON<CampTable>("camp");
            SkillTable = this.ApplyDecodeTableWithJSON<SkillTable>("skill");
            BuildingTable = this.ApplyDecodeTableWithJSON<BuildingTable>("building");
        }

        private T ApplyDecodeTable<T>(string tableName) where T : class
        {
            return this.ProcessDecodeTable<T>(tableName);
        }

        private T ApplyDecodeTableWithJSON<T>(string tableName) where T : class
        {
            return this.ProcessDecodeTableWithJSON<T>(tableName);
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

        #region Creator
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
        #endregion

    }
}
