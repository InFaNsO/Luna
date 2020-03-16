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
    public GameObject MaxBar;
    public GameObject SampleTransitionSlice;

    private List<GameObject> mTransitionSlice = new List<GameObject>();
    private List<GameObject> mTransitionWidth = new List<GameObject>();

    private Player mPlayer;
    private float maxWidth;

    public ParticleSystem successParticle;
    void Awake()
    {
        mPlayer = GameObject.Find("Player").GetComponent<Player>();
        maxWidth = MaxBar.transform.localScale.x;
    }

    void Start()
    {
        UnBind();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mPlayer.transform.position + new Vector3(0.0f, yGap, 0.0f);

        if (mCurrentMoveContext != null)
        {
            float percentage = mCurrentMoveContext.mTotalCounter / mCurrentMoveContext.mTotalTime;
            if (percentage < 0.0f) percentage = 0.0f;
            if (percentage > 1.0f) percentage = 1.0f;

            Indicator.transform.localPosition = new Vector3((percentage) * maxWidth, 0.0f, -6.0f);
        }
    }

    public void Bind(MoveContext moveContext)
    {
        int SliceCount = moveContext.mTotalTransitionSliceCount;
        if (SliceCount > 0)
        {
            gameObject.SetActive(true);
            mCurrentMoveContext = moveContext;
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

        for (int i = 0; i < mTransitionSlice.Count; ++i)
        {
                mTransitionSlice[i].transform.localPosition = new Vector3(0.0f, 0.0f, -3.0f);
                mTransitionWidth[i].transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
        }
        int index = 0;
        foreach (var item in moveContext.mMoveTimeSlices)
        {
            if(item.mType == MoveTimeSliceType.Type_Transition)
            {
                mTransitionSlice[index].transform.localPosition = new Vector3(item.mTimeSlicePropotion * maxWidth - (item.mSliceTotalTime / totalTime) * 0.5f * maxWidth, 0.0f, -3.0f);
                mTransitionWidth[index].transform.localScale = new Vector3((item.mSliceTotalTime / totalTime) , 1.0f, 1.0f);
                ++index;
            }
        }
    }

    public void UnBind()
    {
        gameObject.SetActive(false);
    }

    public void SpwanSuccessParticle()
    {
        if (!successParticle)
            return;

        var spwanPosition = new Vector3(Indicator.transform.position.x, Indicator.transform.position.y + 0.2f, Indicator.transform.position.z);
        ParticleSystem newParticle = Instantiate(successParticle, spwanPosition, Quaternion.identity);
        newParticle.Play();
    }
}
