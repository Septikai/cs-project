namespace Project.Dungeon.Dungeons
{
    public enum DungeonId
    {
        // Each Id corresponds to a specific dungeon, and is used to select the desired one
        Cavern,
        // RandomDungeon is used when in endless mode
        RandomDungeon,
        // NullDungeon is used to deselect a dungeon - to say that the player is not currently in a dungeon
        NullDungeon
    }
}