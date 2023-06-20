using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

namespace GuidancePlatform
{
	internal enum Direction
	{
		left=-1,
		right=1
	}
	internal enum GateType
	{
		walk,
		fall,
		jump,
		falljump
	}
	internal static class DirUtil
	{
		internal static Direction Reverse(Direction dir)
		{
			return (Direction)(-(int)dir);
		}
	}

	public class Area
	{
		private int startX;
		private int endX;
		public int y { get; private set; }
		public bool enabled { get; private set; }
		private List<Gate> _gates;
		public List<Gate> gates
		{
			get
			{
				if(this._gates == null) this._gates = new List<Gate>();
				return this._gates;
			}
		}
		public Vector2Int left
		{
			get{ return new Vector2Int(startX, y); }
		}
		public Vector2Int right
		{
			get{ return new Vector2Int(endX, y); }
		}
		public int length
		{
			get{ return this.endX - this.startX + 1; }
		}

		internal Vector2Int GetEdge(Direction dir)
		{
			return (dir == Direction.left) ? left : right;
		}
		internal Area(int startX, int endX, int y)
		{
			this.startX = startX;
			this.endX = endX;
			if(startX > endX) (this.startX, this.endX) = (endX, startX);
			this.y = y;
			this.enabled = true;
		}

		// debug method
		public void Debug(Vector3Int offset)
		{
			Vector3 center = new Vector3((startX + endX)/2.0f + 0.5f, y+0.5f, 0) + (Vector3)offset;
			Utils.DrawBound( new Bounds(center, new Vector3(length, 0.9f, 0)), Color.green );
			foreach(Gate gate in gates)
			{
				gate.Debug(offset);
			}
		}
		public override string ToString()
		{
			return string.Format("Platform - start:({0},{2}) end:({1},{2})", startX, endX, y);
		}
	}

	public class Gate
	{
		private float _dist;
		private Direction direction;
		internal GateType type {get; private set;}
		public Area start {get; private set;}
		public Area end {get; private set;}
		public Vector2Int startPoint {get; private set;}
		public Vector2Int endPoint
		{
			get
			{
				if (this.type == GateType.fall)
				{
					return new Vector2Int(this.startPoint.x + (int)this.direction, this.end.y);
				}

				if (this.direction == Direction.left) return this.end.right;
				return this.end.left;
			}
		}
		public float distance
		{
			get
			{
				if(!end.enabled) return float.PositiveInfinity;
				return _dist;
			}
		}
		public bool isAvailable
		{
			get{ return true; }
		}

		private Gate(Area a, Area b, Direction dir, GateType type, Vector2Int startPoint)
		{
			this.start = a;
			this.end = b;
			this.direction = dir;
			this.type = type;
			this.startPoint = startPoint;
			this._dist = Vector2Int.Distance(this.startPoint, this.endPoint);
		}
		internal Gate(Area a, Area b, Direction dir, GateType type)
		: this(a, b, dir, type, a.GetEdge(dir))
		{
		}
		internal Gate(Area a, Area b, Direction dir, GateType type, int startX)
		: this(a, b, dir, type, new Vector2Int(startX, a.y))
		{
		}

		// debug method
		public void Debug(Vector3Int offset)
		{
			Vector3 start = (Vector3)(Vector2)this.startPoint + (Vector3)offset + new Vector3(0.4f, 0.4f, 0f);
			Vector3 end = (Vector3)(Vector2)this.endPoint + (Vector3)offset + new Vector3(0.6f, 0.6f, 0f);
			Color color = GetDebugColor();

			UnityEngine.Debug.DrawLine(start, end, color, 100f);
		}
		private Color GetDebugColor()
		{
			switch(this.type)
			{
				case GateType.fall: return Color.red;
				case GateType.jump: return Color.blue;
				case GateType.falljump: return Color.magenta;
				default: return Color.white;
			}
		}
		public override string ToString()
		{
			return string.Format("Gate - start:{0} end:{1} type:{2}", startPoint, endPoint, type);
		}
	}

