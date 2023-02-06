using MarsRover.Controllers;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarsRover.Models
{
    public class Rover
    {
        // rover data
        public int Id { get; set; }

        [DisplayName("Rover Name")]
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [DisplayName("Beginning Position X")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Positive numbers only")]
        public int StartingPositionX { get; set; }

        [DisplayName("Beginning Position Y")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Positive numbers only")]
        public int StartingPositionY { get; set; }

        [DisplayName("Beginning Direction")]
        [RegularExpression(@"^[NnEeSsWw]+$", ErrorMessage = "Values of N, E, S, and W only")]
        public char StartingDirection { get; set; }

        [DisplayName("Plateau Size X")]
        [RegularExpression(@"^(?:[2-9]|\d\d\d*)$", ErrorMessage = "Values greater than 1 only")]
        public int PlateauSizeX { get; set; }

        [DisplayName("Plateau Size Y")]
        [RegularExpression(@"^(?:[2-9]|\d\d\d*)$", ErrorMessage = "Values greater than 1 only")]
        public int PlateauSizeY { get; set; }

        [DisplayName("Input")]
        [RegularExpression(@"^[LlMmRr]+$", ErrorMessage = "Values of L, R, and M only")]
        public string? Input { get; set; }


        [DisplayName("Final Position X")]
        public int? FinalPositionX { get; set; }

        [DisplayName("Final Position Y")]
        public int? FinalPositionY { get; set; }

        [DisplayName("Final Direction")]
        public char FinalDirection { get; set; }

        public string? PlateauMap { get; set; }

        

    }

}
