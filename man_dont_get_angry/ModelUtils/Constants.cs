using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace man_dont_get_angry.ModelUtils
{
    /// <summary>
    /// Defines constants for distinguishing between different colors
    /// </summary>
    public enum Color 
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    /// <summary>
    /// Defines constants for distinguishing between different field types
    /// </summary>
    public enum FieldType
    { 
        NormalField,
        RedArrowField,
        GreenArrowField,
        BlueArrowField,
        YellowArrowField,
        RedStartField,
        GreenStartField,
        BlueStartField,
        YellowStartField,
        RedEndField,
        GreenEndField,
        BlueEndField,
        YellowEndField,
    }

    /// <summary>
    /// Defines constants for distinguishing between different player states
    /// </summary>
    public enum PlayerState
    { 
        ThrowDice,
        MovePieces,
        MovePiecesRepeadetly,
        MoveDone,
    }
}
