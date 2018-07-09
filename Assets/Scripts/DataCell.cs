using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsEntrance { get; set; }
    public bool IsExit { get; set; }
    public bool IsOccupied { get; set; }
    public bool GoUp { get; set; }
    public bool GoDown { get; set; }
    public bool GoLeft { get; set; }
    public bool GoRight { get; set; }    

    public int TileType
    {
        get
        {
            var value = 0;
            if (GoUp) value += 1;
            if (GoDown) value += 2;
            if (GoLeft) value += 4;
            if (GoRight) value += 8;

            return value;
        }
    }
}



