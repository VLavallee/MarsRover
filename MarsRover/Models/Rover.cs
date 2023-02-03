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

        public string[] y5 = { "o", "o", "o", "o", "o", "o" };
        public string[] y4 = { "o", "o", "o", "o", "o", "o" };
        public string[] y3 = { "o", "o", "o", "o", "o", "o" };
        public string[] y2 = { "o", "o", "o", "o", "o", "o" };
        public string[] y1 = { "o", "o", "o", "o", "o", "o" };
        public string[] y0 = { "o", "o", "o", "o", "o", "o" };

        //public string[]? Y5 { get; set; }


        //public string? Y5X0, Y5X1, Y5X2, Y5X3, Y5X4, Y5X5;
        //public string? Y4X0, Y4X1, Y4X2, Y4X3, Y4X4, Y4X5;
        //public string? Y3X0, Y3X1, Y3X2, Y3X3, Y3X4, Y3X5;
        //public string? Y2X0, Y2X1, Y2X2, Y2X3, Y2X4, Y2X5;
        //public string? Y1X0, Y1X1, Y1X2, Y1X3, Y1X4, Y1X5;
        //public string? Y0X0, Y0X1, Y0X2, Y0X3, Y0X4, Y0X5;

    }
    
}
