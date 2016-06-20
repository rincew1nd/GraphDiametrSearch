using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDiametrSearch
{
	class Node
	{
		public string Name;
		public bool IsVisited;
		public int Level;
		public List<Node> Neighbors;

		public Node(string name)
		{
			Name = name;
			Neighbors = new List<Node>();
			IsVisited = false;
			Level = 0;
		}

		public Node AddNeighbor(Node node)
		{
			if(!Neighbors.Contains(node))
				Neighbors.Add(node);
			return this;
		}
	}
}
