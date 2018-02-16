using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class Mode7Controller : MonoBehaviour
{
	public AnimationCurve animationCurve;

	public float animationTime = 0.5f;
	public float timeStep = 0.05f;

	public float animationStartTime;

	private Material _material;
	public Material Material
	{
		get
		{
			if (!_material)
			{
				ReloadMaterial();
			}
			return _material;
		}
	}
	
	public List<Mode7Config> configs = new List<Mode7Config>();

	public Mode7Config InterpolateFromTo(Mode7Config config1, Mode7Config config2, float time)
	{
		float value = animationCurve.Evaluate(time);

		return (config2 - config1) * value + config1;
	}
	
	public Mode7Config GetConfig()
	{
		return new Mode7Config(
			Material.GetFloat("_H"),
			Material.GetFloat("_V"),
			Material.GetFloat("_X0"),
			Material.GetFloat("_Y0"),
			Material.GetFloat("_A"),
			Material.GetFloat("_B"),
			Material.GetFloat("_C"),
			Material.GetFloat("_D")
			);
	}

	public void SetConfig(Mode7Config config)
	{
		Material.SetFloat("_H", config.h);
		Material.SetFloat("_V", config.v);
		Material.SetFloat("_X0", config.x0);
		Material.SetFloat("_Y0", config.y0);
		Material.SetFloat("_A", config.a);
		Material.SetFloat("_B", config.b);
		Material.SetFloat("_C", config.c);
		Material.SetFloat("_D", config.d);
	}

	public void ReloadMaterial()
	{
		_material = GetComponent<Renderer>().sharedMaterial;
	}
}

[System.Serializable]
public struct Mode7Config
{
	public float h;
	public float v;
	public float x0;
	public float y0;

	public float a;
	public float b;
	public float c;
	public float d;

	public Mode7Config(float h, float v, float x0, float y0, float a, float b, float c, float d)
	{
		this.h = h;
		this.v = v;
		this.x0 = x0;
		this.y0 = y0;
		this.a = a;
		this.b = b;
		this.c = c;
		this.d = d;
	}

	public static Mode7Config operator *(Mode7Config config, float mult)
	{
		return new Mode7Config(
			config.h * mult,
			config.v * mult,
			config.x0 * mult,
			config.y0 * mult,
			config.a * mult,
			config.b * mult,
			config.c * mult,
			config.d * mult
			);
	}

	public static Mode7Config operator +(Mode7Config config1, Mode7Config config2)
	{
		return new Mode7Config(
			config1.h + config2.h,
			config1.v + config2.v,
			config1.x0 + config2.x0,
			config1.y0 + config2.y0,
			config1.a + config2.a,
			config1.b + config2.b,
			config1.c + config2.c,
			config1.d + config2.d
			);
	}

	public static Mode7Config operator -(Mode7Config config1, Mode7Config config2)
	{
		return new Mode7Config(
			config1.h - config2.h,
			config1.v - config2.v,
			config1.x0 - config2.x0,
			config1.y0 - config2.y0,
			config1.a - config2.a,
			config1.b - config2.b,
			config1.c - config2.c,
			config1.d - config2.d
			);
	}

	public static bool operator ==(Mode7Config c1, Mode7Config c2)
	{
		return
			c1.h == c2.h &&
			c1.v == c2.v &&
			c1.x0 == c2.x0 &&
			c1.y0 == c2.y0 &&
			c1.a == c2.a &&
			c1.b == c2.b &&
			c1.c == c2.c &&
			c1.d == c2.d;
	}

	public static bool operator !=(Mode7Config c1, Mode7Config c2)
	{
		return !(c1 == c2);
	}
	
	public override string ToString()
	{
		return string.Format("Offset: ({0:00.00}, {1:00.00}), Pivot: ({2:00.00}, {3:00.00}), ABCD: ({4:00.00}, {5:00.00}, {6:00.00}, {7:00.00})", h, v, x0, y0, a, b, c, d);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}