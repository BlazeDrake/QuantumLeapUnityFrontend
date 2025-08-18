# Overview

This program is a combination of the Sensors and Navigation sensors, for the purpose of testing OpenStardrive with a Minimum Viable Product (MVP). The MVP we are using for this testing is the following scenario: Request a course to a ship, fly there, and then scan it. This is built in Unity, and so uses C# for all code. As this program is based around OpenStardrive for the backend, it will be beneficial to read the overview of it here: <https://github.com/openstardrive/server/blob/master/docs/overview.md>

# Structure

The structure of this program is as follows:

- Controllers manage user input & control the UI to reflect the current state of the ship
- These controllers get info on the ship’s state through Data Access Objects (DAOS)
- These DAOS send requests to get or post data to the server through the HTTPController (As of when this document was created, it was OpenStardrive).
- The HTTPController polls the server for information, sending this info to the DAOS, as well as handling all requests to the server.

Below is a diagram showing the process of requesting a scan as an example:

<img width="1207" height="657" alt="image" src="https://github.com/user-attachments/assets/a8b871f9-8c45-4abf-b81b-741d5c94d3ab" />


## Setup Scene Classes

### Setup

This class allows the user to input a URI to connect to, and will start the connection. It will then allow the UI to be loaded

Fields

- public Button loadButton: The Button which is used to load the UI scene once the HTTP controller is setup
- public TextMeshProUGUI errorText: The text where error messages for connecting are displayed
- private HttpController httpController: The HTTP controller this class sets up

Methods:

- Public void SetURI(string input): Attempts to set the Http controller’s URI to a specific value. If successful, calls Connect
- Private void Connect(): Tells HTTP controller to start the connection. Triggered when a uri is inputted
- Public void FailConnect(string error): Called when HttpController runs into an error while setting the URI or connecting. Displays error to some text for the user to see.
- public void LoadScene(string scene): Loads the specified Scene. Used by this scene to load the UI once the HTTP controller is set up

## HTTP Classes

### CommandResult

The result of a command sent with PostCommand. Contains additional properties based on what is present in the json; advised to look at the code for the individual command you’re working with when working with commands

Properties

- Public long rowId {get; set;}: The cursor location of when this command took place
- Public string commandResultId {get; set;}: The id of the commandResult in the server’s database
- Public string type {get; set;}: The type of command this was for
- Public string commandId {get; set;}: The id of the associated command in the server’s database
- Public string clientId {get; set;}: The id of the client this command originated from
- Public string system{get; set;}: What system this command affected
- Public DateTimeOffset timestamp {get; set;}: Timestamp of the command

### GetCommandsResult

Result of a request to view command results from the server.

Properties

- Public List&lt;CommandResult&gt; results {get; set;}: List of all CommandResults returned by the request
- Public long nextCursor {get; set;}: The cursor that the next command will be at after all requested commands execute.

### PostCommandRequest

Class used as a body for requests to post commands

Properties

- Public string ClientSecret {get; set;}: The authorization secret of the client running the command
- Public string Type {get; set;}: The type of command to run
- Public object payload {get; set;}: The payload of the command; contains all data needed to run the requested command on the server.

### RegisterClientRequest

Class used as a body for requests to register the client

Fields

- Public string Name: The name of the client to register
- Public string ClientType: The type of client to register

### RegisterClientResult

Result of client registration

Fields

- Public string ClientId{get; set;}: The id of the client that was registered
- Public string ClientSecret {get; set;}: The authorization secret of the client that was registered

### HttpController

This class controls all access to the OpenStardrive server.

Methods:

- Public void Connect(): Attempts to Connect to the server
- Public bool SetURI(string value): Attempts to change the URI the http client connects with. Will use the http protocol if none is specified. Returns true if successful, and false if there’s an error
- Private async Task Register(): Asynchronously registers the client with the server using registerRequest
- Public async Task&lt;bool&gt; PostCommand(string type, object payload): Asynchronously posts a command to the server. Uses type and payload to generate the body of the request. Returns true if successful, otherwise returns false
- Public async Task Poll(): Updates the commandList based on the most recent server data. Is automatically called on an interval based on updateRate, but can also be manually called
- Public Queue&lt;CommandResult&gt; GetCommands(long startCursor, string system=null, bool onlyGetUpdateCommands = true): Gets commands based on the specified filters in the parameters. Ordered with the oldest commands first.