	public struct Instructor
	{
		public float x{get; internal set;}
		public bool isJump{get; internal set;}
		public bool cancelWhenGrounded{get; internal set;}
		public override string ToString()
		{
			if(isJump) return string.Format("Instructor - jump and go to x:{0}", x);
			return string.Format("Instructor - Go to x:{0}", x);
		}
	}
	internal class Pathfinder
	{
		private struct PathfindData
		{
			internal bool isChecked;
			internal float cost;
			internal Gate previousGate;
			internal PathfindData WithChecked()
			{
				PathfindData res = this;
				res.isChecked = true;
				return res;
			}
			public override string ToString()
			{
				return string.Format("PlatformPathfindData - isChecked : {0}, cost : {1}, prevGate : {2}", 
					isChecked, cost, previousGate);
			}
		}
		public List<Instructor> Pathfind(Graph graph, Area startNode, Area endNode, Vector2Int destination)
		{
			SimplePriorityQueue<Area, float> priorityQueue = new SimplePriorityQueue<Area, float>();
			Dictionary<Area, PathfindData> nodeData = new Dictionary<Area, PathfindData>();
			priorityQueue.Enqueue(startNode, 0f);
			foreach(Area node in graph)
			{
				nodeData[node] = new PathfindData{
					isChecked = false,
					cost = (node == startNode) ? 0f : float.PositiveInfinity
				};
			}

			while(priorityQueue.Count > 0)
			{
				Area node = priorityQueue.Dequeue();
				if(node == endNode) return MakeInstructor(nodeData, endNode);

				if(nodeData[node].isChecked == true) continue;
				nodeData[node] = nodeData[node].WithChecked();
				foreach(Gate gate in node.gates)
				{
					Area nextNode = gate.end;
					if(!gate.isAvailable) continue;
					if(nodeData[nextNode].isChecked) continue;
					float gateCost = GetCost(gate, nodeData[node].previousGate);
					float nextNodeCost = gateCost + nodeData[node].cost + GetHeuristicCost(gate, destination);
					if(nextNodeCost < nodeData[nextNode].cost)
					{
						PathfindData newNextNodeData = nodeData[nextNode];
						newNextNodeData.cost = nextNodeCost;
						newNextNodeData.previousGate = gate;
						nodeData[nextNode] = newNextNodeData;
						priorityQueue.Enqueue(nextNode, nextNodeCost);
					}
				}
			}

			return null;
		}
		
