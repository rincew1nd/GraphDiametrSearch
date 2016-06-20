using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GraphDiametrSearch
{
	class GraphSearch
	{
		private List<Node> _nodeList;
		private Node _startNode;

		public GraphSearch()
		{
			_nodeList = new List<Node>();
		}

		/// <summary>
		/// Ввод графа пользователем
		/// </summary>
		public void FillNodes()
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("Введите смежность двух вершин (А - B). Для окончания ввода графа введите \"done\"");
				var input = Console.ReadLine();
				
				while (true)
				{
					if (input == "done" || input.Split(' ').Length == 3)
						break;
					Console.WriteLine("Невалидные данные. Повторите ввод:");
					input = Console.ReadLine();
				}

				if (input == "done")
					break;

				var nodeOne = GetNode(input.Split(' ')[0], true);
				var nodeTwo = GetNode(input.Split(' ')[2], true);

				nodeTwo.AddNeighbor(nodeOne);
				nodeOne.AddNeighbor(nodeTwo);
			}
		}

		/// <summary>
		/// Найти диаметр графа и построить путь
		/// </summary>
		public void FindDiametr()
		{
			var diametr = _nodeList.Max(z => z.Level);
			Console.WriteLine("Диаметр графа - " + diametr);

			var queue = new Queue<Node>();
			ReconstruatePath(queue, diametr);
		}

		private void ReconstruatePath(Queue<Node> path, int nextDiametr)
		{
			List<Node> nextNodes;
			if (path.Count == 0)
				nextNodes = _nodeList.Where(z => z.Level == nextDiametr).ToList();
			else
				nextNodes = _nodeList.Where(
					z => z.Level == nextDiametr &&
					(z.Level == 0 || z.Neighbors.Select(x => x.Level == nextDiametr--).Any())
				).ToList();
			
			foreach (var diametrNode in nextNodes)
			{
				if (nextDiametr == 0)
				{
					Console.Write("Путь - ");
					foreach (var pathNode in path.ToArray())
					{
						Console.Write(pathNode.Name + " ");
					}
					Console.WriteLine();
				}
				else
				{
					var queue = new Queue<Node>(path);
					queue.Enqueue(diametrNode);
					ReconstruatePath(queue, nextDiametr--);
				}
			}
		}

		/// <summary>
		/// Выбор стартовой ноды для алгоритма
		/// </summary>
		public void StartNode()
		{
			while (true)
			{
				Console.WriteLine("Введите стартовую вершину");
				Console.Write("Доступные ноды для старта - ");
				foreach (var nodeName in _nodeList)
					Console.Write(nodeName.Name + " ");
				Console.WriteLine("");

				var input = Console.ReadLine();
				_startNode = GetNode(input, false);

				if (_startNode == null)
				{
					Console.Clear();
					Console.WriteLine("Нода " + input + " не найдена!");
				}
				else
				{
					break;
				}
			}
		}


		/// <summary>
		/// Печать списка смежности в консоль
		/// </summary>
		public void PrintGraph()
		{
			foreach (var node in _nodeList)
			{
				Console.Write("Нода " + node.Name + " смежна с: ");
				foreach (var neghbor in node.Neighbors)
				{
					Console.Write(neghbor.Name + " ");
				}
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Обойти граф в ширину
		/// </summary>
		public void BfsSearch()
		{
			BFS(_startNode);
			while (_nodeList.Where(z => z.IsVisited == false).Any())
				BFS(_nodeList.Where(z => z.IsVisited == false).First());
		}

		private void BFS(Node startNode)
		{
			startNode.IsVisited = true;
			startNode.Level = 0;
			Console.WriteLine("Смотрим на " + startNode.Name + " с уровнем 0");

			var currentNodeQueue = new Queue<Node>(startNode.Neighbors);
			var futureNodeQueue = new Queue<Node>();
			var currentLevel = 1;

			while (currentNodeQueue.Count != 0)
			{
				foreach (var node in currentNodeQueue)
				{
					if (!node.IsVisited)
					{
						Console.WriteLine("Смотрим на " + node.Name + " с уровнем " + currentLevel);
						node.Level = currentLevel;
						node.IsVisited = true;
						foreach (var futureNode in node.Neighbors)
							futureNodeQueue.Enqueue(futureNode);
					}
				}
				currentLevel++;
				currentNodeQueue = new Queue<Node>(futureNodeQueue);
				futureNodeQueue.Clear();
			}
		}

		/// <summary>
		/// Получить ноду из списка (если canAdd == true, то нода будет автоматически добавлена если её всё ещё нет)
		/// </summary>
		/// <param name="name">Название ноды</param>
		/// <param name="canAdd">Добавить ли ноду если она не найдена</param>
		/// <returns></returns>
		public Node GetNode(string name, bool canAdd)
		{
			Node node;

			if (_nodeList.Where(z => z.Name == name).Any())
				node = _nodeList.Where(z => z.Name == name).First();
			else
			{
				if (canAdd)
				{
					node = new Node(name);
					_nodeList.Add(node);
				}
				else
					return null;
			}
			
			return node;
		}
	}
}
