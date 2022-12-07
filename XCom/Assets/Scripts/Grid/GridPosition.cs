using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When using a struct, you get a copy with the passed values, with a class you get a reference to that object.
public struct GridPosition: IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z) //Constructor
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString() //We override this method that all C# objects have to tells us the positions we want and ease testing
    {
        return "x: "+ x + "; z: " + z;
    }

    public static bool operator ==(GridPosition a, GridPosition b) //We need to define == and != because we created our own struct
    {
        return a.x == b.x&& a.z == b.z;
    }

    public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
    public override bool Equals(object obj) //It's also recommended to create Equals and GetHashCode (Visual Studio takes care of it)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
}