Fields:

- Private RegisterClientRequest registerRequest (serialized): The data for the request made on startup for registering the client
- Public UnityEvent OnPoll: UnityEvent to invoke whenever the controller polls the server. DAOS will subscribe to this in their start methods
- Private float updateRate (serialized): How much of a delay, in seconds, there will be between polls of the server state. Default is 0.5
- Private bool startAtLatestCursor (serialized): If true, program will skip all commands before it started running
- Private string secret: Authentication secret for OpenStardrive
- Private long cursor: Which command to start with on the openStardrive server for future requests
- Private List&lt;CommandResult&gt; commandResults: List of all command results on the server
- Private static HttpClient httpClient: Client for connecting to the OpenStardrive server

Properties

- Public long Cursor {get; } : Returns the current cursor
- Public bool IsReady {get; private set}: Returns whether the httpController is registered & ready to send requests

## DAOS

### IEngineDAO

Interface for accessing info on the ship’s engines.

Methods:

- Public int GetEngineSpeed(): Returns the current fusion speed
- Public Task SetFusionSpeed(int speed): Attempts to set the ship’s fusion speed to speed
- Public int GetMaxFusionSpeed(bool respectPower=true): Returns the highest speed the ship’s fusion engine can use. If respectPower is true, it will only return the highest speed the ship can currently go at, respecting power and other constraints.

### ISensorsDAO

Interface for accessing info on the ship’s sensors

Structs:

- Target:
  - Public Vector3 position: Position of the target, relative to the ship
  - Public float yaw: yaw of the target
  - Public string name: name of the target
  - Public string scanInfo: info displayed when target is scanned

Methods:

- Public List&lt;Target&gt; GetTargets(): Returns all current targets
- Public string CheckForQueryResponse(): Returns the response to the current scan
- Public void SendCustomScanQuery(string query): Sends query as a new scan query to the server, for the Flight Director to respond to

Properties:

- Public float SensorRange {get;}: Returns current range of the sensors for detecting targets

### INavigationDAO

Interface for accessing info on the ship’s course

Methods:

- Public void RequestCourse(string destination): Sends a request to the server to calculate a course. The FD will then be able to set it manually.
- Public Vector3 GetTargetLog(): Returns the relative location of the current destination, from when the course was started
- Public float GetEtaInMilliseconds(int engineSpeed=0): Gets the eta to reach the current destination in milliseconds, using speed engineSpeed

### ServerDAOBase

Base class all implementations of DAO interfaces inherit from. Requires a type T when being inherited from, which will contain all info on the system’s current state

Fields:

- Protected T curState: Data on the system’s current state
- Protected string stationName: The name of the system. For use with commands
- Protected HttpController httpController: The httpController the DAO makes requests to
- Private long cursor: The cursor the DAO was at the last time it updated its state to match the server

Methods:

- Protected void HandleCommands: Updates state based on all new commands in the httpController
- Protected int GenerateId: Generates a random id for use in commands

## System State

### Point

Class used for converting Json data to Vector3 objects. Interchangeable with a vector3, but able to parse data from the server. Contains properties for x, y, and z.

### StandardSystemBaseState

Base class for system states. Contains data common to all systems.

Properties

- Public int CurrentPower {get; set}: The current power in the system
- Public int RequiredPower {get; set}: The amount of power required for the station to function
- Public bool Disabled {get; set}: Whether the system is disabled currently
- Public bool Damaged {get; set}: Whether the system is currently damaged

### SensorSystemState

Inherits form StandardSystemBaseState.

Records:

- SensorScan:
  - Public string ScanID {get; set}: Id of the scan
  - Public string State {get; set}: the state of the scan. Values can be “active”, “completed”, or “canceled”
  - Public string ScanFor {get; set}: The query of the scan
  - Public {get; set}: The result of the scan
- Destination:
  - Public Point Position {get; set;}: Position of the destination
  - Public int RemainingMilliseconds {get; set;}: Milliseconds left for object to arrive
