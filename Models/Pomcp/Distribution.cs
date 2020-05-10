using System;
using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
	public class Distribution<T>
	{
		
		public Dictionary<T, double> Prob { get; private set; }

		private Random rnd = new Random();

		
		public double GetProba(T element)
		{
			// If the element is in the distribution, return its probability
			if (Prob.ContainsKey(element))
				return (Prob[element]);

			// the element doesn't exist ==> null proba
			return (0);
		}

		
		
		public void SetProba(T element, double p)
		{
			Prob[element] = p;
		}

		
		public Distribution()
		{
			Prob = new Dictionary<T, double>();
		}
		
		
		public double GetNorm()
		{
			double c = 0;
			foreach (double s in Prob.Values)
			{
				c += s;
			}
			return c;

		}

		/// <summary>
		/// Normalize the density of probability
		/// </summary>
		public void Normalise()
		{
			double norm = GetNorm();
			Dictionary<T, double> newProb = new Dictionary<T, double>();
			foreach (KeyValuePair<T,double> pair in Prob)
			{
				newProb[pair.Key] = pair.Value / norm;
			}
			Prob = newProb;
		}
		
		/// <summary>
		/// Return a normalized copy of the distribution
		/// </summary>
		/// <returns></returns>
		public Distribution<T> GetNormalisedCopy()
		{
			Distribution<T> d = new Distribution<T>();
			double norm = GetNorm();
			foreach (KeyValuePair<T,double> pair in Prob)
			{
				d.SetProba(pair.Key, pair.Value / norm);
			}
			return d;
		}
		
		/// <summary>
		/// Return all the keys of the distibution
		/// </summary>
		/// <returns></returns>
		public Dictionary<T, double>.KeyCollection GetKeys()
		{
			return Prob.Keys;
		}


		public bool ContainsKey(T key)
		{
			return Prob.ContainsKey(key);
		}
		
		
		//
		/// <summary>
		/// Perform a random draw on the distribution
		/// </summary>
		/// <returns></returns>
		public T Draw()
		{
			if (Prob.Count == 0) return default(T);
			
			double p = rnd.NextDouble();
			Dictionary<T, double>.KeyCollection.Enumerator keys = Prob.Keys.GetEnumerator();
			T s = default(T);
			while (p > 0 && keys.MoveNext())
			{
				s = keys.Current;
				p = p - GetProba(s);
			}
			return s;
		}
	}
}