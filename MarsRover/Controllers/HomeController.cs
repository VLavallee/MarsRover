using MarsRover.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;

namespace MarsRover.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        char leftTurn = 'L';
        char rightTurn = 'R';
        char moveForward = 'M';
        char north = 'N';
        char east = 'E';
        char south = 'S';
        char west = 'W';
        int finalPosX;
        int finalPosY;
        char finalDir;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RoverCalculation(Rover model)
        {
            CalculateRoverInput(model.sPosX, model.sPosY, model.sDir, model.Input);
            model.fPosX = finalPosX;
            model.fPosY = finalPosY;
            model.fDir = finalDir;
            return View(model);
        }

        public void CalculateRoverInput(int rovStartPosX, int rovStartPosY, char rovDir, string roverInput)
        {
            for (int i = 0; i < roverInput.Length; i++)
            {
                char getChar = roverInput[i];
                MoveRover(getChar, rovStartPosX, rovStartPosY, rovDir);
            }
            finalPosX = rovStartPosX;
            finalPosY = rovStartPosY;
            finalDir = rovDir;
            //string finalRovPosAndDir = rovStartPosX.ToString() + " " + rovStartPosY.ToString() + " " + rovDir.ToString();
            //return finalRovPosAndDir;
        }

        public void MoveRover(char MoveType, int posX, int posY, char rovDir)
        {
            if (MoveType == moveForward)
            {
                if (rovDir == north)
                {
                    posY++;
                }
                if (rovDir == east)
                {
                    posX++;
                }
                if (rovDir == south)
                {
                    posY--;
                }
                if (rovDir == west)
                {
                    posX--;
                }
                return;
            }


            if (MoveType == leftTurn)
            {
                if (rovDir == north)
                {
                    rovDir = west;
                    return;
                }
                if (rovDir == west)
                {
                    rovDir = south;
                    return;
                }
                if (rovDir == south)
                {
                    rovDir = east;
                    return;
                }
                if (rovDir == east)
                {
                    rovDir = north;
                    return;
                }
            }

            if (MoveType == rightTurn)
            {
                if (rovDir == north)
                {
                    rovDir = east;
                    return;
                }
                if (rovDir == east)
                {
                    rovDir = south;
                    return;
                }
                if (rovDir == south)
                {
                    rovDir = west;
                    return;
                }
                if (rovDir == west)
                {
                    rovDir = north;
                    return;
                }
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}