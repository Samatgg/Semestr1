using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestr1
{
    internal class Program
    {
        static double Const = 2 / Math.Sqrt(Math.PI);
        static double a = 0, b = 2;

        static List<double> Chebyshev(int n)
        {
            List<double> x_ch = new List<double>();
            double pi = Math.PI;

            if (n % 2 == 1)
            {
                for (int i = 0; i <= n; i++)
                {
                    double x_i = (b + a) / 2 + (b - a) * Math.Cos((2 * i + 1) * pi / (2 * n + 2)) / 2;
                    x_ch.Add(x_i);
                }
            }
            else
            {
                for (int i = 1; i <= n; i++)
                {
                    double x_i = (b + a) / 2 + (b - a) * Math.Cos((2 * i - 1) * pi / (2 * n)) / 2;
                    x_ch.Add(x_i);
                }
            }
            return x_ch;
        }

        static double Erf_T(double x)
        {
            double a_n = x;
            double summ = x;
            int n = 0;

            while (Math.Abs(a_n) >= 1e-6)
            {
                double q = (-1) * ((x * x) / ((2*n+3) * (2*n+2)));
                a_n *= q;
                summ += a_n;
                n++;
            }
            return summ * Const;
        }

        static double Lagrange(double[] x, double[] y, double value)
        {
            double result = 0;

            for (int i = 0; i < x.Length; i++)
            {
                double term = y[i];
                for (int j = 0; j < x.Length; j++)
                {
                    if (i != j)
                    {
                        term *= (value - x[j]) / (x[i] - x[j]);
                    }
                }
                result += term;
            }
            return result;
        }

        static double erf_ch(int n)
        {
            List<double> x_ch = Chebyshev(n);
            List<double> f_ch = new List<double>();

            foreach (var x in x_ch)
            {
                f_ch.Add(Erf_T(x));
            }

            double[] x_i = { 0.0, 0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0 };
            List<double> L_ch = new List<double>();
            List<double> err_ch = new List<double>();

            foreach (var xi in x_i)
            {
                L_ch.Add(Lagrange(x_ch.ToArray(), f_ch.ToArray(), xi));
                err_ch.Add(Math.Abs(Erf_T(xi) - L_ch.Last()));
            }

            return err_ch.Max();
        }
        static void Main(string[] args)
        {
            List<int> n = new List<int>();
            List<double> err = new List<double>();

            for (int i = 5; i < 45; i++)
            {
                if (i % 10 == 0)
                    continue;

                n.Add(i);
                double error = erf_ch(i);
                err.Add(error);
                Console.WriteLine($"{i} {error}");
            }
        }
    }
}