		private float GetCost(Gate gate, Gate previousGate)
		{
			if(previousGate == null) return gate.distance;
			return Math.Abs(gate.startPoint.x - previousGate.endPoint.x) + gate.distance;
		}
		private float GetHeuristicCost(Area node, Vector2Int destination)
		{
			float distLeft = Vector2Int.Distance(node.left, destination);
			float distRight = Vector2Int.Distance(node.right, destination);
			if(distLeft > distRight) return distRight;
			return distLeft;
		}
		private float GetHeuristicCost(Gate gate, Vector2Int destination)
		{
			return Vector2Int.Distance(gate.endPoint, destination);
		}
		private List<Instructor> MakeInstructor(Dictionary<Area, PathfindData> nodeData, Area endNode)
		{
			List<Instructor> result = new List<Instructor>();
			PathfindData current = nodeData[endNode];
			while(current.previousGate != null)
			{
				Gate gate = current.previousGate;

				// If Previous Walk Instructor's x == gate.endPoint.x, pop walk instructor
				if(result.Count > 0 && result[result.Count-1].x == gate.endPoint.x)
				{
					result.RemoveAt(result.Count-1);
				}

				// Add Jump Instructor
				Instructor instructor = new Instructor{
					x = gate.endPoint.x,
					isJump=(gate.type == GateType.jump)
				};
				result.Add(instructor);

				// Add Walk Instructor
				Instructor walkInstructor = new Instructor{
					x = gate.startPoint.x
				};
				result.Add(walkInstructor);
				current = nodeData[gate.start];
			}

			result.Reverse();
			return result;
		}
	}
	public class TrailMaker : IPathfinder
	{
		private Vector2Int offset;
		private int[,] tiles;
		private Graph graph;
		private Pathfinder pathfinder = new Pathfinder();
		public TrailMaker(int[,] tiles, Graph graph, Vector2Int offset)
		{
			this.tiles = tiles;
			this.graph = graph;
			this.offset = offset;
		}
		public Queue<Instructor> Find(Vector2Int start, Vector2Int end)
		{
			List<Instructor> result = _FindTrail(start - this.offset, end - this.offset);
			if(result == null) return null;
			return AdjustInstructorsX(result, this.offset.x);
		}
		public Queue<Instructor> Find(Vector3 start, Vector3 end)
		{
			Vector2Int realStart = graph.GetValidTopPosition((Vector2)start - (Vector2)this.offset, 0.5f);
			return Find(realStart + this.offset, Utils.GetCellPosition(end));
		}
		private List<Instructor> _FindTrail(Vector2Int start, Vector2Int end)
		{
			int endDir = Math.Sign(start.x - end.x);
			if(endDir == 0) endDir = 1;
			Area startPlatform = graph.GetCurrentPlatform(start);
			(Area endPlatform, int lastX) = GetEndPlatform(end, endDir);
			if(startPlatform == null || endPlatform == null) return null;

			// 시작지점과 끝지점이 같은 경우
			if(startPlatform == endPlatform) return GetLastInstructor(end, endPlatform, lastX, start.x);

			// 시작지점과 끝지점이 다른 경우
			List<Instructor> paths = pathfinder.Pathfind(graph, startPlatform, endPlatform, end);

			if(paths == null || paths.Count == 0) return null;

			// 마지막 인스트럭터를 추가
			int preLastX = (int)paths[paths.Count-1].x;
			int realEndDir = Math.Sign(preLastX - end.x);
			(_, int realLastX) = GetEndPlatform(end, realEndDir);
			List<Instructor> lastPaths = GetLastInstructor(end, endPlatform, realLastX, preLastX);
			foreach(Instructor instr in lastPaths) paths.Add(instr);

			return paths;
		}
		private Queue<Instructor> AdjustInstructorsX(List<Instructor> origin, int xOffset)
		{
			int count = origin.Count;
			Queue<Instructor> result = new Queue<Instructor>(count);
			for(int i=0; i<count; i++)
			{
				Instructor newItem = origin[i];

				newItem.x = origin[i].x + xOffset + 0.5f;
				if(i < count-1 && origin[i+1].isJump)
				{
					float xZitter = origin[i+1].x - origin[i].x;
					// 마지막에서 강제점프
					if(i == count - 2 && origin[i+1].cancelWhenGrounded)
					{
						if(xZitter > 0f) newItem.x -= 0.3f;
						else if(xZitter < 0f) newItem.x += 0.3f;
					}
					// 점프 중 1칸 떨어진 점프를 해야 할 때
					else
					{
						if(xZitter == 1f) newItem.x -= 0.1f;
						else if(xZitter == -1f) newItem.x += 0.1f;
					}
				}
				result.Enqueue(newItem);
			}
			return result;
		}
		private (Area, int) GetEndPlatform(Vector2Int end, int dir)
		{
			Dictionary<int, bool> emptyChecker = new Dictionary<int, bool>()
			{
				[-1] = true,
				[0] = true,
				[1] = true
			};
			// y에서 0~1칸 떨어져 있을 때
			int[] xLooper = {0, 1, -1};
			for(int dy=0; dy<=1; dy++)
			{
				foreach(int _dx in xLooper)
				{
					int dx = _dx * dir;
					if(!emptyChecker[dx]) continue;
					int x = end.x+dx;
					int y = end.y-dy;
					if(graph[x, y] != null) return (graph[x, y], end.x);
					else if(!TileMask.IsEmpty(tiles[x,y])) emptyChecker[dx] = false;
				}
			}
			if(!TileMask.IsEmpty(tiles[end.x+2,end.y-1])) emptyChecker[1] = false;
			if(!TileMask.IsEmpty(tiles[end.x-2,end.y-1])) emptyChecker[-1] = false;

			// y에서 2~4칸 떨어져 있으면 
			for(int dy=2; dy<5; dy++)
			{
				for(int a=2; a<dy*2+1; a++)
				{
					int dx = dir * (a%2 == 0 ? 1 : -1) * (dy-(a/2));
					int dxSign = Math.Sign(dx);
					if(!emptyChecker[dxSign]) continue;

					int x = end.x+dx;
					int y = end.y-dy;
					if(!TileMask.IsEmpty(tiles[x,y])) emptyChecker[dxSign] = false;
					else if(graph[x, y] != null)
					{
						if(dy != 4 || IsJumpable(x, y)) return (graph[x, y], x);
					}
				}
			}
			return (null, 0);
		}
		private List<Instructor> GetLastInstructor(Vector2Int end, Area endPlatform, int jumpX, int preLastX)
		{
			List<Instructor> instructors = new List<Instructor>();
			int yOffset = end.y - endPlatform.y;
			int direction = Math.Sign(end.x - jumpX);
			// 점프를 안 해도 됨
			if(yOffset <= 1)
			{
				instructors.Add(new Instructor{x=end.x});
				return instructors;
			}
			if(preLastX != jumpX) instructors.Add(new Instructor{x=jumpX});
			// direction == 0이면 제자리 점프, 아니면 풀점프
			int jumpDestination = jumpX;
			if(direction != 0) jumpDestination = jumpX + direction * Constants.maxJumpWidth;

			instructors.Add(new Instructor{x=jumpDestination, isJump=true, cancelWhenGrounded=true});
			return instructors;
		}
		private bool IsJumpable(int x, int y)
		{
			if(!TileMask.IsEmpty(tiles[x,y+2])) return false;
			if(!TileMask.IsEmpty(tiles[x,y+3])) return false;
			return true;
		}
	}

