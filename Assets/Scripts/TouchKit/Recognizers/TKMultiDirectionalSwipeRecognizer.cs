using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TKMultiDirectionalSwipeRecognizer : TKAbstractGestureRecognizer
{
	/// <summary>
	/// The event that fires when a swipe is recognized.
	/// </summary>
	public event System.Action<TKMultiDirectionalSwipeRecognizer> gestureRecognizedEvent;

	/// <summary>
	/// The maximum amount of time for the motion to be considered a swipe.
	/// Setting to 0f will disable the time restriction completely.
	/// </summary>
	public float timeToSwipe = 0.5f;

	/// <summary>
	/// The velocity of the swipe, in centimeters based on the screen resolution
	/// and pixel density, if available.
	/// </summary>
	public float swipeVelocity { get; private set; }

	/// <summary>
	/// The direction that the swipe was made in. Possibilities include the four
	/// cardinal directions and the four diagonal directions.
	/// </summary>
	public TKSwipeDirection completedSwipeDirection { get; private set; }

	/// <summary>
	/// The minimum number of simultaneous touches (fingers) on the screen to trigger
	/// this swipe recognizer. Default is 1.
	/// </summary>
	public int minimumNumberOfTouches = 1;

	/// <summary>
	/// The maximum number of simultaneous touches (fingers) on the screen to trigger
	/// this swipe recognizer. Default is 2.
	/// </summary>
	public int maximumNumberOfTouches = 2;

	/// <summary>
	/// If true, will trigger on the frame that the criteria for a swipe are first met.
	/// If false, will only trigger on completion of the motion, when the touch is lifted.
	/// </summary>
	public bool triggerWhenCriteriaMet = true;


	/// <summary>
	/// The minimum distance in centimeters that the gesture has to make to be considered
	/// a proper swipe, based on resolution and pixel density. Default is 2cm.
	/// </summary>
	private float _minimumDistance = 2f;

	/// <summary>
	/// The maximum distance in centimeters that the gesture has to make to be considered 
	/// a proper swipe.
	/// </summary>
	private float _maximumDistance = 10f;

	/// <summary>
	/// The individual points that make up the gesture, recorded every frame from when a
	/// finger is first pressed to the screen until it's lifted. Only tracks the first touch
	/// on the screen, in the case of multiple touches.
	/// </summary>
	private List<Vector2> _points = new List<Vector2>();

	/// <summary>
	/// The time that the gesture started. Is used to determine if the time limit has been
	/// passed, and whether to ignore further checks.
	/// </summary>
	private float _startTime;

	private class Swipe
	{
		public float angle;
		public bool isSuccess;

		public Swipe(float _angle, bool _isSuccess = false)
		{
			angle = _angle;
			isSuccess = _isSuccess;
		}
	}

	private List<Swipe> swipes = new List<Swipe>();
	private int curSwipeIdx = 0;

	public void AddSwipe(float _angle)
	{
		swipes.Add(new Swipe(_angle));
	}

	public void ResetSwipes()
	{
		for (int i = 0; i < swipes.Count; i++)
		{
			swipes[i].isSuccess = false;
		}
		curSwipeIdx = 0;
	}

	private int startIdx;

	/// <summary>
	/// The first touch point in the gesture.
	/// </summary>
	public Vector2 startPoint
	{
		//get { return this._points.FirstOrDefault(); }
		get { return this._points[startIdx]; }
	}

	/// <summary>
	/// The last touch point in the gesture.
	/// </summary>
	public Vector2 endPoint
	{
		get { return this._points.LastOrDefault(); }
	}


	public TKMultiDirectionalSwipeRecognizer() : this(2f, 10f)
	{ }

	public TKMultiDirectionalSwipeRecognizer(float minimumDistanceCm, float maximumDistanceCm)
	{
		this._minimumDistance = minimumDistanceCm;
		this._maximumDistance = maximumDistanceCm;
	}


	private bool checkForSwipeCompletion(TKTouch touch)
	{
		// if we have a time stipulation and we exceeded it stop listening for swipes, fail
		//if( timeToSwipe > 0.0f && ( Time.time - this._startTime ) > timeToSwipe )
		//	return false;

		// if we don't have at least two points to test yet, then fail
		if (this._points.Count - startIdx < 3)
			return false;

		// the ideal distance in pixels from the start to the finish
		float idealDistance = Vector2.Distance(startPoint, endPoint);

		// the ideal distance in centimeters, based on the screen pixel density
		float idealDistanceCM = idealDistance / TouchKit.instance.ScreenPixelsPerCm;

		// if the distance moved in cm was less than the minimum,
		//if( idealDistanceCM < this._minimumDistance || idealDistanceCM > this._maximumDistance )
		//	return false;

		// add up distances between all points sampled during the gesture to get the real distance
		float realDistance = 0f;
		for (int i = startIdx + 1; i < this._points.Count; i++)
			realDistance += Vector2.Distance(this._points[i], this._points[i - 1]);

		// if the real distance is 10% greater than the ideal distance, then fail
		// this weeds out really irregular "lines" and curves from being considered swipes
		if (realDistance > idealDistance * 1.3f)
		{
			state = TKGestureRecognizerState.FailedOrEnded;
			return false;
		}

		// the speed in cm/s of the swipe
		swipeVelocity = idealDistanceCM / (Time.time - this._startTime);

		// turn the slope of the ideal swipe line into an angle in degrees
		float longSwipeAngle = getLineAngle(startPoint, endPoint);

		// depending on the angle of the line, give a logical swipe direction
		completedSwipeDirection = getSwipeDirection(longSwipeAngle);

		float shortSwipeAngle = getLineAngle(this._points[this._points.Count - 3], endPoint);

		// depending on the angle of the line, give a logical swipe direction
		TKSwipeDirection swipeDirection = getSwipeDirection(shortSwipeAngle);

		//if (swipeDirection == swipeDirections[currDirIdx].direction
		//	|| ((swipeDirection & swipeDirections[currDirIdx].direction) != 0)
		//	&& (((swipeDirection | swipeDirections[currDirIdx].direction) == TKSwipeDirection.UpLeft)
		//		|| ((swipeDirection | swipeDirections[currDirIdx].direction) == TKSwipeDirection.DownLeft)
		//		|| ((swipeDirection | swipeDirections[currDirIdx].direction) == TKSwipeDirection.UpRight)
		//		|| ((swipeDirection | swipeDirections[currDirIdx].direction) == TKSwipeDirection.DownRight))
		//	)
		if (shortSwipeAngle > swipes[curSwipeIdx].angle - 50 && shortSwipeAngle < swipes[curSwipeIdx].angle + 50)
		{
			//if (completedSwipeDirection == swipeDirections[currDirIdx].direction)
			if (longSwipeAngle > swipes[curSwipeIdx].angle - 20 && longSwipeAngle < swipes[curSwipeIdx].angle + 20)
			{
				if (!swipes[curSwipeIdx].isSuccess)
				{
					Debug.LogFormat("Swipe at index {0} with angle {1} is recognized", curSwipeIdx, swipes[curSwipeIdx].angle);
					swipes[curSwipeIdx].isSuccess = true;
				}
			}
			else
			{
				//swipes[curSwipeIdx].isSuccess = false;
			}
		}
		else
		{
			Debug.Log("Changing direction");
			if (swipes[curSwipeIdx].isSuccess)
			{
				curSwipeIdx++;
				startIdx = this._points.Count - 1;
			}
			if (curSwipeIdx >= swipes.Count)
			{
				state = TKGestureRecognizerState.FailedOrEnded;
				return false;
			}
		}
		return true;
	}

	internal override void fireRecognizedEvent()
	{
		if (gestureRecognizedEvent != null)
			gestureRecognizedEvent(this);
	}

	internal override bool touchesBegan(List<TKTouch> touches)
	{
		if (state == TKGestureRecognizerState.Possible)
		{
			// add any touches on screen
			for (int i = 0; i < touches.Count; i++)
				this._trackingTouches.Add(touches[i]);

			// if the number of touches is within our constraints, begin tracking
			if (this._trackingTouches.Count >= minimumNumberOfTouches && this._trackingTouches.Count <= maximumNumberOfTouches)
			{
				// reset everything
				this._points.Clear();
				this._points.Add(touches[0].position);
				startIdx = 0;

				this._startTime = Time.time;
				state = TKGestureRecognizerState.Began;
			}
		}
		return false;
	}

	internal override void touchesMoved(List<TKTouch> touches)
	{
		// only bother doing anything if we haven't recognized or failed yet
		if (state == TKGestureRecognizerState.Began)
		{
			this._points.Add(touches[0].position);
			//Debug.DrawLine(endPoint, _points[_points.Count - 2], Color.green, 5f, false);

			// if we're triggering when the criteria is met, then check for completion every frame
			checkForSwipeCompletion(touches[0]);
		}
	}

	internal override void touchesEnded(List<TKTouch> touches)
	{
		// if we haven't recognized or failed yet
		if (state == TKGestureRecognizerState.Began)
		{
			if (curSwipeIdx == swipes.Count - 1 && swipes.FindAll((Swipe swipe) => { return swipe.isSuccess; }).Count == swipes.Count)
			{
				state = TKGestureRecognizerState.Recognized;
			}
			else
			{
				state = TKGestureRecognizerState.FailedOrEnded;
			}
		}
	}

	internal override void reset()
	{
		base.reset();
		ResetSwipes();
		this._points.Clear();
	}

	public override string ToString()
	{
		return string.Format("{0}, swipe direction: {1}, swipe velocity: {2}, start point: {3}, end point: {4}",
			base.ToString(), completedSwipeDirection, swipeVelocity, startPoint, endPoint);
	}

	private TKSwipeDirection getSwipeDirection(float angle)
	{
		if (angle >= 292.5f && angle <= 337.5f)
			return TKSwipeDirection.UpRight;
		else if (angle >= 247.5f && angle <= 292.5f)
			return TKSwipeDirection.Up;
		else if (angle >= 202.5f && angle <= 247.5f)
			return TKSwipeDirection.UpLeft;
		else if (angle >= 157.5f && angle <= 202.5f)
			return TKSwipeDirection.Left;
		else if (angle >= 112.5f && angle <= 157.5f)
			return TKSwipeDirection.DownLeft;
		else if (angle >= 67.5f && angle <= 112.5f)
			return TKSwipeDirection.Down;
		else if (angle >= 22.5f && angle <= 67.5f)
			return TKSwipeDirection.DownRight;
		else // angle >= 337.5f || angle <= 22.5f
			return TKSwipeDirection.Right;
	}
}
