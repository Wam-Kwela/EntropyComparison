using System;

namespace EntropyComparison
{
    // Zola Doctrine: entropy as slope
    public static class ZolaDoctrine
    {
        public static double Entropy(double radius, int dimension)
        {
            return radius / dimension;
        }

        public static double Heat(double radius, int dimension, double temperature)
        {
            return temperature * (radius / dimension);
        }
    }

    // Standard Model of Entropy (normalized, no kB)
    public static class StandardModelEntropy
    {
        public static double Entropy(double invariant)
        {
            return Math.Log(invariant);
        }

        public static double Fidelity(double entropy)
        {
            return 1.0 / (1.0 + Math.Max(0, -entropy));
        }
    }

    class Program
    {
        static void RunMonster(double m, double v, double omega, double S,
                               double T1, double T2, double C,
                               double epsilonR, double epsilon0,
                               double deltaE, double deltaT, double Gfactor,
                               string label)
        {
            // Hallways factors
            double Xk = (m * v * v) / (2 * S * (v * omega));
            double Xt = m * C * Math.Log(T2 / T1);
            double Xe = epsilonR * epsilon0 * (deltaE / deltaT);
            double Xg = Gfactor;

            // Unified invariant
            double Xi = Xk * Xt * Xe * Xg;

            // Entropy and fidelity
            double Stotal = StandardModelEntropy.Entropy(Xi);
            double Fmod = StandardModelEntropy.Fidelity(Stotal);

            Console.WriteLine($"\n=== {label} ===");
            Console.WriteLine($"Kinematic factor Xk = {Xk}");
            Console.WriteLine($"Thermal factor Xt = {Xt}");
            Console.WriteLine($"Electromagnetic factor Xe = {Xe}");
            Console.WriteLine($"Gravitational factor Xg = {Xg}");
            Console.WriteLine($"Unified invariant Ξ = {Xi}");
            Console.WriteLine($"Total entropy Stotal = {Stotal}");
            Console.WriteLine($"Fidelity modulation Fmod = {Fmod}");
        }

        static void PlotEntropyFidelity()
        {
            Console.WriteLine("\n=== Entropy vs Fidelity Curve ===");
            for (double Xi = 1e-5; Xi <= 1e5; Xi *= 10)
            {
                double S = StandardModelEntropy.Entropy(Xi);
                double F = StandardModelEntropy.Fidelity(S);
                Console.WriteLine($"Ξ = {Xi}, Entropy = {S}, Fidelity = {F}");
            }
        }

        static void Main()
        {
            // Zola Doctrine
            double r = 10.0;
            int n = 2;
            double T = 298.0;

            double S_zola = ZolaDoctrine.Entropy(r, n);
            double Q_zola = ZolaDoctrine.Heat(r, n, T);

            Console.WriteLine("=== Zola Doctrine ===");
            Console.WriteLine($"Entropy S = {S_zola}");
            Console.WriteLine($"Heat Q = {Q_zola}");

            double epsilon0 = 8.854e-12;

            // Monster Run (Ξ < 1, entropy negative, fidelity low)
            RunMonster(m: 2, v: 5, omega: 3, S: 0.5,
                       T1: 300, T2: 600, C: 1,
                       epsilonR: 80, epsilon0: epsilon0,
                       deltaE: 0.05, deltaT: 0.1, Gfactor: 1,
                       label: "Standard Model Monster Run (Ξ < 1)");

            // Activation Run (Ξ > 1, entropy positive, fidelity high)
            RunMonster(m: 2, v: 50, omega: 3, S: 0.5,
                       T1: 300, T2: 1200, C: 1,
                       epsilonR: 200, epsilon0: epsilon0,
                       deltaE: 0.5, deltaT: 0.1, Gfactor: 1,
                       label: "Activation Run (Ξ > 1)");

            // Plot entropy vs fidelity across a range of invariants
            PlotEntropyFidelity();
        }
    }
}

