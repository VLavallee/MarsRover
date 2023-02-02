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

        // Rover Direction Params
        char leftTurn = 'L';
        char rightTurn = 'R';
        char moveForward = 'M';
        char north = 'N';
        char east = 'E';
        char south = 'S';
        char west = 'W';
        int tempPosX;
        int tempPosY;
        char tempDir;
        
        int finalPosX;
        int finalPosY;
        char finalDir;

        // Plateau Map Base Coordinates
        string[] y5 = { "o", "o", "o", "o", "o", "o" };
        string[] y4 = { "o", "o", "o", "o", "o", "o" };
        string[] y3 = { "o", "o", "o", "o", "o", "o" };
        string[] y2 = { "o", "o", "o", "o", "o", "o" };
        string[] y1 = { "o", "o", "o", "o", "o", "o" };
        string[] y0 = { "o", "o", "o", "o", "o", "o" };

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
        public async Task<IActionResult> Create([Bind("Id,Name,sPosX,sPosY,sDir,input,fPosX,fPosY,fDir, y5, y4, y3, y2, y1, y0")] Rover rover)
        {
            if (ModelState.IsValid)
            {
                CalculateRoverInput(rover.sPosX, rover.sPosY, rover.sDir, rover.input);
                rover.fPosX = finalPosX;
                rover.fPosY = finalPosY;
                rover.fDir = finalDir;
                rover.y5 = y5;
                rover.y4 = y4;
                rover.y3 = y3;
                rover.y2 = y2;
                rover.y1 = y1;
                rover.y0 = y0;
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
        // 2,5,o,o,o,o
        // 3,4,o,o,o,o
        // o,o,o,o,o,o

        
        private void MarkPlateauCoordinate(int posX, int PosY)
        {
            if(PosY == 0)
            {
                y0[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
            if (PosY == 1)
            {
                y1[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
            if (PosY == 2)
            {
                y2[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
            if (PosY == 3)
            {
                y3[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
            if (PosY == 4)
            {
                y4[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
            if (PosY == 5)
            {
                y5[posX] = currentMove.ToString();
                currentMove++;
                return;
            }
        }


        private void CalculateRoverInput(int rovStartPosX, int rovStartPosY, char rovDir, string roverInput)
        {
            tempPosX = rovStartPosX;
            tempPosY = rovStartPosY;
            tempDir = rovDir;
            for (int i = 0; i < roverInput.Length; i++)
            {
                MarkPlateauCoordinate(tempPosX, tempPosY);
                char getChar = roverInput[i];
                MoveRover(getChar, tempPosX, tempPosY, tempDir);
            }
            finalPosX = tempPosX;
            finalPosY = tempPosY;
            finalDir = tempDir;
        }
        private void MoveRover(char MoveType, int posX, int posY, char rovDir)
        {
            if (MoveType == moveForward)
            {
                if (rovDir == north)
                {
                    tempPosY++;
                }
                if (rovDir == east)
                {
                    tempPosX++;
                }
                if (rovDir == south)
                {
                    tempPosY--;
                }
                if (rovDir == west)
                {
                    tempPosX--;
                }
                return;
            }


            if (MoveType == leftTurn)
            {
                if (rovDir == north)
                {
                    tempDir = west;
                    return;
                }
                if (rovDir == west)
                {
                    tempDir = south;
                    return;
                }
                if (rovDir == south)
                {
                    tempDir = east;
                    return;
                }
                if (rovDir == east)
                {
                    tempDir = north;
                    return;
                }
            }

            if (MoveType == rightTurn)
            {
                if (rovDir == north)
                {
                    tempDir = east;
                    return;
                }
                if (rovDir == east)
                {
                    tempDir = south;
                    return;
                }
                if (rovDir == south)
                {
                    tempDir = west;
                    return;
                }
                if (rovDir == west)
                {
                    tempDir = north;
                    return;
                }
            }
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
