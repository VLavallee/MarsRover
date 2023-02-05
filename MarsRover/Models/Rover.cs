using System.ComponentModel;

namespace MarsRover.Models
{
    public class Rover
    {
        // rover data
        public int Id { get; set; }

        [DisplayName("Rover Name")]
        public string? Name { get; set; }

        [DisplayName("Starting Position X")]
        public int StartingPositionX { get; set; }

        [DisplayName("Starting Position Y")]
        public int StartingPositionY { get; set; }

        [DisplayName("Starting Direction")]
        public char StartingDirection { get; set; }

        [DisplayName("Plateau Size X")]
        public int PlateauSizeX { get; set; }

        [DisplayName("Plateau Size Y")]
        public int PlateauSizeY { get; set; }

        [DisplayName("Input")]
        public string? Input { get; set; }


        [DisplayName("Final Position X")]
        public int? FinalPositionX { get; set; }

        [DisplayName("Final Position Y")]
        public int? FinalPositionY { get; set; }

        [DisplayName("Final Dir")]
        public char FinalDirection { get; set; }

        // plateau path data

        public string? PlateauMap { get; set; }

    }

}
