namespace DigitalWorld.Game
{
    public class CharacterManager : UnitManager
    {
        #region Override
        public override string Name => "Characters";
        public override EUnitType Type => EUnitType.Character;
        #endregion
    }
}
