using UnityEngine;
using System.Collections;

/**
 * Degree struct
 */
[System.Serializable]
public class Degree {
	public float Angle;
	public float GetRotation()
	{
		float ret = Angle;
		while(ret >= 180) ret-=360;
		while(ret<-180) ret+=360; 
		return ret;
	}
	
	public Degree(float a)
	{
		this.Angle = a;
	}
	
	private void normalize()
	{
		while(this.Angle >= 360) this.Angle-=360;
		while(this.Angle<0) this.Angle+=360;
	}
	public static Degree operator +(Degree a, Degree b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle + b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator -(Degree a, Degree b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle-b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator /(Degree a, Degree b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle/b.Angle;
		c.normalize();
		return c;
	}
	public static Degree operator *(Degree a, Degree b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle*b.Angle;
		c.normalize();
		return c;
	}
	
	public static Degree operator /(Degree a, float b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle/b;
		c.normalize();
		return c;
	}
	public static Degree operator *(Degree a, float b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle*b;
		c.normalize();
		return c;
	}
	public static Degree operator +(Degree a, float b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle + b;
		c.normalize();
		return c;
	}
	public static Degree operator -(Degree a, float b)
	{
		var c = new Degree(0);
		c.Angle = a.Angle-b;
		c.normalize();
		return c;
	}
	
}

[System.Serializable]
public class SDegree {
	public float Angle;
	
	public SDegree(float a)
	{
		this.Angle = a;
	}
	
	private void normalize()
	{
		while(this.Angle >= 180) this.Angle-=360;
		while(this.Angle<-180) this.Angle+=360;
	}
	public static SDegree operator +(SDegree a, SDegree b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle + b.Angle;
		c.normalize();
		return c;
	}
	public static SDegree operator -(SDegree a, SDegree b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle-b.Angle;
		c.normalize();
		return c;
	}
	public static SDegree operator +(Degree a, SDegree b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle-b.Angle;
		c.normalize();
		return c;
	}
	public static SDegree operator /(SDegree a, SDegree b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle/b.Angle;
		c.normalize();
		return c;
	}
	public static SDegree operator *(SDegree a, SDegree b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle*b.Angle;
		c.normalize();
		return c;
	}
	
	public static SDegree operator /(SDegree a, float b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle/b;
		c.normalize();
		return c;
	}
	public static SDegree operator *(SDegree a, float b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle*b;
		c.normalize();
		return c;
	}
	public static SDegree operator +(SDegree a, float b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle + b;
		c.normalize();
		return c;
	}
	public static SDegree operator -(SDegree a, float b)
	{
		var c = new SDegree(0);
		c.Angle = a.Angle-b;
		c.normalize();
		return c;
	}
}

// parameter for handleaxis

public enum KeyAxis
{
	horizontal=1,
	vertical,
	roll, // + right, - left.
	stopRot
}

[System.Serializable]
public struct AxisEventParam {
	public KeyAxis axis;
	public float amount;
}