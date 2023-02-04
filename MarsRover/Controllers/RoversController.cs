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

namespace MarsRover.Controllers
{
    public class RoversController : Controller
    {
        private readonly MarsRoverContext _context;

        const char moveForward = 'M';
        const char turnLeft = 'L';
        const char turnRight = 'R';
        
        const char north = 'n';
        const char NORTH = 'N';
        const char east = 'e';
        const char EAST = 'E';
        const char south = 's';
        const char SOUTH = 'S';
        const char west = 'w';
        const char WEST = 'W';

        int finalPosX { get; set; }
        int finalPosY { get; set; }
        char finalDir { get; set; }

        string? thePlateau;
        int mapPositionY;

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
        public async Task<IActionResult> Create([Bind("Id,Name,sPosX,sPosY,sDir,input,fPosX,fPosY,fDir,pathData")] Rover rover)
        {
            if (ModelState.IsValid)
            {
                CalculateRoverInput(rover.sPosX, rover.sPosY, rover.sDir, rover.input);
                rover.fPosX = finalPosX;
                rover.fPosY = finalPosY;
                rover.fDir = finalDir;
                rover.pathData = thePlateau;
                _context.Add(rover);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rover);
        }

        private void DrawRoverPlateau(int rows = 6, int columns = 6)
        {
            string newPlateauPoint = "o";
            string plateau = "";
            
            for (int i = 0; i < rows; i++)
            {
                for(int k = 0; k < columns; k++)
                {
                    plateau += newPlateauPoint;
                }
            }
            thePlateau = plateau;
        }
        private void InsertPlateauMapLineReturns(int rows = 6, int charactersPerRow = 6)
        {
            int startingCharactersPerRow = charactersPerRow;
            
            for (int i = 0; i < rows - 1; i++)
            {
                int insertAt = charactersPerRow;
                thePlateau = thePlateau.Insert(insertAt, "\n");
                charactersPerRow = (charactersPerRow + startingCharactersPerRow) + 1;
            }
            
        }
        private void UpdateRoverPlateau(int currentPositionX, int currentPositionY)
        {
            int charactersPerRow = 6;
            currentPositionY = charactersPerRow - currentPositionY - 1;
            int insertAt = (charactersPerRow * currentPositionY) + currentPositionX;
            StringBuilder sb = new StringBuilder(thePlateau);
            sb[insertAt] = 'x';
            
            string updatedPlateau = sb.ToString();
            thePlateau = updatedPlateau;
        }

        private void CalculateRoverInput(int rovStartPosX, int rovStartPosY, char startingDirection, string roverInput)
        {
            roverInput = roverInput.ToUpper();
            int currentPositionX = rovStartPosX;
            int currentPositionY = rovStartPosY;
            char direction = startingDirection;
            DrawRoverPlateau();
            UpdateRoverPlateau(currentPositionX, currentPositionY);
            
            for (int i = 0; i < roverInput.Length; i++)
            {
                currentPositionX = MoveX(roverInput[i], currentPositionX, direction);
                currentPositionY = MoveY(roverInput[i], currentPositionY, direction);
                direction = ChangeDirection(roverInput[i], direction);
                if (roverInput[i] == moveForward)
                {
                    UpdateRoverPlateau(currentPositionX, currentPositionY);
                }
            }
            UpdateRoverPlateau(currentPositionX, currentPositionY);
            InsertPlateauMapLineReturns();
            finalPosX = currentPositionX;
            finalPosY = currentPositionY;
            finalDir = direction;
        }

        private int MoveY(char roverInput, int currentPositionY, char direction)
        {
            if (roverInput == moveForward)
            {
                if (direction == north || direction == NORTH)
                {
                    currentPositionY++;
                }
                
                if (direction == south || direction == SOUTH)
                {
                    currentPositionY--;
                }
            }
            return currentPositionY;
        }
        private int MoveX(char roverInput, int currentPositionX, char direction)
        {
            if (roverInput == moveForward)
            {
                if (direction == east || direction == EAST)
                {
                    currentPositionX++;
                }
                if (direction == west || direction == WEST)
                {
                    currentPositionX--;
                }
            }
            return currentPositionX;
        }
        private char ChangeDirection(char roverInput, char direction)
        {
            if (roverInput == turnLeft)
            {
                if (direction == north || direction == NORTH)
                {
                    direction = west;
                    return direction;
                }
                if (direction == west || direction == WEST)
                {
                    direction = south;
                    return direction;
                }
                if (direction == south || direction == SOUTH)
                {
                    direction = east;
                    return direction;
                }
                if (direction == east || direction == EAST)
                {
                    direction = north;
                    return direction;
                }
            }
            if (roverInput == turnRight)
            {
                if (direction == north || direction == NORTH)
                {
                    direction = east;
                    return direction;
                }
                if (direction == east || direction == EAST)
                {
                    direction = south;
                    return direction;
                }
                if (direction == south || direction == SOUTH)
                {
                    direction = west;
                    return direction;
                }
                if (direction == west || direction == WEST)
                {
                    direction = north;
                    return direction;
                }
            }
            return direction;
        }
        


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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,sPosX,sPosY,sDir,Input,fPosX,fPosY,fDir")] Rover rover)
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
