using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapTool.Core
{
    public class cPosition
    {

        #region Fields

        private float pX;
        private float pY;
        private float pZ;

        private bool pIsOffset;

        #endregion

        #region Properties

        public float X
        {
            get
            {
                return pX;
            }
            set
            {
                if (pIsOffset && value < 0.0 || value > 1920.0)
                {
                    throw new ArgumentOutOfRangeException("value", "This instance of cPosition is currently marked as isOffset. X,Y can only be between 0 and 1920");
                }
                else
                {
                    pX = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return pY;
            }
            set
            {
                if (pIsOffset && value < 0.0 || value > 1920.0)
                {
                    throw new ArgumentOutOfRangeException("value", "This instance of cPosition is currently marked as isOffset. X,Y can only be between 0 and 1920");
                }
                else
                {
                    pY = value;
                }
            }
        }

        public float Z
        {
            get
            {
                return pZ;
            }
            set
            {
                if (float.IsNaN(value))
                {
                    //Z = GetHeightFromNavmesh(pX,pY);
                    Z = 0;
                }
                else
                {
                    pZ = value;
                }
            }
        }

        public byte XSec
        {
            get
            {
                return (byte)Math.Round(Math.Floor((this.pX / 192f) + 135f));
            }
        }
        public byte YSec
        {
            get
            {
                return (byte)Math.Round(Math.Floor((this.pY / 192f) + 92f));
            }
        }

        public float XSec_Offset
        {
            get
            {
                int num = (int)Math.Round(this.pX % 192d);
                if (num < 0)
                {
                    num += 192;
                }
                return num;
            }
        }
        public float YSec_Offset
        {
            get
            {
                int num = (int)Math.Round(this.pY % 192d);
                if (num < 0)
                {
                    num += 192;
                }
                return num;
            }
        }

        public bool isOffset
        {
            get
            {
                return pIsOffset;
            }
            set
            {
                if (value) //convert to Offset
                {
                    if (pIsOffset) //is already offset
                    {
                        throw new Exception("Is already marked as Offset");
                    }
                    else //able to convert
                    {
                        pIsOffset = true;
                        this.X = ToOffsetX();
                        this.Y = ToOffsetY();
                    }
                }
                else //convert to GamePos
                {
                    if (pIsOffset == false) //is already offset
                    {
                        throw new Exception("Is not marked as Offset");
                    }
                    else //able to convert
                    {
                        pIsOffset = false;
                        this.X = ToGameX();
                        this.Y = ToGameY();
                    }
                }
            }
        }

        #endregion

        #region OffsetX <--> GameX

        public float ToOffsetX()
        {
            return ((pX - ((XSec) - 135) * 192) * 10);
        }
        public float ToGameX()
        {
            return ((XSec - 135) * 192 + (pX / 10));
        }

        #endregion

        #region OffsetY <--> GameY

        public float ToOffsetY()
        {
            return ((pY - ((YSec) - 92) * 192) * 10);
        }
        public float ToGameY()
        {
            return ((YSec - 135) * 192 + (pY / 10));
        }

        #endregion

        public string ToRegionId()
        {
            return pY.ToString("X2") + pX.ToString("X2");
        }

        #region Constructor

        public cPosition(float X, float Y, bool isOffset)
        {
            pIsOffset = isOffset;
            this.X = X;
            this.Y = Y;
            this.Z = float.NaN;
        }

        public cPosition(float X, float Y, float Z, bool isOffset)
        {
            pIsOffset = isOffset;
            this.X = X;
            this.Y = Y;
            this.Z = Z;

        }

        #endregion

        #region Operatoren

        public static cPosition operator +(cPosition a, cPosition b)
        {
            a.X += b.X;
            a.Y += b.Y;
            return a;
        }

        #endregion

        #region Distance Operations

        public double DistanceTo(cPosition B)
        {
            return GameDistance(pX, pY, B.X, B.Y);
        }

        private double GameDistance(float X1, float Y1, float X2, float Y2)
        {
            return Math.Sqrt(((X1 - X2) * (X1 - X2)) + ((Y1 - Y2) * (Y1 - Y2)));
        }

        #endregion

    }
}
