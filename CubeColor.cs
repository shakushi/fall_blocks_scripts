using UnityEngine;

public class CubeColor
{
    public enum ColorCode
    {
        Red,
        Blue,
        Green,
        Yellow,
        Other
    }
    public ColorCode Code;
    private Color color;
    public Color Color
    {
        get { return color; }
    }
    public CubeColor()
    {
        ResetColor();
    }

    private bool isWarnColor = false;

    public Color TransColor()
    {
        if (Code == ColorCode.Red)
        {
            color = Color.yellow;
            Code = ColorCode.Yellow;
        }
        else if (Code == ColorCode.Yellow)
        {
            color = Color.green;
            Code = ColorCode.Green;
        }
        else if (Code == ColorCode.Green)
        {
            color = Color.blue;
            Code = ColorCode.Blue;
        }
        else if (Code == ColorCode.Blue)
        {
            color = Color.red;
            Code = ColorCode.Red;
        }
        else
        {
            color = Color.black;
            Code = ColorCode.Other;
        }

        //To be Light Color
        color += new Color(0.2f, 0.2f, 0.2f);

        return color;
    }

    public Color TransWarnColor()
    {
        if (isWarnColor)
        {
            return color;
        }
        isWarnColor = true;
        color -= new Color(0.2f, 0.2f, 0.2f);
        return color;
    }

    public Color ResetColor()
    {
        int index = Random.Range(0, 4);
        switch (index)
        {
            case 0:
                color = Color.red;
                Code = ColorCode.Red;
                break;
            case 1:
                color = Color.yellow;
                Code = ColorCode.Yellow;
                break;
            case 2:
                color = Color.green;
                Code = ColorCode.Green;
                break;
            case 3:
                color = Color.blue;
                Code = ColorCode.Blue;
                break;
            default:
                color = Color.black;
                Code = ColorCode.Other;
                break;
        }

        //To be Light Color
        color += new Color(0.2f, 0.2f, 0.2f);
        isWarnColor = false;

        return color;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }
        CubeColor c = (CubeColor)obj;
        return (this.Code == c.Code);
    }

    public override int GetHashCode()
    {
        return this.Code.GetHashCode();
    }
}
