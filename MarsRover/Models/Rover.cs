using System.ComponentModel;

namespace MarsRover.Models
{
    public class Rover
    {
        // rover data
        public int Id { get; set; }

        [DisplayName("Name")]
        public string? Name { get; set; }

        [DisplayName("Start Pos X")]
        public int StartingPositionX { get; set; }

        [DisplayName("Start Pos Y")]
        public int StartingPositionY { get; set; }

        [DisplayName("Start Dir")]
        public char StartingDirection { get; set; }

        [DisplayName("Plateau Size X")]
        public int PlateauSizeX { get; set; }

        [DisplayName("Plateau Size Y")]
        public int PlateauSizeY { get; set; }

        [DisplayName("Input")]
        public string? Input { get; set; }


        [DisplayName("Final Pos X")]
        public int? FinalPositionX { get; set; }

        [DisplayName("Final Pos Y")]
        public int? FinalPositionY { get; set; }

        [DisplayName("Final Dir")]
        public char FinalDirection { get; set; }

        // plateau path data

        public string? PlateauMap { get; set; }

    }

}