	public class Graph : IEnumerable
	{
		public int width{ get; private set; }
		public int height{ get; private set; }
		private List<Area> platforms = new List<Area>();
		private Area[,] tileRef;
		public Graph(int width, int height)
		{
			this.width = width;
			this.height = height;
			tileRef = new Area[width, height];
		}
		public Area this[int x, int y]
		{
			get { return this.tileRef[x, y]; }
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return platforms.GetEnumerator();
		}
		public Area GetCurrentPlatform(int x, int y)
		{
			for(int yIdx=y; yIdx>=0; yIdx--)
			{
				if(tileRef[x, yIdx] != null) return tileRef[x, yIdx];
			}
			return null;
		}
		public Area GetCurrentPlatform(Vector2Int pos)
		{
			return this.GetCurrentPlatform(pos.x, pos.y);
		}
		public Vector2Int GetValidTopPosition(Vector2 pos, float boundWidth)
		{
			int left = (int)Mathf.Floor(pos.x - boundWidth*0.5f);
			int right = (int)Mathf.Floor(pos.x + boundWidth*0.5f);
			int center = (int)Mathf.Floor(pos.x);
			int y = (int)Mathf.Floor(pos.y);
			if(left == right) return new Vector2Int(left, y);

			for(int yIdx=y; yIdx>=0; yIdx--)
			{
				if(tileRef[center, yIdx] != null) return new Vector2Int(center, y);
				if(tileRef[left, yIdx] != null) return new Vector2Int(left, y);
				if(tileRef[right, yIdx] != null) return new Vector2Int(right, y);
			}
			return new Vector2Int(center, y);
		}
		internal Area AddPlatform(int startX, int endX, int y)
		{
			Area platform = new Area(startX, endX, y);
			for(int x=startX; x<=endX; x++) tileRef[x, y] = platform;
			platforms.Add(platform);
			return platform;
		}
		internal Gate LinkPlatform(Area a, Area b, Direction dir, GateType type)
		{
			Gate gate = new Gate(a, b, dir, type);
			a.gates.Add(gate);
			return gate;
		}
		internal Gate LinkPlatform(Area a, Area b, Direction dir, GateType type, int startX)
		{
			Gate gate = new Gate(a, b, dir, type, startX);
			a.gates.Add(gate);
			return gate;
		}
		// debug method
		public void Debug(Vector3Int offset)
		{
			foreach(Area platform in platforms)
			{
				platform.Debug(offset);
			}
		}
	}

