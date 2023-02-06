using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarsRover.Data;
using MarsRover.Models;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace MarsRover.Controllers
{
    public class RoversController : Controller
    {
        private readonly MarsRoverContext _context;

        #region Rover Params

        public static class Constants
        {
            public const char MoveForward = 'M';
            public const char TurnLeft = 'L';
            public const char TurnRight = 'R';

            public const char NorthUppercase = 'N';
            public const char EastUppercase = 'E';
            public const char SouthUppercase = 'S';
            public const char WestUppercase = 'W';

            public const char StartPoint = 'B';
            public const char FinishPoint = 'F';
        }

        int FinalPositionX { get; set; }
        int FinalPositionY { get; set; }
        char FinalDirection { get; set; }
        string? PlateauMap;
        #endregion

        public RoversController(MarsRoverContext context)
        {
            _context = context;
        }

        // GET: Rovers
        public async Task<IActionResult> Index()
        {
              return _context.Rover != null ? 
                          View(await _context.Rover.ToListAsync()) :
                          Problem("Entity set 'MarsRoverContext.Rover'  is null.");
        }

        // GET: Rovers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rover == null)
            {
                return NotFound();
            }

            var rover = await _context.Rover
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rover == null)
            {
                return NotFound();
            }

            return View(rover);
        }

        // GET: Rovers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rovers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PlateauSizeX,PlateauSizeY,StartingPositionX,StartingPositionY,StartingDirection,Input,FinalPositionX,FinalPositionY,FinalDirection,PlateauMap")] Rover rover)
        {
            if (ModelState.IsValid)
            {
                CalculateRoverInput(rover.PlateauSizeX, rover.PlateauSizeY, rover.StartingPositionX, rover.StartingPositionY, rover.StartingDirection, rover.Input);
                rover.FinalPositionX = FinalPositionX;
                rover.FinalPositionY = FinalPositionY;
                rover.FinalDirection = FinalDirection;
                rover.PlateauMap = PlateauMap;
                _context.Add(rover);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rover);
        }



        #region Plateau Map
        // DrawRoverPlateau will create an empty string named UnmarkedPlateau and will then add in a number of 'o' chars depending on the number of rows and columns given.
        // it will default to 6 rows and 6 columns, giving a total of 36 'o' chars within the string.
        // it is important to understand this plateau is drawn from the the top left to the bottom right,
        // meaning in this case the first 'o' char in the string is to be considered X0 Y5 and the last char 'o' is to be considered X5, Y0
        private void DrawRoverPlateau(int columns = 6, int rows = 6)
        {
            string UnmarkedPlateau = "";
            string NewPlateauPoint = "o";
            for (int i = 0; i < rows; i++)
            {
                for(int k = 0; k < columns; k++)
                {
                    UnmarkedPlateau += NewPlateauPoint;
                }
            }
            PlateauMap = UnmarkedPlateau;
        }


        // when the rover needs to mark the plateau map position, it first needs to calculate which section of the string to replace.
        // to do the calculation the number of characters per row must be given in addition to the position of the rover.


        
        // MarkPositionYCalculation is calculated by taking the characters per row and subtracting the current Y position plus -1
        // The additional -1 is subtracted because the rows go from 0 - 5, not 1 - 6
        // the final insertAt position is then calculated by taking MarkPositionYCalculation and multiplying it by the characters per row,
        // giving it the correct Y position, and then adding the current position X.
        private void MarkPlateauPosition(int currentPositionX, int currentPositionY, int plateauSizeY, char roverDirection)
        {
            int MarkPositionYCalculation = plateauSizeY - currentPositionY - 1;
            int insertAt = (plateauSizeY * MarkPositionYCalculation) + currentPositionX;

            // StringBuilder is used to create a new string equal to the existing PlateauMap string, and inserting a specified char in the calculated position.
            // the PlateauMap string is then changed to the StringBuilder string.
            StringBuilder sb = new StringBuilder(PlateauMap);
            if (roverDirection != Constants.StartPoint && roverDirection != Constants.FinishPoint)
            {
                if (sb[insertAt] != Constants.StartPoint)
                {
                    if (roverDirection == Constants.NorthUppercase)
                    {
                        sb[insertAt] = Constants.NorthUppercase;
                    }
                    else if (roverDirection == Constants.EastUppercase)
                    {
                        sb[insertAt] = Constants.EastUppercase;
                    }
                    else if (roverDirection == Constants.SouthUppercase)
                    {
                        sb[insertAt] = Constants.SouthUppercase;
                    }
                    else if (roverDirection == Constants.WestUppercase)
                    {
                        sb[insertAt] = Constants.WestUppercase;
                    }
                }
            }
            else if (roverDirection == Constants.StartPoint)
            {
                sb[insertAt] = Constants.StartPoint;
            }
            else if (roverDirection == Constants.FinishPoint)
            {
                sb[insertAt] = Constants.FinishPoint;
            }
            PlateauMap = sb.ToString();
        }

        
        
        #endregion

        #region Rover Input

        // Calculate rover input
        // changing roverInput to uppercase means the code only needs one char definition for 'M' 'L' and 'R' each.
        // position and direction will change several times so I create a variable to get and store them seperately from the initial start position and direction.
        // a new plateau map will be drawn for every rover input calculation.

        private void CalculateRoverInput(int plateauSizeX, int plateauSizeY, int rovStartPosX, int rovStartPosY, char startingDirection, string roverInput)
        {
            startingDirection = startingDirection.ToString().ToUpper()[0];
            roverInput = roverInput.ToUpper();
            
            int currentPositionX = rovStartPosX;
            int currentPositionY = rovStartPosY;
            char direction = startingDirection;
            
            DrawRoverPlateau(plateauSizeX, plateauSizeY);
            // this marks the starting position of the rover on the plateau map.
            MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY, Constants.StartPoint);

            // main calculation
            // the for loop checks at position i in the input string for directions and will continue until every character in the string has been checked.
            
            
            for (int i = 0; i < roverInput.Length; i++)
            {
                currentPositionX = MoveX(roverInput[i], currentPositionX, direction);
                currentPositionY = MoveY(roverInput[i], currentPositionY, direction);
                direction = ChangeDirection(roverInput[i], direction);

                // since the rover can turn 90 degrees, it does not need to mark its coordinates on the map for every move.
                // only if it moves forward will the MarkPlateauMap function run
                if (roverInput[i] == Constants.MoveForward)
                {
                    MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY, direction);
                }
            }

            // after every direction has been given the MarkPlateauMap function will run 1 more time in case the rover moved on its last input,
            // since the for loop will only run if the position i is less than the length of the string
            MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY, Constants.FinishPoint);
            //InsertPlateauMapLineReturns();
            FinalPositionX = currentPositionX;
            FinalPositionY = currentPositionY;
            FinalDirection = direction;
        }
        #endregion

        #region Rover Movement
        // MoveY uses position and direction to move on Y: if facing north it increases the value: if facing south it decreases the value
        private int MoveY(char roverInput, int currentPositionY, char direction)
        {
            if (roverInput == Constants.MoveForward)
            {
                if (direction == Constants.NorthUppercase)
                {
                    currentPositionY++;
                }
                
                if (direction == Constants.SouthUppercase)
                {
                    currentPositionY--;
                }
            }
            return currentPositionY;
        }
        // MoveX uses position and direction to move on X: if facing east it increases the value: if facing west it decreases the value
        private int MoveX(char roverInput, int currentPositionX, char direction)
        {
            if (roverInput == Constants.MoveForward)
            {
                if (direction == Constants.EastUppercase)
                {
                    currentPositionX++;
                }
                if (direction == Constants.WestUppercase)
                {
                    currentPositionX--;
                }
            }
            return currentPositionX;
        }
        // changes the direction the rover faces depending on the input and current direction
        private char ChangeDirection(char roverInput, char direction)
        {
            if (roverInput == Constants.TurnLeft)
            {
                if (direction == Constants.NorthUppercase)
                {
                    direction = Constants.WestUppercase;
                    return direction;
                }
                if (direction == Constants.WestUppercase)
                {
                    direction = Constants.SouthUppercase;
                    return direction;
                }
                if (direction == Constants.SouthUppercase)
                {
                    direction = Constants.EastUppercase;
                    return direction;
                }
                if (direction == Constants.EastUppercase)
                {
                    direction = Constants.NorthUppercase;
                    return direction;
                }
            }
            if (roverInput == Constants.TurnRight)
            {
                if (direction == Constants.NorthUppercase)
                {
                    direction = Constants.EastUppercase;
                    return direction;
                }
                if (direction == Constants.EastUppercase)
                {
                    direction = Constants.SouthUppercase;
                    return direction;
                }
                if (direction == Constants.SouthUppercase)
                {
                    direction = Constants.WestUppercase;
                    return direction;
                }
                if (direction == Constants.WestUppercase)
                {
                    direction = Constants.NorthUppercase;
                    return direction;
                }
            }
            return direction;
        }
        #endregion


        // GET: Rovers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rover == null)
            {
                return NotFound();
            }

            var rover = await _context.Rover.FindAsync(id);
            if (rover == null)
            {
                return NotFound();
            }
            return View(rover);
        }

        // POST: Rovers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PlateauSizeX,PlateauSizeY,StartingPositionX,StartingPositionY,StartingDirection,Input,FinalPositionX,FinalPositionY,FinalDirection")] Rover rover)
        {
            if (id != rover.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rover);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoverExists(rover.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rover);
        }

        // GET: Rovers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rover == null)
            {
                return NotFound();
            }

            var rover = await _context.Rover
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rover == null)
            {
                return NotFound();
            }

            return View(rover);
        }

        // POST: Rovers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rover == null)
            {
                return Problem("Entity set 'MarsRoverContext.Rover'  is null.");
            }
            var rover = await _context.Rover.FindAsync(id);
            if (rover != null)
            {
                _context.Rover.Remove(rover);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoverExists(int id)
        {
          return (_context.Rover?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
