using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 GetShipBearing()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetShipHeading()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetTargetLoc()
    {
        return curState?.CurrentCourse?.Coordinates ?? Vector3.zero;    
    }

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

    public void SetShipBearing(Vector3 bearing)
    {
        throw new System.NotImplementedException();
    }

    public void SetShipHeading(Vector3 heading)
    {
        throw new System.NotImplementedException();
    }

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
