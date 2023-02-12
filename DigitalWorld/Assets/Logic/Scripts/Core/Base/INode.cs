namespace DigitalWorld.Logic
{
    public interface INode
    {
        /// <summary>
        /// ID
        /// </summary>
        int Id { get; }

        void Update(int delta);

        /// <summary>
        /// 是否激活的
        /// </summary>
        bool Enabled { get; set; }
    }
}