	public static class GraphMaker
	{
		[Flags]
		private enum JumpableFlag
		{
			none=0,
			forward=1,
			backward=2,
			both=3,
			backwardFall=4,
			bothFall=5
		}
		private enum CellingJumpFlag
		{
			unable,
			empty,
			hitCelling
		}

		public static Graph Make(int[,] tiles)
		{
			Graph graph = MakePlatformBase(tiles);
			MakeFallingGate(graph, tiles);
			MakeJumpGate(graph, tiles);
			return graph;
		}

		#region MakePlatformBase
		// 플랫폼이 생성된 Graph 객체를 생성
		private static Graph MakePlatformBase(int[,] tiles)
		{
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			Graph graph = new Graph(width, height);

			for(int y=0; y<height-2; y++)
			{
				int startX = -1;
				for(int x=0; x<width; x++)
				{
					if(IsAvailablePlatform(x, y, tiles))
					{
						if(startX == -1) startX = x;
					}
					else if(startX != -1) 
					{
						graph.AddPlatform(startX, x-1, y+1);
						startX = -1;
					}
				}
				if(startX != -1) 
				{
					graph.AddPlatform(startX, width-1, y+1);
				}
			}
			return graph;
		}
		// 플랫폼이 가능한 플랫폼인지 여부 체크
		private static bool IsAvailablePlatform(int x, int y, int[,] tiles)
		{
			return (TileMask.IsPlatform(tiles[x,y]) && 
				TileMask.IsEmpty(tiles[x,y+1]) && 
				TileMask.IsEmpty(tiles[x,y+2])
			);
		}
		#endregion

		#region MakeFallingGate
		// 각 플랫폼간 떨어지는 플랫폼 생성
		private static void MakeFallingGate(Graph graph, int[,] tiles)
		{
			Stack<Area>[] stackLeft = new Stack<Area>[graph.width];
			Stack<Area>[] stackRight = new Stack<Area>[graph.width];

			for(int x=0; x<graph.width; x++)
			{
				stackLeft[x] = new Stack<Area>();
				stackRight[x] = new Stack<Area>();

				for(int y=graph.height-1; y>=0; y--)
				{
					int tile = tiles[x, y];
					// 타일이 플랫폼으로 취급되는 타일이면
					if(graph[x, y] != null)
					{
						Area toLand = graph[x, y];
						//스택을 모두 삭제한 후 위쪽 플랫폼과 아래쪽 플랫폼을 연결
						_LinkFallPlatform(graph, toLand, stackLeft[x], Direction.left, x);
						_LinkFallPlatform(graph, toLand, stackRight[x], Direction.right, x);
					}
					// 타일이 플랫폼 타일이 아니고 빈 타일이면
					else if(TileMask.IsEmpty(tile))
					{
						// 양 옆 플랫폼을 스택에 추가를 시도
						if(x != graph.width-1 && graph[x+1, y] != null) stackLeft[x].Push(graph[x+1, y]);
						if(x != 0 && graph[x-1, y] != null) stackRight[x].Push(graph[x-1, y]);
					}
					// 타일에 장애물이 있으면
					else
					{
						stackLeft[x].Clear();
						stackRight[x].Clear();
					}
				}
			}
		}
		// 플랫폼 연결 헬퍼 함수
		private static void _LinkFallPlatform(Graph graph, Area toLand, Stack<Area> stack, Direction dir, int x)
		{
			while(stack.Count > 0)
			{
				Area from = stack.Pop();
				graph.LinkPlatform(from, toLand, dir, GateType.fall);
				// 만약 falling이 가능한 상태에서 점프가 가능하면
				if(from.y - toLand.y <= Constants.maxJumpHeight)
				{
					graph.LinkPlatform(toLand, from, DirUtil.Reverse(dir), GateType.jump, x);
				}
			}
		}
		#endregion

