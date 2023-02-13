using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for any visuals I want to add to the grid
public class GridVisual : IPoolable, IPlaceable
{
    public bool active { get; set; }

    public GridPos myPos { get; set; }
    public GameObject body { get; set; }

    public GameObject GetBody()
    {
        return body;
    }

    public void OnDisableObject()
    {
        if (body != null)
        {
            body.SetActive(false);
        }
    }

    public void OnEnableObject()
    {
        if (body != null)
        {
            body.SetActive(true);
        }
    }

    public void UpdatePos(GridPos _pos)
    {
        myPos = _pos;
    }

    public void UpdateVisual(GridPos _pos)
    {
        body.transform.position = GridSystem.FetchVector2FromGridpos(_pos);
    }

    public virtual void CreateBody()
    {

    }
}
