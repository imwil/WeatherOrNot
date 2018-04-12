using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class TKDiscreteCurveRecognizer : TKAbstractGestureRecognizer
{
	public event Action<TKDiscreteCurveRecognizer> gestureRecognizedEvent;
	public event Action<TKDiscreteCurveRecognizer> gestureCompleteEvent;

	public float reportRotationStep = 20f; //how much rotation (degrees) is needed for the recognized event to fire
	public float squareDistance = 10f; //squared distance of touhes being evaluated
	public float maxSharpnes = 50f; //maximum angle (degrees) a touch is allowed to change direction of movement

	public int minimumNumberOfTouches = 1;
	public int maximumNumberOfTouches = 2;

	//should be read only
	public float deltaRotation = 0f; //rotation since last reported

	private Vector2 _previousLocation;
	private Vector2 _deltaTranslation;//direction vector from previous to current location (current location being the last location far enough from the previous one)
	private Vector2 _previousDeltaTranslation;//last direction

	private List<Vector2> _points = new List<Vector2>();


	internal override void fireRecognizedEvent()
	{
		if (gestureRecognizedEvent != null)
			gestureRecognizedEvent(this);
	}


	internal override bool touchesBegan(List<TKTouch> touches)
	{
		// add new or additional touches to gesture (allows for two or more touches to be added or removed without ending the pan gesture)
		if (state == TKGestureRecognizerState.Possible ||
		   ((state == TKGestureRecognizerState.Began || state == TKGestureRecognizerState.RecognizedAndStillRecognizing) && _trackingTouches.Count < maximumNumberOfTouches))
		{
			for (int i = 0; i < touches.Count; i++)
			{
				// only add touches in the Began phase
				if (touches[i].phase == TouchPhase.Began)
				{
					_trackingTouches.Add(touches[i]);

					if (_trackingTouches.Count == maximumNumberOfTouches)
						break;
				}
			} // end for

			if (_trackingTouches.Count >= minimumNumberOfTouches)
			{
				_previousLocation = touchLocation();
				this._points.Add(_previousLocation);
				if (state != TKGestureRecognizerState.RecognizedAndStillRecognizing)
				{
					this._points.Clear();
					//initialize values, but stay in Possible state. use Began only when there's enough data
					state = TKGestureRecognizerState.Possible;
					deltaRotation = 0f;
					_deltaTranslation = Vector2.zero;
					_previousDeltaTranslation = Vector2.zero;
				}
			}
		}

		return false;
	}


	internal override void touchesMoved(List<TKTouch> touches)
	{
		if (state == TKGestureRecognizerState.Possible)
		{
			//previous delta translation hasn't been set yet, need another itteration

			Vector2 currentLocation = touchLocation();
			this._points.Add(currentLocation);
			Vector2 delta = currentLocation - _previousLocation;
			_deltaTranslation = delta;
			_previousLocation = currentLocation;
			_previousDeltaTranslation = _deltaTranslation;

			state = TKGestureRecognizerState.Began; //got enough data to begin recognizing in next move
		}
		else if (state == TKGestureRecognizerState.RecognizedAndStillRecognizing || state == TKGestureRecognizerState.Began)
		{
			var currentLocation = touchLocation();
			this._points.Add(currentLocation);

			var delta = currentLocation - _previousLocation;
			if (delta.sqrMagnitude >= 10f)
			{
				var a = Vector2.Angle(_previousDeltaTranslation, delta);

				if (a > maxSharpnes)
				{
					Debug.Log("Curve is to sharp: " + a + "  max sharpnes set to:" + maxSharpnes);
					state = TKGestureRecognizerState.FailedOrEnded; //calls reset
				}
				else
				{
					_deltaTranslation = delta;

					float crossZ = Vector3.Cross(_previousDeltaTranslation, delta).z;//  / (_previousDeltaTranslation.magnitude * delta.magnitude);
					if (crossZ > 0f)
						deltaRotation -= a;
					else
						deltaRotation += a;

					//if (Mathf.Abs(deltaRotation) >= reportRotationStep)
					//{
						//if total rotation changed enough: recognize and reset total rotation
						//state = TKGestureRecognizerState.RecognizedAndStillRecognizing; // fires recognized event
						//deltaRotation = 0f;//reset total rotation
					//}
					_previousLocation = currentLocation;
					_previousDeltaTranslation = _deltaTranslation;

				}
			}

		}
	}


	internal override void touchesEnded(List<TKTouch> touches)
	{
		// remove any completed touches
		for (int i = 0; i < touches.Count; i++)
		{
			if (touches[i].phase == TouchPhase.Ended)
				_trackingTouches.Remove(touches[i]);
		}

		// if we still have a touch left continue. no touches means its time to reset
		if (_trackingTouches.Count >= minimumNumberOfTouches)
		{
			_previousLocation = touchLocation();
			state = TKGestureRecognizerState.RecognizedAndStillRecognizing; //fires recognized event
		}
		else
		{
			float idealDistance = Vector2.Distance(this._points.FirstOrDefault(), this._points.LastOrDefault());
			float idealDistanceCM = idealDistance / TouchKit.instance.ScreenPixelsPerCm;
			Vector2 dirVec = (this._points.LastOrDefault() - this._points.FirstOrDefault()).normalized;
			float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
			if (angle < 0)
				angle = 360 + angle;
			angle = 360 - angle;
			TKSwipeDirection completedSwipeDirection;
			if (angle >= 292.5f && angle <= 337.5f)
				completedSwipeDirection = TKSwipeDirection.UpRight;
			else if (angle >= 247.5f && angle <= 292.5f)
				completedSwipeDirection = TKSwipeDirection.Up;
			else if (angle >= 202.5f && angle <= 247.5f)
				completedSwipeDirection = TKSwipeDirection.UpLeft;
			else if (angle >= 157.5f && angle <= 202.5f)
				completedSwipeDirection = TKSwipeDirection.Left;
			else if (angle >= 112.5f && angle <= 157.5f)
				completedSwipeDirection = TKSwipeDirection.DownLeft;
			else if (angle >= 67.5f && angle <= 112.5f)
				completedSwipeDirection = TKSwipeDirection.Down;
			else if (angle >= 22.5f && angle <= 67.5f)
				completedSwipeDirection = TKSwipeDirection.DownRight;
			else // angle >= 337.5f || angle <= 22.5f
				completedSwipeDirection = TKSwipeDirection.Right;

			Debug.LogFormat("deltaRotation = {0}, idealDistanceCM = {1}, completedSwipeDirection = {2}", deltaRotation, idealDistanceCM, completedSwipeDirection);
			if (Mathf.Abs(deltaRotation) >= 500 && Mathf.Abs(deltaRotation) <= 580
				//&& idealDistanceCM >= 1 && idealDistanceCM <= 2
				&& completedSwipeDirection == TKSwipeDirection.Up)
			{
				state = TKGestureRecognizerState.Recognized;
			}
			else
			{
				state = TKGestureRecognizerState.FailedOrEnded;
			}
			// if we had previously been recognizing fire our complete event
			//if (state == TKGestureRecognizerState.RecognizedAndStillRecognizing)
			//{
			//	if (gestureCompleteEvent != null)
			//		gestureCompleteEvent(this);
			//}
			//state = TKGestureRecognizerState.FailedOrEnded; //calls reset
		}
	}


	public override string ToString()
	{
		return string.Format("[{0}] state: {1}, trans: {2}, lastTrans: {3}, totalRot: {4}", this.GetType(), state, _deltaTranslation, _previousDeltaTranslation, deltaRotation);
	}

}