		#region MakeJumpGate
		// 각 플랫폼간 점프 플랫폼 연결
		private static void MakeJumpGate(Graph graph, int[,] tiles)
		{
			foreach(Area platform in graph)
			{
				MakeEachJumpGate(platform, graph, tiles, Direction.left);
				MakeEachJumpGate(platform, graph, tiles, Direction.right);
			}
		}
		// 하나의 플랫폼에서 영역을 스캔해 점프 플랫폼 연결
		private static void MakeEachJumpGate(Area platform, Graph graph, int[,] tiles, Direction dir)
		{
			HashSet<Area> neighbours = FindNeighbours(platform, graph, dir);
			int[,] cropped = CropTiles(platform, tiles, dir);
			foreach(Area dest in neighbours)
			{
				Vector2Int distance = GetPlatformsDist(platform, dest, dir);
				JumpableFlag jumpable = CheckJumpable(cropped, distance);
				// flag에 따라서 graph에 플랫폼과 데스티네이션을 연결
				if(jumpable.HasFlag(JumpableFlag.forward))
				{
					graph.LinkPlatform(platform, dest, dir, GateType.jump);
				}
				if(jumpable.HasFlag(JumpableFlag.backward))
				{
					graph.LinkPlatform(dest, platform, DirUtil.Reverse(dir), GateType.jump);
				}
				if(jumpable.HasFlag(JumpableFlag.backwardFall))
				{
					graph.LinkPlatform(dest, platform, DirUtil.Reverse(dir), GateType.falljump);
				}
			}
		}

		// 플랫폼에 인접한 다른 플랫폼을 찾음
		private static HashSet<Area> FindNeighbours(Area platform, Graph graph, Direction dir)
		{
			HashSet<Area> neighbours = new HashSet<Area>();
			Vector2Int startPos = platform.GetEdge(dir);
			for(int dx=2; dx<=Constants.maxJumpWidth; dx++)
			{
				for(int dy=(dir == Direction.left) ? 1 : 0; dy<=Constants.maxJumpHeight; dy++)
				{
					int x = startPos.x + dx * (int)dir;
					int y = startPos.y + dy;
					if(x < 0 || x >= graph.width || y < 0 || y >= graph.height){
						continue;
					}
					if(graph[x,y] != null && graph[x,y] != platform) neighbours.Add(graph[x,y]);
				}
			}
			return neighbours;
		}

		// 플랫폼 분석에 필요한 타일의 정보를 크로핑
		private static int[,] CropTiles(Area platform, int[,] tiles, Direction dir)
		{
			Vector2Int startPos = platform.GetEdge(dir);
			int[,] cropped = new int[Constants.maxJumpWidth+1, Constants.maxJumpHeight + 4];
			for(int dx=0; dx<=Constants.maxJumpWidth; dx++)
			{
				for(int dy=0; dy<=Constants.maxJumpHeight + 3; dy++)
				{
					if(dx == 0 && dy == 0) continue;
					int x = startPos.x + dx * (int)dir;
					int y = startPos.y + dy;
					if(x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1)){
						cropped[dx, dy] = 0;
					}
					else cropped[dx, dy] = tiles[x, y];
				}
			}
			return cropped;
		}

