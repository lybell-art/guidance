public static class Constants
{
	// layers
	public const int uiLayer = 5;
	public const int platform = 6;
	public const int interactive = 10;
	public const int confiner = 11;

	// name
	public const string ingameManagerName = "IngameManager";
	public const string priorityAssignmentPre = "priorityAssignmentPre";
	public const string movingPlatformTag = "MovePlatform";
	public const string barrierTag = "Barrier";

	public const string priorityAssignmentName = "priorityAssignment";
	public const string flickAssignmentName = "flickAssignment";
	public const string wisdomName = "wisdom";
	public const string deliberationName = "deliberation";
	public const string faderName = "fader";

	// assignment
	public const float assignmentTimeout = 10.0f;

	// others
	public const float cameraBaseSize = 4.5f;
	public const float barrierThreshold = 12f;
	public const int maxWizdomCount = 10;

	// jump width/height
	public const int maxJumpHeight = 3;
	public const int maxJumpWidth = 6;
	public static readonly int[,] jumpableMask = new int[5,4]{
		{
			0b00000_00000_00000_00111_00111_00111, // (2,0)
			0b00000_00000_00000_00110_00111_00111, // (2,1)
			0b00000_00000_00000_01100_01111_01111, // (2,2)
			0b00000_00000_00000_11000_11111_11111  // (2,3)
		},
		{
			0b00000_00000_01111_01111_01111_01111, // (3,0)
			0b00000_00000_01110_01111_01111_01111, // (3,1)
			0b00000_00000_11100_11110_11111_01111, // (3,2)
			0b00000_00000_11000_11110_11111_01111  // (3,3)
		},
		{
			0b00000_01111_01111_01110_01111_01111, // (4,0)
			0b00000_01110_01110_01111_01111_01111, // (4,1)
			0b00000_11100_11110_11110_11111_01111, // (4,2)
			0b00000_11000_11100_11110_11111_01111  // (4,3)
		},
		{
			0b01111_11111_11110_11110_11111_01111, // (5,0)
			0b11110_11110_11110_11111_11111_01111, // (5,1)
			0b11100_11100_11110_11111_11111_01111, // (5,2)
			0 // (5,3)
		},
		{
			0b11111_11110_11100_11110_11111_01111, // (6,0)
			0, // (6,1)
			0, // (6,2)
			0  // (6,3)
		}
	};
}