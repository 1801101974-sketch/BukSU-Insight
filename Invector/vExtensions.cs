using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector;

public static class vExtensions
{
	public static T[] Append<T>(this T[] arrayInitial, T[] arrayToAppend)
	{
		if (arrayToAppend == null)
		{
			throw new ArgumentNullException("The appended object cannot be null");
		}
		if (arrayInitial is string || arrayToAppend is string)
		{
			throw new ArgumentException("The argument must be an enumerable");
		}
		T[] array = new T[arrayInitial.Length + arrayToAppend.Length];
		arrayInitial.CopyTo(array, 0);
		arrayToAppend.CopyTo(array, arrayInitial.Length);
		return array;
	}

	public static T[] vToArray<T>(this List<T> list)
	{
		T[] array = new T[list.Count];
		if (list == null || list.Count == 0)
		{
			return array;
		}
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i];
		}
		return array;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		do
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		while (angle < -360f || angle > 360f);
		return Mathf.Clamp(angle, min, max);
	}

	public static ClipPlanePoints NearClipPlanePoints(this Camera camera, Vector3 pos, float clipPlaneMargin)
	{
		ClipPlanePoints result = default(ClipPlanePoints);
		Transform transform = camera.transform;
		float f = camera.fieldOfView / 2f * (MathF.PI / 180f);
		float aspect = camera.aspect;
		float nearClipPlane = camera.nearClipPlane;
		float num = nearClipPlane * Mathf.Tan(f);
		float num2 = num * aspect;
		num *= 1f + clipPlaneMargin;
		num2 *= 1f + clipPlaneMargin;
		result.LowerRight = pos + transform.right * num2;
		result.LowerRight -= transform.up * num;
		result.LowerRight += transform.forward * nearClipPlane;
		result.LowerLeft = pos - transform.right * num2;
		result.LowerLeft -= transform.up * num;
		result.LowerLeft += transform.forward * nearClipPlane;
		result.UpperRight = pos + transform.right * num2;
		result.UpperRight += transform.up * num;
		result.UpperRight += transform.forward * nearClipPlane;
		result.UpperLeft = pos - transform.right * num2;
		result.UpperLeft += transform.up * num;
		result.UpperLeft += transform.forward * nearClipPlane;
		return result;
	}
}
