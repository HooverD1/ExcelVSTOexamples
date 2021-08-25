﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Regression.Linear;

namespace DNA_Test
{
    public class Regression_SimpleLinear : IRegression   //Fake for testing the use of IRegression before fully implementing/defining classes that will implement IRegression
    {
        private OrdinaryLeastSquares ols = new OrdinaryLeastSquares();
        private SimpleLinearRegression slr { get; set; }
        private Random Rando = new Random();
        public double[] xVals { get; set; }
        public double[] yVals { get; set; }

        public Regression_SimpleLinear(double[] x, double[] y)
        {
            this.xVals = x;
            this.yVals = y;
            slr = ols.Learn(xVals, yVals);
        }

        public Regression_SimpleLinear(DateTime[] x, double[] y)
        {
            this.xVals = ConvertDatesToDoubles(x);
            this.yVals = y;
            slr = ols.Learn(xVals, yVals);
        }

        private double[] ConvertDatesToDoubles(DateTime[] dates)
        {
            double[] x = new double[dates.Length];
            for (int i = 0; i < dates.Length; i++)
            {
                x[i] = dates[i].ToOADate();
            }
            return x;
        }

        public double GetValue(double x)
        {
            return slr.Transform(x);
        }

        public double GetR2()
        {
            return slr.CoefficientOfDetermination(xVals, yVals);
        }

        public double GetAdjustedR2()
        {
            double r2 = slr.CoefficientOfDetermination(xVals, yVals);
            double tl = 1 - r2;
            double tr = yVals.Count() - 1;
            double bottom = yVals.Count() - 1 - 1;
            double adj_r2 = 1 - ((tl * tr) / bottom);
            return adj_r2;
        }

        public double Score()
        {
            return GetAdjustedR2();
        }

        public double GetPredictionError(double x)
        {
            return slr.GetPredictionStandardError(x, xVals, yVals);
        }

        public double GetMeanSquareError()
        {
            throw new NotImplementedException();
            //return slr.GetStandardError(xVals, yVals);      //I don't think this is right
        }

        public override string ToString()
        {
            return $"SLR: {Math.Round(slr.Slope, 1)}x + {Math.Round(slr.Intercept, 1)}";
        }
    }
}
