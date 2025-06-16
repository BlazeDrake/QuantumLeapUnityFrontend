using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorMapController : MonoBehaviour
{
    private static float defaultMapRadius = 200f;

    private ISensorsDAO sensorsDAO;
    private INavigationDAO navigationDAO;

    [SerializeField]
    private GameObject navController;
    [SerializeField]
    private float updateDelay;

    [SerializeField] 
    private MapTarget mapTargetPrefab;
    private List<MapTarget> mapTargets = new List<MapTarget>();
    [SerializeField]
    private RectTransform mapTargetParent;

    [SerializeField]
    private GameObject infoParent;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private Coroutine updateLoop;

    // Start is called before the first frame update
    void Start()
    {
        sensorsDAO = GetComponent<ISensorsDAO>();
        navigationDAO = navController.GetComponent<INavigationDAO>();

        //UpdateMap();
        updateLoop = StartCoroutine(UpdateLoop());
    }

    public void UpdateMap()
    {
        //clear the map
        if (mapTargets != null)
        {
            foreach (var target in mapTargets)
            {
                Destroy(target.gameObject);
            }
            mapTargets.Clear();
        }
        else
        {
            mapTargets = new List<MapTarget>();
        }

            //repopulate map
            float mapScale = defaultMapRadius / sensorsDAO.SensorRange;
            mapTargetParent.localScale = Vector3.one * mapScale;

        var targets = sensorsDAO.GetTargets();
        foreach (var target in targets)
        {
            if(Vector3.Magnitude(target.position) > sensorsDAO.SensorRange)
            {
                continue; //skip targets that are out of range
            }
            var mapTarget = Instantiate<MapTarget>(mapTargetPrefab, mapTargetParent);
            mapTarget.target = target;

            var relPos = target.position;
            var mapPos = new Vector3(relPos.z, relPos.x, 0f);
            mapTarget.transform.localPosition=mapPos;
            mapTarget.transform.localScale = Vector3.one / mapScale;
            mapTarget.transform.localEulerAngles = new Vector3(0, 0, target.yaw);

            mapTargets.Add(mapTarget);  
        }
    }

    public void DisplayTargetInfo(ISensorsDAO.Target target)
    {
        infoParent.SetActive(true);
        nameText.text = target.name;
        descriptionText.text = target.scanInfo;
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            UpdateMap();
            yield return new WaitForSeconds(updateDelay);
        }
    }
}
