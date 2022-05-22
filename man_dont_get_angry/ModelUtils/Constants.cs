using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace man_dont_get_angry.ModelUtils
{
    public enum Color 
    {
        Red,
        Green,
        Blue,
        Yellow
    }

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

    public enum PlayerState
    { 
        ThrowDice,
        MovePieces,
        MovePiecesRepeadetly,
        MoveDone,
    }
}
