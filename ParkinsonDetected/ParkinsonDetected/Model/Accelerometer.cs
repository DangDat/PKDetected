using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkinsonDetected.Model
{
    public class Accelerometer
    {
        //public String AccelerometerID { get; set; }    
        public int Label { get; set; }
        public double X_axis { get; set; }
        public double Y_axis { get; set; }
        public double Z_axis { get; set; }
    }
}
