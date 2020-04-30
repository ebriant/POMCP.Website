using System;
using System.Collections.Generic;

namespace POMCP.Website.Models.Pomcp
{
	public class Distribution<T>
	{


		/**
	 * Discrete list of probabilities
	 */
		private Dictionary<T, double> prob;

		public Random rnd = new Random();

		/**
	 * 
	 */
		public double getProba(T element)
		{
			// If the element is in the distribution, return its probability
			if (prob.ContainsKey(element))
				return (prob[element]);

			// the element doesn't exist ==> null proba
			return (0);
		}

		/**
	 * modifie la proba associee a l'element passe en parametre
	 */
		public void setProba(T element, double p)
		{
			prob[element] = p;
		}

		/**
	 * Constructeur genere une densité vide
	 * 
	 * attention, ce n'est pas une vraie densité (somme des proba != 1)
	 * 
	 * @liste liste des états de la densité
	 */
		public Distribution()
		{
			prob = new Dictionary<T, double>();
		}

		/**
	 * Constructeur genere une densité avec les memes états mais probabilités
	 * nulles
	 * 
	 * @param liste
	 *            des états de la densité
	 */
		public Distribution(Distribution<T> d)
		{
			prob = new Dictionary<T, double>();

			// creer les probabilités constantes à chaque état
			foreach (T s in d.prob.Keys)
			{
				prob[s] = 0;
			}
		}

		/**
	 * permet de calculer la norme d'une densite en thoerie retourne 1 mais peut
	 * servir pour normaliser
	 * 
	 * @return somme des probas de la densité
	 */
		public double GetNorm()
		{
			double c = 0;
			foreach (double s in prob.Values)
			{
				c += s;
			}

			return c;

		}

		/**
	 * Normalize the density of probability
	 */
		public void Normalise()
		{
			double norm = GetNorm();
			foreach (T s in prob.Keys)
			{
				setProba(s, prob[s] / norm);
			}
		}

		/**
	 * Return a normalized copy of the distribution
	 */
		public Distribution<T> GetNormalisedCopy()
		{
			Distribution<T> d = clone();
			d.Normalise();
			return d;
		}

		/**
	 * Return all the keys of the distibution
	 */
		public Dictionary<T, double>.KeyCollection GetKeys()
		{
			return prob.Keys;
		}


		public bool ContainsKey(T key)
		{
			return prob.ContainsKey(key);
		}


		/**
	 * fait un tirage aleatoire selon cette densité
	 * 
	 * @return l'indice qui aura été selectionné
	 */
		public T Draw()
		{
			if (prob.Count == 0) return default(T);
			
			double p = rnd.NextDouble();
			Dictionary<T, double>.KeyCollection.Enumerator keys = prob.Keys.GetEnumerator();
			T s = default(T);
			while (p > 0 && keys.MoveNext())
			{
				s = keys.Current;
				p = p - getProba(s);
			}

			return s;
		}

		// /**
		//  * methode toString
		//  */
		// public String toString() {
		// 	String res = "";
		// 	for (E clef : prob.keySet()) {
		// 		res += clef + "->" + df.format(prob.get(clef));
		// 		res += "\n";
		// 	}
		// 	return (res);
		//


		public Distribution<T> clone()
		{
			Distribution<T> d = new Distribution<T>();
			foreach (T e in prob.Keys)
			{
				d.setProba(e, getProba(e));
			}

			return d;
		}

	}
}