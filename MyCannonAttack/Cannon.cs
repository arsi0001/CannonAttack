using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCannonAttack
{
    public class Cannon
    {
        private readonly string CANNONID = "Human";
        public static readonly int MAXANGLE = 90;
        public static readonly int MINANGLE = 1;
        public static readonly int MAXVELOCITY = 300000000;
        private int distanceOfTarget;
        private readonly double GRAVITY = 9.8;
        private readonly int BURSTRADIUS = 50;
        private static readonly int MAXDISTANCEOFTARGET= 20000; 
        private string CannonID;
        private static Cannon cannonSingletonInstance;
        private static readonly object padlock = new object();
        private int shots;

        #region
        public int DistnaceOfTarget
        {
            get { return distanceOfTarget; }
            set { distanceOfTarget = value; }
        }

        public int Shots {
            get
            {
                return shots;
            }
        }

        public string ID
        {
            get
            {
                return (String.IsNullOrWhiteSpace(CannonID)) ? CANNONID : CannonID;
            }
            set
            {
                CannonID = value;
            }
        }
        #endregion


        public Cannon()
        {
            Random r = new Random();
            SetTarget(r.Next(MAXDISTANCEOFTARGET));

        }



        public Tuple<bool, string> Shoot(int angle, int velocity)
        {
            shots++;
            if (velocity > MAXVELOCITY)
                return Tuple.Create(false, "Velocity of the cannon travel faster than the speed of light");
            if (angle > MAXANGLE || angle < MINANGLE)
                return Tuple.Create(false, "Angle Incorrect");

            string message;
            bool hit;
            int distanceOfShot = CalculateDistanceOfCannonShot(angle, velocity);
            if(distanceOfShot.WithinRange(this.distanceOfTarget, BURSTRADIUS))
            {
                message = String.Format("Hit - {0} Shot(s)", shots);
                hit = true;
            }else
            {
                message = String.Format("Missed cannonball landed at {0} meters", distanceOfShot);
                hit = false;
            }
            
            return Tuple.Create(hit, message);
        }

        public static Cannon GetInstance()
        {
            lock (padlock)
            {
                if (cannonSingletonInstance == null)
                {
                    cannonSingletonInstance = new Cannon();
                }
                return cannonSingletonInstance;
            }
        }

        public void SetTarget(int distanceOfTarget)
        {
            if(!distanceOfTarget.Between(0, MAXDISTANCEOFTARGET))
            {
                throw new ApplicationException(String.Format("Target distance must be between 1 and {0} meters", MAXDISTANCEOFTARGET));
            }
            this.distanceOfTarget = distanceOfTarget;
        }

        public int CalculateDistanceOfCannonShot(int angle, int velocity)
        {
            int time = 0;
            double height = 0;
            double distance = 0;
            double angleInRadians = (3.1415926536 / 180) * angle;
            while (height >= 0)
            {
                time++;
                distance = velocity * Math.Cos(angleInRadians) * time;
                height = (velocity * Math.Sin(angleInRadians) * time) - (GRAVITY * Math.Pow(time, 2)) / 2;
            }
            return (int)distance;
        }

        public void Reset()
        {
            shots = 0;
        }
    }
}