		// 두 플랫폼 간 거리를 구함
		private static Vector2Int GetPlatformsDist(Area start, Area end, Direction dir)
		{
			Vector2Int startPos = start.GetEdge(dir);
			Vector2Int endPos = end.GetEdge(DirUtil.Reverse(dir));
			Vector2Int result = endPos - startPos;
			return new Vector2Int(Math.Abs(result.x), result.y);
		}

		// 두 플랫폼의 연결 가능 여부를 확인
		private static JumpableFlag CheckJumpable(int[,] tiles, Vector2Int dist)
		{
			if(dist.x < 2 || dist.x > Constants.maxJumpWidth) return JumpableFlag.none;

			// 거리 기반으로 인덱스를 찾아 마스킹
			int availableBits = DetectJumpObstacle(tiles);
			int mask = Constants.jumpableMask[dist.x - 2, dist.y];
			if( mask == 0 ) return JumpableFlag.none;
			if( (availableBits & mask) != mask ) return JumpableFlag.none;
			
			//dist.y == 3일 때에는 반대쪽에서 떨어질 수 있다.
			if(dist.y == 3) return JumpableFlag.bothFall;
			//dist.y == 2이고 가로 길이가 2~3칸 떨어져 있으면 반대쪽에서 떨어질 수 있으나, forward쪽에서는 판별 필요
			if(dist.y == 2 && dist.x < 4)
			{
				// 가장 위에 가시가 존재하면 forward 점프 불가능
				for(int x=1; x<=dist.x; x++)
				{
					if(TileMask.IsTrap(tiles[x,4])) return JumpableFlag.backwardFall;
				}
				return JumpableFlag.bothFall;
			}
			// (5,0)일 때는 mask 조건만 충족하면 양쪽에서 점프 가능
			if(dist.y == 0 && dist.x == 5) return JumpableFlag.both;
			// (6,0)일 때는 mask 조건 + 체크하지 않은 xOffset=6이 비어있으면 점프 가능
			if(dist.y == 0 && dist.x == 6)
			{
				int x = Constants.maxJumpWidth;
				for(int y=0; y<Constants.maxJumpHeight + 1; y++)
				{
					if(!TileMask.IsEmpty(tiles[x,y])) return JumpableFlag.none;
				}
				return JumpableFlag.both;
			}
			// (4,1), (5,2)일 때는 백워드일 때 forward-celling y + 1가 완전히 비어있어야 한다.
			if(dist == new Vector2Int(4,1) || dist == new Vector2Int(5,2))
			{
				int toCheckY = GetMinCellingHeightForward(dist.x, dist.y);
				for(int x=0; x<=dist.x; x++)
				{
					if(!TileMask.IsEmpty(tiles[x,toCheckY])) return JumpableFlag.forward;
				}
			}
			// forward-celling y가 5이고 y가 1칸 이상일 때에는 백워드일 때 (0,4) 위치가 완전히 비어있어야 한다.
			if(GetMinCellingHeightForward(dist.x, dist.y) == 5 && dist.y > 0)
			{
				if(!TileMask.IsEmpty(tiles[0,4])) return JumpableFlag.forward;
			}
			// 나머지 경우는 천장을 체크해야 한다.
			return CheckCelling(tiles, dist);
		}
		// 크롭된 타일을 분석하여 가능성 배열 반환 (가로 6, 세로 5)
		private static int DetectJumpObstacle(int[,] tiles)
		{
			int obstacleBit = 1;
			int index = 1;
			for(int x=0; x<Constants.maxJumpWidth; x++)
			{
				for(int y=0; y<Constants.maxJumpHeight + 2; y++)
				{
					if(x == 0 && y == 0) continue;
					if(TileMask.IsEmpty(tiles[x,y])) obstacleBit |= (1<<index);
					index++;
				}
			}
			return obstacleBit;
		}
		// 특수한 경우에, 천장을 확인하게 된다.
		// forward, backward 방향에서 모두 천장을 체크하고, 플래그를 반환
		private static JumpableFlag CheckCelling(int[,] tiles, Vector2Int dist)
		{
			JumpableFlag flag = JumpableFlag.none;
			if(CheckCellingForward(tiles, dist)) flag |= JumpableFlag.forward;
			if(CheckCellingBackward(tiles, dist)) flag |= JumpableFlag.backward;
			return flag;
		}
		// forward 방향에서 천장을 체크
		private static bool CheckCellingForward(int[,] tiles, Vector2Int dist)
		{
			int minCelling = GetMinCellingHeightForward(dist.x, dist.y);
			return _CheckCelling(tiles, minCelling, 0, Direction.right, dist.x);
		}
		// backward 방향에서 천장을 체크
		private static bool CheckCellingBackward(int[,] tiles, Vector2Int dist)
		{
			int minCelling = GetMinCellingHeightInverse(dist.x, dist.y);
			return _CheckCelling(tiles, minCelling, dist.y, Direction.left, dist.x);
		}
		// 천장 체크 공통부
		private static bool _CheckCelling(int[,] tiles, int minCelling, int platformY, Direction dir, int width)
		{
			// 최소 천장 높이부터 4(캐릭터가 박을 수 있는 천장의 한도)까지
			for( int y=minCelling; y<5; y++ )
			{
				// 천장 높이에 따른 x 오프셋을 구함
				int x = GetCellingXDist(y);
				CellingJumpFlag flag = CanCellingJump(tiles, x, y+platformY, dir, width);
				if(flag == CellingJumpFlag.unable) return false;
				if(flag == CellingJumpFlag.hitCelling) return true;
			}
			return true;
		}

