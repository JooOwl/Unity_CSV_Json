using UnityEngine;
using System.Collections;

public class DragScrollPostionAddOn : MonoBehaviour 
{
	public UIScrollView mScrollView;
	public UIPanel mGridpanel;
	public UIGrid mGridObject;

	void Awake()
	{
		if (mScrollView == null) 
		{
			mScrollView = GetComponent<UIScrollView> ();
		}

		if (mGridpanel == null) 
		{
			mGridpanel = GetComponent<UIPanel> ();
		}

		if (mGridObject == null) 
		{
			mGridObject = this.transform.GetComponentInChildren<UIGrid> ();
		}
	}

	public void ItemViewPostion(int nItemNumber)
	{
		float width = mGridObject.cellWidth;
		//float height = mGridObject.cellHeight;

		Vector3 postionVec3 = VPostion (width, 0f, nItemNumber, 0);

		mScrollView.MoveRelative (postionVec3);
		mScrollView.RestrictWithinBounds (true);
	}

	Vector3 VPostion(float x, float y, int nItemNumber, int limit)
	{
		Vector3 tempPostion = Vector3.zero;

		if( limit > 1 )
		{
			nItemNumber /= limit;
		}

		tempPostion.x = x * nItemNumber * (-1f);
		tempPostion.y = y * nItemNumber * (-1f);

		tempPostion.x -= mGridpanel.transform.localPosition.x;
		tempPostion.y -= mGridpanel.transform.localPosition.y;

		return tempPostion;
	}

	public void PostionChage(int nItemNumber, int nItemMax)
	{
		float postion = (float)nItemNumber / (float)nItemMax;
		mScrollView.SetDragAmount (postion, 0f, false);
	}
}
