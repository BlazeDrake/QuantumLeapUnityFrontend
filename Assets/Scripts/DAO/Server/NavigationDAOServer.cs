using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data access object for navigation, handling course requests and ETA calculations on the server.
/// </summary>
public class NavigationDAOServer : ServerDAOBase<NavigationState>, INavigationDAO
{

    private Dictionary<int, int> speedToTravelTime;



    private class RequestCoursePayload
    {
        public string courseId { get; set; }
        public string destination { get; set; }
    }

    private class SetEtaPayload
    {
        public string engineSystem { get; set; }
        public int speed { get; set; }
        public int arriveInMilliseconds { get; set; }
    }

    private class SetCoursePayload
    {
        public string courseId { get; set; }
        public string destination { get; set; }
        public Point coordinates { get; set; }
        public SetEtaPayload eta { get; set; }
    }

    protected override void Start()
    {
        base.Start();
        stationName = "navigation";
    }

    protected override void HandleCommands()
    {
        base.HandleCommands();
        speedToTravelTime = new Dictionary<int, int>();
        if (curState?.CurrentCourse?.Eta?.TravelTimes != null)
        {
            foreach(var time in curState.CurrentCourse.Eta.TravelTimes)
            {
                speedToTravelTime.Add(time.Speed,time.ArriveInMilliseconds);
            }
        }
    }

    /// <summary>
    /// Gets the estimated time of arrival in milliseconds for a given engine speed.
    /// </summary>
    /// <param name="engineSpeed">The engine speed to calculate ETA for.</param>
    /// <returns>ETA in milliseconds, or -1 iif there is an error.</returns>
    public float GetETAInMilliseconds(int engineSpeed = 0)
    {
        if (curState == null || curState.CurrentCourse == null)
        {
            return -1;
        }
        else
        {
            int returnVal = -1;
            speedToTravelTime.TryGetValue(engineSpeed, out returnVal);
            return returnVal;
        }
    }

    /// <summary>
    /// Gets the target location for the current course.
    /// </summary>
    /// <returns>The current target locatoin</returns>
    public Vector3 GetTargetLoc()
    {
        return curState?.CurrentCourse?.Coordinates ?? Vector3.zero;    
    }

    /// <summary>
    /// Sends a course request to the flight director for the specified destination.
    /// </summary>
    /// <param name="destination">The destination for the course.</param>
    public async void RequestCourse(string destination)
    {
        var payload = new RequestCoursePayload
        {
            courseId = GenerateId().ToString(),
            destination = destination
        };

        await httpController.PostCommand("request-course-calculation", payload);

        //FIXME: remove for actual build
        StartCoroutine(TestCourse());
    }

    /// <summary>
    /// Test case for a course request
    /// </summary>
    private IEnumerator TestCourse()
    {
        yield return new WaitUntil(() => httpController.IsReady);

        yield return new WaitForSeconds(3f);
        //Test course

        var testCourse = new SetCoursePayload
        {
            courseId = GenerateId().ToString(),
            destination = "Test Destination",
            coordinates = new Vector3(0, 10, 0),
            eta = new SetEtaPayload
            {
                engineSystem = "sublight-engines",
                speed = 1,
                arriveInMilliseconds = 60000 // 1 minute
            }
        };
        yield return CoroutineUtil.WaitForTask(httpController.PostCommand("set-course", testCourse));
    }
}
