namespace MarsRover.Models
{
    public class Rover
    {
        public int Id { get; set; }
        public string? Name{ get; set; }
        public int sPosX { get; set; }
        public int sPosY { get; set; }
        public char sDir { get; set; }
        public string? Input { get; set; }

        public int? fPosX { get; set; }
        public int? fPosY { get; set; }
        public char? fDir { get; set; }
    }
}
