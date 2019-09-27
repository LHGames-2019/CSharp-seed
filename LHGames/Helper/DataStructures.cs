using System.Collections.Generic;

/// <summary>
/// !!! DO NOT EDIT !!!
/// </summary>
namespace LHGames.Helper
{
    public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3, Invalid = 4 };

    public interface IPlayer
    {
        int TeamNumber { get; set; }
        Point Position { get; set; }
        int SizeOfTail { get; set; }
        int SizeOfBody { get; set; }
    }

    public class HostPlayer : IPlayer
    {
        public int TeamNumber { get; set; }
        public Point Position { get; set; }
        public int SizeOfTail { get; set; }
        public int SizeOfBody { get; set; }
        public int MaxMovement { get; set; }
        public int MovementLeft { get; set; }
        public Direction LastMove { get; set; }
    }

    public class OtherPlayer : IPlayer
    {
        public int TeamNumber { get; set; }
        public Point Position { get; set; }
        public int SizeOfTail { get; set; }
        public int SizeOfBody { get; set; }
    }

    public class GameInfo
    {
        public string[] Map { get; set; }
        public HostPlayer Self { get; set; }
        public List<OtherPlayer> Others { get; set; }
    }

    public class Tile
    {
        public int Color { get; set; }
        public int Tail { get; set; }
        public ISet<int> Heads { get; }
        public bool IsBorder { get; }
        public IList<bool> IsAlreadyChecked { get; }
        public Point Position { get; }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public int NTiles { get; }
        public int NTeam { get; }
        public List<List<Tile>> Grid { get; set; }
    }
    
}
