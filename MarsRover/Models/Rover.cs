using System.ComponentModel;

namespace MarsRover.Models
{
    public class Rover
    {
        // rover data
        public int Id { get; set; }
        public string? Name{ get; set; }
        public int sPosX { get; set; }
        public int sPosY { get; set; }
        public char sDir { get; set; }
        public string? input { get; set; }

        public int? fPosX { get; set; }
        public int? fPosY { get; set; }
        public char? fDir { get; set; }

        // plateau path data
        
    }
}