		// 동일한 높이의 플랫폼간 점프에서, 점프했을 때 천장의 높이
		private static int GetMinCellingHeightForward(int dx, int dy)
		{
			int diag = dx + dy;
			if(diag < 2) return 2;
			if(diag > 5) return 5;
			return 2 + diag/2;
		}
		// 고저차가 있는 플랫폼간 점프에서, 역으로 점프했을 때 천장의 높이
		private static int GetMinCellingHeightInverse(int dx, int dy)
		{
			if(dy == 0) return GetMinCellingHeightForward(dx, 0);
			int corr = dy + ((dx + dy >= 6) ? 1 : 0);
			return dx + dy - corr;
		}
		// 점프 시도시 천장 높이에 따른 요구 천장의 x-dist
		private static int GetCellingXDist(int cellingY)
		{
			if(cellingY == 4) return 2;
			if(cellingY < 4) return 1;
			return -1; 
		}
		// 천장에 박아 점프를 할 수 있는가?
		private static CellingJumpFlag CanCellingJump(int[,] tiles, int cellingX, int y, Direction dir, int xDist)
		{
			int GetIndex(int index)
			{
				if(dir == Direction.right) return index;
				return xDist - index;
			}
			//예외적 상황
			if(cellingX < 0) return CellingJumpFlag.empty;
			//천장 위치에 플랫폼이 존재하면 celling-hit
			if(TileMask.IsPlatform(tiles[GetIndex(cellingX), y])) return CellingJumpFlag.hitCelling;
			//cellingX == 2일 때, (cellingX-1, y)에 trap이 있으면 점프 불가
			if(cellingX == 2 && TileMask.IsTrap(tiles[GetIndex(cellingX-1),y])) return CellingJumpFlag.unable;
			//천장 다음 위치에 플랫폼이나 장애물이 있으면 점프 불가
			for(int i=cellingX; i<=xDist; i++)
			{
				int x = GetIndex(i);
				if(!TileMask.IsEmpty(tiles[x,y])) return CellingJumpFlag.unable;
			}
			// 아예 없으면 점프 가능
			return CellingJumpFlag.empty;
		}

		#endregion
	}
}