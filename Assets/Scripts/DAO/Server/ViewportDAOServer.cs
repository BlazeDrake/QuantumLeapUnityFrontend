using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportDAOServer : ServerDAOBase<ViewportSystemState>
{
    private class SetCardsPayload
    {
        public string[] value { get; set; }
    }
    protected override void Start()
    {
        base.Start();
        stationName = "json-plugin-viewscreen";
    }

    public string GetCurrentImage()
    {
        return curState?.JsonState?.CurrentImage ?? "None";
    }

    public async void SetCards(string[] cards)
    {
        await httpController.PostCommand("update-viewscreen-Cards", new SetCardsPayload
        {
            value= cards
        });
    }
}
