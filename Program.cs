using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDiametrSearch
{
	class Program
	{
		private static GraphSearch graphSearch;

		static void Main(string[] args)
		{
			graphSearch = new GraphSearch();

			graphSearch.FillNodes();
			graphSearch.StartNode();
			graphSearch.PrintGraph();
			graphSearch.BfsSearch();
			graphSearch.FindDiametr();

			Console.ReadLine();
		}
	}
}
