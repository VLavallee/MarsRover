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

        const char MoveForward = 'M';
        const char TurnLeft = 'L';
        const char TurnRight = 'R';
        
        const char NorthLowercase = 'n';
        const char NorthUppercase = 'N';
        const char EastLowercase = 'e';
        const char EastUppercase = 'E';
        const char SouthLowercase = 's';
        const char SouthUppercase = 'S';
        const char WestLowercase = 'w';
        const char WestUppercase = 'W';

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


        // add check for characters that are not L M R

        private void MarkPlateauPosition(int currentPositionX, int currentPositionY, int plateauSizeY)
        {
            // MarkPositionYCalculation is calculated by taking the characters per row and subtracting the current Y position plus -1
            // The additional -1 is subtracted because the rows go from 0 - 5, not 1 - 6
            // the final insertAt position is then calculated by taking MarkPositionYCalculation and multiplying it by the characters per row,
            // giving it the correct Y position, and then adding the current position X.
            
            int MarkPositionYCalculation = plateauSizeY - currentPositionY - 1;
            int insertAt = (plateauSizeY * MarkPositionYCalculation) + currentPositionX;

            // StringBuilder is then used to create a new string equal to the existing PlateauMap string, and inserting an 'x' char in the calculated position.
            // the PlateauMap string is then changed to the StringBuilder string.
            StringBuilder sb = new StringBuilder(PlateauMap);
            try
            {
                sb[insertAt] = 'x';
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught " + ex);
            }
            PlateauMap = sb.ToString();
        }

        // this function will simply insert a line return after every row except the last
        // I found it much simpler to do this after the map has been marked.
        private void InsertPlateauMapLineReturns(int rows = 6, int charactersPerRow = 6)
        {
            int startingCharactersPerRow = charactersPerRow;

            for (int i = 0; i < rows - 1; i++)
            {
                try
                {
                    PlateauMap = PlateauMap.Insert(charactersPerRow, "\n");
                    charactersPerRow = (charactersPerRow + startingCharactersPerRow) + 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught " + ex);
                }
            }
        }
        #endregion

        #region Rover Input
        // Calculate rover input
        private void CalculateRoverInput(int plateauSizeX, int plateauSizeY, int rovStartPosX, int rovStartPosY, char startingDirection, string roverInput)
        {
            // changing roverInput to uppercase means the code only needs one char definition for 'M' 'L' and 'R' each.
            roverInput = roverInput.ToUpper();
            // position and direction will change several times so I create a variable to get and store them seperately from the initial start position and direction.
            int currentPositionX = rovStartPosX;
            int currentPositionY = rovStartPosY;
            char direction = startingDirection;
            // a new plateau map will be drawn for every rover input calculation.
            DrawRoverPlateau(plateauSizeX, plateauSizeY);
            // this marks the starting position of the rover on the plateau map.
            MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY);

            // main calculation
            // the for loop checks at position i in the input string for directions and will continue until every character in the string has been checked.
            
            
            for (int i = 0; i < roverInput.Length; i++)
            {
                currentPositionX = MoveX(roverInput[i], currentPositionX, direction);
                currentPositionY = MoveY(roverInput[i], currentPositionY, direction);
                direction = ChangeDirection(roverInput[i], direction);

                // since the rover can turn 90 degrees, it does not need to mark its coordinates on the map for every move.
                // only if it moves forward will the MarkPlateauMap function run
                if (roverInput[i] == MoveForward)
                {
                    MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY);
                }
            }

            // after every direction has been given the MarkPlateauMap function will run 1 more time in case the rover moved on its last input,
            // since the for loop will only run if the position i is less than the length of the string
            MarkPlateauPosition(currentPositionX, currentPositionY, plateauSizeY);
            InsertPlateauMapLineReturns();
            FinalPositionX = currentPositionX;
            FinalPositionY = currentPositionY;
            FinalDirection = direction;
        }
        #endregion

        #region Rover Movement
        // MoveY uses position and direction to move on Y: if facing north it increases the value: if facing south it decreases the value
        private int MoveY(char roverInput, int currentPositionY, char direction)
        {
            if (roverInput == MoveForward)
            {
                if (direction == NorthLowercase || direction == NorthUppercase)
                {
                    currentPositionY++;
                }
                
                if (direction == SouthLowercase || direction == SouthUppercase)
                {
                    currentPositionY--;
                }
            }
            return currentPositionY;
        }
        // MoveX uses position and direction to move on X: if facing east it increases the value: if facing west it decreases the value
        private int MoveX(char roverInput, int currentPositionX, char direction)
        {
            if (roverInput == MoveForward)
            {
                if (direction == EastLowercase || direction == EastUppercase)
                {
                    currentPositionX++;
                }
                if (direction == WestLowercase || direction == WestUppercase)
                {
                    currentPositionX--;
                }
            }
            return currentPositionX;
        }
        // changes the direction the rover faces depending on the input and current direction
        private char ChangeDirection(char roverInput, char direction)
        {
            if (roverInput == TurnLeft)
            {
                if (direction == NorthLowercase || direction == NorthUppercase)
                {
                    direction = WestLowercase;
                    return direction;
                }
                if (direction == WestLowercase || direction == WestUppercase)
                {
                    direction = SouthLowercase;
                    return direction;
                }
                if (direction == SouthLowercase || direction == SouthUppercase)
                {
                    direction = EastLowercase;
                    return direction;
                }
                if (direction == EastLowercase || direction == EastUppercase)
                {
                    direction = NorthLowercase;
                    return direction;
                }
            }
            if (roverInput == TurnRight)
            {
                if (direction == NorthLowercase || direction == NorthUppercase)
                {
                    direction = EastLowercase;
                    return direction;
                }
                if (direction == EastLowercase || direction == EastUppercase)
                {
                    direction = SouthLowercase;
                    return direction;
                }
                if (direction == SouthLowercase || direction == SouthUppercase)
                {
                    direction = WestLowercase;
                    return direction;
                }
                if (direction == WestLowercase || direction == WestUppercase)
                {
                    direction = NorthLowercase;
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