- SensorContact:
  - Public string ContactId {get; set;}: Id of the contact
  - Public string name {get; set;}: Display name of the contact
  - Public string icon {get; set;}: Identifier for what kind of icon should be used for the contact
  - Public Point position {get; set;}: Position of the contact
  - Public Destination\[\] destinations {get; set;}: List of destinations for the contact to move towards. Will complete the first one, remove it, and then go to the next one until empty.

Properties

- Public List&lt;SensorScan&gt; activeScans {get; set;}: All manual scans that are currently active
- Public SensorScan LastUpdatedScan {get; set;}: The scan that was most recently updated
- Public List&lt;SensorContact&gt; {get; set;}: All contacts that are currently loaded.

### NavigationSystemState

Inherits form StandardSystemBaseState.

Records:

- TravelTime:
  - public int Speed { get; set; }: What engine speed this travel time is for
  - public int ArriveInMilliseconds { get; set; }: How long in milliseconds it will take to arrive while travelling at Speed
- Eta:
  - public string EngineSystem { get; set; }: What engine system is being used for this course. Should be sublight-engines or ftl-engines
  - public TravelTime\[\] TravelTimes { get; set; }: Travel time for each engine speed
- RequestedCourseCalculation:
  - Public string CourseId {get; set;}: The id of the course
  - Public string destination {get; set;}: The name for the course’s destination
  - Public DateTimeOffset RequestedAt {get; set;}: The timestamp for when the course was requested
- CalculatedCourse:
  - public string CourseId { get; set; }: Id of the course
  - public string Destination { get; set; }: The name of the course’s destination
  - public Point Coordinates { get; set; }: The relative coordinates of the player for the destination of the course. Does not update as the player moves
  - public Eta Eta { get; set; }: ETA for when the ship will arrive at the destination
  - public DateTimeOffset CalculatedAt { get; set; }: The timestamp for when the course was calculated
- CurrentCourse:
  - public string CourseId { get; set; }: Id of the course
  - public string Destination { get; set; }: The name of the course’s destination
  - public Point Coordinates { get; set; }: The relative coordinates of the player for the destination of the course. Does not update as the player moves
  - public Eta Eta { get; set; }: ETA for when the ship will arrive at the destination
  - public DateTimeOffset CourseSetAt{ get; set; }: The timestamp for when the course was selected

Properties

- public RequestedCourseCalculation\[\] RequestedCourseCalculations { get; set; }: All current requests for courses
- public CalculatedCourse\[\] CalculatedCourses { get; set;}: All courses that have been calculated and not cleared
- public CurrentCourse CurrentCourse { get; set;}: The ship’s current course

### EngineSystemState

Inherits form StandardSystemBaseState.

Records:

- EngineSpeedConfig:
  - public int MaxSpeed { get; set; }: The max speed the engines can go at
  - public int CruisingSpeed { get; set; }: The cruising speed of the engines
- EngineHeatConfig:
  - public int PoweredHeat { get; set; }: The target heat while powered
  - public int CruisingHeat { get; set; }: The target heat while cruising
  - public int MaxHeat { get; set; }: The max heat before overheating
  - public int MinutesAtMaxSpeed { get; set; } The time it takes to fully heat up at max speed
  - public int MinutesToCoolDown { get; set; } How long it takes to cool down.
- SpeedPowerRequirement:
  - public int Speed { get; set; }: The speed level this is for
  - public int PowerNeeded { get; set; }: What the system’s power level needs to be at to reach that speed

Properties

- public int CurrentSpeed { get; set; }: The current speed of the engines
- public EngineSpeedConfig SpeedConfig { get; set; }: Config for the speed of the engines
- public int CurrentHeat { get; set; }: Current heat of the system
- public EngineHeatConfig HeatConfig { get; set; }: Information on heating/cooling
- public SpeedPowerRequirement\[\] SpeedPowerRequirements { get; set; }: Power requirements for all speeds that need more than the minimum requirement

## Controllers

### HeadingController

Not implemented yet

### CourseController

Desc.

Fields:

