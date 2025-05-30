using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private INavigationDAO navigationDAO;

    [SerializeField]
    private TextMeshProUGUI posText;

    [SerializeField]
    private string posFormat;

    [SerializeField]
    private Transform moveRep;

    [SerializeField]
    private float updateDelay;

    private Coroutine updateRoutine;
    // Start is called before the first frame update
    void Start()
    {
        navigationDAO=GetComponent<INavigationDAO>();
        updateRoutine = StartCoroutine(UpdateRoutine());
    }


    public void SetFusonSpeed(float value)
    {
        navigationDAO.SetFusionSpeed(value*navigationDAO.GetMaxFusionSpeed());
    }

    public IEnumerator UpdateRoutine()
    {
        moveRep.eulerAngles = navigationDAO.GetShipHeading();
        var moveVector = moveRep.forward * navigationDAO.GetFusionSpeed() * updateDelay;
        var newPos = navigationDAO.GetShipPos() + moveVector;
        navigationDAO.SetShipPos(newPos);

        var displayPos=VectorUtil.RoundVector(newPos);

        posText.text = string.Format(posFormat, displayPos.x, displayPos.y, displayPos.z);
        yield return new WaitForSeconds(updateDelay);
        updateRoutine=StartCoroutine(UpdateRoutine());
    }
}
