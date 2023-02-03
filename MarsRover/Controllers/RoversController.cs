using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarsRover.Data;
using MarsRover.Models;

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

        

        int currentMove = 0;

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
        public async Task<IActionResult> Create([Bind("Id,Name,sPosX,sPosY,sDir,input,fPosX,fPosY,fDir")] Rover rover)
        {
            if (ModelState.IsValid)
            {
                CalculateRoverInput(rover.sPosX, rover.sPosY, rover.sDir, rover.input);
                rover.fPosX = finalPosX;
                rover.fPosY = finalPosY;
                rover.fDir = finalDir;
                _context.Add(rover);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rover);
        }

        

        // each time the rover moves it will save the position in a model for the plateau
        // lets map out all the positions for rover 1 first:
        // x1,y2 (start)
        // x0,y2
        // x0,y1
        // x1,y1
        // x1,y2
        // x1,y3 (finish)

        // the path taken will be marked sequentially. "o"s denote a
        // place the rover did not touch.
        // o,o,o,o,o,o
        // o,o,o,o,o,o
        // o,6,o,o,o,o
        // 2,1,o,o,o,o
        // 3,4,o,o,o,o
        // o,o,o,o,o,o

        
        //private void MarkPlateauCoordinate(int posX, int PosY, int currentMove)
        //{
        //    if(PosY == 0)
        //    {
        //        y0[posX] = currentMove.ToString();
        //        return;
        //    }
        //    if (PosY == 1)
        //    {
        //        y1[posX] = currentMove.ToString();
        //        return;
        //    }
        //    if (PosY == 2)
        //    {
        //        y2[posX] = currentMove.ToString();
        //        return;
        //    }
        //    if (PosY == 3)
        //    {
        //        y3[posX] = currentMove.ToString();
        //        return;
        //    }
        //    if (PosY == 4)
        //    {
        //        y4[posX] = currentMove.ToString();
        //        return;
        //    }
        //    if (PosY == 5)
        //    {
        //        y5[posX] = currentMove.ToString();
        //        return;
        //    }
        //}


        private void CalculateRoverInput(int rovStartPosX, int rovStartPosY, char startingDirection, string roverInput)
        {
            roverInput = roverInput.ToUpper();
            int currentPositionX = rovStartPosX;
            int currentPositionY = rovStartPosY;
            char direction = startingDirection;
            for (int i = 0; i < roverInput.Length; i++)
            {
                currentPositionX = MoveX(roverInput[i], currentPositionX, direction);
                currentPositionY = MoveY(roverInput[i], currentPositionY, direction);
                direction = ChangeDirection(roverInput[i], direction);

            }
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
