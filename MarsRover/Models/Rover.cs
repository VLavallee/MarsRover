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
        public char fDir { get; set; }

        // plateau path data

        public string? pathData { get; set; }


        //public string[] y5 = { "o", "o", "o", "o", "o", "o" };
        //public string[] y4 = { "o", "o", "o", "o", "o", "o" };
        //public string[] y3 = { "o", "o", "o", "o", "o", "o" };
        //public string[] y2 = { "o", "o", "o", "o", "o", "o" };
        //public string[] y1 = { "o", "o", "o", "o", "o", "o" };
        //public string[] y0 = { "o", "o", "o", "o", "o", "o" };
        

    }

}