- private INavigationDAO navigationDAO: NavigationDAO this uses
- private IEngineDAO engineDAO: EngineDAO this uses
- private Transform playerRep (serialized): Representation of the player for bearing calculations. Not yet implemented
- private TextMeshProUGUI etaText (serialized): Text to display information on the eta to
- private string offCourseString (serialized): What to display when the ship is off course
- private string notMovingString (serialized): What to display when the ship is on course but not moving
- private string arrivalString (serialized): What to display when the ship arrives at its destination
- private string etaFormat (serialized): The format of the eta text
- private bool hasCourse: Whether the ship has a course. Default value is false.
- private Vector3 prevTargetLoc: The target of the ship when the server was last polled. Defaults to the origin
- public UnityEvent OnTargetSe: UnityEvent to trigger when the course is set or changed
- public UnityEvent OnTargetRemoved: UnityEvent to trigger when the course is removed
- public UnityEvent OnBearingUpdate: UnityEvent to trigger when the bearing is changed. Not yet implemented

Methods:

- public void RequestCourse(string destination): Sends a new course request to the server.

### EngineController

Desc.

Fields:

- private IEngineDAO engineDAO: EngineDAO this uses
- private TextMeshProUGUI speedText (serialized): What text to display the ship speed to
- private Button speedUp (serialized): The button that decreases the ship’s speed
- private Button speedDown (serialized): The button that increases the ship’s speed

Methods:

- public async void ModifyFusionSpeed(int val): Changes the ship’s sublight speed by val
- public async void SetFusionSpeed(int value): Sets the ship’s sublight speed to val
- public void UpdateUI(): Refreshes all ui elements to reflect the current state

### SensorMapController

Controls the objects that appear within the sensor’s range.

Fields:

- private static float defaultMapRadius: The radius of the map by default .Used for scaling purposes.
- private ISensorsDAO sensorsDAO: The sensors DAO
- private INavigationDAO navigationDAO: The navigation dao
- private GameObject navController (serialized): The object that contains controllers
- private float updateDelay (serialized): How often the map should be updated in seconds
- private MapTarget mapTargetPrefab (serialized): The base prefab for map targets
- private List&lt;MapTarget&gt; mapTargets: All current map targets
- private RectTransform mapTargetParent: The object all mapTargets will be instantiated as children of
- private GameObject infoParent (serialized): The parent of all ui describing the currently scanned object
- private TextMeshProUGUI nameText (serialized): The text that contains the name of the object currently being scanned
- private TextMeshProUGUI descriptionText (serialized): The text that contains the description of the object currently being scanned
- private Coroutine updateLoop: The routine that controls updates. Can be paused or resumed.

Methods:

- public void UpdateMap(): Updates the map to match the current state
- public void DisplayTargetInfo(ISensorsDAO.Target target): Displays information on target to the ui

### ManualQueryController

Desc.

Fields:

- private ISensorsDAO sensorsDAO: The sensorsDAO for this controller
- private TextMeshProUGUI responseText (serialized): The text that displays the response to the scan
- private GameObject resultParent (serialized):The parent gameObject of all ui elements for displaying responses to scans
- private GameObject waitingParent (serialized): The parent gameObject of all ui elements for waiting for a response
- private string lastResponse: The last response to a scan recorded from the server. Used to determine if there’s been a new message

Methods:

- public void SendQuery(string query): Sends a scan query to the server.

## Misc

### VectorUtil

Utility class for working with vectors

Methods:

- public static Vector3 RoundVector(Vector3 inputVec): Returns inputVec with all 3 components rounded to the nearest integer
- public static bool VectorApproximatelyEq(Vector3 a, Vector3 b): Returns true if all 3 components in vectors a and b are approximately equal, using Mathf.approximately.

### CoroutineUtil

Utility class for integrating C# Tasks with Unity Coroutines

Methods:

- public static IEnumerator WaitForTask(Task task): yields until the task is completed.

### SteppedSlider

Class that constrains a unity slider to specific steps

Fields:

- public Slider slider: The Slider this component is based on
- public float minValue: Minimum value of the slider
- public float maxValue: Maximum value for the slider
- public float stepRate: How large the steps are.
- public UnityEvent&lt;float&gt; OnValueChanged: Event to trigger when the value of slider is changed, except it respects the step restriction

### ButtonHoldDetector

Class that lets you know if a unity button is being held down or not.

Fields:

- private Button attachedButton: The button this script is attached to. Automatically set on start as a button component on the same gameobject as this script
- private bool buttonPressed: Whether the button is pressed or not.

Properties

- Public bool IsPressed {get;}: Encapsulates buttonPressed, giving public read access.
