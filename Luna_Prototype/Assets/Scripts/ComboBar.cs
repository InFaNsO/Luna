using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBar : MonoBehaviour
{
    public float yGap = 1.0f;

    // Start is called before the first frame update
    private MoveContext mCurrentMoveContext;

    public GameObject localHolder;
    public GameObject Indicator;
    public GameObject SampleTransitionSlice;

    private List<GameObject> mTransitionSlice = new List<GameObject>();
    private List<GameObject> mTransitionWidth = new List<GameObject>();

    private Player mPlayer;
    void Awake()
    {
        mPlayer = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mPlayer.transform.position + new Vector3(0.0f, yGap, 0.0f);
    }

    public void Bind(MoveContext moveContext)
    {
        int SliceCount = moveContext.mTotalTransitionSliceCount;
        if (SliceCount > 0)
        {
            gameObject.SetActive(true);
        }

        if (mTransitionSlice.Count < SliceCount)
        {
            for (int i = 0; i < SliceCount - mTransitionSlice.Count; i++)
            {
                var newTransitionSlice = Instantiate(SampleTransitionSlice, localHolder.transform);
                mTransitionSlice.Add(newTransitionSlice);
                newTransitionSlice.transform.localPosition = Vector3.zero;
                mTransitionWidth.Add(newTransitionSlice.transform.Find("TransitionWidth").gameObject);
            }
        }

        float totalTime = moveContext.mTotalTime;

        int index = 0;
        foreach (var item in moveContext.mMoveTimeSlices)
        {
            if(item.mType == MoveTimeSliceType.Type_Transition)
            {
                mTransitionSlice[index].transform.localPosition = new Vector3(item.mTimeSlicePropotion, 0.0f, -3.0f);
                mTransitionWidth[index].transform.localScale = new Vector3((item.mSliceTotalTime / totalTime) * 0.5f, 1.0f, 1.0f);
            }
        }
    }

    public void UnBind()
    {
        gameObject.SetActive(false);
    }
}
